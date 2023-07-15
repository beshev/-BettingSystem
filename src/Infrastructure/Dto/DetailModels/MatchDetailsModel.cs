namespace Infrastructure.Dto.DetailModels
{
    public class MatchDetailsModel
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public IEnumerable<BetDetailsModel> ActiveBets { get; set; }

        public IEnumerable<BetDetailsModel> InActiveBets { get; set; }
    }
}
