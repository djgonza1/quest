using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class Grabbed : SMState<CardGO>
{
    public override void Begin()
    {
        BoardSceneManager.Instance.GrabCard(_context);
        Events.Instance.AddListener<CardGOEvent>(OnCardEvent);
    }

    public override void End()
    {
        Events.Instance.RemoveListener<CardGOEvent>(OnCardEvent);
    }

    public void OnCardEvent(CardGOEvent msg)
    {
        if (msg.CardObject == _context && msg.Type == CardGOEvent.EventType.TAP_RELEASE)
        {
            OnCardTapRelease();
        }
    }

    public override void Update()
    {
        _context.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnCardTapRelease()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

        foreach (var hit in hits)
        {
            if (hit.collider.tag == "PlaySpace")
            {
                BoardSceneManager.Instance.PlayCard(_context, () =>
                {
                    _context.FlipCard(false);
                    Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.PLAYED, _context, Player.First));
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
