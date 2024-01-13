namespace ICTreats
{
    abstract class IceCream
    {
        public string option { get; set; }

        public int scoops { get; set; }

        public List<Flavour> flavours { get; set; } = new List<Flavour>();

        public List<Topping> toppings { get; set; } = new List<Topping>();

        public IceCream() { }

        public IceCream(string option, int scoops, List<Flavour> flavourList, List<Topping> toppingList)
        {
            this.option = option;
            this.scoops = scoops;
            this.flavours = flavourList;
            this.toppings = toppingList;
        }

        public abstract double CalculatePrice();

        public override string ToString()
        {
            string flavoursString = string.Empty;
            foreach (Flavour flavour in flavours)
            {
                flavoursString = flavoursString + '\n' + flavour;
            }
            string toppingsString = string.Empty;
            foreach (Topping topping in toppings)
            {
                toppingsString = toppingsString + '\n' + topping;
            }
            return "Option: " + option + "\tScoops: " + scoops + flavoursString + '\n' + toppingsString;
        }
    }

    class Cup : IceCream
    {
        public Cup() { }

        public Cup(string option, int scoops,  List<Flavour> flavourList, List<Topping> toppingList) : base(option, scoops, flavourList, toppingList)
        {

        }

        public override double CalculatePrice()
        {
            return 0.0; // TBC MUST DO
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    class Cone : IceCream
    {   
        public bool dipped { get; set; }

        public Cone() { }

        public Cone(string option, int scoops, List<Flavour> flavourList, List<Topping> toppingList, bool dipped) : base(option, scoops, flavourList, toppingList)
        {
            this.dipped = dipped;
        }

        public override double CalculatePrice()
        {
            return 0.0; // TBC MUST DO
        }

        public override string ToString()
        {
            return base.ToString() + "\tDipped: " + dipped;
        }
    }

    class Waffle : IceCream
    {

        public string waffleFlavour { get; set; }
        public Waffle() { }

        public Waffle(string option, int scoops, List<Flavour> flavourList, List<Topping> toppingList, string waffleFlavour) : base(option, scoops, flavourList, toppingList)
        {
            this.waffleFlavour = waffleFlavour;
        }

        public override double CalculatePrice()
        {
            return 0.0; // TBC MUST DO
        }

        public override string ToString()
        {
            return base.ToString() + "\tWaffle flavour: " + waffleFlavour;
        } 
    }
}