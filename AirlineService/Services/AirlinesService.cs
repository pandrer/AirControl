using AirlineService.Storage.Interfaz;
using AirlineService.Storage.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AirlineService.Services
{
    public class AirlinesService : AirlineEntry.AirlineEntryBase
    {
        private readonly ILogger<AirlinesService> _logger;
        private readonly IAirlineRepository _airlineRepository;

        public AirlinesService(ILogger<AirlinesService> logger, IAirlineRepository airlineRepository)
        {
            _logger = logger;
            _airlineRepository = airlineRepository;
        }


        public override Task<Empty> AddFlight(FlightRequestModel request, ServerCallContext context)
        {
            try
            {
                var flight = new FlightRaw()
                {
                    Aircraft = request.Aircraft,
                    FligthRoute = request.FligthRoute,
                    State = request.State
                };
                _airlineRepository.AddFligth(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Adding Flight");
            }

            return Task.FromResult(new Empty());
        }

        public override Task<Empty> AddFlightRoute(FlightRouteRequestModel request, ServerCallContext context)
        {
            try
            {
                var fligthRoute = new FlightRouteRaw()
                {
                    Source = request.Source,
                    Target = request.Target,
                    EstimatedTime = request.EstimatedTime
                };
                _airlineRepository.AddFligthRoute(fligthRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Adding FligthRoute");
            }

            return Task.FromResult(new Empty());
        }

    }
}
