using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class Grabbed : CardState
{
    public override void Begin()
    {
        BoardSceneManager.Instance.GrabCard(_context);
    }

    public override void Update()
    {
        _context.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void OnCardTapRelease()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

        foreach (var hit in hits)
        {
            if (hit.collider.tag == "PlaySpace")
            {
                BoardSceneManager.Instance.PlayCard(_context, () =>
                {
                    Events.Instance.Raise(new CardObjectEvent(CardObjectEvent.EventType.PLAYED, _context, Player.First));
                    _stateMachine.ChangeState<InPlay>();
                });

                return;
            }
        }

        BoardSceneManager.Instance.ResetCardInHand(_context, () =>
        {
            _stateMachine.ChangeState<InHand>();
        });
    }
}
