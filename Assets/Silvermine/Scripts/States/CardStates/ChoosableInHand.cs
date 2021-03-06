﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class ChoosableInHand : SMState<PlayableCardBehaviour>
{
    private enum InHandState { Neutral, Reseting, Highlighted};

    private InHandState handState;
    private CardHandController _handController;

    public ChoosableInHand(CardHandController handController)
    {
        _handController = handController;
    }

    public override void Begin()
    {
        handState = InHandState.Neutral;

        _context.OnMouseEnterCard += CardEnter;
        _context.OnMouseExitCard += OnCardExit;
        _context.OnMouseHoverCard += OnCardHover;
        _context.OnMouseDragCard += OnCardDrag;
    }

    public override void End()
    {
        _context.OnMouseEnterCard -= CardEnter;
        _context.OnMouseExitCard -= OnCardExit;
        _context.OnMouseHoverCard -= OnCardHover;
        _context.OnMouseDragCard -= OnCardDrag;
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
        _handController.ResetCardInHand(_context, ()=> 
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

    private void OnCardDrag(PlayableCardBehaviour card)
    {
        card.Highlight(false);
        _stateMachine.ChangeState<Grabbed>();
    }
}
