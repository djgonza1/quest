using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class EnemyBehavior : PlayerBehavior
{
    private PlayableCardBehaviour _cardChoice;

    public override PlayableCardBehaviour CardChoice => _cardChoice;

    public override IEnumerator PlayCard(Action<PlayableCardBehaviour> callback = null)
    {
        _cardChoice = null;

        foreach (var card in Info.Hand)
        {
            if (card != null)
            {
                _cardChoice = HandController.GetCard(card);
                break;
            }
        }

        bool wait = true;
        HandController.PlayCard(_cardChoice, () => 
        {
            wait = false;
        });
        while (wait) { yield return 0; }

        yield return _cardChoice;
        
        callback(CardChoice);
    }
}
