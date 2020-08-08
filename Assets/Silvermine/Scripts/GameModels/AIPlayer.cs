using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class AIPlayer : IPlayer
    {
        private Board _gameBoard;
        private PlayerInfo _info;

        public AIPlayer(Board board, PlayerType player)
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

        public void RequestCardChoice(Action<AbilityCard> onCardChosen)
        {
            var cardChosen = ChooseCardToPlay();

            onCardChosen?.Invoke(cardChosen);
        }
    }
}

