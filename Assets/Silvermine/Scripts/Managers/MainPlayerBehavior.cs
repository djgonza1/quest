using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class MainPlayerBehavior : PlayerBehavior
{
    [SerializeField] private CardFocusHolder _focusHolder;

    private PlayableCardBehaviour _cardChoice;

    public override PlayableCardBehaviour CardChoice => _cardChoice;

    public override IEnumerator PlayCard(Action<PlayableCardBehaviour> callback = null)
    {
        _cardChoice = null;

        //Wait for player to chose a card to focus
        PlayableCardBehaviour cardchosenForFocus = null;
        yield return ChooseCardToFocus((card)=> cardchosenForFocus = card);

        yield return _focusHolder.FocusCardAndChoseTargets((card)=>
        {
            //TODO - set chosen card and targets here;
        });

        Debug.LogWarning("Choosing a card...");
        while(_cardChoice == null) { yield return 0; }
        Debug.LogWarning("CardChosen");
        callback?.Invoke(CardChoice);
    }

    private IEnumerator ChooseCardToFocus(Action<PlayableCardBehaviour> callback)
    {
        PlayableCardBehaviour choiceForFocus = null;
        Action<PlayableCardBehaviour> onCardPlayed = null;
        onCardPlayed = (card) =>
        {
            HandController.OnCardPlayed -= onCardPlayed;

            choiceForFocus = card;
        };
        HandController.OnCardPlayed += onCardPlayed;

        Debug.LogWarning("Choosing a card...");
        while(choiceForFocus == null) { yield return 0; }

        callback(choiceForFocus);
    }
}
