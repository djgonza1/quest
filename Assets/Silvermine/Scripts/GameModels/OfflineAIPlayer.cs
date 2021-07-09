using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    // public class OfflineAIPlayer : IPlayer
    // {
    //     public PlayerInfo Info { get => _aiPlayer.Info; }
    //     private AIPlayer _aiPlayer;
    //     private BoardSceneManager _sceneManager;

    //     public OfflineAIPlayer(Board board, PlayerInfo info, BoardSceneManager sceneManager)
    //     {
    //         _aiPlayer = new AIPlayer(board, info);
    //         _sceneManager = sceneManager;
    //     }

    //     public void RequestCardChoice(Action<AbilityCard> onCardChosen)
    //     {
    //         AbilityCard cardChoice = _aiPlayer.ChooseCardToPlay();

    //         _sceneManager.PlayerTwoChoice = _sceneManager.EnemyHandController.GetCard(cardChoice);
    //         onCardChosen(cardChoice);

    //         PlayableCardBehaviour enemyGO = _sceneManager.EnemyHandController.GetCard(cardChoice);
    //         _sceneManager.EnemyHandController.PlayCard(enemyGO);
    //     }
    // }
}
