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
            yield return _context.OpenChooseCardPopup();

            var currentPlayer = _context.Session.CurrentTurnPlayer;

            AbilityCard chosenCard = null;
            currentPlayer.RequestCardChoice((card)=>
            {
                chosenCard = card;
            });
            while(chosenCard == null) { yield return 0; }

            _context.Session.SetPlayerChoice(currentPlayer, chosenCard);

            _stateMachine.ChangeState<BattlePhase>();
        }
    }


    private class BattlePhase : SMState<BoardSceneManager>
    {
        public override IEnumerator Reason()
        {
            yield return _context.BattlePhaseStart();

            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
    #endregion
}
