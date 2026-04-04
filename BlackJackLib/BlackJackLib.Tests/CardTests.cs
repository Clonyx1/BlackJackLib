using BlackJackLib;

namespace BlackJackLib.Tests
{
    public class CardTests
    {
        [Theory]
        [InlineData(CardRank.Ace, 11)]
        [InlineData(CardRank.Two, 2)]
        [InlineData(CardRank.Jack, 10)]
        public void GetCardValue_VariousRanks_ReturnCorrectValues(CardRank rank, int expectedValue)
        {
            //Arrange
            var card = new Card(rank, Suit.Hearts);

            //Act
            var value = card.GetCardValue();

            //Assert
            Assert.Equal(expectedValue, value);
        }
    }
}
