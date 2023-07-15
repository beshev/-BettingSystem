namespace Infrastructure.Dto.DetailModels
{
    public class BetDetailsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public IEnumerable<OddDetailsModel> Odds { get; set; }
    }
}
