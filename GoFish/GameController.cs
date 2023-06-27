using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class GameController
    {
        public static Random Random = new Random();
        private GameState gameState;
        public bool GameOver { get { return gameState.GameOver; } }
        public Player HumanPlayer { get { return gameState.HumanPlayer; } }
        public IEnumerable<Player> Opponents { get { return gameState.Opponents; } }
        public string Status { get; private set; }

        /// <summary>
        ///  Constructs a new GameController
        /// </summary>
        /// <param name="humanPlayerName"></param>
        /// <param name="computerPlayerNames"></param>
        /// <exception cref="NotImplementedException"></exception>
        public GameController(string humanPlayerName, IEnumerable<string> computerPlayerNames)
        {
            gameState = new GameState(humanPlayerName, computerPlayerNames, new Deck().Shuffle());
            Status = $"{Environment.NewLine}Starting a new game with players {humanPlayerName}, {string.Join(", ", computerPlayerNames)}";        
        }
        /// <summary>
        /// Plays the next round, ending the game if everyone ran out of cards
        /// </summary>
        /// <param name="playerToAsk"></param>
        /// <param name="valueToAskFor"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void NextRound(Player playerToAsk, Values valueToAskFor)
        {
            Status = gameState.PlayRound(HumanPlayer ,playerToAsk, valueToAskFor, gameState.Stock) + Environment.NewLine;
            ComputerPlayersPlayNextRound();
            Status += Environment.NewLine + gameState.CheckForWinner();
        }
        /// <summary>
        /// / All of the computer players that have cards play the next round. If the human is
        /// out of cards, then the deck is depleted and they play out the rest of the game.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void ComputerPlayersPlayNextRound()
        {
            IEnumerable<Player> WhoAsk;
            do
            {
                WhoAsk = gameState.Opponents.Where(player => player.Hand.Count() > 0);
                foreach (Player player in WhoAsk)
                {
                    var randomPlayer = gameState.RandomPlayer(player);
                    var randomValue = player.RandomValueFromHand();
                    Status += gameState.PlayRound(player, randomPlayer, randomValue, gameState.Stock) + Environment.NewLine;
                }
               
            }
            while ((gameState.HumanPlayer.Hand.Count() == 0));      
        }
        /// <summary>
        /// Starts a new game with the same player names
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void NewGame()
        {
            Status = "Starting a new game";
            gameState = new GameState(gameState.HumanPlayer.Name, gameState.Opponents.Select(player => player.Name), new Deck().Shuffle());
        }
    }
}
