using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class Grabbed : SMState<PlayableCardBehaviour>
{
    private CardHandController _handController;

    public Grabbed(CardHandController handController)
    {
        _handController = handController;
    }

    public override void Begin()
    {
        _context.OnMouseUnclicksCard += OnCardTapRelease;
        
        _handController.OnCardGrabbed(_context);

        Vector2 handScale = _handController.GetHandCardScale();
        LeanTween.scale(_context.gameObject, handScale, 0.2f);
    }

    public override void End()
    {
        _context.OnMouseUnclicksCard -= OnCardTapRelease;
    }

    public override void Update()
    {
        _context.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnCardTapRelease(PlayableCardBehaviour card)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

        foreach (var hit in hits)
        {
            if (hit.collider.tag == "PlaySpace")
            {
                _handController.PlayCard(card);
                return;
            }
        }

        _handController.ResetCardInHand(_context, () =>
        {
            _stateMachine.ChangeState<ChoosableInHand>();
        });
    }
}
