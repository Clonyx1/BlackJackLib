using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    /// <summary>
    /// Contains all Black Jack hand logic
    /// </summary>
    public class Hand
    {
        private List<Card> _cards = new List<Card>();
        /// <summary>
        /// Collection of cards in hand
        /// </summary>
        public IReadOnlyCollection<Card> Cards { get { return _cards; } }
        /// <summary>
        /// Hand bet amount
        /// </summary>
        public decimal BetAmount { get; private set; }
        /// <summary>
        /// Determines whether hand is soft
        /// </summary>
        public bool IsSoft => IsHandSoft();
        /// <summary>
        /// Determines whether hand has performed any kind of action
        /// </summary>
        public bool HasPerformedAction { get; private set; } = false;
        /// <summary>
        /// Keeps track of whether hand was doubled
        /// </summary>
        public bool WasDoubled { get; private set; } = false;
        /// <summary>
        /// Set to true for Hands that are derived from splitting hand
        /// </summary>
        public bool IsResultOfSplit { get; private set; }
        /// <summary>
        /// Keeps track of whether hand has more than 21 total value
        /// </summary>
        public bool IsBusted => GetTotalValue() > 21;
        /// <summary>
        /// True if Hand has 21 total value, but consists of more than 2 cards
        /// Note that HasTwentyOne will be false when Hand has a Black Jack
        /// </summary>
        public bool HasTwentyOne => GetTotalValue() == 21 && _cards.Count > 2;
        /// <summary>
        /// True if Hand has 21 total value and consists of exactly 2 cards
        /// </summary>
        public bool HasBlackJack => GetTotalValue() == 21 && _cards.Count == 2;
        /// <summary>
        /// Keeps track of whether Hand is standing
        /// </summary>
        public bool IsStanding { get; private set; } = false;
        /// <summary>
        /// Keeps track of whether Hand is standing
        /// </summary>
        public bool IsSurrendered { get; private set; } = false;
        /// <summary>
        /// Used to quickly determine whether it is possible to play with Hand
        /// </summary>
        public bool IsFinished  => IsBusted || HasBlackJack || HasTwentyOne || IsStanding || WasDoubled;

        public Hand(bool isResultOfSplit = false) 
        { 
            this.IsResultOfSplit = isResultOfSplit;
        }
        public Hand(decimal betAmount, bool isResultOfSplit = false)
        {
            this.IsResultOfSplit = isResultOfSplit;
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
        /// Gives player a card from deck
        /// </summary>
        /// <param name="deck">Deck to draw card from</param>
        /// <returns></returns>
        public Result<Card> Hit(IDeck deck)
        {
            var validation = ValidateHit();
            if (validation.IsFailure) return (Result<Card>)validation;

            var result = deck.Draw();
            if (result.IsFailure) return result;

            Card card = result.Value;
            AddCard(card);

            HasPerformedAction = true;
            return Result<Card>.Success(card);
        }
        /// <summary>
        /// Checks if hit is possible with this hand
        /// </summary>
        /// <returns></returns>
        public Result ValidateHit()
        {
            if (IsFinished) return Result.Failure("Hand is finished");

            return Result.Success();
        }
        /// <summary>
        /// Sets IsFinished to true
        /// </summary>
        public void Stand()
        {
            HasPerformedAction = true;
            IsStanding = true;
        }
        /// <summary>
        /// Sets IsSurrendered to true
        /// </summary>
        public void Surrender()
        {
            HasPerformedAction = true;
            IsSurrendered = true;
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
                totalValue += card.GetCardValue();
                if (card.Rank == CardRank.Ace) aceCount++;
            }


            if(totalValue > 21 && aceCount > 0)
            {
                for(int i = 1; i <= aceCount && totalValue > 21; i++)
                {
                    totalValue -= 10;
                }
            }

            return totalValue;
        }
        /// <summary>
        /// Method to determine whether hand is soft
        /// </summary>
        /// <returns></returns>
        private bool IsHandSoft()
        {
            int totalValue = 0;
            int aceCount = 0;

            foreach (Card card in _cards)
            {
                totalValue += card.GetCardValue();
                if (card.Rank == CardRank.Ace) aceCount++;
            }

            while(totalValue > 21 && aceCount > 0)
            {
                totalValue -= 10;
                aceCount--;
            }

            return aceCount > 0;
        }

        /// <summary>
        /// Sets BetAmount to a given number
        /// </summary>
        /// <param name="betAmount"></param>
        /// <exception cref="ArgumentException">Bet has to be positive number</exception>
        public void PlaceBet(decimal betAmount)
        {
            if (betAmount <= 0) throw new ArgumentException("Bet must be positive");

            BetAmount += betAmount;
        }
        /// <summary>
        /// Doubles BetAmount, draws a card and finishes hand
        /// </summary>
        public void DoubleDown(IDeck deck)
        {
            HasPerformedAction = true;
            WasDoubled = true;
            BetAmount *= 2;
            Hit(deck);
        }
        /// <summary>
        /// Returns BetAmount * multiplier
        /// Multiplier is set to 2 by default
        /// </summary>
        /// <param name="multiplier"></param>
        public decimal Win(decimal multiplier = 2.0m)
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
