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
        public void PlaceBet_VariousValues_ShouldThrowArgumentException(decimal playerBalance, decimal betValue)
        {
            //Arrange
            Player player = new Player(playerBalance);

            //Act & Asset
            Assert.Throws<ArgumentException>(() => player.PlaceBet(betValue));
        }
    }
}
