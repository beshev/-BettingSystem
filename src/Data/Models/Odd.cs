namespace Models
{
    public class Odd : BaseModel<int>
    {
        public string Value { get; set; }

        public string SpecialBetValue { get; set; }

        public int BetId { get; set; }

        public virtual Bet Bet { get; set; }
    }
}
