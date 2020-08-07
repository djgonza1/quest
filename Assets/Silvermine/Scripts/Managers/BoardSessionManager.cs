using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public enum Player { None, First, Second };

    public class BoardSessionManager
    {
        public Board GameBoard { get; private set; }
        public SMStateMachine<BoardSessionManager> StateMachine { get; private set; }
        public IBattleEventManager EventsManager { get; }

        public BoardSessionManager(Board gameBoard, IBattleEventManager eventsManager)
        {
            GameBoard = gameBoard;
            EventsManager = eventsManager;
        }

        public void StartSession()
        {
            SMState<BoardSessionManager>[] states =
            {
                new BattleStart(),
                new ChoosingPhase(),
                new BattlePhase()
            };

            StateMachine = new SMStateMachine<BoardSessionManager>(this, states);
        }

        public void Update()
        {
            StateMachine.Update();
        }
    }
}
