using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public enum PlayerType { None, First, Second };

    public class BoardSessionManager
    {
        public Board GameBoard { get; private set; }
        public IPlayer PlayerOne;
        public IPlayer PlayerTwo;
        public int CurrentTurn;
        public IPlayer CurrentTurnPlayer { get; private set; }

        public BoardSessionManager(Board gameBoard, IPlayer playerOne, IPlayer playerTwo)
        {
            GameBoard = gameBoard;
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
        
        public void SetPlayerChoice(IPlayer player, AbilityCard card)
        {
            player.Info.BattleChoice = card;
            player.Info.Hand.Remove(card);
        }
    }
}
