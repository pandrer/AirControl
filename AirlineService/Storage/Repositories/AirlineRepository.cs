using AirlineService.Storage.Interfaz;
using AirlineService.Storage.Models;
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
    }
}
