using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class Player
    {
        public static Random random = new Random();
        private List<Card> hand = new List<Card>();
        private List<Values> book = new List<Values>();
       
        /// <summary>
        /// the cards in the player's hand
        /// </summary>
        public IEnumerable<Card> Hand => hand;

        /// <summary>
        /// The books that the player has pulled out
        /// </summary>
        public IEnumerable<Values> Book => book;
        public readonly string Name;

        /// <summary>
        /// Pluralize a word, adding "s" if a value isn't equal to 1
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string S(int s) => s == 1 ? "" : "s";

        /// <summary>
        /// Returns the current status of the player: the number of cards and books
        /// </summary>
        public string Status => $"{Name} has {hand.Count()} card{S(hand.Count())} and {book.Count()} book{S(book.Count())}";

        /// <summary>
        /// Constructor to create a player
        /// </summary>
        /// <param name="name"></param>
        public Player(string name) => Name = name;
        public Player(string name, IEnumerable<Card> cards)
        {
            Name = name;
            hand.AddRange(cards);
        }

        /// <summary>
        ///  Gets up to five cards from the stock (набор карт в руку до 5штук)
        /// </summary>
        /// <param name="stock"></param>
        public void GetNextHand(Deck stock)
        {
            while ((stock.Count() > 0) && (hand.Count < 5))
            {
                hand.Add(stock.Deal(0));
            }
        }
        /// <summary>
        /// If I have any cards that match the value, return them. If I run out of cards, get
        /// the next hand from the deck.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="deck"></param>
        /// <returns>The cards that were pulled out of the other player's hand</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Card> DoYouHaveAny(Values value, Deck deck)
        {
            var matchingCards = hand.Where(card => card.Value == value)
            .OrderBy(Card => Card.Suit);
            hand = hand.Where(card => card.Value != value).ToList();

            if (hand.Count() == 0)
                GetNextHand(deck);

            return matchingCards;
        }
        /// <summary>
        ///  When the player receives cards from another player, adds them to the hand
        /// and pulls out any matching books
        /// </summary>
        /// <param name="cards"></param>
        public void AddCardsAndPullOutBooks(IEnumerable<Card> cards)
        {
            hand.AddRange(cards);
            var foundBook = hand.GroupBy(card => card.Value)
                .Where(group =>  group.Count() ==4)
                .Select(group => group.Key);
            book.AddRange(foundBook);
            book.Sort();
            hand = hand.Where(card => !book.Contains(card.Value))
                .ToList();
        }
        /// <summary>
        ///  Draws a card from the stock and add it to the player's hand
        /// </summary>
        /// <param name="stock"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DrawCard(Deck stock)
        {
            if (stock.Count > 0)
                AddCardsAndPullOutBooks(new List<Card>() { stock.Deal(0) });
        }
        /// <summary>
        ///  Gets a random value from the player's hand
        /// </summary>
        /// <returns>The value of a randomly selected card in the player's hand</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Values RandomValueFromHand() => hand.OrderBy(card => card.Value)
                                                    .Select(card => card.Value)
                                                    .Skip(random.Next(hand.Count()))
                                                    .First();

        public override string ToString() => Name;
        



    }
}
