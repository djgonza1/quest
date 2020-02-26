using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public static class CardObjectSceneExtensions
{
    public static void ResetCardInHand(this BoardSceneManager manager, PlayableCardGO card, Action callback = null)
    {
        Vector2 handPosition = manager.GetCardHandPosition(card);
        Vector2 handScale = manager.GetHandCardScale();

        LeanTween.scale(card.gameObject, handScale, 0.2f);
        LeanTween.move(card.gameObject, new Vector2(handPosition.x, handPosition.y), 0.2f).setOnComplete(() =>
        {
            callback?.Invoke();
        });

        (card as ICardGO).SetSortingOrder(0);
    }

    public static void GrabCard(this BoardSceneManager manager, PlayableCardGO card)
    {
        Vector2 handScale = manager.GetHandCardScale();
        LeanTween.scale(card.gameObject, handScale, 0.2f);
    }

    public static void PlayCard(this BoardSceneManager manager, PlayableCardGO card, Action callback = null)
    {
        Vector3 playPosition = manager.GetBoardPlayPosition(card);
        Vector3 playScale = manager.GetPlayBoardCardScale();

        LeanTween.scale(card.gameObject, playScale, 0.3f);

        Vector3 start = card.transform.position;
        Vector3 point1 = start + new Vector3(0, 2f);
        Vector3 point2 = playPosition + new Vector3(0, 2f);

        LTBezierPath path = new LTBezierPath(new Vector3[] { start, point2, point1, playPosition });

        LeanTween.move(card.gameObject, path, 0.3f).setOnComplete(() =>
        {
            callback?.Invoke();
        });

        (card as ICardGO).SetSortingOrder(0);
    }
}
