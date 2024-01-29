//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================
using System.Text;

namespace ICTreats
{
    class Cup : IceCream
    {
        public Cup() : base() { }

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

                    for (int i = 0; i < scoops; i++)
                    {
                        if (flavours[i].premium == true )
                        {
                            TotalPrice += 2.00;
                        }
                    }
                    break;
                case 2: // double scoops
                    TotalPrice += 5.50;

                    for (int i = 0; i < scoops; i++)
                    {
                        if (flavours[i].premium == true)
                        {
                            TotalPrice += 2.00;
                        }
                    }
                    break;
                case 3: // tripe scooops
                    TotalPrice += 6.50;

                    for (int i = 0; i < scoops; i++)
                    {
                        if (flavours[i].premium == true)
                        {
                            TotalPrice += 2.00;
                        }
                    }
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