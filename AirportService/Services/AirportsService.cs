using AirportService.Storage.Interfaz;
using AirportService.Storage.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
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

        public override Task<Empty> AddAirport(AirportRequestModel request, ServerCallContext context)
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

            return Task.FromResult(new Empty());
        }
    }
}
