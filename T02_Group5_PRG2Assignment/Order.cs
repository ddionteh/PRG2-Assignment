﻿//==========================================================
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


           Console.WriteLine("[1] Modify option \n[2] Modify number of scoops \n[3] Modify flavours \n[4] Modify toppings" +
                "\n[5] Modify dipped cone \n[6] Modify waffle flavour");
           Console.Write("Enter the number for the modifications to make to the ice cream: ");

           try
           {
               int modifyOption = int.Parse(Console.ReadLine());
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

                Console.Write("Enter the option you want: ");
                string selectedOption = Console.ReadLine().Trim().ToLower();

                if (Program.optionsAvailable.ContainsKey(selectedOption))
                {
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

                                    Console.Write("Enter the waffle flavour you want: ");
                                    string selectedWaffleFlavour = Console.ReadLine().Trim().ToLower();

                                    if (Program.waffleFlavour.ContainsKey(selectedWaffleFlavour) && selectedWaffleFlavour != "original")
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
            }
            
            // change the number of scoops
            void ModifyScoop()
            {
                Console.Write($"Enter the number of scoops (1 - {maxScoop}): ");
                int scoop = int.Parse(Console.ReadLine()); // validate it to be between 1 and/or = maxscoop

                modifyIceCream.scoops = scoop;
                return;
            }

            // modify the current flavours
            void ModifyFlavours()
            {
                // no need to check if flavour is 0, minimally one here

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

                for (int i = 0; i < modifyIceCream.flavours.Count; i++)
                {
                    Console.Write($"Enter new flavour for flavour {i+1}: ");

                    string newFlavour = Console.ReadLine().Trim().ToLower();

                    bool premium;
                    if (Program.flavourDict.ContainsKey(newFlavour))
                    {   
                        // if flavour is premium
                        if (Program.flavourDict[newFlavour] == true)
                        {
                            premium = true;
                            modifyIceCream.flavours[i] = new Flavour(newFlavour, premium, 1);
                            continue;
                        }

                        // if flavour is regular
                        premium = false;
                        modifyIceCream.flavours[i] = new Flavour(newFlavour, premium, 1);
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Flavour is does not exist!");
                        i--;
                        continue;
                    }
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

                    if (Program.toppingDict.TryGetValue(topping, out double value) == true)
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

                string dipAnswer;
                if (((Cone)modifyIceCream).dipped == true)
                {
                    Console.Write("Would you like to NOT dip your cone? (Y/N)");
                    dipAnswer = Console.ReadLine().Trim().ToUpper();

                    if (dipAnswer == "Y")
                    {
                        ((Cone)modifyIceCream).dipped = false;
                        Console.WriteLine("Cone is no longer dipped.");
                        return;
                    }
                    Console.WriteLine("Cone is still dipped.");
                    return; 
                }

                Console.Write("Would you like to dip your cone? (Y/N): ");
                dipAnswer = Console.ReadLine().Trim().ToUpper();

                if (dipAnswer == "Y")
                {
                    ((Cone)modifyIceCream).dipped = true;
                    Console.Write("Cone is now dipped.");
                    return;
                }

                Console.WriteLine("Cone is still not dipped.");
                return;
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