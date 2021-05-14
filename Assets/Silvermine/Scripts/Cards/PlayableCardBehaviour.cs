using UnityEngine;
using UnityEngine.EventSystems;
using Silvermine.Battle.Core;
using System;

public class PlayableCardBehaviour : MonoBehaviour, ICardBehavior 
{
    public const string AssetName = "Card";

    [SerializeField] private CardBehaviour _cardGO = null;
    [SerializeField] private PlayableCardStateMachine _stateMachine;
    
    public event Action<PlayableCardBehaviour> OnMouseEnterCard;
    public event Action<PlayableCardBehaviour> OnMouseExitCard;
    public event Action<PlayableCardBehaviour> OnMouseClicksCard;
    public event Action<PlayableCardBehaviour> OnMouseUnclicksCard;
    public event Action<PlayableCardBehaviour> OnMouseDragCard;
    public event Action<PlayableCardBehaviour> OnMouseHoverCard;

    public PlayableCardStateMachine StateMachine => _stateMachine;

    public void Init(AbilityCard card)
    {
        _cardGO.Init(card);

        FlipCard(true);
    }

    #region Interfaces
    public AbilityCard Card { get => _cardGO.Card; }

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

    public void FlipCard(bool showFront)
    {
        _cardGO.FlipCard(showFront);
    }

    #region Mouse Callbacks
    public void OnMouseEnter()
    {
        OnMouseEnterCard?.Invoke(this);
    }

    public void OnMouseExit()
    {
        OnMouseExitCard?.Invoke(this);
    }

    public void OnMouseDown()
    {
        OnMouseClicksCard?.Invoke(this);
    }

    public void OnMouseUp()
    {
        OnMouseUnclicksCard?.Invoke(this);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            OnMouseDragCard?.Invoke(this);
        }
        else if(!Input.GetMouseButton(0))
        {
            OnMouseHoverCard?.Invoke(this);
        }
    }
    #endregion
} 
