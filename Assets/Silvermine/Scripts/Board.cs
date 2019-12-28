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

            CreatePlayerHands();
        }

        public PlayerInfo this[Player player]
        {
            get
            {
                switch(player)
                {
                    case Player.First:
                        return playerOne;
                    case Player.Second:
                        return playerTwo;
                    default:
                        return null;
                }
            }
        }
        
        private void CreatePlayerHands()
        {
            playerOne.Hand = new List<BaseMagicCard>()
            {
                new BaseMagicCard(CardColor.Red, 0),
                new BaseMagicCard(CardColor.Green, 0),
                new BaseMagicCard(CardColor.Blue, 0)
            };

            playerTwo.Hand = new List<BaseMagicCard>(playerOne.Hand);
        }

        //Returns a copy
        public List<BaseMagicCard> GetPlayerHand(Player player)
        {
            switch (player)
            {
                case Player.First:
                    return playerOne.Hand;
                case Player.Second:
                    return playerTwo.Hand;
                default:
                    return null;
            }
        }

        public void SetPlayerChoice(Player player, BaseMagicCard card)
        {

        }
    }

    public class PlayerInfo
    {
        public float Health;
        public List<BaseMagicCard> Hand;
        public BaseMagicCard BattleChoice;

        public PlayerInfo()
        {
            Health = 100f;
        }
    }
}