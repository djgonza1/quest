using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class CardStateMachine : ICardStateMachine
    {
        private SMStateMachine<CardObject> _stateMachine;

        public CardStateMachine(CardObject context, CardState[] cardStates)
        {
            _stateMachine = new SMStateMachine<CardObject>(context, cardStates);
        }

        public void Update()
        {
            _stateMachine.Update();
        }

        public void ChangeState<S>() where S : CardState
        {
            _stateMachine.ChangeState<S>();
        }

        public void OnCardEnter()
        {
            CardState state = _stateMachine.CurrentState as CardState;
            state.OnCardEnter();
        }

        public void OnCardExit()
        {
            CardState state = _stateMachine.CurrentState as CardState;
            state.OnCardExit();
        }

        public void OnCardTapDown()
        {
            CardState state = _stateMachine.CurrentState as CardState;
            state.OnCardTapDown();
        }

        public void OnCardTapRelease()
        {
            CardState state = _stateMachine.CurrentState as CardState;
            state.OnCardTapRelease();
        }

        public void OnCardHover()
        {
            CardState state = _stateMachine.CurrentState as CardState;
            state.OnCardHover();
        }

        public void OnCardDrag()
        {
            CardState state = _stateMachine.CurrentState as CardState;
            state.OnCardDrag();
        }
    }
}
