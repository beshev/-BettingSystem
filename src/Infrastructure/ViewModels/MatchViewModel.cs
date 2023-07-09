namespace Infrastructure.ViewModels
{
    public class MatchViewModel
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public IEnumerable<BetViewModel> Bets { get; set; }
    }
}
