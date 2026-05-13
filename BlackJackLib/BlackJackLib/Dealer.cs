namespace BlackJackLib
{
    /// <summary>
    /// Used to represent Dealer
    /// </summary>
    public class Dealer
    {
        private Hand Hand { get; } = new Hand();
        
        /// <summary>
        /// Dealer gets a card
        /// </summary>
        public void Hit(IDeck deck)
        {
            Hand.Hit(deck);
        }
        /// <summary>
        /// Returns true if dealer should hit
        /// </summary>
        /// <returns></returns>
        public bool ShouldHit()
        {
            if (Hand.GetTotalValue() < 17) return true;

            return false;
        }
    }
}
