using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Card
    {
        public CardRank Rank { get; }
        public Suit Suit { get; }

        public Card(CardRank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public int GetCardValue()
        {
            return Rank switch
            {
                CardRank.Jack or CardRank.Queen or CardRank.Queen => 10,
                CardRank.Ace => 11,
                _ => (int)Rank + 2 //Ex. Three is index 1 in enum, so 1 + 2 == 3
            };
        }
    }
}
