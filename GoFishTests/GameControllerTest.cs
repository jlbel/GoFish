using GoFish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFishTests
{
    [TestClass]
    public class GameControllerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Player.random = new MockRandom() { ValueToReturn = 0 };
        }

        [TestMethod]
        public void TestConstructor()
        {
            var gameController = new GameController("Human", new List<string>() { "Player1", "Player2", "Player3" });
            Assert.AreEqual("Starting a new game with players Human, Player1, Player2, Player3", gameController.Status);
        }

        [TestMethod]
        public void TestNextRound()
        {
            var gameController = new GameController("Owen", new List<string>() { "Brittney" });
            gameController.NextRound(gameController.Opponents.First(), Values.Six);
            Assert.AreEqual("Owen asked Brittney for Sixes" + 
            Environment.NewLine + "Brittney has 1 Six card" + 
            Environment.NewLine + "Brittney asked Owen for Sevens" + 
            Environment.NewLine + "Brittney drew a card" + 
            Environment.NewLine + "Owen has 6 cards and 0 books" + 
            Environment.NewLine + "Brittney has 5 cards and 0 books" + 
            Environment.NewLine + "The stock has 41 cards" + 
            Environment.NewLine, gameController.Status);
        }

        [TestMethod]
        public void TestNewGame()
        {
            Player.random = new MockRandom() { ValueToReturn = 0 };
            var gameController = new GameController("Owen", new List<string>() { "Brittney" });
            gameController.NextRound(gameController.Opponents.First(), Values.Six);
            gameController.NewGame();
            Assert.AreEqual("Owen", gameController.HumanPlayer.Name);
            Assert.AreEqual("Brittney", gameController.Opponents.First().Name);
            Assert.AreEqual("Starting a new game", gameController.Status);

        }
    }
}
