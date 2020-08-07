﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class BattleStart : SMState<BoardSessionManager>
    {
        public override void Begin()
        {
            _context.EventsManager.BoardOpen();
            _stateMachine.ChangeState<ChoosingPhase>();
        }
    }
}