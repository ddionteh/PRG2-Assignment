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
            Order order = new Order(); // I cant use the 2 parameters constructor here, need some help Ahmed
            currentOrder = order;
            orderHistory.Add(order);

            return order;
        }

        public bool IsBirthday()
        {
            return (currentOrder.timeReceived.Date == dob.Date); // returns true if the customer orders on their birthday
        }

        public override string ToString()
        {
            // Memory efficient and performs better than regular string concatenation 
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Name: {name} Member ID: {memberid} Date Of Birth: {dob} \nCurrent Order: {currentOrder.ToString()} \nOrder History:");

            // Checks if there's any orders in the past
            if (orderHistory.Any())
            {
                stringBuilder.AppendLine();
                foreach (Order order in orderHistory)
                {
                    stringBuilder.AppendLine(order.ToString());
                }
            }

            return stringBuilder.ToString();
        }
    }
}