namespace Models
{
    public class Sport : BaseModel<int>
    {
        public Sport()
        {
            Events = new HashSet<Event>();
        }

        public virtual ICollection<Event> Events { get; set; }
    }
}
