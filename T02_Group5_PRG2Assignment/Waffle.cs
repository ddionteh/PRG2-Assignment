using System.Text;

namespace ICTreats
{
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
            double TotalPrice = 0;

            if (waffleFlavour != "Original")
            {
                TotalPrice += 3;
            }

            switch (scoops)
            {
                case 0: // no scoops
                    break;
                case 1: // single scoop
                    TotalPrice += 7.00;
                    break;
                case 2: // double scoops
                    TotalPrice += 8.50;
                    break;
                case 3: // tripe scoops
                    TotalPrice += 9.50;
                    break;
            }

            TotalPrice += toppings.Count;

            return TotalPrice;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Waffle Flavour: {waffleFlavour}";
        } 
    }
}