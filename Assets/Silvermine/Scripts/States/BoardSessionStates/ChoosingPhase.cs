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
            _context.SceneManager.OnChoosingPhase(OnPlayerOneCardChosen, OnPlayerTwoCardChosen, OnChoosingPhaseEnd);
        }
        
        private void OnPlayerOneCardChosen(BaseMagicCard firstCard)
        {
            Debug.LogWarning("OnPlayerOneCardChosen");
        }

        private void OnPlayerTwoCardChosen(BaseMagicCard secondCard)
        {
            Debug.LogWarning("OnPlayerTwoCardChosen");
        }

        private void OnChoosingPhaseEnd()
        {
            Debug.LogWarning("ChoosingPhase End");
        }
    }
}