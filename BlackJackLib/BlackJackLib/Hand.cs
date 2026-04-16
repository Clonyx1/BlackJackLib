using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Hand
    {
        private List<Card> _cards = new List<Card>();
        public IReadOnlyCollection<Card> Cards { get { return _cards; } }
        public decimal BetAmount { get; private set; }
        public bool IsSoft { get; private set; } = false; //Set to false, because a hand without Ace is always hard
        public bool IsBusted => GetTotalValue() > 21;
        public bool HasBlackJack => GetTotalValue() == 21;
        public bool IsStanding = false;
        public bool IsFinished  => IsBusted || HasBlackJack || IsStanding;//Whether player stands/already busted

        public Hand() { }
        public Hand(decimal betAmount)
        {
            PlaceBet(betAmount);
        }
        /// <summary>
        /// Adds card into hand
        /// Sets IsFinished to true if hand has Black Jack or is busted
        /// </summary>
        /// <param name="card">Card to add into hand</param>
        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
        /// <summary>
        /// Returns true if hand has score of less than 21
        /// </summary>
        /// <returns></returns>
        public bool CanHit()
        {
            if(GetTotalValue() < 21) return true;

            return false;
        }
        /// <summary>
        /// Gives player a card from deck
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        public Result<Card> Hit(IDeck deck)
        {
            if (IsFinished) return Result<Card>.Failure("This hand is already finished");
            if (!CanHit()) return Result<Card>.Failure("Player can not hit with this hand");

            var result = deck.Draw();
            if (!result.IsSuccess) return Result<Card>.Failure("Deck is empty");

            Card card = result.Value;
            AddCard(card);

            return Result<Card>.Success(card);
        }
        /// <summary>
        /// Sets IsFinished to true
        /// </summary>
        public void Stand()
        {
            IsStanding = true;
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
