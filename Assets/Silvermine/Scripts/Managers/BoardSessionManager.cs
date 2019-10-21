using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public enum Player { First, Second };

    public class BoardSessionManager
    {
        public BoardSessionManager()
        {

        }

        public bool TryPlayCard(Player player, BaseMagicCard card)
        {
            return true;
        }
    }
}
