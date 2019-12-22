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

    private Dictionary<CardObject, CardObjectSceneInfo> _playerHandMap;
    private Dictionary<CardObject, CardObjectSceneInfo> _enemyHandMap;
    private BoardSessionManager _session;
    
    // Start is called before the first frame update
    void Start()
    {
        _session = new BoardSessionManager(this);

        List<BaseMagicCard> playerCards = _session.GetPlayerHand(Player.First);

        //TODO - Replace this with session cards but hide their colors from current player
        List<BaseMagicCard> enemyCards = new List<BaseMagicCard>()
        {
            new BaseMagicCard(CardColor.None, 0),
            new BaseMagicCard(CardColor.None, 0),
            new BaseMagicCard(CardColor.None, 0)
        };
        
        _playerHandMap = CreateHand(playerCards);
        _enemyHandMap = CreateHand(enemyCards, false);

        Events.Instance.AddListener<CardObjectEvent>(OnCardEvent);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cardInfo in _playerHandMap.Values)
        {
            cardInfo.StateMachine.Update();
        }
    }

    private Dictionary<CardObject, CardObjectSceneInfo> CreateHand(List<BaseMagicCard> cards, bool isPlayerHand = true)
    {
        Dictionary<CardObject, CardObjectSceneInfo> handMap = new Dictionary<CardObject, CardObjectSceneInfo>();

        for (int i = 0; i < cards.Count; i++)
        {
            CardObject cardObject = ContentManager.Instance.LoadSpellCardObject(cards[i]);

            CardState[] states =
            {
                new InHand(),
                new Grabbed(),
                new InPlay()
            };

            CardStateMachine machine = new CardStateMachine(cardObject, states);

            cardObject.Init(cards[i], false);

            Transform handLoc = isPlayerHand ? _playerCardLocators[i] : _enemyCardLocators[i];

            CardObjectSceneInfo cardInfo = new CardObjectSceneInfo(handLoc.position, machine);

            handMap.Add(cardObject, cardInfo);
        }

        foreach (var pair in handMap)
        {
            var card = pair.Key;
            var position = pair.Value.HandPosition;

            card.transform.position = position;
            card.transform.localScale = GetHandCardScale();
        }

        return handMap;
    }
    
    public Vector3 GetCardHandPosition(CardObject card)
    {
        if (_playerHandMap.ContainsKey(card))
        {
            return _playerHandMap[card].HandPosition;
        }

        if (_enemyHandMap.ContainsKey(card))
        {
            return _enemyHandMap[card].HandPosition;
        }

        Debug.LogError("No hand locator found for card object");
        return Vector2.zero;
    }

    public Vector3 GetBoardPlayPosition(CardObject card)
    {
        if (_leftSpellBoardLocator)
        {
            return _leftSpellBoardLocator.position;
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

    public void OnCardEvent(CardObjectEvent e)
    {
        if (!_playerHandMap.ContainsKey(e.CardObject))
        {
            return;
        }

        switch(e.Type)
        {
            case CardObjectEvent.EventType.ENTER:
                _playerHandMap[e.CardObject].StateMachine.OnCardEnter();
                break;
            case CardObjectEvent.EventType.EXIT:
                _playerHandMap[e.CardObject].StateMachine.OnCardExit();
                break;
            case CardObjectEvent.EventType.HOVER:
                _playerHandMap[e.CardObject].StateMachine.OnCardHover();
                break;
            case CardObjectEvent.EventType.DRAG:
                _playerHandMap[e.CardObject].StateMachine.OnCardDrag();
                break;
            case CardObjectEvent.EventType.TAP_DOWN:
                _playerHandMap[e.CardObject].StateMachine.OnCardTapDown();
                break;
            case CardObjectEvent.EventType.TAP_RELEASE:
                _playerHandMap[e.CardObject].StateMachine.OnCardTapRelease();
                break;
        }
    }
    
    public void OnBattleStart(Action onComplete)
    {
        DelayCall(() =>
        {
            foreach (var card in _playerHandMap.Keys)
            {
                card.IsTappable = false;
            }
            
            onComplete();
        }, 2f);
    }

    public void OnChoosingPhase(Action<BaseMagicCard> onFirstCardChosen, Action<BaseMagicCard> onSecondCardChosen, Action onComplete)
    {
        _battleText.text = "Choosing Phase";

        CallbackCounter callbackCount = new CallbackCounter(2);

        Action<CardObjectEvent> onPlayerCardChosen = null;
        onPlayerCardChosen = (msg) =>
        {
            if (msg.Player == Player.First)
            {
                Events.Instance.RemoveListener(onPlayerCardChosen);
                onFirstCardChosen(msg.CardObject.Card);
                callbackCount--;
            }
        };
        
        Events.Instance.AddListener(onPlayerCardChosen);

        //TODO - Replace with enemy card pick logic
        onSecondCardChosen(null);
        callbackCount--;

        WaitForCallbacks(callbackCount, onComplete);
        
        DelayCall(() =>
        {
            foreach (var card in _playerHandMap.Keys)
            {
                card.IsTappable = true;
            }

            _battleText.gameObject.SetActive(false);
        }, 2f);
    }

    private void OnDestroy()
    {
        Events.Instance.RemoveListener<CardObjectEvent>(OnCardEvent);
    }

    public void DelayCall(Action callback, float delay)
    {
        StartCoroutine(DelayedAction(callback, delay));
    }

    private IEnumerator DelayedAction(Action callback, float delay)
    {
        if(delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        callback.Invoke();
    }

    public void WaitForCallbacks(CallbackCounter counter, Action onComplete)
    {
        StartCoroutine(WaitForCallbacksRoutine(counter, onComplete));
    }

    private IEnumerator WaitForCallbacksRoutine(CallbackCounter counter, Action onComplete)
    {
        while (counter.Count > 0)
        {
            yield return null;
        }

        onComplete();
    }

    private struct CardObjectSceneInfo
    {
        public Vector3 HandPosition;
        public CardStateMachine StateMachine;

        public CardObjectSceneInfo(Vector3 handPosition, CardStateMachine stateMachine)
        {
            HandPosition = handPosition;
            StateMachine = stateMachine;
        }
    }
}
