using UnityEngine;
using UnityEngine.EventSystems;
using Silvermine.Battle.Core;
using System;

public class PlayableCardGO : MonoBehaviour, ICardGO 
{
    public const string AssetName = "Card";

    [SerializeField] private CardGO _cardGO = null;
    
    public bool IsTappable { get; set; }
    public AbilityCard Card { get; private set; }

    public void Init(AbilityCard card)
    {
        Card = card;
        _cardGO.SetColor(Card.Color);

        FlipCard(true);

        IsTappable = true;
    }

    public void FlipCard(bool showFront)
    {
        _cardGO.FlipCard(showFront);
    }

    #region Mouse Events

    public void OnMouseEnter()
    {
        Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.MOUSE_ENTER, this));
    }

    public void OnMouseExit()
    {
        Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.MOUSE_EXIT, this));
    }

    public void OnMouseDown()
    {
        if (!IsTappable)
        {
            return;
        }

        Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.TAP_DOWN, this));
    }

    public void OnMouseUp()
    {
        if (!IsTappable)
        {
            return;
        }

        Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.TAP_RELEASE, this));
    }

    public void OnMouseOver()
    {
        if (IsTappable && Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.MOUSE_DRAG, this));
        }
        else if(!Input.GetMouseButton(0))
        {
            Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.MOUSE_HOVER, this));
        }
    }

    #endregion

    #region ICardGO Implementation

    public void SetSortingOrder(int order)
    {
        _cardGO.SetSortingOrder(order);
    }

    public void Highlight(bool enable)
    {
        _cardGO.Highlight(enable);
        _cardGO.SetSortingOrder(1);
    }
    
    #endregion
} 
