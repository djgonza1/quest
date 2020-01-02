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
            _context.SceneManager.ChooseCards(OnCardsChosen);
        }

        private void OnCardsChosen(AbilityCard playerOneChoice, AbilityCard playerTwoChoice)
        {
            _context.GameBoard.SetPlayerChoice(Player.First, playerOneChoice);
            _context.GameBoard.SetPlayerChoice(Player.Second, playerTwoChoice);

            _stateMachine.ChangeState<BattlePhase>();
        }
    }
}