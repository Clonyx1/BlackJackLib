using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public abstract class Participant
    {
        public Hand Hand { get; } = new Hand();

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
            if (Hand.GetTotalValue() == 21) return true;

            return false;
        }
    }
}
