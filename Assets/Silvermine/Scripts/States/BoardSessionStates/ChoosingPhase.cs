using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class ChoosingPhase : SMState<BoardSessionManager>
    {
        public override void Begin()
        {
            _context.SceneManager.OnChoosingPhase();
            Events.Instance.AddListener<SessionCardEvent>(OnCardEvent);
        }

        public override void End()
        {
            Events.Instance.RemoveListener<SessionCardEvent>(OnCardEvent);
        }

        private void OnCardEvent(SessionCardEvent msg)
        {
            if (msg.Type == SessionCardEvent.EventType.PLAYED)
            {
                
            }
        }

        

        private void OnChoosingPhaseEnd()
        {
            //TODO - add next state logic
        }
    }
}