namespace ICTreats
{
    class Flavour
    {
        public string type { get; set; }

        public bool premium { get; set; }

        public int quantity { get; set; }

        public Flavour() { }

        public Flavour(string type, bool premium, int quantity)
        {
            this.type = type;
            this.premium = premium;
            this.quantity = quantity;
        }

        public override string ToString()
        {
            return "Type: " + type + "\tPremium: " + premium + "\tQuantity: " + quantity;
        }
    }

    class Topping
    {
        public string type { get; set; }

        public Topping() { }

        public Topping(string type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            return "Type: " + type;
        }
    }
}