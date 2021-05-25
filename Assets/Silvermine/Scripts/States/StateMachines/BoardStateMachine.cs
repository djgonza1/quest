using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silvermine.Battle.Core;

public class BoardStateMachine : SMStateMachine<BoardSceneManager>
{
    [SerializeField] private BoardSceneManager _manager;

    public void Init()
    {
        SMState<BoardSceneManager>[] states =
        {
            new BattleStart(),
            new ChoosingPhase(),
            new BattlePhase()
        };

        Begin(_manager, states);
    }

#region Battle States
    private class BattleStart : SMState<BoardSceneManager>
    {
        public override IEnumerator Reason()
        {
            yield return _context.OpenBoard();

            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
    
    private class ChoosingPhase : SMState<BoardSceneManager>
    {
        private bool _playerOneCardChosen;
        private bool _playerTwoCardChosen;

        public override void Begin()
        {
            _playerOneCardChosen = false;
            _playerTwoCardChosen = false;

            _context.OnChoosingPhaseStart();

            _context.Player.RequestCardChoice(OnPlayerOneCardChosen);
            _context.Enemy.RequestCardChoice(OnPlayerTwoCardChosen);
        }

        private void OnPlayerOneCardChosen(AbilityCard cardChoice)
        {
            _context.Session.GameBoard.SetPlayerChoice(PlayerType.First, cardChoice);
            _playerOneCardChosen = true;

            if (_playerOneCardChosen && _playerTwoCardChosen)
            {
                _stateMachine.ChangeState<BattlePhase>();
            }
        }

        private void OnPlayerTwoCardChosen(AbilityCard playerTwoChoice)
        {
            _context.Session.GameBoard.SetPlayerChoice(PlayerType.Second, playerTwoChoice);
            _playerTwoCardChosen = true;

            if (_playerOneCardChosen && _playerTwoCardChosen)
            {
                _stateMachine.ChangeState<BattlePhase>();
            }
        }
    }

    private class BattlePhase : SMState<BoardSceneManager>
    {
        public override void Begin()
        {
            _context.OnBattlePhaseStart(PlayerType.First);
            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
    #endregion
}
