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
    class Program
    {
        // O(1) lookup time complexity, hence dictionary over list (below)
        // allows me to check for the flavour available faster than list
        public static Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>();
        // stores the type of flavour (key) available and value is true if flavour is premium
        public static Dictionary<string, bool> flavourDict = new Dictionary<string, bool>();
        // stores the type of topping (key) available and cost (placeholder)
        public static Dictionary<string, double> toppingDict = new Dictionary<string, double>();
        // stores the type of waffle flavour (key) available and cost (placeholder)
        public static Dictionary<string, double> waffleFlavour = new Dictionary<string, double>();
        // stores the type of ice cream (options) available as key and value
        public static Dictionary<string, string> optionsAvailable = new Dictionary<string, string> { { "cup", "cup" }, { "cone", "cone" }, { "waffle", "waffle" } };

        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1);
        }

        public static void Main(string[] args)
        {
            Queue<Order> regularQueue = new Queue<Order>();
            Queue<Order> goldQueue = new Queue<Order>();

            // Getting data into the program first
            CustomerCreation();
            ToppingCostCreation();
            FlavourCostCreation();
            OptionCreation();
            OrderCreation();

            int option = -1;
            while (option != 0)
            {   
                try 
                {
                    DisplayMenu();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // Display menu
            void DisplayMenu()
            {
                Console.WriteLine("========ICE CREAM SHOP MENU========");
                Console.WriteLine("[1] List all customers \n[2] List all current orders \n[3] Register a new customer " +
                    "\n[4] Create a new customer's order \n[5] Display order details of a customer \n[6] Modify order details \n[0] Exit program");
                Console.WriteLine("==================================="); 
                Console.Write("Enter your option: ");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0: // exit program
                        break;
                    case 1: // Option 1
                        DisplayAllCustomers();
                        break;
                    case 2: // Option 2
                        DisplayQueue();
                        break;
                    case 3:
                        RegisterCustomer();
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

                        toppingDict.Add(splitLine[0], double.Parse(splitLine[1]));
                    }

                }
            }

            // Storing each type (key) of flavour in a dictionary
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
                        bool premium = false;
                        if (double.Parse(splitLine[1]) > 0)
                        {
                            premium = true;
                        }
                        flavourDict.Add(splitLine[0], premium);
                    }

                }
            }

            // storing each type of Waffle flavour in a dictionary
            void OptionCreation()
            {
                using (StreamReader sr = new StreamReader("options.csv"))
                {
                    string? header = sr.ReadLine();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Split into elements and remove all leading or trailing whitespace
                        string[] splitLine = line.Split(',').Select(s => s.Trim().ToLower()).ToArray();

                        // checks if the waffle flavour is not inside
                        if (!waffleFlavour.ContainsKey(splitLine[4]))
                        {
                            waffleFlavour.Add(splitLine[4], 3); 
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
                        // Split into elements and remove all leading or trailing whitespace
                        string[] splitLine = line.Split(',').Select(s => s.Trim().ToLower()).ToArray();
                        // Reference to the customer who made the order
                        Customer customer = customerDict[int.Parse(splitLine[1])];

                        bool newOrder = true;
                        Order order;
                        if (customer.currentOrder != null && customer.currentOrder.timeReceived == Convert.ToDateTime(splitLine[2])) 
                        {
                            order = customer.currentOrder;
                            newOrder = false;
                        }
                        else // new order
                        {
                            // reference to the Order object returned by the function
                            order = customer.MakeOrder();
                            // updating order details (ID and Time Received)
                            order.id = int.Parse(splitLine[0]);
                            order.timeReceived = Convert.ToDateTime(splitLine[2]);
                        }

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
                            if (flavourDict.TryGetValue(splitLine[i], out bool premium))
                            {
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
                                    flavours.Add(new Flavour((splitLine[i]), premium, 1));
                                }
                            }
                            else if (String.IsNullOrEmpty(splitLine[i])) 
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Error, no such flavour available.");
                                continue;
                            }
                        }

                        // Adding toppings 
                        for (int i = 11; i < splitLine.Length; i++)
                        {
                            // if-else is faster than try-catch here
                            // Checking if toppings actually exists
                            if (toppingDict.TryGetValue(splitLine[i], out double cost))
                            {
                                toppings.Add(new Topping(splitLine[i]));
                            }
                            else if (String.IsNullOrEmpty(splitLine[i]))
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Error, no such topping available.");
                                continue;
                            }
                        }


                        // checking if the option exists
                        if (optionsAvailable.ContainsKey(splitLine[4]))
                        {
                            // Queue for gold member queue
                            if (customer.rewards.tier == "Gold")
                            {
                                if (newOrder == true)
                                {
                                    goldQueue.Enqueue(order);
                                }
                            }
                            else if (newOrder == true)
                            {   
                                regularQueue.Enqueue(order);
                            }

                            switch (splitLine[4])
                            {   
                                case "cup":
                                    IceCream iceCreamCup = new Cup(splitLine[4], int.Parse(splitLine[5]), flavours, toppings);
                                    order.AddIceCream(iceCreamCup);

                                    continue;
                                case "cone":
                                    IceCream iceCreamCone = new Cone(splitLine[4], int.Parse(splitLine[5]), flavours, toppings, bool.Parse(splitLine[6]));
                                    order.AddIceCream(iceCreamCone);

                                    continue;
                                case "waffle":
                                    IceCream iceCreamWaffle = new Waffle(splitLine[4], int.Parse(splitLine[5]), flavours, toppings, splitLine[7]);
                                    order.AddIceCream(iceCreamWaffle);

                                    continue;
                                default:
                                    Console.WriteLine("Error, no such option available");
                                    continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error, no such option available");
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
            void DisplayQueue()
            {
                int x = 1;
                Console.WriteLine("\n------ REGULAR QUEUE ------");
                if (regularQueue.Count > 0)
                {
                    foreach (Order order in regularQueue)
                    {
                        Console.WriteLine($"[{x}] {order.ToString()}");
                        x++;
                    }
                }
                else
                {
                    Console.WriteLine("Nothing in regular queue!");
                }

                int i = 1;
                Console.WriteLine("\n------ GOLD MEMBER QUEUE ------");
                if (goldQueue.Count > 0)
                {
                    foreach (Order order in goldQueue)
                    {
                        Console.WriteLine($"[{i}] {order.ToString()}");
                        i++;
                    }
                }
                else
                {
                    Console.WriteLine("Nothing in gold queue!");
                }
            }

            // Option 3
            void RegisterCustomer()
            {
                Console.Write("Enter a name: ");
                string name = Console.ReadLine().Trim();
                Console.Write("Enter a ID Number (6 digits): ");
                int id = int.Parse(Console.ReadLine());

                if (customerDict.ContainsKey(id))
                {
                    Console.WriteLine("ID already exists!");
                    return;
                }
                else if (Convert.ToString(id).Length != 6)
                {
                    Console.WriteLine("ID is not 6 digits long!");
                    return;
                }

                Console.Write("Enter your date of birth (in dd/mm/YYYY format): ");
                DateTime dob = Convert.ToDateTime(Console.ReadLine());
                
                Customer customer = new Customer(name, id, dob);
                PointCard pointCard = new PointCard(0,0);
                customer.rewards = pointCard;

                using (StreamWriter sw = new StreamWriter("customers.csv", true))
                {
                    sw.WriteLine($"{customer.name},{customer.memberid},{customer.dob},Ordinary,0,0");
                }

                Console.WriteLine("Registration successful!");

                customerDict.Add(customer.memberid,customer);
            }

        }
    }
}
