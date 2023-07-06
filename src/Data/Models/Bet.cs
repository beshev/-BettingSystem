namespace Models
{
    public class Bet : BaseModel<int>
    {
        public Bet()
        {
            Odds = new HashSet<Odd>();
        }

        public bool IsLive { get; set; }

        public int MatchId { get; set; }

        public virtual Match Match { get; set; }

        public virtual ICollection<Odd> Odds { get; set; }
    }
}
