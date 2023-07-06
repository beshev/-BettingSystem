namespace Models
{
    public class Match : BaseModel<int>
    {
        public Match()
        {
            Bets = new HashSet<Bet>();
        }

        public DateTime StartDate { get; set; }

        public Enums.MatchType MatchType { get; set; }

        public int EventId { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
