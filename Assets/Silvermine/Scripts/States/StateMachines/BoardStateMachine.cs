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
        public override void Begin()
        {
            _context.Session.StartNextTurn();
        }

        public override IEnumerator Reason()
        {
            Debug.LogWarning("Starting ChoosingPhase");
            yield return _context.OpenChooseCardPopup();
            
            var currentPlayer = _context.CurrentPlayer;

            PlayableCardBehaviour cardBehavior = null;
            yield return currentPlayer.PlayCard((card)=> { cardBehavior = card; });

            Debug.LogWarning("After chose card in state machine");

            _context.Session.SetPlayerChoice(currentPlayer.Info, cardBehavior.Card);

            _stateMachine.ChangeState<BattlePhase>();
        }
    }

    private class BattlePhase : SMState<BoardSceneManager>
    {
        public override IEnumerator Reason()
        {
            Debug.LogWarning("Starting BattlePhase");
            yield return _context.BattlePhaseStart();

            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
    #endregion
}
