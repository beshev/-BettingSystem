namespace Models
{
    public class Odd : BaseModel<int>
    {
        public double Value { get; set; }

        public double? SpecialBetValue { get; set; }

        public int BetId { get; set; }

        public virtual Bet Bet { get; set; }
    }
}
