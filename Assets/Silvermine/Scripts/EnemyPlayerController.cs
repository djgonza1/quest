using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class EnemyPlayerController
    {
        private Board _gameBoard;
        private PlayerInfo _info;

        public EnemyPlayerController(Board board, Player player)
        {
            _gameBoard = board;
            _info = _gameBoard[player];
        }

        public AbilityCard ChooseCardToPlay()
        {
            AbilityCard chosenCard = null;

            foreach (var card in _info.Hand)
            {
                if (card != null)
                {
                    chosenCard = card;
                    break;
                }
            }

            return chosenCard;
        }
    }
}

