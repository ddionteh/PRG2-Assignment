namespace ICTreats
{
    class PointCard
    {
        public int points { get; set; }

        public int punchCard { get; set; }

        public string tier { get; set; }

        public PointCard() { }

        public PointCard(int points, int punchCard)
        {
            this.points = points;
            this.punchCard = punchCard;
        }

        public void AddPoints(int points)
        {
            this.points += points; // CHANGE ?
        }

        public void RedeemPoints(int points)
        {
            this.points -= points; // CHANGE ?
        }

        public void Punch()
        {
            // what is this?
        }

        public override string ToString()
        {
            return "Points: " + points + "\tPunch Card: " + punchCard + "\tTier: " + tier;
        }



    }
}