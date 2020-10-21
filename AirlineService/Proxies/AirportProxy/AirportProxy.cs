using AirlineService.Proxies.AirportProxy.Models;
using AirportService;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineService.Proxies.AirportProxy
{
    public class AirportProxy : IAirportProxy
    {
        private readonly ILogger<AirportProxy> _logger;
        private readonly AirportEntry.AirportEntryClient _client;

        public AirportProxy(ILogger<AirportProxy> logger, AirportEntry.AirportEntryClient client)
        {
            _client = client;
            _logger = logger;
        }


        public async Task<IList<AirportProxyModel>> GetAllAirports()
        {
            var response = _client.GetAllAirports(new AirportService.Empty());
            return response.Airports
                .Select(x => new AirportProxyModel()
                {
                    City = x.City,
                    Country = x.Country,
                    Name = x.Name,
                    Id = x.Id
                }).ToList();
        }
    }
}
