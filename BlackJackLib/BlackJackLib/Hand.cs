using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Hand
    {
        private List<Card> Cards = new List<Card>();

        public Hand() { }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

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

            if(totalValue > 21 && aceCount > 0)
            {
                for(int i = 0; i < aceCount && totalValue > 21; i++)
                {
                    totalValue -= 10;
                }
            }

            return totalValue;
        }
    }
}
