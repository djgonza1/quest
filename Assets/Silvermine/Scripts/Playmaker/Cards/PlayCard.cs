using HutongGames.PlayMaker;

namespace Silvermine.Playmaker.Actions
{
    public class PlayCard : FsmStateAction
    {
        public FsmOwnerDefault Card;

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(Card);

            go.GetComponent<CardObject>().PlayCard(Finish);
        }
    }
}
