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
        public SMStateMachine<BoardSessionManager> StateMachine { get; }
        public IBoardSceneManager SceneManager { get; }
        
        private List<BaseMagicCard> _playerOneHand;
        private List<BaseMagicCard> _playerTwoHand;

        public BoardSessionManager(IBoardSceneManager sceneManager)
        {
            SceneManager = sceneManager;

            GameBoard = new Board();

            SMState<BoardSessionManager>[] states =
            {
                new BattleStart(),
                new ChoosingPhase()
            };

            StateMachine = new SMStateMachine<BoardSessionManager>(this, states);
            
        }

        private void CreatePlayerHands()
        {
            _playerOneHand = new List<BaseMagicCard>()
            {
                new BaseMagicCard(CardColor.Red, 0),
                new BaseMagicCard(CardColor.Green, 0),
                new BaseMagicCard(CardColor.Blue, 0)
            };

            _playerTwoHand = new List<BaseMagicCard>()
            {
                new BaseMagicCard(CardColor.Red, 0),
                new BaseMagicCard(CardColor.Green, 0),
                new BaseMagicCard(CardColor.Blue, 0)
            };
        }

        //Returns a copy
        public List<BaseMagicCard> GetPlayerHand(Player player)
        {
            switch(player)
            {
                case Player.First:
                    return _playerOneHand.ConvertAll(card => new BaseMagicCard(card));
                case Player.Second:
                    return _playerTwoHand.ConvertAll(card => new BaseMagicCard(card));
                default:
                    return null;
            }
        }
    }
}
