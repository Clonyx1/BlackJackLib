using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BlackJackLib.Tests
{
    public class PlayerTests
    {
        [Theory]
        [InlineData(150, 150.7)]
        [InlineData(150, -15)]
        public void PlaceBet_VariousValues_ShouldReturnFailure(decimal playerBalance, decimal betValue)
        {
            //Arrange
            Player player = new Player(playerBalance);

            //Act
            var result = player.PlaceBet(betValue);

            //Assert
            Assert.False(result.IsSuccess);
        }
        [Fact]
        public void DoubleBet_ShouldDoubleBet()
        {
            //Arrange
            Player player = new Player(100);
            player.PlaceBet(50);

            //Act
            var result = player.DoubleBet();
            Hand hand = result.Value;

            //Assert
            Assert.True(result.IsSuccess);
            var expectedValue = 100;
            Assert.Equal(expectedValue, hand.BetAmount);
        }
    }
}
