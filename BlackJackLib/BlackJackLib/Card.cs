using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    /// <summary>
    /// Represents card
    /// Contains information about Rank, Suit, card value
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card's rank
        /// </summary>
        public CardRank Rank { get; }
        /// <summary>
        /// Card's suit
        /// </summary>
        public Suit Suit { get; }

        public Card(CardRank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }
        /// <summary>
        /// Returns value of a card
        /// This method will always return the max value of a Card (Ace will always return 11 using this method)
        /// </summary>
        /// <returns></returns>
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
