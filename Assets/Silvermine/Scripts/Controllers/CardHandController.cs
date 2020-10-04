using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class CardHandController : MonoBehaviour
{
    [SerializeField] private Transform[] _cardLocators = null;

    private Dictionary<AbilityCard, HandCardInfo> _handMap;

    void Update()
    {
        if (_handMap == null)
        {
            return;
        }

        foreach (var cardInfo in _handMap.Values)
        {
            cardInfo.StateMachine.Update();
        }
    }
    
    public void CreateHand(List<AbilityCard> cards, PlayerType playerType = PlayerType.First)
    {
        _handMap = new Dictionary<AbilityCard, HandCardInfo>();
        
        for (int i = 0; i < cards.Count; i++)
        {
            PlayableCardBehaviour cardObject = ContentManager.Instance.CreateCardObject(cards[i]);

            cardObject.FlipCard(playerType == PlayerType.First);  

            Transform handLoc = _cardLocators[i];

            SMStateMachine<PlayableCardBehaviour> machine = null;

            //Set state machine for card depending on player type
            if (playerType == PlayerType.First)
            {
                SMState<PlayableCardBehaviour>[] states =
                {
                    new ChoosableInHand(),
                    new HighlightableInHand(),
                    new Grabbed(),
                    new InPlay()
                };
                
                machine = new SMStateMachine<PlayableCardBehaviour>(cardObject, states);
            }
            else
            {
                SMState<PlayableCardBehaviour>[] states =
                {
                    new HighlightableInHand(),
                    new Grabbed(),
                    new InPlay()
                };

                machine = new SMStateMachine<PlayableCardBehaviour>(cardObject, states);
            }

            HandCardInfo cardInfo = new HandCardInfo(cardObject, handLoc.position, BoardSceneManager.Instance.GetHandCardScale(), machine);

            _handMap.Add(cards[i], cardInfo);
        }

        foreach (var cInfo in _handMap.Values)
        {
            var position = cInfo.HandPosition;

            cInfo.CardGO.transform.position = position;
            cInfo.CardGO.transform.localScale = BoardSceneManager.Instance.GetHandCardScale();
        }
    }
    
    public Vector3 GetCardHandPosition(PlayableCardBehaviour cardGO)
    {
        return _handMap[cardGO.Card].HandPosition;
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
