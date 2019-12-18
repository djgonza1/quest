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
    private Action _onCardPlayed;
    
    // Start is called before the first frame update
    void Start()
    {
        _session = new BoardSessionManager(this);

        BaseMagicCard[] playerCards =
        {
            new BaseMagicCard(CardColor.Red, 0),
            new BaseMagicCard(CardColor.Green, 0),
            new BaseMagicCard(CardColor.Blue, 0)
        };

        BaseMagicCard[] enemyCards =
        {
            new BaseMagicCard(CardColor.None, 0),
            new BaseMagicCard(CardColor.None, 0),
            new BaseMagicCard(CardColor.None, 0)
        };
        

        _playerHandMap = CreateHand(playerCards);
        _enemyHandMap = CreateHand(enemyCards, false);
        _onCardPlayed = null;

        Events.Instance.AddListener<CardEvent>(OnCardEvent);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cardInfo in _playerHandMap.Values)
        {
            cardInfo.StateMachine.Update();
        }
    }

    private Dictionary<CardObject, CardObjectSceneInfo> CreateHand(BaseMagicCard[] cards, bool isPlayerHand = true)
    {
        Dictionary<CardObject, CardObjectSceneInfo> handMap = new Dictionary<CardObject, CardObjectSceneInfo>();

        for (int i = 0; i < cards.Length; i++)
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

    public void OnCardEvent(CardEvent e)
    {
        if (!_playerHandMap.ContainsKey(e.Card))
        {
            return;
        }

        switch(e.Type)
        {
            case CardEvent.EventType.ENTER:
                _playerHandMap[e.Card].StateMachine.OnCardEnter();
                break;
            case CardEvent.EventType.EXIT:
                _playerHandMap[e.Card].StateMachine.OnCardExit();
                break;
            case CardEvent.EventType.HOVER:
                _playerHandMap[e.Card].StateMachine.OnCardHover();
                break;
            case CardEvent.EventType.DRAG:
                _playerHandMap[e.Card].StateMachine.OnCardDrag();
                break;
            case CardEvent.EventType.TAP_DOWN:
                _playerHandMap[e.Card].StateMachine.OnCardTapDown();
                break;
            case CardEvent.EventType.TAP_RELEASE:
                _playerHandMap[e.Card].StateMachine.OnCardTapRelease();
                break;
            case CardEvent.EventType.PLAYED:
                _onCardPlayed?.Invoke();
                _onCardPlayed = null;
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

    public void OnChoosingPhase(Action onComplete)
    {
        _battleText.text = "Choosing Phase";

        DelayCall(() =>
        {
            foreach (var card in _playerHandMap.Keys)
            {
                card.IsTappable = true;
            }

            _battleText.gameObject.SetActive(false);
        }, 2f);
        
        _onCardPlayed = onComplete;
    }

    private void OnDestroy()
    {
        Events.Instance.RemoveListener<CardEvent>(OnCardEvent);
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
