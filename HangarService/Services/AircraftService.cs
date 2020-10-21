using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using HangarService.Storage.Interfaz;
using HangarService.Storage.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HangarService.Services
{
    public class AircraftService : AircraftEntry.AircraftEntryBase
    {
        private readonly ILogger<AircraftService> _logger;
        private readonly IAircraftRepository _aircraftRepository;

        public AircraftService(ILogger<AircraftService> logger, IAircraftRepository aircraftRepository)
        {
            _logger = logger;
            _aircraftRepository = aircraftRepository;
        }

        public override Task<Empty> AddAircraft(AircraftRequestModel request, ServerCallContext context)
        {
            try
            {
                var aircraft = new AircraftRaw()
                {
                    LicensePlate = request.LicensePlate,
                    Model = request.Model,
                    Passengers = request.Passengers
                };
                _aircraftRepository.AddAirCraft(aircraft);
                _logger.LogInformation($"Add {request.LicensePlate} Aircraft");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Add {request.LicensePlate} Aircraft");
            }

            return Task.FromResult(new Empty());
        }
    }
}
