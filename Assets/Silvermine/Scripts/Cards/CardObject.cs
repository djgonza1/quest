using UnityEngine;
using UnityEngine.Rendering;
using Silvermine.Battle.Core;
using System;

public class CardObject : MonoBehaviour
{
    public const string AssetName = "Card";

    [SerializeField]
    public SortingGroup SortingGroup;
    [SerializeField]
    private SpriteRenderer _cardFront;

    public bool IsTappable { get; set; }

    private BaseMagicCard _card;
    private Vector2 _originalScale;

    public void Init(BaseMagicCard card, bool isTappable = true)
    {
        _card = card;
        _originalScale = transform.localScale;
        _cardFront.color = CardUtilities.ToColor(_card.Color);

        IsTappable = isTappable;
    }

    public void OnMouseEnter()
    {
        Events.Instance.Raise(new CardEvent(CardEvent.EventType.ENTER, this));
    }

    public void OnMouseExit()
    {
        Events.Instance.Raise(new CardEvent(CardEvent.EventType.EXIT, this));
    }

    public void OnMouseDown()
    {
        if (!IsTappable)
        {
            return;
        }

        Events.Instance.Raise(new CardEvent(CardEvent.EventType.TAP_DOWN, this));
    }

    public void OnMouseUp()
    {
        if (!IsTappable)
        {
            return;
        }

        Events.Instance.Raise(new CardEvent(CardEvent.EventType.TAP_RELEASE, this));
    }

    public void OnMouseOver()
    {
        if (IsTappable && Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            Events.Instance.Raise(new CardEvent(CardEvent.EventType.DRAG, this));
        }
        else if(!Input.GetMouseButton(0))
        {
            Events.Instance.Raise(new CardEvent(CardEvent.EventType.HOVER, this));
        }
    }
}
