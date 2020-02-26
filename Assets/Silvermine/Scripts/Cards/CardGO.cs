using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Silvermine.Battle.Core;

public class CardGO : MonoBehaviour, ICardGO
{
    [SerializeField] public SortingGroup SortingGroup;
    [SerializeField] private SpriteRenderer _cardFront;
    [SerializeField] private SpriteRenderer _portrait;
    [SerializeField] private SpriteRenderer _cardBack;
    
    public void SetColor(CardColor color)
    {
        _cardFront.color = CardUtilities.ToColor(color);
    }

    public void FlipCard(bool showFront)
    {
        _cardFront.gameObject.SetActive(showFront);
        _portrait.gameObject.SetActive(showFront);
        _cardBack.gameObject.SetActive(!showFront);
    }

    public void SetSortingOrder(int order)
    {
        SortingGroup.sortingOrder = order;
    }

    public void Highlight(float scaleRatio)
    {
        Vector2 handPosition = gameObject.transform.position;

        LeanTween.move(gameObject, new Vector2(handPosition.x, handPosition.y + BoardSceneManager.CardOverSizePosOffset), 0.2f);
        LeanTween.scale(gameObject, new Vector2(BoardSceneManager.CardOverSizeScale, BoardSceneManager.CardOverSizeScale), 0f);
    }
}
