using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class OfflineAIPlayer : IPlayer
    {
        private AIPlayer _aiPlayer;
        private BoardSceneManager _sceneManager;

        public OfflineAIPlayer(Board board, PlayerType player, BoardSceneManager sceneManager)
        {
            _aiPlayer = new AIPlayer(board, player);
            _sceneManager = sceneManager;
        }

        public void RequestCardChoice(Action<AbilityCard> onCardChosen)
        {
            var cardChosen = _aiPlayer.ChooseCardToPlay();

           QueuedAction chooseCardsStart = (chooseCardEnd) =>
            {
                AbilityCard cardChoice = null;

                cardChoice = _aiPlayer.ChooseCardToPlay();
                onCardChosen(cardChoice);

                PlayableCardBehaviour enemyGO = _sceneManager.EnemyHandMap[cardChoice].CardGO;
                _sceneManager.PlayCard(enemyGO, () =>
                {
                    Events.Instance.Raise(new CardGOEvent(CardGOEvent.EventType.PLAYED, enemyGO, PlayerType.First));
                    _sceneManager.EnemyHandMap[cardChoice].StateMachine.ChangeState<InPlay>();
                    chooseCardEnd();
                });
            };

            _sceneManager.CallbackQueue.QueuedCall(chooseCardsStart);
        }
    }
}
