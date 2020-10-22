using AirlineService.Storage.Interfaz;
using AirlineService.Storage.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineService.Storage.Repositories
{
    public class AirlineRepository : IAirlineRepository
    {
        private readonly AirlineContext _context;

        public AirlineRepository(AirlineContext context)
        {
            _context = context;
        }

        public async Task<int> AddFligth(FlightRaw model)
        {
            _context.Flight.Add(model);
            return _context.SaveChanges();
        }

        public async Task<int> AddFligthRoute(FlightRouteRaw model)
        {
            _context.FlightRoute.Add(model);
            return _context.SaveChanges();
        }

        public async Task<FlightRouteRaw> GetFligthRoute(int source, int target)
        {
            var flightRoute = _context.FlightRoute.Where(x => x.Source == source && x.Target == target).FirstOrDefault();
            return flightRoute;
        }

        public async Task<IList<FlightRouteRaw>> GetFligthRoutes()
        {
            var routes = _context.FlightRoute
                .ToList();
            return routes;
        }

        public async Task<IList<FlightRaw>> GetFligth(int id, string licensePlate)
        {
            var flight = _context.Flight
                .Where(x => x.Id == id && x.Aircraft == licensePlate)
                .ToList();
            return flight;
        }

        public async Task<IList<FlightRaw>> GetFligths()
        {
            var flight = _context.Flight
                .ToList();
            return flight;
        }

        public async Task<int> UpdateFligthStatus(int fligthId, int status)
        {
            var flight = _context.Flight
                .Where(x => x.Id == fligthId)
                .FirstOrDefault();
            flight.State = status;
            return _context.SaveChanges();
        }
    }
}
