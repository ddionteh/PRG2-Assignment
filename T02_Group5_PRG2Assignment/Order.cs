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
            // help us!
        }

        public void AddIceCream(IceCream iceCream)
        {
            // OH NO !
        }

        public void DeleteIceCream(int id)
        {
            // GG WP !
        }
        public double CalculateTotal()
        {
            return 0.0; // GG
        }

        public override string ToString()
        {   
            string iceCreamString = string.Empty;
            foreach (IceCream iceCream in iceCreamList)
            {
                iceCreamString = iceCreamString + '\n' + iceCream; 
            }
            return "ID: " + id + "\tTime Received: " + timeReceived + "\tTime Fulfilled: " + timeFulfilled + iceCreamString;
        }
    }
}