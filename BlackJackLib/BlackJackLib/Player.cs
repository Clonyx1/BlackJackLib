using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
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
        /// <param name="deck"></param>
        /// <returns></returns>
        public Result<Card> HitActiveHand(IDeck deck)
        {
            var hand = GetActiveHand();

            if (hand == null) return Result<Card>.Failure("All hands are finished");

            return hand.Hit(deck);
        }

        public Result PlaceBet(decimal betAmount)
        {
            if (!CanPlaceBet()) return Result.Failure("Can not place a bet");

            if (betAmount <= 0) return Result.Failure("Bet must be positive");
            if (Balance < betAmount) return Result.Failure("Insufficient balance");

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
        public bool CanPlaceBet()
        {
            if (_hands.Count == 0 && playerState == PlayerState.Betting) return true;

            return false;
        }
        /// <summary>
        /// Doubles player's bet in active hand
        /// </summary>
        public Result<Hand> DoubleBet()
        {
            if(playerState != PlayerState.Playing) return Result<Hand>.Failure("Player is not in playing state");
            
            var hand = GetActiveHand();
            if (hand == null) return Result<Hand>.Failure("Player does not have any active hands");

            hand.DoubleBet();

            return Result<Hand>.Success(hand);
        }
        /// <summary>
        /// Splits player's hand into two separate hands and draws an additional card into each
        /// </summary>
        public Result<List<Hand>> Split(IDeck deck)
        {
            if (!CanSplit()) return Result<List<Hand>>.Failure("Player can not split hand");

            Hand currentHand = _hands[0];
            decimal betAmount = currentHand.BetAmount;


            List<Card> cardsInHand = currentHand.Cards.ToList();
            var card1 = cardsInHand[0];
            var card2 = cardsInHand[1];

            Hand hand1 = new Hand(betAmount);
            hand1.AddCard(card1);
            hand1.AddCard(deck.Draw());

            Hand hand2 = new Hand(betAmount);
            hand2.AddCard(card2);
            hand2.AddCard(deck.Draw());

            _hands.Clear();

            _hands.Add(hand1);
            _hands.Add(hand2);

            return Result<List<Hand>>.Success(_hands);
        }
        /// <summary>
        /// Determines whether player can split according to standard Black Jack rules
        /// (= Has one hand that contains precisely 2 cards with the same rank and has enough balance)
        /// </summary>
        /// <returns></returns>
        public bool CanSplit()
        {
            if(Hands.Count == 1 && playerState == PlayerState.Playing)
            {
                if (Balance > _hands[0].BetAmount * 2)
                {
                    var hand = _hands[0];
                    List<Card> cardsInHand = hand.Cards.ToList();

                    if (cardsInHand.Count == 2)
                    {
                        var card1 = cardsInHand[0];
                        var card2 = cardsInHand[1];

                        if (card1.Rank == card2.Rank) return true;
                    }
                }
            }

            return false;
        }
    }
}
