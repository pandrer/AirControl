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

        private static int[] STATUSACCEPT = { 1, 2 };

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

        public override async Task<FligthsResponse> GetFligths(Empty request, ServerCallContext context)
        {
            var response = new FligthsResponse();
            var airports = await _airportProxy.GetAllAirports();
            var aircrafts = await _hangarProxy.GetAircrafts();
            var fligths = await _airlineRepository.GetFligths();
            var routes = await _airlineRepository.GetFligthRoutes();

            foreach (var fligth in fligths)
            {
                var fligthModel = new FligthModel();
                fligthModel.Aircraft = fligth.Aircraft;
                var route = routes.Where(x => x.Id == fligth.FligthRoute).FirstOrDefault();
                fligthModel.Source = airports.Where(x => x.Id == route.Source).FirstOrDefault().City;
                fligthModel.Target = airports.Where(x => x.Id == route.Target).FirstOrDefault().City;
                fligthModel.State = fligth.State switch
                {
                    1 => "CURRENTLY-FLYING",
                    2 => "CANCELED",
                    _ => "SCHEDULED"
                };
                response.Fligths.Add(fligthModel);
            }

            return response;
        }

        public override async Task<FligthsResponse> GetFligthsFilter(FligthFilter request, ServerCallContext context)
        {
            var fligthsResponse = new FligthsResponse();


            return fligthsResponse;
        }

        public override async Task<RoutesResponse> GetRoutes(Empty request, ServerCallContext context)
        {
            var response = new RoutesResponse();
            try
            {
                var airports = await _airportProxy.GetAllAirports();
                var routes = (await _airlineRepository.GetFligthRoutes())
                    .Select(x => new RouteModel()
                    {
                        Source = airports.Where(y => y.Id == x.Source).FirstOrDefault().City,
                        Target = airports.Where(y => y.Id == x.Target).FirstOrDefault().City,
                    });
                _logger.LogInformation($"Get all Routes");

                response.Routes.AddRange(routes);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Get all Routes");
            }
            return response;
        }

        public override async Task<DefaultResponse> UpdateFligthStatus(ControlTowerRequestModel request, ServerCallContext context)
        {
            var response = new DefaultResponse();
            try
            {

                if (!STATUSACCEPT.Where(x => x == request.Status).Any())
                {
                    response.Message = $"Status {request.Status} is not vaild";
                    return response;
                }
                
                _airlineRepository.UpdateFligthStatus(request.FligthId, request.Status);

                var statusString = request.Status switch
                {
                    1 => "CURRENTLY-FLYING",
                    2 => "CANCELED",
                    _ => "SCHEDULED"
                };

                response.Message = $"Fligth status changed to {statusString}";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Update fligth status");
            }
            return response;
        }

    }
}
