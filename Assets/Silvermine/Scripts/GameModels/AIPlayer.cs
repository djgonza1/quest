using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class AIPlayer : IPlayer
    {
    public PlayerInfo Info { get; }
        private Board _gameBoard;

        public AIPlayer(Board board, PlayerInfo info)
        {
            _gameBoard = board;
            Info = info;
        }

        public AbilityCard ChooseCardToPlay()
        {
            AbilityCard chosenCard = null;

            foreach (var card in Info.Hand)
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

