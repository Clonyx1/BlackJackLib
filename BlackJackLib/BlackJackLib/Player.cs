using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Player
    {
        private List<Hand> _hands = new();
        public IReadOnlyCollection<Hand> Hands { get { return _hands; } }
        private decimal Balance;
        private PlayerState playerState = PlayerState.Betting;

        public Player(decimal balance)
        {
            Balance = balance;
        }
        /// <summary>
        /// Returns true if player has score of less than 21 in at least one hand
        /// </summary>
        /// <returns></returns>
        public bool CanHit()
        {
            foreach (var hand in _hands)
            {
                if (hand.GetTotalValue() < 21) return true;
            }

            return false;
        }

        public void PlaceBet(decimal betAmount)
        {
            if(_hands.Count == 0 && playerState == PlayerState.Betting)
            {
                if (betAmount <= 0) throw new ArgumentException("Bet must be positive");
                if (Balance < betAmount) throw new ArgumentException("Insufficient balance");

                Hand hand = new Hand();
                hand.PlaceBet(betAmount);
                _hands.Add(hand);
                Balance -= betAmount;
                playerState = PlayerState.Playing;
            }
            else
            {
                throw new InvalidOperationException("Can not place a bet if player has more than one hand");
            }
        }
        /// <summary>
        /// Doubles player's bet
        /// </summary>
        public void DoubleBet()
        {
            if(_hands.Count == 1 && playerState == PlayerState.Playing)
            {
                _hands[0].DoubleBet();
            }
        }
    }
}
