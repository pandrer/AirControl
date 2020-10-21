using AirportService.Storage.Interfaz;
using AirportService.Storage.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirportService.Services
{
    public class AirportsService : AirportEntry.AirportEntryBase
    {

        private readonly ILogger<AirportsService> _logger;
        private readonly IAirportRepository _airportRepository;

        public AirportsService(ILogger<AirportsService> logger, IAirportRepository airportRepository)
        {
            _logger = logger;
            _airportRepository = airportRepository;
        }

        public override Task<DefaultResponse> AddAirport(AirportModel request, ServerCallContext context)
        {
            try
            {
                var airport = new AirportRaw()
                {
                    City = request.City,
                    Country = request.Country,
                    Name = request.Name
                };
                _airportRepository.AddAirport(airport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Add {request.Name} Airport");
            }

            return Task.FromResult(new DefaultResponse());
        }

        public override async Task<AirportResponseModel> GetAirport(AirportFilter request, ServerCallContext context)
        {
            var response = new AirportResponseModel();
            try
            {
                var airport = await _airportRepository.GetAirport(request.City);
                if (airport == null)
                {
                    response.Message = $"Airport {request.City} does not exist";
                    _logger.LogInformation($"Get Airport {request.City} does not exist");
                    return response;
                }
                response.City = airport.City;
                response.Country = airport.Country;
                response.Name = airport.Name;

                _logger.LogInformation($"Get {airport.Name} Aircraft");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Get {request.City} Aircraft");
            }
            return response;
        }

        public override async Task<AirportsResponseModel> GetAllAirports(Empty request, ServerCallContext context)
        {
            var response = new AirportsResponseModel();
            try
            {
                var airpors = (await _airportRepository.GetAllAirports())
                    .Select(x => new AirportModel()
                    {
                        City = x.City,
                        Country = x.Country,
                        Name = x.Name,
                        Id = x.Id
                    });
                _logger.LogInformation($"Get all Aircrafts");

                response.Airports.AddRange(airpors);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Get all Airports");
            }
            return response;
        }
    }
}
