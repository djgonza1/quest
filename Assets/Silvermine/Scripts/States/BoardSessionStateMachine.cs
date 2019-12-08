using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class BoardSessionStateMachine
    {
        private SMStateMachine<BoardSessionManager> _stateMachine;

        public BoardSessionStateMachine(BoardSessionManager _context)
        {
            _stateMachine = new SMStateMachine<BoardSessionManager>(_context, null);
        }

        public void Update()
        {
            _stateMachine.Update();
        }
    }
}