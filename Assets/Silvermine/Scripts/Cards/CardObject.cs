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

    private BaseMagicCard _card;
    private BoardSceneManager _manager;
    private Vector2 _originalScale;
    
    public void Init(BaseMagicCard card)
    {
        _card = card;
        _originalScale = transform.localScale;
        _cardFront.color = CardUtilities.ToColor(_card.Color);

        _manager = BoardSceneManager.Instance;
    }

    public void OnMouseEnter()
    {
        Events.Instance.Raise<CardEvent>(new CardEvent(CardEvent.EventType.ENTER, this));
    }

    public void OnMouseExit()
    {
        Events.Instance.Raise<CardEvent>(new CardEvent(CardEvent.EventType.EXIT, this));
    }

    public void OnMouseDown()
    {
        Events.Instance.Raise<CardEvent>(new CardEvent(CardEvent.EventType.TAP_DOWN, this));
    }

    public void OnMouseUp()
    {
        Events.Instance.Raise<CardEvent>(new CardEvent(CardEvent.EventType.TAP_RELEASE, this));
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f))
        {
            Events.Instance.Raise<CardEvent>(new CardEvent(CardEvent.EventType.DRAG, this));
        }
        else if(!Input.GetMouseButton(0))
        {
            Events.Instance.Raise<CardEvent>(new CardEvent(CardEvent.EventType.HOVER, this));
        }
    }
}
