using UnityEngine;
using UnityEngine.Rendering;
using Silvermine.Battle.Core;
using System;

public class CardGO : MonoBehaviour
{
    public const string AssetName = "Card";

    [SerializeField]
    public SortingGroup SortingGroup;
    [SerializeField]
    private SpriteRenderer _cardFront;

    public bool IsTappable { get; set; }
    public AbilityCard Card { get; private set; }

    private Vector2 _originalScale;

    public void Init(AbilityCard card, bool isTappable = true)
    {
        Card = card;
        _originalScale = transform.localScale;
        _cardFront.color = CardUtilities.ToColor(Card.Color);

        IsTappable = isTappable;
    }

    public void FlipCard(bool showFront)
    {
        CardColor color = showFront ? Card.Color : CardColor.None;

        _cardFront.color = CardUtilities.ToColor(color);
    }

    public void OnMouseEnter()
    {
        Events.Instance.Raise(new CardObjectEvent(CardObjectEvent.EventType.ENTER, this));
    }

    public void OnMouseExit()
    {
        Events.Instance.Raise(new CardObjectEvent(CardObjectEvent.EventType.EXIT, this));
    }

    public void OnMouseDown()
    {
        if (!IsTappable)
        {
            return;
        }

        Events.Instance.Raise(new CardObjectEvent(CardObjectEvent.EventType.TAP_DOWN, this));
    }

    public void OnMouseUp()
    {
        if (!IsTappable)
        {
            return;
        }

        Events.Instance.Raise(new CardObjectEvent(CardObjectEvent.EventType.TAP_RELEASE, this));
    }

    public void OnMouseOver()
    {
        if (IsTappable && Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            Events.Instance.Raise(new CardObjectEvent(CardObjectEvent.EventType.DRAG, this));
        }
        else if(!Input.GetMouseButton(0))
        {
            Events.Instance.Raise(new CardObjectEvent(CardObjectEvent.EventType.HOVER, this));
        }
    }
}
