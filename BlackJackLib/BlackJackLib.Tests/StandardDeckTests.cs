using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlackJackLib.Tests
{
    public class StandardDeckTests
    {
        [Fact]
        public void Initialize_ShouldFillDeckWith52Cards()
        {
            //Arrange
            StandardDeck standardDeck = new StandardDeck();
            int expectedCount = 52;

            //Act
            int remainingCardsCount = standardDeck.RemainingCardsCount;

            //Assert
            Assert.Equal(expectedCount, remainingCardsCount);
        }

        [Fact]
        public void Shuffle_Cards_ShouldBeShuffled()
        {
            //Arrange
            List<Card> unshuffledCards = new List<Card>();
            StandardDeck standardDeck = new StandardDeck(); //Should contain shuffled list of cards

            //Fill unshuffled list
            foreach(var suitIndex in Enum.GetValues(typeof(Suit)))
            {
                foreach(var cardRankIndex in Enum.GetValues(typeof(CardRank)))
                {
                    var suit = (Suit)suitIndex;
                    var rank = (CardRank)cardRankIndex;

                    unshuffledCards.Add(new Card(rank, suit));
                }
            }

            //Act
            int sameCount = 0;

            for(int i = 0; i < 1000; i++)
            {
                standardDeck.Shuffle();
                List<Card> shuffledCards = (List<Card>)standardDeck.Cards;

                bool isSame = unshuffledCards.SequenceEqual(shuffledCards);

                sameCount += isSame ? 1 : 0;
            }

            //Assert
            Assert.True(sameCount < 2);
        }
    }
}
