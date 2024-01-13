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
            return currentOrder; // HELP
        }

        public bool IsBirthday()
        {
            return true; // CHANGE
        }

        public override string ToString()
        {
            string orderHistoryString = string.Empty;
            foreach (Order order in orderHistory)
            {
                orderHistoryString = orderHistoryString + '\n' + order.ToString();
            }
            return "Name: " + name + "\tMember ID: " + memberid + "\tDate Of Birth: " + dob + "\tCurrent Order: " + currentOrder + "\tOrder History:\n" + orderHistoryString + "\n Rewards: " + rewards;
        }
    }
}