using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardObjectSceneExtensions
{
    public static void HighlightCard(this BoardSceneManager manager, CardObject card)
    {
        Vector2 handPosition = manager.GetCardHandPosition(card);

        LeanTween.move(card.gameObject, new Vector2(handPosition.x, handPosition.y + BoardSceneManager.CardOverSizePosOffset), 0.2f);
        LeanTween.scale(card.gameObject, new Vector2(BoardSceneManager.CardOverSizeScale, BoardSceneManager.CardOverSizeScale), 0f);

        card.SortingGroup.sortingOrder++;
    }

    public static void ResetCardInHand(this BoardSceneManager manager, CardObject card, Action callback = null)
    {
        Vector2 handPosition = manager.GetCardHandPosition(card);
        Vector2 handScale = manager.GetHandCardScale();

        LeanTween.scale(card.gameObject, handScale, 0.2f);
        LeanTween.move(card.gameObject, new Vector2(handPosition.x, handPosition.y), 0.2f).setOnComplete(() =>
        {
            callback?.Invoke();
        });

        card.SortingGroup.sortingOrder = 0;
    }

    public static void GrabCard(this BoardSceneManager manager, CardObject card)
    {
        Vector2 handScale = manager.GetHandCardScale();
        LeanTween.scale(card.gameObject, handScale, 0.2f);
    }

    public static void PlayCard(this BoardSceneManager manage, CardObject card, Action callback = null)
    {
        Vector3 playPosition = manage.GetBoardPlayPosition(card);
        Vector3 playScale = manage.GetPlayBoardCardScale();

        LeanTween.scale(card.gameObject, playScale, 0.3f);

        Vector3 start = card.transform.position;
        Vector3 point1 = start + new Vector3(0, 2f);
        Vector3 point2 = playPosition + new Vector3(0, 2f);

        LTBezierPath path = new LTBezierPath(new Vector3[] { start, point2, point1, playPosition });

        LeanTween.move(card.gameObject, path, 0.3f).setOnComplete(() =>
        {
            callback?.Invoke();
        });

        card.SortingGroup.sortingOrder = 0;
    }
}
