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
        public IBattleEventManager EventsManager { get; }

        public BoardSessionManager(Board gameBoard, IPlayer playerOne, IPlayer playerTwo, IBattleEventManager eventsManager)
        {
            GameBoard = gameBoard;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            EventsManager = eventsManager;
        }
    }
}
