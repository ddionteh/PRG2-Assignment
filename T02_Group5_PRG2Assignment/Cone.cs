using System.Text;

namespace ICTreats
{
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
            double TotalPrice = 0;

            switch (scoops)
            {
                case 0: // no scoop
                    break;
                case 1: // single scoop
                    TotalPrice += 4.00;
                    break;
                case 2: // double scoops
                    TotalPrice += 5.50;
                    break;
                case 3: // triple scoops
                    TotalPrice += 6.50;
                    break;
            }

            if (dipped)
            {
                TotalPrice += 2;
            }
            TotalPrice += toppings.Count; 
            // Note: there is a shorter way but less readable and may be confusing
            return TotalPrice;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Dipped: {dipped}";
        }
    }
}