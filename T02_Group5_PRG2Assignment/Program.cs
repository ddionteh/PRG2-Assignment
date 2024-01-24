//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================

using ICTreats;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTreats
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
            Dictionary<string, double> flavourCostDict = new Dictionary<string, double>();
            Dictionary<string, double> toppingCostDict = new Dictionary<string, double>();
            Queue<Customer> regularCustomerQueue = new Queue<Customer>();
            Queue<Customer> goldMemberQueue = new Queue<Customer>();

            // O(1) lookup time complexity, hence dict over list (below)

            // to store the options available, int is just a placeholder 
            Dictionary<string, int> optionsAvailable = new Dictionary<string, int>();

            // to store number of scoops allowed
            Dictionary<string, int> scoopsAvailable = new Dictionary<string, int>();

            // to store whether the option can be dipped 
            Dictionary<string, bool> dippedAvailable = new Dictionary<string, bool>();

            // to store the waffle flavours available (ONLY FOR WAFFLE CLASS)
            // value is cost/placeholder
            Dictionary<string, double> waffleFlavourAvailable = new Dictionary<string, double>();



            // Getting data into the program first
            CustomerCreation();
            ToppingCostCreation();
            FlavourCostCreation();
            OptionCreation();
            OrderCreation();

            int option = -1;
            while (option != 0)
            {
                DisplayMenu();
            }

            // Display menu
            void DisplayMenu()
            {
                Console.WriteLine("[1] List all customers \n[2] List all current orders \n[3] Register a new customer " +
                    "\n[4] Create a new customer's order \n[5] Display order details of a customer \n[6] Modify order details");
                Console.Write("Enter your option: ");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0: // exit program
                        break;
                    case 1: // Option 1
                        DisplayAllCustomers();
                        break;
                    case 2:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(option), $"Enter a valid option!");
                }
            }

            // Create customer objects from CSV file data of customers
            void CustomerCreation()
            {
                using (StreamReader sr = new StreamReader("customers.csv"))
                {
                    string? header = sr.ReadLine();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {   
                        // Split into elements and remove all leading or trailing whitespace
                        string[] splitLine = line.Split(',').Select(s => s.Trim().ToLower()).ToArray();
                        Customer customer = new Customer(splitLine[0], int.Parse(splitLine[1]), Convert.ToDateTime(splitLine[2]));
                        customer.rewards = new PointCard(int.Parse(splitLine[4]), int.Parse(splitLine[5]));
                        customerDict.Add(customer.memberid, customer);
                        Console.WriteLine(customer.ToString());
                    }

                }
            }

            // Storing costs (value) for each type (key) of topping in a dictionary
            void ToppingCostCreation()
            {
                using (StreamReader sr = new StreamReader("toppings.csv"))
                {
                    string? header = sr.ReadLine();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Split into elements and remove all leading or trailing whitespace
                        string[] splitLine = line.Split(',').Select(s => s.Trim().ToLower()).ToArray();

                        toppingCostDict.Add(splitLine[0], double.Parse(splitLine[1]));
                    }

                }
            }

            // Storing costs (value) for each type (key) of flavour in a dictionary
            void FlavourCostCreation()
            {
                using (StreamReader sr = new StreamReader("flavours.csv"))
                {
                    string? header = sr.ReadLine();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Split into elements and remove all leading or trailing whitespace
                        string[] splitLine = line.Split(',').Select(s => s.Trim().ToLower()).ToArray();

                        flavourCostDict.Add(splitLine[0], double.Parse(splitLine[1]));
                    }

                }
            }

            // 
            void OptionCreation()
            {
                using (StreamReader sr = new StreamReader("orders.csv"))
                {
                    string? header = sr.ReadLine();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Split into elements and remove all leading or trailing whitespace
                        string[] splitLine = line.Split(',').Select(s => s.Trim().ToLower()).ToArray();

                        // checks if option is not in dictionary
                        if (!optionsAvailable.ContainsKey(splitLine[0]))
                        {
                            optionsAvailable.Add(splitLine[0], 0);
                        }

                        // checks if number of scoop is not in dictionary
                        if (!scoopsAvailable.ContainsValue(int.Parse(splitLine[1])))
                        {
                            scoopsAvailable.Add(splitLine[0], int.Parse(splitLine[1]));
                        }

                        // checks if the option is not stored (key)
                        if (!dippedAvailable.ContainsKey(splitLine[0]))
                        {
                            bool dippedStatus = true;
                            if (string.IsNullOrEmpty(splitLine[2]))
                            {
                                dippedStatus = false;
                            }

                            dippedAvailable.Add(splitLine[0], dippedStatus);
                        }

                        // checks if the waffle flavour is not inside
                        if (!waffleFlavourAvailable.ContainsKey(splitLine[4]))
                        {
                            waffleFlavourAvailable.Add(splitLine[4], 3); // kinda hard-coded here, need to change before final submission
                        }
                    }
                }
            }

            // Storing orders into program
            void OrderCreation()
            {
                using (StreamReader sr = new StreamReader("orders.csv"))
                {
                    string? header = sr.ReadLine();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        // Split into elements and remove all leading or trailing whitespace
                        string[] splitLine = line.Split(',').Select(s => s.Trim().ToLower()).ToArray();

                        // Reference to the customer who made the order
                        Customer customer = customerDict[int.Parse(splitLine[1])];

                        Console.WriteLine(customer);
                        Order order = new Order(int.Parse(splitLine[0]), Convert.ToDateTime(splitLine[2]));
                        Console.WriteLine(order);
                        if (!String.IsNullOrWhiteSpace(splitLine[3]))
                        {
                            order.timeFulfilled = Convert.ToDateTime(splitLine[3]);
                        }

                        List<Flavour> flavours = new List<Flavour>();
                        List<Topping> toppings = new List<Topping>();

                        // Maximum of 3 flavours
                        for (int i = 8; i < 8 + 3; i++)
                        {
                            // if-else is faster than try-catch here
                            // Checking if flavour actually exists
                            if (flavourCostDict.TryGetValue(splitLine[i], out double cost))
                            {
                                bool premium = false;
                                bool isFound = false;

                                // checks if flavour already exists
                                foreach (Flavour flavour in flavours)
                                {
                                    if (flavour.type == splitLine[i])
                                    {
                                        flavour.quantity += 1;
                                        isFound = true;
                                        break;
                                    }
                                }

                                if (!isFound)
                                {
                                    if (cost > 0)
                                    {
                                        premium = true;
                                    }
                                    flavours.Add(new Flavour((splitLine[i]), premium, 1));
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error, no such flavour available.");
                                break;
                            }
                        }

                        for (int i = 11; i < splitLine.Length; i++)
                        {
                            // if-else is faster than try-catch here
                            // Checking if toppings actually exists
                            if (toppingCostDict.TryGetValue(splitLine[i], out double cost))
                            {
                                toppings.Add(new Topping(splitLine[i]));
                            }
                            else
                            {
                                Console.WriteLine("Error, no such topping available.");
                                break;
                            }
                        }

                        if (optionsAvailable.ContainsKey(splitLine[4]))
                        {
                            switch (splitLine[4])
                            {
                                // hard coded this part, need to fix it
                                case "cup":
                                    IceCream iceCreamCup = new Cup(splitLine[4], int.Parse(splitLine[5]), flavours, toppings);
                                    order.AddIceCream(iceCreamCup);

                                    // customerOrder1 is referencing the specific Order that is returned by the function
                                    Order customerOrder1 = customer.MakeOrder();
                                    customerOrder1 = order;

                                    break;
                                case "cone":
                                    IceCream iceCreamCone = new Cone(splitLine[4], int.Parse(splitLine[5]), flavours, toppings, bool.Parse(splitLine[5]));
                                    order.AddIceCream(iceCreamCone);

                                    // customerOrder2 is referencing the specific Order that is returned by the function
                                    Order customerOrder2 = customer.MakeOrder();
                                    customerOrder2 = order;
                                    break;
                                case "waffle":
                                    IceCream iceCreamWaffle = new Waffle(splitLine[4], int.Parse(splitLine[5]), flavours, toppings, splitLine[6]);
                                    order.AddIceCream(iceCreamWaffle);

                                    // customerOrder3 is referencing the specific Order that is returned by the function
                                    Order customerOrder3 = customer.MakeOrder();
                                    customerOrder3 = order;
                                    break;
                                default:
                                    Console.WriteLine("Error, no such option available");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error, no such option available");
                            break;
                        }
                    }

                }
            }
            // Option 1
            void DisplayAllCustomers()
            {
                foreach (Customer customer in customerDict.Values)
                {
                    Console.WriteLine(customer.ToString());
                }
            }

            // Option 2
        }
    }
}
