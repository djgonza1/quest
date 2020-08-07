using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class BattlePhase : SMState<BoardSessionManager>
    {
        public override void Begin()
        {
            _context.EventsManager.StartBattlePhase(Player.First);
            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
}