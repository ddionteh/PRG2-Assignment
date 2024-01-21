namespace ICTreats
{
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
            return $"Type: {type}";
        }
    }
}