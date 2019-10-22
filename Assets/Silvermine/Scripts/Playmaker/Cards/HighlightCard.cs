using HutongGames.PlayMaker;

namespace Silvermine.Playmaker.Actions
{
    [ActionCategory("Silvermine")]
    public class HighlightCard : FsmStateAction
    {
        public FsmOwnerDefault Card;

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(Card);

            go.GetComponent<CardObject>().HighlightCard();
            Finish();
        }
    }
}
