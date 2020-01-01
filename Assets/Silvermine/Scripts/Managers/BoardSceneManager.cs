using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Silvermine.Battle.Core;

public class BoardSceneManager : SingletonGameObject<BoardSceneManager>, IBoardSceneManager
{
    public const float CardOverSizePosOffset = 1.3f;
    public const float CardOverSizeScale = 1.5f;

    [SerializeField]
    private Transform[] _playerCardLocators;
    [SerializeField]
    private Transform[] _enemyCardLocators;
    [SerializeField]
    private Transform _leftSpellBoardLocator;
    [SerializeField]
    private Transform _rightSpellBoardLocator;
    [SerializeField]
    private Text _battleText;

    private Dictionary<AbilityCard, CardObjectSceneInfo> _playerHandMap;
    private Dictionary<AbilityCard, CardObjectSceneInfo> _enemyHandMap;
    private BoardSessionManager _session;
    private EnemyPlayerController _enemyAI;
    
    // Start is called before the first frame update
    void Start()
    {
        _session = new BoardSessionManager(this);
        _enemyAI = new EnemyPlayerController(_session.GameBoard, Player.Second);
        
        _playerHandMap = CreateHand(_session.GameBoard.GetPlayerHand(Player.First));
        _enemyHandMap = CreateHand(_session.GameBoard.GetPlayerHand(Player.Second), false);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cardInfo in _playerHandMap.Values)
        {
            cardInfo.StateMachine.Update();
        }
    }

    private Dictionary<AbilityCard, CardObjectSceneInfo> CreateHand(List<AbilityCard> cards, bool isPlayerHand = true)
    {
        Dictionary<AbilityCard, CardObjectSceneInfo> handMap = new Dictionary<AbilityCard, CardObjectSceneInfo>();

        for (int i = 0; i < cards.Count; i++)
        {
            CardGO cardObject = ContentManager.Instance.LoadSpellCardObject(cards[i]);
            
            cardObject.Init(cards[i], false);
            cardObject.FlipCard(isPlayerHand);

            Transform handLoc = isPlayerHand ? _playerCardLocators[i] : _enemyCardLocators[i];

            SMState<CardGO>[] states =
            {
                new InHand(),
                new Grabbed(),
                new InPlay()
            };

            SMStateMachine<CardGO> machine = new SMStateMachine<CardGO>(cardObject, states);

            CardObjectSceneInfo cardInfo = new CardObjectSceneInfo(cardObject, handLoc.position, machine);
            
            handMap.Add(cards[i], cardInfo);
        }

        foreach (var cInfo in handMap.Values)
        {
            var position = cInfo.HandPosition;

            cInfo.CardGO.transform.position = position;
            cInfo.CardGO.transform.localScale = GetHandCardScale();
        }

        return handMap;
    }
    
    public Vector3 GetCardHandPosition(CardGO cardGO)
    {

        if (_playerHandMap.ContainsKey(cardGO.Card))
        {
            return _playerHandMap[cardGO.Card].HandPosition;
        }

        if (_enemyHandMap.ContainsKey(cardGO.Card))
        {
            return _enemyHandMap[cardGO.Card].HandPosition;
        }

        Debug.LogError("No hand locator found for card object");
        return Vector2.zero;
    }

    public Vector3 GetBoardPlayPosition(CardGO cardGO)
    {
        if (_playerHandMap.ContainsKey(cardGO.Card))
        {
            return _leftSpellBoardLocator.position;
        }
        else if (_enemyHandMap.ContainsKey(cardGO.Card))
        {
            return _rightSpellBoardLocator.position;
        }

        Debug.LogError("No play locator found for card object");
        return Vector2.zero;
    }

    public Vector2 GetHandCardScale()
    {
        return new Vector2(0.6f, 0.6f);
    }

    public Vector2 GetPlayBoardCardScale()
    {
        return GetHandCardScale();
    }
    
    public void BoardOpen(Action onComplete)
    {
        DelayCall(2f, () =>
        {
            foreach (var cInfo in _playerHandMap.Values)
            {
                cInfo.CardGO.IsTappable = false;
            }
            
            onComplete();
        });
    }

    public void ChooseCards(Action<AbilityCard> onFirstCardChosen, Action<AbilityCard> onSecondCardChosen, Action onComplete)
    {
        _battleText.text = "Choosing Phase";

        CallbackCounter callbackCount = new CallbackCounter(2, onComplete);

        Action<CardGOEvent> onPlayerCardChosen = null;
        onPlayerCardChosen = (msg) =>
        {
            if (msg.Player != Player.First || msg.Type != CardGOEvent.EventType.PLAYED)
            {
                return;
            }

            Events.Instance.RemoveListener(onPlayerCardChosen);
            onFirstCardChosen(msg.CardObject.Card);
            callbackCount--;

            AbilityCard enemyCard = _enemyAI.ChooseCardToPlay();
            CardGO enemyGO = _enemyHandMap[enemyCard].CardGO;

            this.PlayCard(enemyGO, () =>
            {
                onSecondCardChosen(enemyGO.Card);
                callbackCount--;
            });
        };
        
        Events.Instance.AddListener(onPlayerCardChosen);

        DelayCall(2f, () =>
        {
            foreach (var cInfo in _playerHandMap.Values)
            {
                cInfo.CardGO.IsTappable = true;
            }

            _battleText.gameObject.SetActive(false);
        });
    }

    public void StartBattlePhase(Player winner)
    {
        _battleText.text = "Flip Cards";
        _battleText.gameObject.SetActive(true);

        DelayCall(1f, () =>
        {
            _battleText.gameObject.SetActive(false);
        });

        DelayCall(2f, () =>
        {
            AbilityCard card = _session.GameBoard.playerTwo.BattleChoice;
            CardGO cardGO = _enemyHandMap[card].CardGO;
            cardGO.FlipCard(true);

            card = _session.GameBoard.playerOne.BattleChoice;
            cardGO = _playerHandMap[card].CardGO;
            cardGO.FlipCard(true);
        });
    }

    public void DelayCall(float delay, Action callback)
    {
        StartCoroutine(DelayedAction(delay, callback));
    }
    
    private IEnumerator DelayedAction(float delay, Action callback)
    {
        if(delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        callback.Invoke();
    }

    private struct CardObjectSceneInfo
    {
        public CardGO CardGO;
        public Vector3 HandPosition;
        public SMStateMachine<CardGO> StateMachine;

        public CardObjectSceneInfo(CardGO cardGO, Vector3 handPosition, SMStateMachine<CardGO> stateMachine)
        {
            CardGO = cardGO;
            HandPosition = handPosition;
            StateMachine = stateMachine;
        }
    }
}
