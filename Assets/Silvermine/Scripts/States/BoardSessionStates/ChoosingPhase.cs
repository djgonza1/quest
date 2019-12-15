using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class ChoosingPhase : SMState<BoardSessionManager>
    {
        public override void Begin()
        {
            _context.SceneManager.OnBattleStart(OnChoosingPhaseEnd);
        }

        private void OnChoosingPhaseEnd()
        {

        }
    }
}