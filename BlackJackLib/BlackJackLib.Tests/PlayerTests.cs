using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BlackJackLib.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void PlaceBet_ShouldChangeBalanceAndSetBet()
        {
            //Arrange
            decimal playerBalance = 150;
            decimal betValue = 45.5m;

            decimal expectedBalance = 104.5m;
            decimal expectedBet = 45.5m;

            Player player = new Player(playerBalance);

            //Act
            player.PlaceBet(betValue);

            //Assert
            Assert.Equal(expectedBalance, player.Balance);
            Assert.Equal(expectedBet, player.Bet);
        }

        [Theory]
        [InlineData(150, 150.7)]
        [InlineData(150, -15)]
        public void PlaceBet_VariousValues_ShouldThrowArgumentException(decimal playerBalance, decimal betValue)
        {
            //Arrange
            Player player = new Player(playerBalance);

            //Act & Asset
            Assert.Throws<ArgumentException>(() => player.PlaceBet(betValue));
        }
    }
}
