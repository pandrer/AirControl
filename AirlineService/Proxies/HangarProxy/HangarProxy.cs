using AirlineService.Proxies.HangarProxy.Models;
using HangarService;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<HangarAircraftModel>> GetAircrafts()
        {
            var aircraftsFromService = _client.GetAllAircrafts(new HangarService.Empty());

            var aircrafts = aircraftsFromService.Aircrafts.Select(x => new HangarAircraftModel()
            {
                LicensePlate = x.LicensePlate,
                Model = x.Model,
                Passengers = x.Passengers,
            }).ToList();
            _logger.LogInformation($"CALL HANGAR PROXY: GET Aircrafts");
            return aircrafts;
        }
    }
}
