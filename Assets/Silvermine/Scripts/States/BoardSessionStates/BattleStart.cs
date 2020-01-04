using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class BattleStart : SMState<BoardSessionManager>
    {
        public override void Begin()
        {
            _context.SceneManager.BoardOpen();
            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
}