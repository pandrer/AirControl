using AirportService.Storage.Interfaz;
using AirportService.Storage.Models;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<AirportRaw> GetAirport(string city)
        {
            var airport = _context.Airports
                .Where(x => x.City == city)
                .FirstOrDefault();
            return airport;
        }

        public async Task<IList<AirportRaw>> GetAllAirports()
        {
            var airports = _context.Airports.ToList();
            return airports;
        }


    }
}
