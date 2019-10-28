using HutongGames.PlayMaker;
using UnityEngine;

namespace Silvermine.Playmaker.Actions
{
    [ActionCategory("Silvermine")]
    public class GrabCard : FsmStateAction
    {
        public FsmOwnerDefault Card;

        private GameObject _go;

        public override void OnEnter()
        {
            _go = Fsm.GetOwnerDefaultTarget(Card);

            _go.GetComponent<CardObject>().GrabCard();
            //Finish();
        }

        public override void OnUpdate()
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("mouse position: " + position);
            _go.transform.position = position;
        }
    }
}