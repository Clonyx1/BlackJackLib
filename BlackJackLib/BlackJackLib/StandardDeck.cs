using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class StandardDeck : BaseDeck
    {
        protected override void Initialize()
        {
            foreach(var suitIndex in Enum.GetValues(typeof(Suit)))
            {
                foreach(var cardRankIndex in Enum.GetValues(typeof(CardRank))){
                    Suit suit = (Suit)suitIndex;
                    CardRank rank = (CardRank)cardRankIndex;

                    Card card = new Card(rank, suit);
                    _cards.Add(card);
                }
            }
        }
    }
}
