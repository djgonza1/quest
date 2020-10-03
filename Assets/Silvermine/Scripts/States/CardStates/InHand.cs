using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class InHand : SMState<PlayableCardBehaviour>
{
    private enum InHandState { Neutral, Reseting, Highlighted};

    private InHandState handState;

    public override void Begin()
    {

        handState = InHandState.Neutral;

        Events.Instance.AddListener<CardGOEvent>(OnCardEvent);
    }

    public override void End()
    {
        Events.Instance.RemoveListener<CardGOEvent>(OnCardEvent);
    }

    private void OnCardEvent(CardGOEvent msg)
    {
        if (msg.CardObject != _context)
        {
            return;
        }

        switch (msg.Type)
        {
            case CardGOEvent.EventType.MOUSE_ENTER:
                OnCardEnter();
                break;
            case CardGOEvent.EventType.MOUSE_EXIT:
                OnCardExit();
                break;
            case CardGOEvent.EventType.MOUSE_HOVER:
                OnCardHover();
                break;
            case CardGOEvent.EventType.MOUSE_DRAG:
                OnCardDrag();
                break;
        }
    }

    private void OnCardEnter()
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            (_context as ICardBehavior).Highlight(true);
        }
    }

    private void OnCardExit()
    {
        var card = _context as PlayableCardBehaviour;

        if (handState != InHandState.Highlighted)
        {
            return;
        }

        handState = InHandState.Reseting;

        (_context as ICardBehavior).Highlight(false);
        BoardSceneManager.Instance.ResetCardInHand(_context, ()=> 
        {
            handState = InHandState.Neutral;
            (_context as ICardBehavior).SetSortingOrder(0);
        });
    }

    private void OnCardHover()
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            (_context as ICardBehavior).Highlight(true);
        }
    }

    private void OnCardDrag()
    {
        (_context as ICardBehavior).Highlight(false);
        _stateMachine.ChangeState<Grabbed>();
    }
}
