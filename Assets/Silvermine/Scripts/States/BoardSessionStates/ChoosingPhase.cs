using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class ChoosingPhase : SMState<BoardSessionManager>
    {
        bool firstChosen;
        bool secondSchosen;

        public override void Begin()
        {
            firstChosen = false;
            secondSchosen = false;
            _context.SceneManager.ChooseCards(OnPlayerOneCardChosen, OnPlayerTwoCardChosen, OnChoosingPhaseEnd);
        }
        
        private void OnPlayerOneCardChosen(AbilityCard card)
        {
            _context.GameBoard.SetPlayerChoice(Player.First, card);
            Debug.LogWarning("OnPlayerOneCardChosen");
        }

        private void OnPlayerTwoCardChosen(AbilityCard card)
        {
            Debug.LogWarning("OnPlayerTwoCardChosen");
            _context.GameBoard.SetPlayerChoice(Player.Second, card);
        }

        private void OnChoosingPhaseEnd()
        {
            Debug.LogWarning("ChoosingPhase End");
            _stateMachine.ChangeState<BattlePhase>();
        }
    }
}