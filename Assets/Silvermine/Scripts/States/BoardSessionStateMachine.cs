using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class BoardSessionStateMachine
    {
        private SMStateMachine<BoardSessionManager> _stateMachine;

        public BoardSessionStateMachine(BoardSessionManager context, SMState<BoardSessionManager>[] states)
        {
            _stateMachine = new SMStateMachine<BoardSessionManager>(context, states);
        }

        public void Update()
        {
            _stateMachine.Update();
        }
    }
}