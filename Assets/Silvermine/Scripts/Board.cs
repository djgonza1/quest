﻿using System.Collections;
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
            playerOne.Hand = new List<AbilityCard>()
            {
                new AbilityCard(CardColor.Red, 0),
                new AbilityCard(CardColor.Green, 0),
                new AbilityCard(CardColor.Blue, 0)
            };

            playerTwo.Hand = playerOne.Hand.ConvertAll(card => new AbilityCard(card));
        }

        //Returns a copy
        public List<AbilityCard> GetPlayerHand(Player player)
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

        public void SetPlayerChoice(Player player, AbilityCard card)
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
        }
    }
}