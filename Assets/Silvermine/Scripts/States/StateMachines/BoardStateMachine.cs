using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class BoardStateMachine : SMStateMachine<BoardSessionManager>
{
    public void Begin(BoardSessionManager session)
    {
        SMState<BoardSessionManager>[] states =
        {
            new BattleStart(),
            new ChoosingPhase(),
            new BattlePhase()
        };

        Begin(session, states);
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
    #endregion
}
