using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{

    public enum CardColor { None, Red, Green, Blue };

    public class BaseMagicCard
    {
        public CardColor Color { get; private set; }

        public int Damage { get; private set; }

        public BaseMagicCard()
        {
            Color = CardColor.None;
            Damage = 0;
        }

        public BaseMagicCard(CardColor color, int power)
        {
            this.Color = color;
            this.Damage = power;
        }
    }
}
