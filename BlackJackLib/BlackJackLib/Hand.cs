using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Hand
    {
        private List<Card> Cards = new List<Card>();
        public bool IsSoft { get; private set; } = false; //Set to false, because a hand without Ace is always hard

        public Hand() { }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        /// <summary>
        /// Returns total value of player's hand
        /// </summary>
        /// <returns></returns>
        public int GetTotalValue()
        {
            int totalValue = 0;
            int aceCount = 0;

            foreach (Card card in Cards)
            {
                if(card.Rank == CardRank.Ace)
                {
                    aceCount++;
                }
                totalValue += card.GetCardValue();
            }

            if (aceCount > 0) IsSoft = true;

            if(totalValue > 21 && aceCount > 0)
            {
                for(int i = 1; i <= aceCount && totalValue > 21; i++)
                {
                    totalValue -= 10;
                    if (i == aceCount) IsSoft = false;
                }
            }

            return totalValue;
        }
    }
}
