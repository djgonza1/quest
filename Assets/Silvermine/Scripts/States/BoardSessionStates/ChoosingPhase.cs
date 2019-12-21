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
            _context.SceneManager.OnChoosingPhase(OnCardChosen, OnCardChosen);
        }
        
        private void OnCardChosen(SessionCardEvent card)
        {
            switch(card.Player)
            {
                case Player.First:
                    Debug.LogWarning("Player card chosen");
                    break;
                case Player.Second:
                    break;
            }
        }
    }
}