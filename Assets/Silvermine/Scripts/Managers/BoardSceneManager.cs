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

    public CallbackQueue EffectsQueue;

    private Dictionary<AbilityCard, CardGOSceneInfo> _playerHandMap;
    private Dictionary<AbilityCard, CardGOSceneInfo> _enemyHandMap;
    private BoardSessionManager _session;
    private EnemyPlayerController _enemyAI;
    
    // Start is called before the first frame update
    void Start()
    {
        _session = new BoardSessionManager(this);
        _enemyAI = new EnemyPlayerController(_session.GameBoard, Player.Second);
        EffectsQueue = new CallbackQueue();
        
        _playerHandMap = CreateHand(_session.GameBoard.GetPlayerHand(Player.First));
        _enemyHandMap = CreateHand(_session.GameBoard.GetPlayerHand(Player.Second), false);

        _session.StartSession();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cardInfo in _playerHandMap.Values)
        {
            cardInfo.StateMachine.Update();
        }
    }

    private Dictionary<AbilityCard, CardGOSceneInfo> CreateHand(List<AbilityCard> cards, bool isPlayerHand = true)
    {
        Dictionary<AbilityCard, CardGOSceneInfo> handMap = new Dictionary<AbilityCard, CardGOSceneInfo>();

        for (int i = 0; i < cards.Count; i++)
        {
            CardGO cardObject = ContentManager.Instance.LoadSpellCardObject(cards[i]);
            
            cardObject.Init(cards[i], isPlayerHand);
            cardObject.FlipCard(isPlayerHand);

            Transform handLoc = isPlayerHand ? _playerCardLocators[i] : _enemyCardLocators[i];

            SMState<CardGO>[] states =
            {
                new InHand(),
                new Grabbed(),
                new InPlay()
            };

            SMStateMachine<CardGO> machine = new SMStateMachine<CardGO>(cardObject, states);

            CardGOSceneInfo cardInfo = new CardGOSceneInfo(cardObject, handLoc.position, machine);
            
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
    
    public void BoardOpen()
    {
        Action<Action> boardOpenStart = (boardOpenEnd) =>
        {
            _battleText.text = "GameStart";
            _battleText.gameObject.SetActive(true);

            foreach (var cInfo in _playerHandMap.Values)
            {
                cInfo.CardGO.IsTappable = false;
            }

            DelayCall(2f, () =>
            {
                _battleText.gameObject.SetActive(false);
                boardOpenEnd();
            });
        };

        EffectsQueue.QueuedCall(boardOpenStart);
    }

    public void ChooseCards(Action<AbilityCard, AbilityCard> onCardsChosen)
    {
        Action<Action> popupStart = (popupEnd) =>
        {
            _battleText.text = "Choose Card";
            _battleText.gameObject.SetActive(true);

            DelayCall(2f, () =>
            {
                foreach (var cInfo in _playerHandMap.Values)
                {
                    cInfo.CardGO.IsTappable = true;
                }

                _battleText.gameObject.SetActive(false);
                popupEnd();
            });
        };

        Action<Action> chooseCardsStart = (chooseCardsEnd) =>
        {
            AbilityCard firstChoice = null;
            AbilityCard secondChoice = null;

            Action<CardGOEvent> onPlayerCardChosen = null;
            onPlayerCardChosen = (msg) =>
            {
                if (msg.Player != Player.First || msg.Type != CardGOEvent.EventType.CHOSEN)
                {
                    return;
                }

                foreach (var cInfo in _playerHandMap.Values)
                {
                    cInfo.CardGO.IsTappable = false;
                }

                Events.Instance.RemoveListener(onPlayerCardChosen);

                firstChoice = msg.CardObject.Card;
                secondChoice = _enemyAI.ChooseCardToPlay();
                onCardsChosen(firstChoice, secondChoice);

                CardGO enemyGO = _enemyHandMap[secondChoice].CardGO;
                this.PlayCard(enemyGO, () =>
                {
                    Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.PLAYED, enemyGO, Player.First));
                    _enemyHandMap[secondChoice].StateMachine.ChangeState<InPlay>();
                    chooseCardsEnd();
                });
            };
            Events.Instance.AddListener(onPlayerCardChosen);
        };

        EffectsQueue.QueuedCall(popupStart)
                    .QueuedCall(chooseCardsStart);
    }

    public void StartBattlePhase(Player winner)
    {
        Action<Action> phaseStart = (phaseEnd) =>
        {
            _battleText.text = "Flip Cards";
            _battleText.gameObject.SetActive(true);

            DelayCall(2f, () =>
            {
                _battleText.text = "Flip Cards";
                _battleText.gameObject.SetActive(false);
                phaseEnd();
            });
        };

        Action<Action> flipStart = (flipEnd) =>
        {
            AbilityCard card = _session.GameBoard.playerTwo.BattleChoice;
            CardGO cardGO = _enemyHandMap[card].CardGO;
            cardGO.FlipCard(true);

            card = _session.GameBoard.playerOne.BattleChoice;
            cardGO = _playerHandMap[card].CardGO;
            cardGO.FlipCard(true);

            DelayCall(1f, () =>
            {
                flipEnd();
            });
        };

        EffectsQueue.QueuedCall(phaseStart)
                    .QueuedCall(flipStart);
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

    private struct CardGOSceneInfo
    {
        public CardGO CardGO;
        public Vector3 HandPosition;
        public SMStateMachine<CardGO> StateMachine;

        public CardGOSceneInfo(CardGO cardGO, Vector3 handPosition, SMStateMachine<CardGO> stateMachine)
        {
            CardGO = cardGO;
            HandPosition = handPosition;
            StateMachine = stateMachine;
        }
    }
}
