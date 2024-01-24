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
            return (currentOrder.timeReceived.Date == dob.Date); // returns true if the customer orders on their birthday
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Name: {name} Member ID: {memberid} Date Of Birth: {dob.ToShortDateString()} ");

            if (currentOrder != null)
            {
                stringBuilder.Append($"\nCurrent Order: {currentOrder}");
            }
            else
            {
                stringBuilder.Append("\nCurrent Order: None");
            }

            stringBuilder.Append("\nOrder History:");
            if (orderHistory.Any())
            {
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