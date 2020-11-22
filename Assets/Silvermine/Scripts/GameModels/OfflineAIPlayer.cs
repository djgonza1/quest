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
           QueuedAction chooseCardsStart = (chooseCardEnd) =>
            {
                AbilityCard cardChoice = _aiPlayer.ChooseCardToPlay();

                _sceneManager.PlayerTwoChoice = _sceneManager.EnemyHandController.GetCard(cardChoice);
                onCardChosen(cardChoice);

                PlayableCardBehaviour enemyGO = _sceneManager.EnemyHandController.GetCard(cardChoice);
                _sceneManager.EnemyHandController.PlayCard(enemyGO, () =>
                {
                    chooseCardEnd();
                });
            };

            _sceneManager.CallbackQueue.QueuedCall(chooseCardsStart);
        }
    }
}
