using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class GameState
    {
        public readonly IEnumerable<Player> Players;
        public readonly IEnumerable<Player> Opponents;
        public readonly Player HumanPlayer;
        public bool GameOver { get; private set; } = false;
        public readonly Deck Stock;

        /// <summary>
        /// Constructor creates the players and deals their first hands
        /// </summary>
        /// <param name="humanPlayerName"></param>
        /// <param name="opponentNames"></param>
        /// <param name="stock"></param>
        public GameState(string humanPlayerName, IEnumerable<string> opponentNames, Deck stock)
        {
            Stock = stock;
            HumanPlayer = new Player(humanPlayerName);
            HumanPlayer.GetNextHand(Stock);
            var opponents = new List<Player>();
            foreach (var opponentName in opponentNames)
            {
                var opponent = new Player(opponentName);
                opponent.GetNextHand(Stock);
                opponents.Add(opponent);
            }
            Opponents = opponents;
            Players = new List<Player>() { HumanPlayer }.Concat(Opponents);
        }
        /// <summary>
        /// Gets a random player that doesn't match the current player
        /// </summary>
        /// <param name="currentPlayer"></param>
        /// <returns>A random player that the current player can ask for a card</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Player RandomPlayer(Player currentPlayer) =>
                Players.Where(player => player != currentPlayer)
                .Skip(Player.random.Next(Players.Count() - 1))
                .First();
        
        /// <summary>
        /// Makes one player play a round
        /// </summary>
        /// <param name="player"></param>
        /// <param name="playerToAsk"></param>
        /// <param name="valueToAskFor"></param>
        /// <param name="stock"></param>
        /// <returns>A message that describes what just happened</returns>
        public string PlayRound(Player player, Player playerToAsk, Values valueToAskFor, Deck stock)
        {
            var valueTo = (valueToAskFor == Values.Six) ? "Sixes" :$"{valueToAskFor}s";
            var message = $"{Environment.NewLine}{player} asked {playerToAsk} for {valueTo}";
                var card = playerToAsk.DoYouHaveAny(valueToAskFor, stock);
                if (card.Count() > 0)
                {
                    player.AddCardsAndPullOutBooks(card);
                    var s = (card.Count() >= 2) ? "s" : "";
                    message += $"{Environment.NewLine}{playerToAsk} has {card.Count()} {valueToAskFor} card{s}";
                }
            
            else if (stock.Count==0) message += $"{Environment.NewLine}The stock is out of cards";
            else 
            {
                player.DrawCard(stock);
                message += $"{Environment.NewLine}{player.Name} drew a card";
            }
            if (player.Hand.Count() == 0)
            {
                player.GetNextHand(stock);
                message += $"{Environment.NewLine}{player.Name} ran out of cards,"
                + $" drew {player.Hand.Count()} from the stock";
            }
            return message ;

        }
        /// <summary>
        /// Checks for a winner by seeing if any players have any cards left, sets GameOver
        /// if the game is over and there's a winner
        /// </summary>
        /// <returns>A string with the winners, an empty string if there are no winners</returns>
        public string CheckForWinner()
        {
            var playerCards = Players.Select(player => player.Hand.Count()).Sum();
            if (playerCards > 0) return "";
            GameOver = true;
            var winningBookCount = Players.Select(player => player.Book.Count()).Max();
            var winners = Players.Where(player => player.Book.Count() == winningBookCount);
            if (winners.Count() == 1) return $"The winner is {winners.First().Name}";
            return $"The winners are {string.Join(" and ", winners)}";
        }
    }
}
