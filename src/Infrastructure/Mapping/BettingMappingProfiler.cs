namespace Infrastructure.Mapping
{
    using AutoMapper;
    using Infrastructure.Constants;
    using Infrastructure.Dto.DetailModels;
    using Infrastructure.Dto.InputModels;
    using Infrastructure.Dto.UpdateModels;
    using Infrastructure.Dto.ViewModels;
    using Models;

    internal class BettingMappingProfiler : Profile
    {
        public BettingMappingProfiler()
        {
            // Input models
            CreateMap<SportInputModel, Sport>();
            CreateMap<EventInputModel, Event>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Matches,
                    opt => opt.MapFrom(x => x.Matches.Where(m => m.MatchType != Models.Enums.MatchType.OutRight)));

            CreateMap<MatchInputModel, Match>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<BetInputModel, Bet>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<OddInputModel, Odd>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // View models
            CreateMap<Match, MatchViewModel>()
                .ForMember(dest => dest.Bets, opt => opt.MapFrom(src => src.Bets
                    .Where(b => (b.Name.Equals(BettingSystemCommonConstants.NameOfMatchWinner) 
                              || b.Name.Equals(BettingSystemCommonConstants.NameOfMapAdvantage) 
                              || b.Name.Equals(BettingSystemCommonConstants.NameOfTotalMapsPlayed)) && b.IsActive)));

            CreateMap<Odd, OddViewModel>();
            CreateMap<Bet, BetViewModel>()
                .ForMember(dest => dest.Odds, opt => opt.MapFrom(src => src.Odds.Where(o => o.IsActive)))
                .AfterMap((src, dest) =>
                {
                    dest.Odds = dest.Odds.Any(o => o.SpecialBetValue != null)
                        ? dest.Odds.GroupBy(o => o.SpecialBetValue).FirstOrDefault().ToList()
                        : dest.Odds;
                });

            // Details models
            CreateMap<Match, MatchDetailsModel>()
                .ForMember(dest => dest.ActiveBets, opt => opt.MapFrom(src => src.Bets.Where(b => b.IsActive)
                    .Select(b => new BetDetailsModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Odds = b.Odds
                        .Where(o => o.IsActive)
                        .Select(o => new OddDetailsModel
                        {
                            Id = o.Id,
                            Name = o.Name,
                            Value = o.Value,
                            SpecialBetValue = o.SpecialBetValue
                        })
                    })))
                .ForMember(dest => dest.InActiveBets, opt => opt.MapFrom(src => src.Bets.Where(b => !b.IsActive)));

            CreateMap<Bet, BetDetailsModel>();
            CreateMap<Odd, OddDetailsModel>();

            // Update models
            CreateMap<Match, MatchUpdateModel>();
            CreateMap<Bet, BetUpdateModel>();
            CreateMap<Odd, OddUpdateModel>();
        }
    }
}
