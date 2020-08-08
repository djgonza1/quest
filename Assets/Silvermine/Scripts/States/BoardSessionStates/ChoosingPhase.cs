using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class ChoosingPhase : SMState<BoardSessionManager>
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
}