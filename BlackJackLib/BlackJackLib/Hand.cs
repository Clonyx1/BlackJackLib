using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Hand
    {
        private List<Card> _cards = new List<Card>();
        public decimal BetAmount { get; private set; }
        public bool IsSoft { get; private set; } = false; //Set to false, because a hand without Ace is always hard

        public Hand() { }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
        /// <summary>
        /// Returns total value of hand
        /// </summary>
        /// <returns></returns>
        public int GetTotalValue()
        {
            int totalValue = 0;
            int aceCount = 0;

            foreach (Card card in _cards)
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
        /// <summary>
        /// Returns true if hand is over 21
        /// </summary>
        /// <returns></returns>
        public bool IsBusted()
        {
            if (GetTotalValue() > 21) return true;

            return false;
        }
        /// <summary>
        /// Returns true if hand equals 21
        /// </summary>
        /// <returns></returns>
        public bool HasBlackJack()
        {
            if (GetTotalValue() == 21) return true;

            return false;
        }

        /// <summary>
        /// Sets BetAmount to a given number
        /// </summary>
        /// <param name="betAmount"></param>
        /// <exception cref="ArgumentException"></exception>
        public void PlaceBet(decimal betAmount)
        {
            if (betAmount <= 0) throw new ArgumentException("Bet must be positive");

            BetAmount += betAmount;
        }
        /// <summary>
        /// Doubles BetAmount
        /// </summary>
        public void DoubleBet()
        {
            BetAmount *= 2;
        }
        /// <summary>
        /// Returns BetAmount * multiplier
        /// </summary>
        /// <param name="multiplier"></param>
        public decimal Win(decimal multiplier)
        {
            return BetAmount * multiplier;
        }
        /// <summary>
        /// Returns BetAmount
        /// </summary>
        public decimal Push()
        {
            return BetAmount;
        }
    }
}
