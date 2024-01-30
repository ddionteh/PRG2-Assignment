//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================
using static System.Formats.Asn1.AsnWriter;
using System.Text;

namespace ICTreats
{
    class Customer
    {
        public string name { get; set; }

        public int memberid { get; set; }

        public DateTime dob { get; set; }

        public Order currentOrder { get; set; }

        public List<Order> orderHistory { get; set; } = new List<Order>();

        public PointCard rewards { get; set; } 

        public Customer() { }

        public Customer(string name, int memberid, DateTime dob)
        {
            this.name = name;
            this.memberid = memberid;
            this.dob = dob;
        }

        public Order MakeOrder()
        {
            Order order = new Order(); 
            currentOrder = order;
            orderHistory.Add(order);

            return order; // reference-type
        }

        public bool IsBirthday()
        {
            // returns true if the customer orders on their birthday
            return ((currentOrder.timeReceived.Month == dob.Month) && (currentOrder.timeReceived.Day == dob.Day)); 
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Name: {Program.CapitalizeFirstLetter(name)} Member ID: {memberid} Date Of Birth: {dob.ToShortDateString()} ");

            if (currentOrder != null)
            {
                stringBuilder.Append($"\nCurrent Order: \n{currentOrder.ToString()}");
            }
            else
            {
                stringBuilder.Append("\nCurrent Order: None");
            }

            stringBuilder.Append("\nOrder History:");
            if (orderHistory.Any())
            {
                stringBuilder.Append('\n');
                foreach (Order order in orderHistory)
                {
                    stringBuilder.AppendLine(order.ToString());
                }
            }
            else
            {
                stringBuilder.AppendLine(" None");
            }

            return stringBuilder.ToString();
        }

    }
}