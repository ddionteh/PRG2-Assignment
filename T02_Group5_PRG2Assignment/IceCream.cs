//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================
using System.Text;
using System.Xml.Linq;

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
            flavours = flavourList;
            toppings = toppingList;
        }

        public abstract double CalculatePrice();

        public override string ToString()
        {
            // Memory efficient and performs better than regular string concatenation 
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Option: {Program.CapitalizeFirstLetter(option)} Scoops: {scoops}");

            // Checks if there's any flavour
            if (flavours.Any())
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Flavours:");
                foreach (Flavour flavour in flavours)
                {
                    stringBuilder.AppendLine(Program.CapitalizeFirstLetter(flavour.ToString()));
                }
            }

            // Checks if there's any toppings
            if (toppings.Any())
            {
                stringBuilder.AppendLine("Toppings:");
                foreach (Topping topping in toppings)
                {
                    stringBuilder.AppendLine(Program.CapitalizeFirstLetter(topping.ToString()));
                }
            }

            return stringBuilder.ToString();
        }
    }
}