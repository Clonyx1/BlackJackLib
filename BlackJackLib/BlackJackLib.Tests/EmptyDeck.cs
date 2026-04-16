using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib.Tests
{
    public class EmptyDeck : BaseDeck
    {
        protected override void Initialize()
        {
            _cards = new List<Card>();

        }
    }
}
