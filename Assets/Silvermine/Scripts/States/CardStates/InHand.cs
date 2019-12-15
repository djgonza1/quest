using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class InHand : CardState
{
    private enum InHandState { Neutral, Reseting, Highlighted};

    private InHandState handState;

    public override void Begin()
    {
        handState = InHandState.Neutral;
    }

    public override void OnCardEnter()
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            BoardSceneManager.Instance.HighlightCard(_context);
        }
    }

    public override void OnCardExit()
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

    public override void OnCardHover()
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            BoardSceneManager.Instance.HighlightCard(_context);
        }
    }

    public override void OnCardDrag()
    {
        _stateMachine.ChangeState<Grabbed>();
    }
}
