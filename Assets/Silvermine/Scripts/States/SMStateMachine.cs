
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class SMStateMachine<T> : MonoBehaviour
    {
        public SMState<T> CurrentState;

        private Coroutine _currentReason;
        private T _context;
        private Dictionary<Type, SMState<T>> _stateMap;

        public void Begin(T context, SMState<T>[] states)
        {
            _stateMap = new Dictionary<Type, SMState<T>>();
            _context = context;

            foreach(var state in states)
            {
                state.SetMachineAndContext(this, _context);
                _stateMap[state.GetType()] = state;
            }

            CurrentState = _stateMap[states[0].GetType()];
            CurrentState.Begin();
            _currentReason = StartCoroutine(CurrentState.Reason());
        }

        public void ChangeState<S>() where S : SMState<T>
        {
            if (!_stateMap.ContainsKey(typeof(S)))
            {
                Debug.LogError("StateMachine does not contain state: " + typeof(S).ToString());
                return;
            }

            CurrentState.End();

            CurrentState = _stateMap[typeof(S)];

            CurrentState.Begin();
            _currentReason = StartCoroutine(CurrentState.Reason());
        }
    }
}
    
