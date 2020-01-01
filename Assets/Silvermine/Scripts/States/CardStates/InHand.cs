using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class InHand : SMState<CardGO>
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
            case CardGOEvent.EventType.ENTER:
                OnCardEnter();
                break;
            case CardGOEvent.EventType.EXIT:
                OnCardExit();
                break;
            case CardGOEvent.EventType.HOVER:
                OnCardHover();
                break;
            case CardGOEvent.EventType.DRAG:
                OnCardDrag();
                break;
        }
    }

    private void OnCardEnter()
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            BoardSceneManager.Instance.HighlightCard(_context);
        }
    }

    private void OnCardExit()
    {
        if (handState != InHandState.Highlighted)
        {
            return;
        }

        handState = InHandState.Reseting;
        BoardSceneManager.Instance.ResetCardInHand(_context, ()=> 
        {
            handState = InHandState.Neutral;
        });
    }

    private void OnCardHover()
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            BoardSceneManager.Instance.HighlightCard(_context);
        }
    }

    private void OnCardDrag()
    {
        _stateMachine.ChangeState<Grabbed>();
    }
}
