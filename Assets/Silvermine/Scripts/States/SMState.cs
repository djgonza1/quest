using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public abstract class SMState<T>
    {
        protected SMStateMachine<T> _stateMachine;
        protected T _context;
        
        public virtual void Begin()
        {

        }

        public virtual IEnumerator Reason()
        {
            yield break;
        }
        
        public virtual void End()
        {

        }

        public void SetMachineAndContext(SMStateMachine<T> stateMachine, T context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }
    }
}