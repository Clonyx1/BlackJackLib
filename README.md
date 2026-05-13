# Black Jack Library
Simple library written in C# containing Black Jack logic

## Functionality
Library contains all the necessary tools to make a Black Jack game

Below is a brief explanation of classes and their capablities:

### Core classes

* **Player** - Represents Player. Handles player actions such as: `PlaceBet`, `HitActiveHand`, `Split`, `DoubleDown`, `Surrender` and `Stand`
* **Dealer** - Manages dealer-specific logic, including automatic hitting rules (e.g., hit on soft 17)
* **Hand** - Represents a collection of cards. Contains logic for calculating values, determining if a hand is "soft", and tracking its state (e.g., `IsFinished` attribute)
* **Card** - Simple class for representing card's rank, suit and value
* **IDeck** - Interface for deck creation
* **BaseDeck** - Premade abstract class with standard Black Jack functionality
* **StandardDeck** - Simple Black Jack deck with 52 cards created using `BaseDeck`

The deck system is interface-based allowing for creation of rigged decks for testing purposes.

### Result pattern
To ensure stability, this library uses **Result pattern** instead of throwing errors for invalid player actions.

* Methods such as `HitActiveHand`, `Split`, `DoubleBet` all return a Result object

Result can either be **success** or **failure**

* **Successful result** will provide `Value` if relevant for method (you can tell based on return value of method)
* **Failed result** will provide `ErrorMessage` with what went wrong (e.g., Insufficient funds)

You can easily tell whether Result is **success** or **failure** with `IsSuccess` and `IsFailure` atrributes.

## Code Example
This simple example demonstrates working with library
```
using BlackJackLib;

//Game initialization
var deck = new StandardDeck();
var player = new Player(100); //sets initial balance to 100
var dealer = new Dealer();

// Bet using result pattern
var betResult = player.PlaceBet(50);
if (betResult.IsFailure)
{
    Console.WriteLine($"Error placing bet: {betResult.ErrorMessage}");
    return;
}

// Player Hit demonstration
var hitResult = player.HitActiveHand(deck);
if (hitResult.IsSuccess)
{
    Card card = hitResult.Value;
    Console.WriteLine($"Player got card: {card.Rank} {card.Suit}");
}

// Dealer logic
if (dealer.ShouldHit())
{
    dealer.Hit(deck);
}

// Game state
Console.WriteLine($"Player's cards current total value: {player.GetActiveHand().GetTotalValue()}");
```