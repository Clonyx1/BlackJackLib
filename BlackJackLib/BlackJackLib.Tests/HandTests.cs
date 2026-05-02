using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
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

        [Fact]
        public void Hit_ReturnSuccess()
        {
            //Arrange
            StandardDeck deck = new StandardDeck();
            Hand hand = new Hand();
            
            //Act
            Result<Card> result = hand.Hit(deck);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Contains(result.Value, hand.Cards);
        }

        [Fact]
        public void Hit_HandStand_ReturnFailure()
        {
            //Arrange
            StandardDeck deck = new StandardDeck();
            Hand hand = new Hand();
            hand.Stand();
            //Act
            Result result = hand.Hit(deck);

            //Assert
            Assert.False(result.IsSuccess);
        }
        [Fact]
        public void Hit_HandHasBlackJack_ReturnFailure()
        {
            //Arrange
            StandardDeck deck = new StandardDeck();
            var hand = new Hand();
            hand.AddCard(new Card(CardRank.Ace, Suit.Hearts));
            hand.AddCard(new Card(CardRank.Jack, Suit.Hearts));

            //Act
            Result result = hand.Hit(deck);

            //Assert
            Assert.False(result.IsSuccess);
        }
        [Fact]
        public void Hit_HandIsBusted_ReturnFailure()
        {
            //Arrange
            StandardDeck deck = new StandardDeck();
            Hand hand = new Hand();
            hand.AddCard(new Card(CardRank.Jack, Suit.Hearts));
            hand.AddCard(new Card(CardRank.Jack, Suit.Spades));
            hand.AddCard(new Card(CardRank.Jack, Suit.Diamonds));

            //Act
            Result result = hand.Hit(deck);

            //Assert
            Assert.False(result.IsSuccess);
        }
        [Fact]
        public void Hit_EmptyDeck_ReturnFailure()
        {
            //Arranege
            EmptyDeck emptyDeck = new EmptyDeck();
            var hand = new Hand();

            //Act
            var result = hand.Hit(emptyDeck);

            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void DoubleBet_ShouldDoubleBet()
        {
            //Arrange
            var hand = new Hand(100);
            hand.PlaceBet(50);

            //Act
            hand.DoubleBet();

            //Assert
            var expectedValue = 100;
            Assert.Equal(expectedValue, hand.BetAmount);
        }
        public static IEnumerable<object[]> TestData2 =>
            new List<object[]>
            {
                new object[] {new List<Card> { new Card(CardRank.Ace, Suit.Hearts), new Card(CardRank.Ace, Suit.Diamonds), new Card(CardRank.Five, Suit.Spades) }, true },
                new object[] { new List<Card> { new Card(CardRank.King, Suit.Diamonds), new Card(CardRank.Eight, Suit.Hearts)}, false },
                new object[] { new List<Card> { new Card(CardRank.King, Suit.Spades), new Card(CardRank.Queen, Suit.Clubs), new Card(CardRank.Ace, Suit.Spades)}, false },
                new object[] {new List<Card> { new Card(CardRank.Ace, Suit.Hearts), new Card(CardRank.Ace, Suit.Clubs), new Card(CardRank.Ace, Suit.Diamonds)}, true }
            };

        [Theory]
        [MemberData(nameof(TestData2))]
        public void IsHandSoft_VariousInputs_IsSoftIsCorrectState(List<Card> cards, bool expectedState)
        {
            //Arrange
            Hand hand = new Hand();
            foreach (Card card in cards)
            {
                hand.AddCard(card);
            }

            //Act & Assert
            Assert.Equal(expectedState, hand.IsSoft);
        }
    }
}
