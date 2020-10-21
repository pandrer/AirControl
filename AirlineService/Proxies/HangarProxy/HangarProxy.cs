using AirlineService.Proxies.HangarProxy.Models;
using HangarService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AirlineService.Proxies.HangarProxy
{
    public class HangarProxy : IHangarProxy
    {
        private readonly ILogger<HangarProxy> _logger;
        private readonly AircraftEntry.AircraftEntryClient _client;

        public HangarProxy(ILogger<HangarProxy> logger, AircraftEntry.AircraftEntryClient client)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<HangarAircraftModel> GetAircraft(string licensePlate)
        {
            var filter = new AircraftsFilter() { LicensePlate = licensePlate };
            var aircraftFromService = _client.GetAircraft(filter);
            var hangarAircraftModel = new HangarAircraftModel()
            {
                LicensePlate = aircraftFromService.LicensePlate,
                Model = aircraftFromService.Model,
                Passengers = aircraftFromService.Passengers,
                Message = aircraftFromService.Message
            };
            _logger.LogInformation($"CALL HANGAR PROXY: GET {licensePlate} Aircraft");
            return hangarAircraftModel;
        }
    }
}
