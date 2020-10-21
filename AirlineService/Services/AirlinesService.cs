using AirlineService.Proxies.AirportProxy;
using AirlineService.Proxies.HangarProxy;
using AirlineService.Storage.Interfaz;
using AirlineService.Storage.Models;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineService.Services
{
    public class AirlinesService : AirlineEntry.AirlineEntryBase
    {
        private readonly ILogger<AirlinesService> _logger;
        private readonly IAirlineRepository _airlineRepository;
        private readonly IHangarProxy _hangarProxy;
        private readonly IAirportProxy _airportProxy;

        public AirlinesService(ILogger<AirlinesService> logger, IAirlineRepository airlineRepository, IHangarProxy hangarProxy, IAirportProxy airportProxy)
        {
            _logger = logger;
            _airlineRepository = airlineRepository;
            _hangarProxy = hangarProxy;
            _airportProxy = airportProxy;
        }


        public override async Task<DefaultResponse> AddFlight(FlightRequestModel request, ServerCallContext context)
        {
            var result = new DefaultResponse();
            try
            {
                var aircraft = await _hangarProxy.GetAircraft(request.Aircraft);
                if (string.IsNullOrEmpty(aircraft?.LicensePlate))
                {
                    result.Message = aircraft.Message;
                    return result;
                }

                var airports = await _airportProxy.GetAllAirports();

                if (!airports.Where(x => x.City == request.Source).Any())
                {
                    result.Message = $"There is no connection with the airport in {request.Source}";
                    return result;
                }

                if (!airports.Where(x => x.City == request.Target).Any())
                {
                    result.Message = $"There is no connection with the airport in {request.Target}";
                    return result;
                }

                var flightRoute = await _airlineRepository.GetFligthRoute(airports.Where(x => x.City == request.Source).FirstOrDefault().Id, airports.Where(x => x.City == request.Target).FirstOrDefault().Id);

                if (flightRoute == null)
                {
                    result.Message = $"There is no air route between {request.Source} and {request.Target}";
                    return result;
                }

                var flight = new FlightRaw()
                {
                    Aircraft = request.Aircraft,
                    FligthRoute = flightRoute.Id,
                    State = request.State
                };
                _airlineRepository.AddFligth(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Adding Flight");
            }

            return result;
        }

        public override async Task<DefaultResponse> AddFlightRoute(FlightRouteRequestModel request, ServerCallContext context)
        {
            var result = new DefaultResponse();
            try
            {
                var airports = await _airportProxy.GetAllAirports();

                if (!airports.Where(x => x.City == request.Source).Any())
                {
                    result.Message = $"There is no connection with the airport in {request.Source}";
                    return result;
                }

                if (!airports.Where(x => x.City == request.Target).Any())
                {
                    result.Message = $"There is no connection with the airport in {request.Target}";
                    return result;
                }

                var fligthRoute = new FlightRouteRaw()
                {
                    Source = airports.Where(x => x.City == request.Source).FirstOrDefault().Id,
                    Target = airports.Where(x => x.City == request.Target).FirstOrDefault().Id,
                    EstimatedTime = request.EstimatedTime
                };
                _airlineRepository.AddFligthRoute(fligthRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Adding FligthRoute");
            }

            return result;
        }

    }
}
