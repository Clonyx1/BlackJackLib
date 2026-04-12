namespace BlackJackLib
{
    public class Dealer
    {
        private Hand Hand { get; } = new Hand();
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
