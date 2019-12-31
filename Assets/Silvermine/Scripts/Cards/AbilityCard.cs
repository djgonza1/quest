using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public enum CardColor { None, Red, Green, Blue };

    public class AbilityCard
    {
        public CardColor Color { get; private set; }
        public int Power { get; private set; }

        public AbilityCard()
        {
            Color = CardColor.None;
            Power = 0;
        }

        public AbilityCard(CardColor color, int power)
        {
            this.Color = color;
            this.Power = power;
        }

        public AbilityCard(AbilityCard card) : this(card.Color, card.Power)
        {

        }
    }
}
