using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    /// <summary>
    /// Standard Black Jack deck logic
    /// </summary>
    public abstract class BaseDeck : IDeck
    {
        protected List<Card> _cards = new List<Card>();
        public int RemainingCards => _cards.Count;

        protected BaseDeck()
        {
            Initialize();
        }
        protected abstract void Initialize();

        /// <summary>
        /// Returns a card from top of the deck and then removes it from the deck
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Card Draw()
        {
            if (RemainingCards == 0) throw new InvalidOperationException("Balíček je prázdný");

            Card card = _cards[0];
            _cards.RemoveAt(0);

            return card;
        }
        /// <summary>
        /// Shuffles cards in deck
        /// </summary>
        public void Shuffle()
        {
            Random r = new Random();

            _cards = _cards.OrderBy(c => r.Next()).ToList();
        }
    }
}
