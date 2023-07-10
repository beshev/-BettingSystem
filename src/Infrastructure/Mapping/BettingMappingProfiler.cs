namespace Infrastructure.Mapping
{
    using AutoMapper;
    using Infrastructure.Constants;
    using Infrastructure.Dto.InputModels;
    using Infrastructure.Dto.UpdateModels;
    using Infrastructure.Dto.ViewModels;
    using Models;

    internal class BettingMappingProfiler : Profile
    {
        public BettingMappingProfiler()
        {
            CreateMap<SportInputModel, Sport>();
            CreateMap<EventInputModel, Event>()
                .ForMember(dest => dest.Matches,
                    opt => opt.MapFrom(x => x.Matches.Where(m => m.MatchType != Models.Enums.MatchType.OutRight)));

            CreateMap<MatchInputModel, Match>();
            CreateMap<BetInputModel, Bet>();
            CreateMap<OddInputModel, Odd>();

            CreateMap<Match, MatchViewModel>()
                .ForMember(dest => dest.Bets,
                                   opt => opt.MapFrom(
                                         src => src.Bets.Where(b => b.Name.Equals(BettingSystemCommonConstants.NameOfMatchWinner)
                                                            || b.Name.Equals(BettingSystemCommonConstants.NameOfMapAdvantage)
                                                            || b.Name.Equals(BettingSystemCommonConstants.NameOfTotalMapsPlayed))));

            CreateMap<Match, MatchViewModel>()
                .ForMember(dest => dest.Bets,
                   opt => opt.MapFrom(
                            src => src.Bets.Where(b => b.Name.Equals(BettingSystemCommonConstants.NameOfMatchWinner)
                                               || b.Name.Equals(BettingSystemCommonConstants.NameOfMapAdvantage)
                                               || b.Name.Equals(BettingSystemCommonConstants.NameOfTotalMapsPlayed))));



            CreateMap<Bet, BetViewModel>()
                .AfterMap((src, dest) =>
                {
                    dest.Odds = dest.Odds.Any(o => o.SpecialBetValue != null)
                        ? dest.Odds.GroupBy(o => o.SpecialBetValue).FirstOrDefault().ToList()
                        : dest.Odds;
                });

            CreateMap<Odd, OddViewModel>();
            CreateMap<Match, MatchDetailsModel>()
                .ForMember(dest => dest.ActiveBets, opt => opt.MapFrom(src => src.Bets.Where(b => b.IsLive)))
                .ForMember(dest => dest.InActiveBets, opt => opt.MapFrom(src => src.Bets.Where(b => !b.IsLive)));

            CreateMap<Match, MatchUpdateModel>();
            CreateMap<Bet, BetUpdateModel>();
            CreateMap<Odd, OddUpdateModel>();
        }
    }
}
