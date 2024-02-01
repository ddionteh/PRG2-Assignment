//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================
using System.Text;
using System.Xml.Linq;

namespace ICTreats
{
    class Waffle : IceCream
    {

        public string waffleFlavour { get; set; }
        public Waffle() : base() { }

        public Waffle(string option, int scoops, List<Flavour> flavourList, List<Topping> toppingList, string waffleFlavour) : base(option, scoops, flavourList, toppingList)
        {
            this.waffleFlavour = waffleFlavour;
        }

        public override double CalculatePrice()
        {   
            double TotalPrice = 0;

            if (waffleFlavour != "original")
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
                case 3: // triple scoops
                    TotalPrice += 9.50;
                    break;
            }

            for (int i = 0; i < flavours.Count; i++)
            {
                if (flavours[i].premium == true)
                {
                    TotalPrice += 2.00 * flavours[i].quantity;
                }
            }

            TotalPrice += toppings.Count;

            return TotalPrice;
        }

        public override string ToString()
        {
            return $"{base.ToString()}Waffle Flavour: {Program.CapitalizeFirstLetter(waffleFlavour)}";
        } 
    }
}