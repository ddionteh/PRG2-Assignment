﻿//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================
using static System.Formats.Asn1.AsnWriter;
using System.Text;

namespace ICTreats
{
    class Order
    {
        public int id {  get; set;}

        public DateTime timeReceived { get; set;}

        // Find out how this works !!!!
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
           void ModifyOption()
           {
                Console.WriteLine("Options available:");
               // foreach (string option in option)

            }
           IceCream modifyIceCream = iceCreamList[id-1];

           if (modifyIceCream is Cup)
            {
                Console.WriteLine("[1] Modify option \n[2] Modify number of scoops \n[3] Modify flavours \n[4] Modify toppings");
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
                            break;
                        case 3:
                            break;
                        case 4:
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
            }
           else if (modifyIceCream is Cone)
            {
                Console.WriteLine("[1] Modify option \n[2] Modify number of scoops \n[3] Modify flavours \n[4] Modify toppings" +
                "\n[5] Modify dipped cone");
                Console.Write("Enter the number for the modifications to make to the ice cream: ");

                try
                {
                    int modifyOption = int.Parse(Console.ReadLine());
                    switch (modifyOption)
                    {
                        case 1:

                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
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
            }
           else
            {
                Console.WriteLine("[1] Modify option \n[2] Modify number of scoops \n[3] Modify flavours \n[4] Modify toppings" +
                "\n[5] Modify waffle flavour");
                Console.Write("Enter the number for the modifications to make to the ice cream: ");

                try
                {
                    int modifyOption = int.Parse(Console.ReadLine());
                    switch (modifyOption)
                    {
                        case 1:

                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
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
                foreach (IceCream iceCream in iceCreamList)
                {
                    stringBuilder.AppendLine(iceCream.ToString());
                }
            }

            return stringBuilder.ToString();
        }
    }
}