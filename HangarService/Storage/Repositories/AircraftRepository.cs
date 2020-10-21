using HangarService.Storage.Interfaz;
using HangarService.Storage.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangarService.Storage.Repositories
{
    public class AircraftRepository : IAircraftRepository
    {
        private readonly HangarContext _context;
        public AircraftRepository(HangarContext context)
        {
            _context = context;
        }
        public async Task<int> AddAirCraft(AircraftRaw model)
        {
            _context.Aircrafts.Add(model);
            return _context.SaveChanges();
        }

        public async Task<AircraftRaw> GetAirCraft(string licensePlate)
        {
            var aircraft = _context.Aircrafts
                .Where(x => x.LicensePlate == licensePlate)
                .FirstOrDefault();
            return aircraft;
        }

        public async Task<IList<AircraftRaw>> GetAllAirCraft()
        {
            var aircrafts = _context.Aircrafts.ToList();
            return aircrafts;
        }
    }
}
