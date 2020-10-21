using Grpc.Core;
using HangarService.Storage.Interfaz;
using HangarService.Storage.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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

        public override Task<DefaultResponse> AddAircraft(AircraftRequestModel request, ServerCallContext context)
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

            return Task.FromResult(new DefaultResponse());
        }

        public override async Task<AircraftRequestModel> GetAircraft(AircraftsFilter request, ServerCallContext context)
        {
            var response = new AircraftRequestModel();
            try
            {
                var aircrafts = await _aircraftRepository.GetAirCraft(request.LicensePlate);
                if (aircrafts == null)
                {
                    response.Message = $"Aircraft {request.LicensePlate} does not exist";
                    _logger.LogInformation($"Get Aircraft {request.LicensePlate} does not exist");
                    return response;
                }
                response.LicensePlate = aircrafts.LicensePlate;
                response.Model = aircrafts.Model;
                response.Passengers = aircrafts.Passengers;

                _logger.LogInformation($"Get {aircrafts.LicensePlate} Aircraft");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Get {request.LicensePlate} Aircraft");
            }
            return response;
        }

        public override async Task<AircraftsResponseModel> GetAllAircrafts(Empty request, ServerCallContext context)
        {
            var response = new AircraftsResponseModel();
            try
            {
                var aircrafts = (await _aircraftRepository.GetAllAirCraft())
                    .Select(x => new AircraftModel()
                    {
                        LicensePlate = x.LicensePlate,
                        Model = x.Model,
                        Passengers = x.Passengers
                    });
                _logger.LogInformation($"Get all Aircrafts");

                response.Aircrafts.AddRange(aircrafts);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Get all Aircrafts");
            }
            return response;
        }

    }
}
