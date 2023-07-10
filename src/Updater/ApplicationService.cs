namespace Updater
{
    using Infrastructure.Dto.InputModels;
    using Infrastructure.Services;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Xml.Serialization;

    internal class ApplicationService : BackgroundService
    {
        private readonly ISportsService _sportsService;
        private readonly ILogger _logger;

        public ApplicationService(
            ISportsService sportsService,
            ILogger<ApplicationService> logger)
        {
            _sportsService = sportsService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (true)
                {
                    var sports = await GetAllSportsAsync();
                    if (await _sportsService.IsExistsAsync(sports.Sport.Id))
                    {
                        await _sportsService.UpdateAsync(sports.Sport);
                    }
                    else
                    {
                        await _sportsService.CreateAsync(sports.Sport);
                    }

                    var timeSpan = TimeSpan.FromSeconds(60);
                    await Task.Delay(timeSpan, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException?.Message);
            }
        }

        private async Task<XmlSportInputModel> GetAllSportsAsync()
        {
            var url = "https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7";
            var client = new HttpClient();
            var httpMassage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(httpMassage);
            if (!response.IsSuccessStatusCode)
            {
                // Do some logic.
            }

            var responseAsString = await response.Content.ReadAsStringAsync();

            var xmlSerializer = new XmlSerializer(typeof(XmlSportInputModel), new XmlRootAttribute("XmlSports"));
            using var reader = new StringReader(responseAsString);
            var resposeAsObject = (XmlSportInputModel)xmlSerializer.Deserialize(reader);

            return resposeAsObject;
        }
    }
}
