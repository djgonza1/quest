using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Silvermine.Battle.Core;

public class CardGO : MonoBehaviour, ICardGO
{
    [SerializeField] public SortingGroup SortingGroup = null;
    [SerializeField] private SpriteRenderer _cardFront = null;
    [SerializeField] private SpriteRenderer _portrait = null;
    [SerializeField] private SpriteRenderer _cardBack = null;
    
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

    public void Highlight(bool enable)
    {
        if (enable)
        {
            float scale = BoardSceneManager.CardOverSizeScale;
            Vector2 handPosition = gameObject.transform.position;
            
            gameObject.transform.localPosition = new Vector3(0f, BoardSceneManager.CardOverSizePosOffset);
            gameObject.transform.localScale = new Vector3(scale, scale);
        }
        else
        {
            LeanTween.cancel(gameObject);

            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
        }
    }
}
