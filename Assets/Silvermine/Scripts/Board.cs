using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Silvermine.Battle.Core
{
    public class Board
    {
        public PlayerInfo playerOne { get; private set; }
        public PlayerInfo playerTwo { get; private set; }

        public Board()
        {
            playerOne = new PlayerInfo();
            playerTwo = new PlayerInfo();
        }

        public Board(PlayerInfo playerOneInfo, PlayerInfo playerTwoInfo)
        {
            playerOne = playerOneInfo;
            playerTwo = playerTwoInfo;
        }

        public PlayerInfo this[PlayerType player]
        {
            get
            {
                switch(player)
                {
                    case PlayerType.First:
                        return playerOne;
                    case PlayerType.Second:
                        return playerTwo;
                    default:
                        return null;
                }
            }
        }

        //Returns a copy
        public List<AbilityCard> GetPlayerHand(PlayerType player)
        {
            switch (player)
            {
                case PlayerType.First:
                    return playerOne.Hand;
                case PlayerType.Second:
                    return playerTwo.Hand;
                default:
                    return null;
            }
        }

        public void SetPlayerChoice(PlayerType player, AbilityCard card)
        {
            this[player].BattleChoice = card;
            this[player].Hand.Remove(card);
        }
    }

    public class PlayerInfo
    {
        public float Health;
        public List<AbilityCard> Hand;
        public AbilityCard BattleChoice;

        public PlayerInfo()
        {
            Health = 100f;
            BattleChoice = null;

            CreateDefaultPlayerHand();
        }
        
        private void CreateDefaultPlayerHand()
        {
            Hand = new List<AbilityCard>()
            {
                new AbilityCard(CardColor.Red, 0),
                new AbilityCard(CardColor.Red, 0),
                new AbilityCard(CardColor.Green, 0),
                new AbilityCard(CardColor.Green, 0),
                new AbilityCard(CardColor.Blue, 0),
                new AbilityCard(CardColor.Blue, 0)
            };
        }
    }
}