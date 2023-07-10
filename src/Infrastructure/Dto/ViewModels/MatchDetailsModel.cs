namespace Infrastructure.Dto.ViewModels
{
    public class MatchDetailsModel
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public IEnumerable<BetViewModel> ActiveBets { get; set; }

        public IEnumerable<BetViewModel> InActiveBets { get; set; }
    }
}
