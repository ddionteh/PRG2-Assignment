//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================

using ICTreats;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        public static Dictionary<string, double> waffleFlavour = new Dictionary<string, double>() { { "original", 0 } };

        // stores the type of ice cream (options) available as key and value
        // hard coded this bcuz a new ice cream class needs to be manually created to work anyway
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
            int maxScoops = 3;

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
                    case 3: // Option 3
                        RegisterCustomer();
                        break;
                    case 4: // Option 4
                        CreateCustomerOrder();
                        break;
                    case 5: // Option 5
                        DisplayCustomerOrderDetails();
                        break;
                    case 6: // Option 6
                        ModifyOrderDetails();
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
                        Customer customer = new Customer(splitLine[0], int.Parse(splitLine[1]), DateTime.ParseExact(splitLine[2], "dd/MM/yyyy", CultureInfo.InvariantCulture));
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

                        // assuming minimum scoop is 1
                        for (int i = 8; i < 8 + maxScoops; i++)
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
                                    flavours.Add(new Flavour(splitLine[i], premium, 1));
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
                        for (int i = 8 + maxScoops; i < splitLine.Length; i++)
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
                Console.WriteLine($"{"Name",-15}{"Member ID",-10}{"Date Of Birth",-15}{"Membership Status",-20}{"Membership Points",-20}{"Punch Card",-10}");
                foreach (Customer customer in customerDict.Values)
                {
                    Console.WriteLine($"{customer.name,-15}{customer.memberid,-10}{customer.dob.ToShortDateString(),-15}{customer.rewards.tier,-20}{customer.rewards.points,-20}{customer.rewards.punchCard,-10}");
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
                DateTime dob = DateTime.Parse(Console.ReadLine()).Date;

                Customer customer = new Customer(name, id, dob);
                PointCard pointCard = new PointCard(0, 0);
                customer.rewards = pointCard;

                using (StreamWriter sw = new StreamWriter("customers.csv", true))
                {
                    sw.WriteLine($"{customer.name},{customer.memberid},{customer.dob},Ordinary,0,0");
                }

                Console.WriteLine("Registration successful!");

                customerDict.Add(customer.memberid, customer);
            }

            // Option 4
            void CreateCustomerOrder()
            {
                DisplayAllCustomers();

                Console.Write("Enter a customer ID to be retrieved: ");
                int id = int.Parse(Console.ReadLine());

                Customer retrievedCustomer = customerDict[id];
                Order order = retrievedCustomer.MakeOrder();
                order.timeReceived = DateTime.Now.Date;
                order.id = regularQueue.Count() + goldQueue.Count() + 1;
                AddIceCream(retrievedCustomer, maxScoops);

                while (true)
                {
                    Console.Write("Would you like to add another ice cream to the order? (Y/N): ");
                    string addAnotherAnswer = Console.ReadLine().Trim().ToUpper();

                    if (addAnotherAnswer == "Y")
                    {
                        AddIceCream(retrievedCustomer, maxScoops);
                        continue;
                    }
                    else if (addAnotherAnswer == "N")
                    {
                        break;
                    }
                    Console.WriteLine("Please enter 'Y' for Yes, 'N' for No!");
                }

                if (retrievedCustomer.rewards.tier == "Gold")
                {
                    goldQueue.Enqueue(retrievedCustomer.currentOrder);
                    Console.WriteLine("Order has been made successfully! Currently queued in gold queue.");
                }
                else
                {
                    regularQueue.Enqueue(retrievedCustomer.currentOrder);
                    Console.WriteLine("Order has been made successfully! Currently queued in regular queue.");
                }
            } 

            void AddIceCream(Customer customer, int maxScoop)
            {   
                List<Flavour> flavourList = new List<Flavour>();
                List<Topping> toppingList = new List<Topping>();

                int x = 1;
                foreach (string iceCream in optionsAvailable.Keys)
                {
                    Console.WriteLine($"Option {x}: {CapitalizeFirstLetter(iceCream)}");
                    x++;
                }
                Console.Write("Enter your option (name): ");
                string option = Console.ReadLine().Trim();

                Console.Write($"Enter the number of scoops you want, from 1 - {maxScoop} scoops: ");
                int scoop = int.Parse(Console.ReadLine());

                // list out the available flavours
                foreach (KeyValuePair<string,bool> flavour in flavourDict)
                {   
                    if (flavour.Value == true)
                    {
                        Console.WriteLine($"Premium Flavour: {CapitalizeFirstLetter(flavour.Key)}");
                        x++;
                        continue;
                    }
                    Console.WriteLine($"Regular Flavour: {CapitalizeFirstLetter(flavour.Key)}");
                }

                for (int i = 0; i < scoop; i++)
                {
                    Console.Write($"Enter the name of the flavour (no repeating): ");
                    string flavour = Console.ReadLine().Trim().ToLower();

                    // checks if flavour already selected by user
                    // returns null if its a new flavour, otherwise a reference to it
                    Flavour flavourToUpdate = flavourList.FirstOrDefault(flavourInList => flavourInList.type == flavour);

                    // +1 to a pre-existing flavour in user selection
                    if (flavourToUpdate != null)
                    {
                        flavourToUpdate.quantity += 1;
                    }
                    else if (flavourDict.ContainsKey(flavour)) // new flavour and is an available option
                    {
                        Flavour newFlavour = new Flavour(flavour, flavourDict[flavour], 1);
                        flavourList.Add(newFlavour);
                        continue;
                    }
                   
                    Console.WriteLine("Flavour chosen does not exist!");
                    i--;
                }

                // reassigning value to 1 for the same variable rather than creating new variables to save space
                x = 1;
                // list out the available toppings
                foreach (string topping in toppingDict.Keys)
                {
                    Console.WriteLine($"Topping {x}: {topping.ToString()}");
                    x++;
                }

                Console.Write($"How many toppings would you like? (0 - {x-1})");
                int totalToppings = int.Parse(Console.ReadLine());

                for (int i = 0; i < totalToppings; i++)
                {
                    Console.Write("Enter the name of the topping (no repeating): ");
                    string topping = Console.ReadLine().Trim().ToLower();

                    // checks if topping already selected by user
                    bool toppingAlreadyChosen = toppingList.Any(toppingInList => toppingInList.type == topping);

                    if (toppingAlreadyChosen)
                    {
                        Console.WriteLine("Topping already chosen! Choose another one!");
                        i--;
                        continue;
                    }
                    else if (toppingDict.ContainsKey(topping))
                    {
                        Topping newTopping = new Topping(topping);
                        toppingList.Add(newTopping);
                        continue;
                    }
                   
                    Console.WriteLine("Topping chosen does not exist!");
                    i--;
                }

                // looking for a more efficient way to do this, a similar method done above in OrderCreation() function
                switch (option)
                {
                    case "cup":
                        IceCream iceCreamCup = new Cup(option, scoop, flavourList, toppingList);
                        customer.currentOrder.AddIceCream(iceCreamCup);
                        break;
                    case "cone":
                        Console.Write("Would you like to upgrade your ice cream cone to chocolate-dipped cone? (Y/N): ");
                        string dippedAnswer = Console.ReadLine().Trim().ToUpper();

                        bool dipped;
                        while(true)
                        {
                            if (dippedAnswer == "Y")
                            {
                                dipped = true;
                                break;
                            }
                            else if (dippedAnswer == "N")
                            {
                                dipped = false;
                                break;
                            }
                            Console.WriteLine("Enter 'Y' for Yes and 'N' for No!");
                        }

                        IceCream iceCreamCone = new Cone(option, scoop, flavourList, toppingList, dipped);
                        customer.currentOrder.AddIceCream(iceCreamCone);
                        break;
                    case "waffle":
                        Console.Write("Would you like to add-on a flavour for your waffle? (Y/N): ");
                        string waffleFlavourAnswer = Console.ReadLine().Trim().ToUpper();

                        while (true)
                        {
                            if (waffleFlavourAnswer == "Y")
                            {
                                Console.WriteLine("Waffle Flavours available: ");
                                foreach (KeyValuePair<string,double> flavourAndCost in waffleFlavour)
                                {
                                    if (flavourAndCost.Value > 0)
                                    {
                                        Console.WriteLine(CapitalizeFirstLetter(flavourAndCost.Key));
                                    }
                                }

                                Console.Write("Enter the waffle flavour you want: ");
                                string selectedWaffleFlavour = Console.ReadLine().Trim().ToLower();
                                
                                // might not need original check here, see how later
                                if (waffleFlavour.ContainsKey(selectedWaffleFlavour) && selectedWaffleFlavour != "original")
                                {
                                    IceCream iceCreamWaffle = new Waffle(option, scoop, flavourList, toppingList, selectedWaffleFlavour);
                                    customer.currentOrder.AddIceCream(iceCreamWaffle);
                                    break;
                                }
                                Console.WriteLine("Enter a valid waffle flavour!");
                                continue;
                            }
                            else if (waffleFlavourAnswer == "N")
                            {
                                IceCream iceCreamWaffle = new Waffle(option, scoop, flavourList, toppingList, "original");
                                customer.currentOrder.AddIceCream(iceCreamWaffle);
                                break;
                            }
                            Console.WriteLine("Enter 'Y' for Yes, 'N' for No!");
                        }
                        break;
                }
            }

            // Option 5
            void DisplayCustomerOrderDetails()
            {
                DisplayAllCustomers();

                Console.Write("Enter the Member ID of the customer (6 digits): ");
                int id = int.Parse(Console.ReadLine());

                Customer selectedCustomer = customerDict[id];

                Console.WriteLine($"Current Order: {selectedCustomer.currentOrder.ToString()}");

                Console.WriteLine("Order History: ");
                foreach (Order pastOrder in selectedCustomer.orderHistory)
                {
                    Console.WriteLine(pastOrder.ToString());
                }
            }

            // Option 6
            void ModifyOrderDetails()
            {
                DisplayAllCustomers();

                Console.Write("Select a customer by ID: ");

                // ahmed u can seperate for validation if uw
                Customer selectedCustomer = customerDict[int.Parse(Console.ReadLine())]; 

                if (selectedCustomer.currentOrder == null)
                {
                    Console.WriteLine("There is no order currently!");
                    return;
                }

                int iceCreamNumber = 1;
                foreach (IceCream iceCream in selectedCustomer.currentOrder.iceCreamList)
                {
                    Console.WriteLine($"Ice Cream {iceCreamNumber}: {iceCream.ToString()}");
                    iceCreamNumber++;
                }


                Console.WriteLine("[1] Modify an existing ice cream \n[2] Add new ice cream to order \n[3] Delete existing ice cream from order \n[0] Exit to main menu");

                Console.Write("Choose one of the options above: ");
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0:
                        break;
                    case 1:
                        Console.Write("Select the numerical value of the ice cream to modify: ");
                        int selectedIceCreamNumber = int.Parse(Console.ReadLine());

                        selectedCustomer.currentOrder.ModifyIceCream(selectedIceCreamNumber - 1); // still broken, fix it later
                        break;
                    case 2:
                        AddIceCream(selectedCustomer, maxScoops); // i think this shld work, idk bro its 3 am
                        break;
                    case 3:
                        while (true)
                        {
                            Console.Write("Select the numerical value of the ice cream to delete: ");
                            int iceCreamDelete = int.Parse(Console.ReadLine());  // validate it to be within the scope

                            if (selectedCustomer.currentOrder.iceCreamList.Count == 1)
                            {
                                Console.WriteLine("Not allowed to remove the only ice cream in the order.");
                                continue;
                            }
                            else if (selectedCustomer.currentOrder.iceCreamList.Count == 0)
                            {
                                Console.WriteLine("There is no ice cream to delete!");
                                break;
                            }
                            selectedCustomer.currentOrder.DeleteIceCream(iceCreamDelete - 1);
                            break;
                        }
                        Console.WriteLine("--------Updated Order--------");
                        Console.WriteLine(selectedCustomer.currentOrder.ToString());
                        break;
                }

            }
        }
    }
}
