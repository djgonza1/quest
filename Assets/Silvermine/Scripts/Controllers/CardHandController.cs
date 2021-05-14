using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class CardHandController : MonoBehaviour
{
    [SerializeField] private Transform[] _cardLocators = null;
    [SerializeField] private Transform _playCardLocator;
    [SerializeField] private PlayableCardStateMachine _stateMachine;

    private Dictionary<AbilityCard, HandCardInfo> _handMap;
    private PlayerType _playerType;

    public event Action<AbilityCard> OnCardChosen;

    public void Init(List<AbilityCard> cards, PlayerType playerType = PlayerType.First)
    {
        _playerType = playerType;
        _handMap = CreateHandMap(cards, playerType);
    }
    
    public Dictionary<AbilityCard, HandCardInfo> CreateHandMap(List<AbilityCard> cards, PlayerType playerType = PlayerType.First)
    {
        var handMap = new Dictionary<AbilityCard, HandCardInfo>();
        
        for (int i = 0; i < cards.Count; i++)
        {
            PlayableCardBehaviour cardObject = ContentManager.Instance.CreateCardObject(cards[i]);

            cardObject.FlipCard(playerType == PlayerType.First);  

            Transform handLoc = _cardLocators[i];

            //Set state machine for card depending on player type
            if (playerType == PlayerType.First)
            {
                SMState<PlayableCardBehaviour>[] states =
                {
                    new ChoosableInHand(this),
                    new HighlightableInHand(this),
                    new Grabbed(this),
                    new InPlay()
                };
                
                cardObject.StateMachine.Begin(cardObject, states);
            }
            else
            {
                SMState<PlayableCardBehaviour>[] states =
                {
                    new HighlightableInHand(this),
                    new InPlay()
                };

                cardObject.StateMachine.Begin(cardObject, states);
            }

            HandCardInfo cardInfo = new HandCardInfo(cardObject, handLoc.position, GetHandCardScale(), cardObject.StateMachine);

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

    public void PlaceCardOnBoard(PlayableCardBehaviour card)
    {
        if (!ContainsCard(card))
        {
            Debug.LogError("Cannot place card that does not exist in hand: " + this.name);
            return;
        }

        card.FlipCard(false);

        _handMap[card.Card].StateMachine.ChangeState<InPlay>();

        _handMap.Remove(card.Card);
    }
    
    public Vector3 GetCardHandPosition(PlayableCardBehaviour cardGO)
    {
        return _handMap[cardGO.Card].HandPosition;
    }

    public Vector2 GetHandCardScale()
    {
        return new Vector2(0.6f, 0.6f);
    }

    public bool ContainsCard(PlayableCardBehaviour cardGO)
    {
        return _handMap.ContainsKey(cardGO.Card);
    }

    public PlayableCardBehaviour GetCard(AbilityCard card)
    {
        return _handMap[card].CardGO;
    }

    public SMStateMachine<PlayableCardBehaviour> GetStateMachine(AbilityCard card)
    {
        return _handMap[card].StateMachine;
    }

    public void ResetCardInHand(PlayableCardBehaviour card, Action callback = null)
    {
        Vector2 handPosition = GetCardHandPosition(card);
        Vector2 handScale = GetHandCardScale();

        LeanTween.scale(card.gameObject, handScale, 0.2f);
        LeanTween.move(card.gameObject, new Vector2(handPosition.x, handPosition.y), 0.2f).setOnComplete(() =>
        {
            callback?.Invoke();
        });

        (card as ICardBehavior).SetSortingOrder(0);
    }

    public void PlayCard(PlayableCardBehaviour card, Action callback = null)
    {
        OnCardChosen?.Invoke(card.Card);

        Vector3 playPosition = _playCardLocator.position;
        Vector3 playScale = new Vector2(0.6f, 0.6f);

        LeanTween.scale(card.gameObject, playScale, 0.3f);

        Vector3 start = card.transform.position;
        Vector3 point1 = start + new Vector3(0, 2f);
        Vector3 point2 = playPosition + new Vector3(0, 2f);

        LTBezierPath path = new LTBezierPath(new Vector3[] { start, point2, point1, playPosition });

        LeanTween.move(card.gameObject, path, 0.3f).setOnComplete(() =>
        {
            PlaceCardOnBoard(card);

            callback?.Invoke();
        });

        (card as ICardBehavior).SetSortingOrder(0);
    }

    public void OnCardGrabbed(PlayableCardBehaviour grabbedCard)
    {
        foreach(var handCardInfo in _handMap.Values)
        {
            var handCard = handCardInfo.CardGO;

            if(handCard != grabbedCard)
            {
                Debug.LogWarning("Set rest of cards to highlightable only");
                handCardInfo.StateMachine.ChangeState<HighlightableInHand>();
            }
        }
    }

    public void SetCardsAsChoosable()
    {
        foreach(var handCardInfo in _handMap.Values)
        {
            handCardInfo.StateMachine.ChangeState<ChoosableInHand>();
        }
    }

    public struct HandCardInfo
    {
        public PlayableCardBehaviour CardGO;
        public Vector3 HandPosition;
        public Vector2 HandScale;
        public SMStateMachine<PlayableCardBehaviour> StateMachine;

        public HandCardInfo(PlayableCardBehaviour cardGO, Vector3 handPosition, Vector2 handScale, SMStateMachine<PlayableCardBehaviour> stateMachine)
        {
            CardGO = cardGO;
            HandPosition = handPosition;
            HandScale = handScale;
            StateMachine = stateMachine;
        }
    }
}
