using HangarService.Storage.Interfaz;
using HangarService.Storage.Models;
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
    }
}
