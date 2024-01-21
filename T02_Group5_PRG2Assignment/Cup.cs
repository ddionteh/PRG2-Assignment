using System.Text;

namespace ICTreats
{
    class Cup : IceCream
    {
        public Cup() { }

        public Cup(string option, int scoops,  List<Flavour> flavourList, List<Topping> toppingList) : base(option, scoops, flavourList, toppingList)
        {

        }

        public override double CalculatePrice()
        {
            double TotalPrice = 0;
            switch(scoops)
            {
                case 0: // no scoops
                    break;
                case 1: // single scoop
                    TotalPrice += 4.00;
                    break;
                case 2: // double scoops
                    TotalPrice += 5.50;
                    break;
                case 3: // tripe scooops
                    TotalPrice += 6.50;
                    break;
            }
            TotalPrice += toppings.Count;
            return TotalPrice;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}