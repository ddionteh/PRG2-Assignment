//==========================================================
// Student Number : S10258624J
// Student Name : Diontae Low
// Partner Name : Ahmed Uzair
//==========================================================
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

            if (points >= 100)
            {
                tier = "Gold";
            }
            else if (points >= 50) 
            {
                tier = "Silver";
            }
            else
            {
                tier = "Ordinary";
            }
        }

        public void AddPoints(int Points)
        {
            points += (int)Math.Floor(Points * 0.72);

            if (points >= 100)
            {
                tier = "Gold";
            }
            else if (points >= 50)
            {
                tier = "Silver";
            }
        }

        public void RedeemPoints(int points)
        {
            this.points -= points; // CHANGE ?
        }

        public void Punch()
        {
            if (punchCard == 10)
            {
                punchCard = 0;
                return;
            }
            punchCard++;
        }

        public override string ToString()
        {
            return $"Points: {points} Punch Card: {punchCard} Tier: {tier}";
        }



    }
}