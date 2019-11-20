using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class Grabbed : CardState
{
    public override void Begin()
    {
        _context.GrabCard();
    }

    public override void Update()
    {
        _context.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void End()
    {
        
    }

    public override void OnCardTapRelease()
    {
        _context.ResetCardInHand(() =>
        {
            _stateMachine.ChangeState<InHand>();
        });
    }
}
