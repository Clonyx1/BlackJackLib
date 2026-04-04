using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib.Tests
{
    public class HandTests
    {
        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { new List<Card> { new Card(CardRank.Ace, Suit.Hearts), new Card(CardRank.Ace, Suit.Diamonds), new Card(CardRank.Ace, Suit.Spades), new Card(CardRank.Ace, Suit.Clubs) }, 14 },
                new object[] { new List<Card> { new Card(CardRank.Three, Suit.Hearts), new Card(CardRank.Jack, Suit.Diamonds) }, 13 },
                new object[] { new List<Card> { new Card(CardRank.Ace, Suit.Hearts), new Card(CardRank.Jack, Suit.Hearts), new Card(CardRank.Three, Suit.Diamonds)}, 14 }
            };


        [Theory]
        [MemberData(nameof(TestData))]
        public void GetTotalValue_VariousInputs_ReturnCorrectValues(List<Card> cards, int expectedValue)
        {
            //Arrange
            Hand hand = new Hand();
            foreach (Card card in cards)
            {
                hand.AddCard(card);
            }

            //Act
            int totalValue = hand.GetTotalValue();

            //Assert
            Assert.Equal(expectedValue, totalValue);
        }
    }
}
