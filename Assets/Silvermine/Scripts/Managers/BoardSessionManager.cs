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
        public IBoardSceneManager SceneManager { get; }
        
        private List<AbilityCard> _playerOneHand;
        private List<AbilityCard> _playerTwoHand;

        public BoardSessionManager(IBoardSceneManager sceneManager)
        {
            SceneManager = sceneManager;

            GameBoard = new Board();
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

        private void CreatePlayerHands()
        {
            _playerOneHand = new List<AbilityCard>()
            {
                new AbilityCard(CardColor.Red, 0),
                new AbilityCard(CardColor.Green, 0),
                new AbilityCard(CardColor.Blue, 0)
            };

            _playerTwoHand = new List<AbilityCard>()
            {
                new AbilityCard(CardColor.Red, 0),
                new AbilityCard(CardColor.Green, 0),
                new AbilityCard(CardColor.Blue, 0)
            };
        }
    }
}
