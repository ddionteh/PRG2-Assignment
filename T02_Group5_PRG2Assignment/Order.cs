//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================
using static System.Formats.Asn1.AsnWriter;
using System.Text;
using System.Xml.Linq;

namespace ICTreats
{
    class Order
    {
        public int id {  get; set;}

        public DateTime timeReceived { get; set;}

        public DateTime? timeFulfilled { get; set;}

        public List<IceCream> iceCreamList { get; set;} = new List<IceCream>();

        public Order() { }

        public Order(int id, DateTime timeReceived)
        {
            this.id = id;
            this.timeReceived = timeReceived;
        }

        public void ModifyIceCream(int id) 
        {
           int maxScoop = 3;
           IceCream modifyIceCream = iceCreamList[id-1];

           try
           {
               int modifyOption = Program.IntegerValidator("[1] Modify option \n[2] Modify number of scoops \n[3] Modify flavours \n[4] Modify toppings" +
                 "\n[5] Modify dipped cone \n[6] Modify waffle flavour \nEnter the number for the modifications to make to the ice cream: ", 6,1);
               switch (modifyOption)
               {
                   case 1:
                       ModifyOption();
                       break;
                   case 2:
                       ModifyScoop();
                       break;
                   case 3:
                       ModifyFlavours();
                       break;
                   case 4:
                       ModifyToppings();
                       break;
                   case 5:
                       ModifyDipped();
                       break;
                   case 6:
                       ModifyWaffleFlavour();
                       break;
                   default:
                       throw new ArgumentOutOfRangeException(nameof(modifyOption), $"Invalid option: {modifyOption}");
               }
           }
           catch (FormatException ex)
           {
               Console.WriteLine(ex.Message);
               Console.WriteLine("Please enter an integer!");
               return;
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
           }
            
            // change ice cream type
            void ModifyOption()
            {
                Console.WriteLine("Options available:");
                foreach (string option in Program.optionsAvailable.Keys)
                {
                    Console.WriteLine($"{Program.CapitalizeFirstLetter(option),-15}");
                }

                string selectedOption = "";
                while (true)
                {
                    try
                    {
                        Console.Write("Enter the option you want: ");
                        selectedOption = Console.ReadLine().Trim().ToLower();

                        if (selectedOption == "")
                        {
                            Console.WriteLine("Selected option cannot be empty!");
                            continue;
                        }
                        else if (!Program.optionsAvailable.Keys.Contains(selectedOption))
                        {
                            Console.WriteLine("Not a valid option!");
                            continue;
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }

                switch (selectedOption)
                {
                    case "cup":
                        iceCreamList[id - 1] = new Cup(selectedOption, modifyIceCream.scoops, modifyIceCream.flavours, modifyIceCream.toppings);
                        Console.WriteLine("Ice Cream is now a Cup!");
                        break;
                    case "cone":
                        Console.Write("Would you like to dip your cone? (Y/N): ");
                        string dipped = Console.ReadLine().Trim().ToUpper();

                        if (dipped == "Y")
                        {
                            iceCreamList[id - 1] = new Cone(selectedOption, modifyIceCream.scoops, modifyIceCream.flavours, modifyIceCream.toppings, true);
                            Console.WriteLine("Ice Cream is now a dipped Cone!");
                            break;
                        }
                        iceCreamList[id - 1] = new Cone(selectedOption, modifyIceCream.scoops, modifyIceCream.flavours, modifyIceCream.toppings, false);
                        Console.WriteLine("Ice Cream is now a regular Cone!");
                        break;
                    case "waffle":
                        while (true)
                        {
                            Console.Write("Would you like to add-on a flavour for your waffle? (Y/N): ");
                            string waffleFlavourAnswer = Console.ReadLine().Trim().ToUpper();
                            if (waffleFlavourAnswer == "Y")
                            {
                                Console.WriteLine("Waffle Flavours available: ");
                                foreach (KeyValuePair<string, double> flavourAndCost in Program.waffleFlavour)
                                {
                                    if (flavourAndCost.Value > 0)
                                    {
                                        Console.WriteLine(Program.CapitalizeFirstLetter(flavourAndCost.Key));
                                        continue;
                                    }
                                }

                                string selectedWaffleFlavour = "";
                                while (true)
                                {
                                    try
                                    {
                                        Console.Write("Enter the waffle flavour you want: ");
                                        selectedOption = Console.ReadLine().Trim().ToLower();

                                        if (selectedOption == "")
                                        {
                                            Console.WriteLine("Selected waffle flavour cannot be empty!");
                                            continue;
                                        }
                                        else if (selectedOption == "original")
                                        {
                                            Console.WriteLine("Selected waffle flavour cannot be original!");
                                            continue;
                                        }
                                        else if (!Program.waffleFlavour.Keys.Contains(selectedOption))
                                        {
                                            Console.WriteLine("Not a valid option!");
                                            continue;
                                        }
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error: " + ex.Message);
                                    }
                                }

                                // if flavour is not original
                                if (selectedWaffleFlavour != "original")
                                {
                                    iceCreamList[id - 1] = new Waffle(selectedOption, modifyIceCream.scoops, modifyIceCream.flavours, modifyIceCream.toppings, selectedWaffleFlavour);
                                    break;
                                }
                                Console.WriteLine("Enter a valid waffle flavour!");
                                continue;
                            }
                            else if (waffleFlavourAnswer == "N")
                            {
                                iceCreamList[id - 1] = new Waffle(selectedOption, modifyIceCream.scoops, modifyIceCream.flavours, modifyIceCream.toppings, "original");
                                break;
                            }
                            Console.WriteLine("Enter 'Y' for Yes, 'N' for No!");
                        }
                        break;
                }
            }
            
            // change the number of scoops
            void ModifyScoop()
            {
                Console.Write($"Enter the number of scoops (1 - {maxScoop}): ");
                int scoop = int.Parse(Console.ReadLine()); // validate it to be between 1 and/or = maxscoop

                modifyIceCream.scoops = scoop;

                ModifyFlavours();
                return;
            }

            // modify the current flavours
            void ModifyFlavours()
            {
                // no need to check if flavour is 0, minimally one to create an order anyway

                // list out the available flavours
                foreach (KeyValuePair<string, bool> flavour in Program.flavourDict)
                {
                    if (flavour.Value == true)
                    {
                        Console.WriteLine($"Premium Flavour: {Program.CapitalizeFirstLetter(flavour.Key)}");
                        continue;
                    }
                    Console.WriteLine($"Regular Flavour: {Program.CapitalizeFirstLetter(flavour.Key)}");
                }

                // change each flavour for the number of flavours available 
                for (int i = 0; i < modifyIceCream.scoops; i++)
                {
                    Console.Write($"Enter new flavour for flavour {i+1}: ");

                    string newFlavour = Console.ReadLine().Trim().ToLower();

                    // check if flavour is a valid option
                    if (Program.flavourDict.ContainsKey(newFlavour))
                    {
                        // if user has selected a repeated flavour
                        Flavour existingFlavour = modifyIceCream.flavours.FirstOrDefault(flavourInList => flavourInList.type == newFlavour);

                        // If the flavour is not new, increment its quantity
                        if (existingFlavour != null)
                        {
                            existingFlavour.quantity += 1;
                            continue;
                        }
                        // if new flavour is premium
                        else if (Program.flavourDict[newFlavour] == true)
                        {
                            modifyIceCream.flavours[i] = new Flavour(newFlavour, true, 1);
                            continue;
                        }

                        // if new flavour is regular
                        modifyIceCream.flavours[i] = new Flavour(newFlavour, false, 1);
                        continue;
                    }
                    Console.WriteLine("Flavour does not exist!");
                    i--;
                    continue;
                }
            }

            // modify the current toppings
            void ModifyToppings()
            {
                if (modifyIceCream.toppings.Count() == 0)
                {
                    Console.WriteLine("No toppings to modify");
                    return;
                }

                foreach (string topping in Program.toppingDict.Keys)
                {
                    Console.WriteLine($"Topping: {topping}");
                }

                for (int i = 0; i < modifyIceCream.toppings.Count(); i++)
                {
                    Console.Write($"Enter the new topping for topping {i+1}");
                    string topping = Console.ReadLine().Trim().ToLower();
                    if (topping == "")
                    {
                        Console.WriteLine("Topping chosen cannot be empty!");
                        i--;
                        continue;
                    }
                    else if (Program.toppingDict.TryGetValue(topping, out double value) == true)
                    {
                        modifyIceCream.toppings[i] = new Topping(topping);
                        Console.WriteLine("Topping has been successfully modified.");
                        continue;
                    }

                    Console.WriteLine("Topping does not exist!");
                    i--;
                    continue;
                }

            }

            void ModifyDipped()
            {
                if (modifyIceCream.option != "cone")
                {
                    Console.WriteLine("Ice Cream must be a Cone to be modified!");
                    return;
                }

                Console.Write("Do you want to dip your cone? (Y/N): ");

                string dipAnswer = "";
                while (true)
                {
                    dipAnswer = Program.YesNoValidator(dipAnswer);

                    if (dipAnswer == "Y")
                    {
                        ((Cone)modifyIceCream).dipped = true;
                        break;
                    }
                    else if (dipAnswer == "N")
                    {
                        ((Cone)modifyIceCream).dipped = false;
                        break;
                    }
                }
            }

            void ModifyWaffleFlavour()
            {
                Console.WriteLine("Waffle Flavours available: ");
                foreach (KeyValuePair<string, double> flavourAndCost in Program.waffleFlavour)
                {
                    if (flavourAndCost.Value > 0)
                    {
                        Console.WriteLine(Program.CapitalizeFirstLetter(flavourAndCost.Key));
                        continue;
                    }
                }

                Console.Write("Enter the new waffle flavour: ");
                string selectedWaffleFlavour = Console.ReadLine().Trim().ToLower();

                if (Program.waffleFlavour.TryGetValue(selectedWaffleFlavour, out double cost) == true)
                {
                    if (cost > 0)
                    {
                        ((Waffle)iceCreamList[id - 1]).waffleFlavour = selectedWaffleFlavour;
                        Console.WriteLine($"Waffle flavour is now {selectedWaffleFlavour}.");
                        return;
                    }
                    ((Waffle)iceCreamList[id - 1]).waffleFlavour = "original";
                    return;
                }
            }
        }

        public void AddIceCream(IceCream iceCream)
        {
            iceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int id)
        {
            try
            {
                iceCreamList.RemoveAt(id - 1);
                Console.WriteLine("Ice Cream successfully removed!");
            }
            catch (IndexOutOfRangeException) // the only kind of error you would get here
            {
                Console.WriteLine("Ice Cream was not found. Please enter a valid ID.");
            }
            
        }
        public double CalculateTotal()
        {
            double TotalPrice = 0;
            foreach (IceCream iceCream in  iceCreamList)
            {
                TotalPrice += iceCream.CalculatePrice();
            }
            return TotalPrice;
        }

        public override string ToString()
        {
            // Memory efficient and performs better than regular string concatenation 
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"ID: {id} Time Received: {timeReceived} Time Fulfilled: {timeFulfilled}");

            // Checks if there's any ice cream in the list
            if (iceCreamList.Any())
            {
                stringBuilder.AppendLine();

                int x = 1;
                foreach (IceCream iceCream in iceCreamList)
                {
                    stringBuilder.Append($"-----ICE CREAM [{x}]-----\n");
                    stringBuilder.AppendLine(iceCream.ToString());
                    x++;
                }
            }

            return stringBuilder.ToString();
        }
    }
}