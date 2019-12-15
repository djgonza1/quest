using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public enum Player { First, Second };

    public class BoardSessionManager
    {
        public SMStateMachine<BoardSessionManager> StateMachine { get; }
        public IBoardSessionUserInterface SceneManager { get; }

        public BoardSessionManager(IBoardSessionUserInterface sceneManager)
        {
            SceneManager = sceneManager;

            SMState<BoardSessionManager>[] states =
            {
                new BattleStart(),
                new ChoosingPhase()
            };

            StateMachine = new SMStateMachine<BoardSessionManager>(this, states);
        }

        public bool TryPlayCard(Player player, BaseMagicCard card)
        {
            return true;
        }
    }
}
