using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    /// <summary>
    /// Used to represent Player
    /// </summary>
    public class Player
    {
        private List<Hand> _hands = new();
        public IReadOnlyCollection<Hand> Hands { get { return _hands; } }
        private decimal Balance;
        private PlayerState playerState = PlayerState.Betting;

        public Player(decimal balance)
        {
            Balance = balance;
        }
        /// <summary>
        /// Returns first hand that is not finished
        /// </summary>
        /// <returns></returns>
        public Hand GetActiveHand()
        {
            return _hands.FirstOrDefault(h => !h.IsFinished);
        }
        /// <summary>
        /// Player gets a card from deck
        /// </summary>
        /// <param name="deck">Deck to take card from</param>
        /// <returns></returns>
        public Result<Card> HitActiveHand(IDeck deck)
        {
            var validation = ValidateHit();

            if (validation.IsFailure) return (Result<Card>)validation;

            var hand = GetActiveHand();

            return hand.Hit(deck);
        }
        /// <summary>
        /// Determines whether hit is possible
        /// </summary>
        /// <returns></returns>
        public Result ValidateHit()
        {
            var hand = GetActiveHand();
            if (hand == null) return Result.Failure("All hands are finished");

            var validation = hand.ValidateHit();
            if (validation.IsFailure) return validation;

            return Result.Success();
        }
        /// <summary>
        /// Creates new hand, assigns a bet of given amount to hand and adds hand into hands list
        /// </summary>
        /// <param name="betAmount"></param>
        /// <returns></returns>
        public Result PlaceBet(decimal betAmount)
        {
            var validation = ValidatePlaceBet(betAmount);

            if (validation.IsFailure) return validation;

            Hand hand = new Hand();
            hand.PlaceBet(betAmount);
            _hands.Add(hand);
            Balance -= betAmount;
            playerState = PlayerState.Playing;

            return Result.Success();
        }
        /// <summary>
        /// Determines whether player can place a bet
        /// </summary>
        /// <returns></returns>
        public Result ValidatePlaceBet(decimal betAmount)
        {
            if (betAmount <= 0) return Result.Failure("Bet must be positive");
            if (Balance < betAmount) return Result.Failure("Insufficient balance");

            return Result.Success();
        }
        /// <summary>
        /// Doubles player's bet in active hand, gives player one final card and finishes Hand
        /// </summary>
        public Result<Hand> DoubleDown(IDeck deck)
        {
            var validation = ValidateDoubleDown();
            
            if(validation.IsFailure) return (Result<Hand>)validation;

            var hand = GetActiveHand();
            hand.DoubleDown(deck);

            return Result<Hand>.Success(hand);
        }
        /// <summary>
        /// Validates whether player can use DoubleDown method
        /// </summary>
        /// <returns></returns>
        public Result ValidateDoubleDown()
        {
            var hand = GetActiveHand();
            if (hand == null) return Result.Failure("Player does not have any active hands");
            if (hand.BetAmount > Balance) return Result.Failure("Insufficient funds");
            if (playerState != PlayerState.Playing) return Result.Failure("Player is not playing");
            if (hand.WasDoubled) return Result.Failure("Hand was already doubled");

            return Result.Success();
        }
        /// <summary>
        /// Makes player surrender
        /// </summary>
        /// <returns></returns>
        public Result Surrender()
        {
            var validation = ValidateSurrender();

            if (validation.IsFailure) return validation;

            foreach(var hand in _hands)
            {
                hand.Surrender();
            }

            return Result.Success();
        }
        /// <summary>
        /// Determines whether player can surrender
        /// </summary>
        /// <returns></returns>
        public Result ValidateSurrender()
        {
            if (_hands.Count > 1) return Result.Failure("Can not surrender after split");
            var hand = GetActiveHand();
            if (hand.Cards.Count > 2) return Result.Failure("Can not surrender with more than 2 cards");
            if (hand.HasPerformedAction) return Result.Failure("Can not surrender after performing action");

            return Result.Success();
        }
        /// <summary>
        /// Splits player's hand into two separate hands and draws an additional card into each
        /// </summary>
        public Result<List<Hand>> Split(IDeck deck)
        {
            var validation = ValidateSplit();
            if (validation.IsFailure) return (Result<List<Hand>>)validation;

            Hand currentHand = _hands[0];
            decimal betAmount = currentHand.BetAmount;


            List<Card> cardsInHand = currentHand.Cards.ToList();
            var card1 = cardsInHand[0];
            var card2 = cardsInHand[1];

            var result = CreateSplitHand(card1, betAmount, deck);
            if (result.IsFailure) return Result<List<Hand>>.Failure(result.ErrorMessage);
            var hand1 = result.Value;

            var result2 = CreateSplitHand(card2, betAmount, deck);
            if(result2.IsFailure) return Result<List<Hand>>.Failure(result2.ErrorMessage);
            var hand2 = result2.Value;

            _hands.Clear();

            _hands.Add(hand1);
            _hands.Add(hand2);

            return Result<List<Hand>>.Success(_hands);
        }
        /// <summary>
        /// Helper method for Split()
        /// Creates hand from given parameters
        /// </summary>
        /// <returns></returns>
        private Result<Hand> CreateSplitHand(Card card, decimal betAmount, IDeck deck)
        {
            Hand hand = new Hand(betAmount, true); //set is result of split to true
            hand.AddCard(card);

            var result = deck.Draw();
            if (result.IsFailure) return Result<Hand>.Failure("Deck is empty");

            var drawnCard = result.Value;
            hand.AddCard(drawnCard);

            return Result<Hand>.Success(hand);
        }
        /// <summary>
        /// Determines whether player can split according to standard Black Jack rules
        /// (= Has one hand that contains precisely 2 cards with the same value and has enough balance)
        /// (This method allows split based on value => for example King and Queen split is possible)
        /// </summary>
        /// <returns></returns>
        public Result ValidateSplit()
        {
            if (Hands.Count > 1) return Result.Failure("Can not split with more than 1 hand");
            if (playerState != PlayerState.Playing) return Result.Failure("Player is not playing");
            var hand = GetActiveHand();
            if (Balance < hand.BetAmount) return Result.Failure("Insufficient balance");

            var cardsInHand = hand.Cards.ToList();

            if (cardsInHand.Count < 2) return Result.Failure("Can not split with less than 2 cards in hand");
            if (cardsInHand.Count > 2) return Result.Failure("Can not split with more than 2 cards in hand");

            var card1 = cardsInHand[0];
            var card2 = cardsInHand[1];
            if (card1.GetCardValue != card2.GetCardValue) return Result.Failure("Can not split hand with 2 cards of different values");

            return Result.Success();
        }
        /// <summary>
        /// Makes active hand finished
        /// </summary>
        public void Stand()
        {
            var hand = GetActiveHand();
            if (hand != null)
            {
                hand.Stand();
            }
        }
    }
}
