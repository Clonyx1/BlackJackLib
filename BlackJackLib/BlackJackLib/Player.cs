using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Player
    {
        public Hand Hand { get;} = new Hand();
        public decimal Balance { get; private set; }
        public decimal Bet { get; private set; }

        public Player(decimal balance)
        {
            Balance = balance;
        }

        public bool CanHit()
        {
            if (Hand.GetTotalValue() < 21) return true;

            return false;
        }
        /// <summary>
        /// Returns true if player's hand is over 21
        /// </summary>
        /// <returns></returns>
        public bool IsBusted()
        {
            if (Hand.GetTotalValue() > 21) return true;

            return false;
        }
        /// <summary>
        /// Returns true if player's hand equals 21
        /// </summary>
        /// <returns></returns>
        public bool HasBlackJack()
        {
            if(Hand.GetTotalValue() == 21) return true;

            return false;
        }

        public void PlaceBet(decimal betValue)
        {
            if (betValue <= 0) throw new ArgumentException("Bet must be positive");
            if (Balance < betValue) throw new ArgumentException("Insufficient balance");

            Bet += betValue;
            Balance -= betValue;
        }
        /// <summary>
        /// Adds player's bet * multiplier to player's balance, sets bet to 0
        /// </summary>
        /// <param name="multiplier"></param>
        public void Win(decimal multiplier)
        {
            Balance += Bet * multiplier;
            Bet = 0;
        }
        /// <summary>
        /// Returns player's bet to balance, sets bet to 0
        /// </summary>
        public void Push()
        {
            Balance += Bet;
            Bet = 0;
        }
        /// <summary>
        /// Sets bet to 0
        /// </summary>
        public void Lose()
        {
            Bet = 0;
        }
    }
}
