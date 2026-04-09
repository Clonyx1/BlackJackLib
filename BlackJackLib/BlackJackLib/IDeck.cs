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
        Card Draw();
        void Shuffle();
        int RemainingCards { get; }
    }
}
