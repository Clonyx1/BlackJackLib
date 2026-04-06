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

        public static IEnumerable<object[]> TestData2 =>
            new List<object[]>
            {
                new object[] {new List<Card> { new Card(CardRank.Ace, Suit.Hearts), new Card(CardRank.Ace, Suit.Diamonds), new Card(CardRank.Five, Suit.Spades) }, true },
                new object[] { new List<Card> { new Card(CardRank.King, Suit.Diamonds), new Card(CardRank.Eight, Suit.Hearts)}, false },
                new object[] { new List<Card> { new Card(CardRank.King, Suit.Spades), new Card(CardRank.Queen, Suit.Clubs), new Card(CardRank.Ace, Suit.Spades)}, false }
            };

        [Theory]
        [MemberData(nameof(TestData2))]
        public void GetTotalValue_VariousInputs_SetIsSoftToCorrectState(List<Card> cards, bool expectedState)
        {
            //Arrange
            Hand hand = new Hand();
            foreach (Card card in cards)
            {
                hand.AddCard(card);
            }

            //Act
            hand.GetTotalValue(); //Should change IsSoft state

            //Assert
            Assert.Equal(expectedState, hand.IsSoft);
        }
    }
}
