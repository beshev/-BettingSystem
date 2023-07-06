namespace Models
{
    public class Event : BaseModel<int>
    {
        public Event()
        {
            Matches = new HashSet<Match>();
        }

        public bool IsLive { get; set; }

        public int CategoryId { get; set; }

        public int SportId { get; set; }

        public virtual Sport Sport { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
