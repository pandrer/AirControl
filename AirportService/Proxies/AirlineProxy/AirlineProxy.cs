using AirlineService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AirportService.Proxies.AirlineProxy
{
    public class AirlineProxy : IAirlineProxy
    {

        private readonly ILogger<AirlineProxy> _logger;
        private readonly AirlineEntry.AirlineEntryClient _client;

        public AirlineProxy(ILogger<AirlineProxy> logger, AirlineEntry.AirlineEntryClient client)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<string> UpdateFligthStatus(int fligthId, int status)
        {
            var response = _client.UpdateFligthStatus(new AirlineService.ControlTowerRequestModel() { FligthId = fligthId, Status = status });
            return response.Message;
        }
    }
}
