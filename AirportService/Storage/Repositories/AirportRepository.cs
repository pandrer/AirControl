    using AirportService.Storage.Interfaz;
using AirportService.Storage.Models;
using System.Threading.Tasks;

namespace AirportService.Storage.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly AirportContext _context;
        public AirportRepository(AirportContext context)
        {
            _context = context;
        }
        public async Task<int> AddAirport(AirportRaw model)
        {
            _context.Airports.Add(model);
            return _context.SaveChanges();
        }
    }
}
