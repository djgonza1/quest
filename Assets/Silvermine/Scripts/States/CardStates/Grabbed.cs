using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class Grabbed : SMState<PlayableCardBehaviour>
{
    public override void Begin()
    {
        _context.OnMouseUnclicksCard += OnCardTapRelease;

        BoardSceneManager.Instance.GrabCard(_context);
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
                Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.CHOSEN, _context, PlayerType.First));

                BoardSceneManager.Instance.PlayCard(_context, () =>
                {
                    _context.FlipCard(false);
                    _stateMachine.ChangeState<InPlay>();
                    Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.PLAYED, _context, PlayerType.First));
                });

                return;
            }
        }

        BoardSceneManager.Instance.ResetCardInHand(_context, () =>
        {
            _stateMachine.ChangeState<ChoosableInHand>();
        });
    }
}
