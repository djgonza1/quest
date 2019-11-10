using UnityEngine;
using UnityEngine.Rendering;
using Silvermine.Battle.Core;
using System;

public class CardObject : MonoBehaviour
{
    public const string AssetName = "Card";
    
    private const float OverSizePosOffset = 1.3f;
    private const float OverSizeScale = 1.5f;

    public PlayMakerFSM CardFsm;

    [SerializeField]
    private SpriteRenderer _cardFront;
    [SerializeField]
    private SortingGroup _sortingGroup;

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
    
    public void HighlightCard()
    {
        Vector2 handPosition = _manager.GetCardHandPosition(this);

        LeanTween.scale(this.gameObject, new Vector2(OverSizeScale, OverSizeScale), 0.2f);
        LeanTween.move(this.gameObject, new Vector2(handPosition.x, handPosition.y + OverSizePosOffset), 0.2f);

        _sortingGroup.sortingOrder++;
    }
    
    public void ResetCardInHand(Action callback = null)
    {
        Vector2 handPosition = _manager.GetCardHandPosition(this);
        Vector2 handScale = _manager.GetHandCardScale();

        LeanTween.scale(this.gameObject, handScale, 0.2f);
        LeanTween.move(this.gameObject, new Vector2(handPosition.x, handPosition.y), 0.2f).setOnComplete(()=> 
        {
            callback?.Invoke();
        });

        _sortingGroup.sortingOrder = 0;
    }

    public void GrabCard()
    {
        Vector2 handScale = _manager.GetHandCardScale();
        LeanTween.scale(this.gameObject, handScale, 0.2f);
    }

    public void PlayCard(Action callback = null)
    {
        Vector3 playPosition = _manager.GetBoardPlayPosition(this);
        Vector3 playScale = _manager.GetPlayBoardCardScale();

        LeanTween.scale(this.gameObject, playScale, 1);

        Vector3 start = this.transform.position;
        Vector3 point1 = start + new Vector3(0, 2f);
        Vector3 point2 = playPosition + new Vector3(0, 2f);

        LTBezierPath path = new LTBezierPath(new Vector3[] { start, point2, point1, playPosition});

        LeanTween.move(this.gameObject, path, 0.3f).setOnComplete(() =>
        {
            callback?.Invoke();
        });

        _sortingGroup.sortingOrder = 0;
    }
}
