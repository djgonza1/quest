using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public enum PlayerType { None, First, Second };

    public class BoardSessionManager
    {
        public PlayerInfo PlayerOne;
        public PlayerInfo PlayerTwo;
        public int CurrentTurn;
        public PlayerInfo CurrentTurnPlayer { get; private set; }

        public BoardSessionManager(PlayerInfo playerOne, PlayerInfo playerTwo)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            CurrentTurnPlayer = PlayerOne;
            CurrentTurn = 0;
        }

        public void StartNextTurn()
        {
            CurrentTurn++;
            CurrentTurnPlayer = CurrentTurn % 2 == 0 ? PlayerTwo : PlayerOne;
        }
        
        public void SetPlayerChoice(PlayerInfo player, AbilityCard card)
        {
            player.BattleChoice = card;
            player.Hand.Remove(card);
        }
    }
}
