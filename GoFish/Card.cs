using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish
{
    public class Card
    {
        public Values Value { get; private set; }
        public Suits Suit { get; private set; }  
        public Card(Values value, Suits suit) 
        {
            this.Value = value;
            this.Suit = suit;
        }
        public string Name
        {
            get { return ($"{Value} of {Suit}"); }
        }
        public override string ToString() => Name;
    }
}
