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
        public SMStateMachine<BoardSessionManager> StateMachine { get; private set; }
        public IBattleEventManager EventsManager { get; }

        public BoardSessionManager(Board gameBoard, IPlayer playerOne, IPlayer playerTwo, IBattleEventManager eventsManager)
        {
            GameBoard = gameBoard;
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
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

    #region Battle States
        private class BattleStart : SMState<BoardSessionManager>
        {
            public override void Begin()
            {
                _context.EventsManager.OnBoardOpen();
                _stateMachine.ChangeState<ChoosingPhase>();
            }
        }
        
        private class ChoosingPhase : SMState<BoardSessionManager>
        {
            private bool _playerOneCardChosen;
            private bool _playerTwoCardChosen;

            public override void Begin()
            {
                _playerOneCardChosen = false;
                _playerTwoCardChosen = false;

                _context.EventsManager.OnChoosingPhaseStart();

                _context.PlayerOne.RequestCardChoice(OnPlayerOneCardChosen);
                _context.PlayerTwo.RequestCardChoice(OnPlayerTwoCardChosen);
            }

            private void OnPlayerOneCardChosen(AbilityCard cardChoice)
            {
                _context.GameBoard.SetPlayerChoice(PlayerType.First, cardChoice);
                _playerOneCardChosen = true;

                if (_playerOneCardChosen && _playerTwoCardChosen)
                {
                    _stateMachine.ChangeState<BattlePhase>();
                }
            }

            private void OnPlayerTwoCardChosen(AbilityCard playerTwoChoice)
            {
                _context.GameBoard.SetPlayerChoice(PlayerType.Second, playerTwoChoice);
                _playerTwoCardChosen = true;

                if (_playerOneCardChosen && _playerTwoCardChosen)
                {
                    _stateMachine.ChangeState<BattlePhase>();
                }
            }
        }

        private class BattlePhase : SMState<BoardSessionManager>
        {
            public override void Begin()
            {
                _context.EventsManager.OnBattlePhaseStart(PlayerType.First);
                _stateMachine.ChangeState<ChoosingPhase>();
            }
        }
    }
    #endregion

}
