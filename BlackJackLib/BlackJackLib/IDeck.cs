using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    /// <summary>
    /// Interface for creating decks of cards
    /// </summary>
    public interface IDeck
    {
        Result<Card> Draw();
        void Shuffle();
        int RemainingCardsCount { get; }
    }
}
