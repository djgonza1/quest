using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class HighlightableInHand : SMState<PlayableCardBehaviour>
{
    private enum InHandState { Neutral, Reseting, Highlighted};

    private InHandState handState;

    public override void Begin()
    {
        handState = InHandState.Neutral;

        _context.OnMouseEnterCard += CardEnter;
        _context.OnMouseExitCard += OnCardExit;
        _context.OnMouseHoverCard += OnCardHover;
    }

    public override void End()
    {
        _context.OnMouseEnterCard -= CardEnter;
        _context.OnMouseExitCard -= OnCardExit;
        _context.OnMouseHoverCard -= OnCardHover;
    }

    private void CardEnter(PlayableCardBehaviour card)
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            card.Highlight(true);
        }
    }

    private void OnCardExit(PlayableCardBehaviour card)
    {
        if (handState != InHandState.Highlighted)
        {
            return;
        }

        handState = InHandState.Reseting;

        card.Highlight(false);
        BoardSceneManager.Instance.ResetCardInHand(_context, ()=> 
        {
            handState = InHandState.Neutral;
            card.SetSortingOrder(0);
        });
    }

    private void OnCardHover(PlayableCardBehaviour card)
    {
        if (handState == InHandState.Neutral)
        {
            handState = InHandState.Highlighted;
            card.Highlight(true);
        }
    }
}