using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class BattleStart : SMState<BoardSessionManager>
    {
        public override void Begin()
        {
            _context.SceneManager.OnBattleStart(OnBattleStartEnd);
        }

        public void OnBattleStartEnd()
        {
            Debug.LogWarning("OnBattleStartEnd");
            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
}