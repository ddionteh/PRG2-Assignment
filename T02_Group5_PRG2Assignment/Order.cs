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

        public void ModifyIceCream(int id) // Ahmed can check if ID supposed to be -1? TQ
        {
            try
            {
                // HELP LOL 
            }
            catch
            {

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