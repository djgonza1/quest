using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public static class CardUtilities 
    {
        public static Color ToColor(CardColor cardColor)
        {
            switch(cardColor)
            {
                case CardColor.Red:
                    return new Color(1f, 0.333f, 0.333f, 1f);
                case CardColor.Blue:
                    return new Color(0.267f, 0.525f, 1f, 1f);
                case CardColor.Green:
                    return new Color(0.333f, 1f, 0.333f, 1f);
                default:
                    return Color.white;
            }
        }
    }
}