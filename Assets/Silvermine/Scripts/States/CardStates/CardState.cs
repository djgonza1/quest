using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{ 
    public abstract class CardState : SMState<CardGO>
    {
        public virtual void OnCardEnter()
        {

        }

        public virtual void OnCardExit()
        {
            
        }

        public virtual void OnCardTapDown()
        {

        }

        public virtual void OnCardTapRelease()
        {

        }
        
        public virtual void OnCardHover()
        {

        }

        public virtual void OnCardDragged()
        {

        }

        public virtual void OnCardDrag()
        {

        }
    }
}