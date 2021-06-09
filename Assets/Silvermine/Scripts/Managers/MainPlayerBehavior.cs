using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class MainPlayerBehavior : PlayerBehavior
{
    private PlayableCardBehaviour _cardChoice;

    public override PlayableCardBehaviour CardChoice => _cardChoice;

    protected override IEnumerator WaitForCardPlayed()
    {
        _cardChoice = null;
        Action<PlayableCardBehaviour> onCardPlayed = null;
        onCardPlayed = (card) =>
        {
            HandController.OnCardPlayed -= onCardPlayed;

            _cardChoice = card;
        };
        HandController.OnCardPlayed += onCardPlayed;

        while(_cardChoice == null) { yield return 0; }
    }
}
