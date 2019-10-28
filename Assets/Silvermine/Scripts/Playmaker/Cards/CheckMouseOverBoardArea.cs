using HutongGames.PlayMaker;
using UnityEngine;

namespace Silvermine.Playmaker.Actions
{
    public class CheckMouseOverBoardArea : FsmStateAction
    {
        public FsmEvent _onBoardEvent;

        public override void OnEnter()
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector3.forward);

            foreach (var hit in hits)
            {
                if (hit.transform.tag == "PlaySpace")
                {
                    Fsm.Event(_onBoardEvent);
                    break;
                }
            }

            Finish();
        }
    }
}