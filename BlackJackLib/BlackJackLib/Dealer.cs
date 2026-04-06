namespace BlackJackLib
{
    public class Dealer : Participant
    {
        /// <summary>
        /// Returns true if dealer has to play
        /// </summary>
        /// <returns></returns>
        public override bool ShouldHit()
        {
            if (Hand.GetTotalValue() < 17) return true;

            return false;
        }
    }
}
