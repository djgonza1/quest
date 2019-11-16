
using System;
using System.Collections.Generic;

namespace Silvermine.Battle.Core
{
    public class SMStateMachine<T>
    {
        private T _context;
        private SMState<T> _currentState;
        private Dictionary<Type, SMState<T>> _stateMap;

        public SMStateMachine(T context, SMState<T>[] states)
        {
            _stateMap = new Dictionary<Type, SMState<T>>();
            _context = context;

            foreach(var state in states)
            {
                state.SetMachineAndContext(this, _context);
                _stateMap[state.GetType()] = state;
            }

            _currentState = _stateMap[states[0].GetType()];
            _currentState.Begin();
        }

        public void Update()
        {
            _currentState.Update();
        }

        public void ChangeState<S>() where S : SMState<T>
        {
            if (!_stateMap.ContainsKey(typeof(S)))
            {
                return;
            }

            _currentState.End();
            _currentState = _stateMap[typeof(S)];
            _currentState.Begin();
        }
    }
}
    
