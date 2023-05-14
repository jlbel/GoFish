using GoFish;
using System.Collections.Generic;

namespace GoFishTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void TestGetNextHand()
        {
            var player = new Player("Owen", new List<Card>());
            player.GetNextHand(new Deck());
            CollectionAssert.AreEqual(
            new Deck().Take(5).Select(card => card.ToString()).ToList(),
            player.Hand.Select(card => card.ToString()).ToList());
        }

        [TestMethod]
        public void TestDoYouHaveAny()
        {
            IEnumerable<Card> cards = new List<Card>()
            {
                new Card(Values.Jack, Suits.Spades),
                new Card(Values.Three, Suits.Clubs),
                new Card(Values.Three, Suits.Hearts),
                new Card(Values.Four, Suits.Diamonds),
                new Card(Values.Three, Suits.Diamonds),
                new Card(Values.Jack, Suits.Clubs),
            };
            var player = new Player("Owen", cards);
            var threes = player.DoYouHaveAny(Values.Three, new Deck())
                .Select(Card => Card.ToString())
                .ToList();
            CollectionAssert.AreEqual(new List<string>() {
                "Three of Diamonds",
                "Three of Clubs",
                "Three of Hearts",
            }, threes);

            Assert.AreEqual(3, player.Hand.Count());

            var jack = player.DoYouHaveAny(Values.Jack, new Deck())
                .Select(Card => Card.ToString())
                .ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "Jack of Clubs",
                "Jack of Spades"
            }, jack);

            var hand = player.Hand.Select(Card => Card.ToString()).ToList();
            CollectionAssert.AreEqual(new List<string>() { "Four of Diamonds" }, hand);
            Assert.AreEqual("Owen has 1 card and 0 books", player.Status);
        }

        [TestMethod]
        public void TestAddCardsAndPullOutBooks()
        {
            IEnumerable<Card> cards = new List<Card>()
            {
                new Card (Values.Jack, Suits.Spades),
                new Card(Values.Three, Suits.Clubs),
                new Card(Values.Jack, Suits.Hearts),
                new Card(Values.Three, Suits.Hearts),
                new Card(Values.Four, Suits.Diamonds),
                new Card(Values.Jack, Suits.Diamonds),
                new Card(Values.Jack, Suits.Clubs),
            };
            var player = new Player("Owen", cards);
            Assert.AreEqual(0, player.Book.Count());

            var CardsToAdd = new List<Card>() 
            {
                new Card(Values.Three, Suits.Diamonds),
                new Card(Values.Three, Suits.Spades),
            };
            player.AddCardsAndPullOutBooks(CardsToAdd);
            var books = player.Book.ToList();
            var expectedRes = new List<Values>() { Values.Three, Values.Jack };
            CollectionAssert.AreEqual(expectedRes, books);

            var hand = player.Hand.Select(Card => Card.ToString()).ToList();
            CollectionAssert.AreEqual(new List<string> { "Four of Diamonds"}, hand);
            Assert.AreEqual("Owen has 1 card and 2 books", player.Status);
        }
        [TestMethod]
        public void TestDrawCard()
        {
            var player = new Player("Owen", new List<Card>());
            player.DrawCard(new Deck());
            Assert.AreEqual(1, player.Hand.Count());
            Assert.AreEqual("Ace of Diamonds", player.Hand.First().ToString());
        }
        [TestMethod]
        public void TestRandomValueFromHand()
        {
            var player = new Player("Owen", new Deck());
            Player.random = new MockRandom() { ValueToReturn = 0 };
            Assert.AreEqual("Ace", player.RandomValueFromHand().ToString());
            Player.random = new MockRandom() { ValueToReturn = 4 };
            Assert.AreEqual("Two", player.RandomValueFromHand().ToString());
            Player.random = new MockRandom() { ValueToReturn = 8 };
            Assert.AreEqual("Three", player.RandomValueFromHand().ToString());

        }

    }
}