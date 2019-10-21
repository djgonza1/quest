using UnityEngine;
using Silvermine.Battle.Core;

public class CardObject : MonoBehaviour
{
    private static bool _playerHoldsCard = false;

    private enum CardState { None, InHand, Grabbed, Reseting};

    private const float OverSizePosOffset = 1.3f;
    private const float OverSizeScale = 1.5f;

    [SerializeField]
    SpriteRenderer _renderer;
    [SerializeField]
    private CardColor _color;
    [SerializeField]
    private int _damange;

    private BaseMagicCard _card;
    private BoardSceneManager _manager;
    private Vector2 _originalScale;
    private CardState _state;

    void Start()
    {
        _card = new BaseMagicCard(_color, _damange);
        _manager = BoardSceneManager.Instance;
        _originalScale = transform.localScale;
        _state = CardState.InHand;
    }
    
    void Update()
    {
        if(_state != CardState.Grabbed)
        {
            return;
        }
        
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = position;
    }

    private void OnMouseUp()
    {
        if(_state == CardState.Grabbed)
        {
            DropCard();
        }
        else
        {
            GrabCard();
        }
    }

    private void OnMouseDrag()
    {
        if (_state != CardState.InHand)
        {
            return;
        }

        if (Input.GetAxis("Mouse X") > 0f || Input.GetAxis("Mouse Y") > 0f)
        {
            GrabCard();
        }
    }

    private void OnMouseEnter()
    {
        if (_state != CardState.InHand || _playerHoldsCard)
        {
            return;
        }
        
        HighlightCard();
    }

    private void OnMouseExit()
    {
        if(_state != CardState.InHand)
        {
            return;
        }

        ResetCardInHand();
    }

    private void HighlightCard()
    {
        Vector2 handPosition = _manager.GetCardHandPosition(this);

        LeanTween.scale(this.gameObject, new Vector2(OverSizeScale, OverSizeScale), 0.2f);
        LeanTween.move(this.gameObject, new Vector2(handPosition.x, handPosition.y + OverSizePosOffset), 0.2f);

        _renderer.sortingOrder++;
    }
    
    private void ResetCardInHand()
    {
        _state = CardState.Reseting;
        Vector2 handPosition = _manager.GetCardHandPosition(this);
        Vector2 handScale = _manager.GetHandCardScale();

        LeanTween.scale(this.gameObject, handScale, 0.2f);
        LeanTween.move(this.gameObject, new Vector2(handPosition.x, handPosition.y), 0.2f).setOnComplete(()=> 
        {
            _state = CardState.InHand;
        });

        _renderer.sortingOrder = 0;
    }

    private void GrabCard()
    {
        _state = CardState.Grabbed;
        _playerHoldsCard = true;
        Vector2 handScale = _manager.GetHandCardScale();
        LeanTween.scale(this.gameObject, handScale, 0.2f);
    }

    private void DropCard()
    {
        Debug.Log("DropCard");
        _playerHoldsCard = false;


        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector3.forward);

        foreach(var hit in hits)
        {
            Debug.Log("Did Hit");
            if (hit.transform.tag == "PlaySpace")
            {
                Debug.Log("Did Hit PlaySpace");

                Events.Instance.Raise<CardEvent>(new CardEvent(CardEvent.EventType.CARD_PLAYED, this));
                break;
            }
        }

        ResetCardInHand();
    }
}
