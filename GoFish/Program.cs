using System.Diagnostics.Metrics;
using System.Numerics;

namespace GoFish
{
    public class Program
    {
        static GameController gameController;
        static void Main(string[] args)
        {
            Console.WriteLine($"Enter your name: ");
            var HumanPlayer = Console.ReadLine();
            Console.WriteLine("Enter the number of computer opponents: ");
            int number;
            
            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out number) || number < 1 || number > 4)
            {
                Console.WriteLine(Environment.NewLine+"Please enter a number from 1 to 4");
            }
            
            var opponent = number switch
            {
                1 => new List<string>() { "Computer #1" },
                2 => new List<string>() { "Computer #1", "Computer#2" },
                3 => new List<string>() { "Computer #1", "Computer#2", "Computer #3" },
                4 => new List<string>() { "Computer #1", "Computer#2", "Computer #3", "Computer #4" },

            };

            Console.WriteLine($"{Environment.NewLine}Welcome to the game, {HumanPlayer}");
            gameController = new GameController(HumanPlayer, opponent);
            
            while (!gameController.GameOver)
            {
                Console.WriteLine(gameController.Status);
                
                while (!gameController.GameOver)
                {
                    Console.WriteLine("Your hand: ");
                    
                    foreach (var card in gameController.HumanPlayer.Hand)
                        Console.WriteLine(card.Name);
                    var promptValue = PromptForAValue();
                    var promptOpponent = PromptForAnOpponent();
                    gameController.NextRound(promptOpponent, promptValue);
                    Console.WriteLine(gameController.Status);
                    Console.WriteLine(Environment.NewLine + gameController.HumanPlayer.Status);
                    
                    foreach (Player opponents in gameController.Opponents)
                    {
                        Console.WriteLine(opponents.Status);
                    }
                }
                Console.WriteLine("Press Q to quit, any other key for a new game.");
                
                if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "N")
                    gameController.NewGame();
            }
        }

        /// <summary>
        ///  Prompt the human player for a card value
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static Values PromptForAValue()
        {
            var handValue = gameController.HumanPlayer.Hand.Select(card => card.Value).ToList();
            Console.WriteLine("What card value do you want to ask for?");
           
            while (true)
            {
                if (Enum.TryParse(typeof(Values), Console.ReadLine(), out var value) &&
                    handValue.Contains((Values)value))
                    return (Values)value;
                else
                    Console.WriteLine("Please enter a value in your hand.");
            }
        }
        /// <summary>
        /// Prompt the human player for an opponent
        /// to ask for a card
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static Player PromptForAnOpponent()
        {
            var forAsk = gameController.Opponents.ToList();
                
            for (int i = 1; i <= forAsk.Count(); i++)
                    Console.WriteLine($"{i}. {forAsk[i-1]}");
            Console.WriteLine(Environment.NewLine+"Who do you want to ask for a card?");
            
            while (true)
            {
               if(int.TryParse(Console.ReadKey(false).KeyChar.ToString(), out int numberToAsk) && numberToAsk >=1 && numberToAsk <= forAsk.Count())
                {
                    var opponent = numberToAsk switch
                    {
                        1 => gameController.Opponents.First(),
                        2 => gameController.Opponents.Skip(1).First(),
                        3 => gameController.Opponents.Skip(2).First(),
                        4 => gameController.Opponents.Skip(3).First(),
                    };
                    
                    return opponent;
                }
                
                else Console.WriteLine($"Please enter a number from 1 to {forAsk.Count()}: ");
            }
        }
    }
}