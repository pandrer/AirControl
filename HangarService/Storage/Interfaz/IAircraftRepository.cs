using HangarService.Storage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HangarService.Storage.Interfaz
{
    public interface IAircraftRepository
    {
        Task<int> AddAirCraft(AircraftRaw model);
        Task<IList<AircraftRaw>> GetAllAirCraft();
        Task<AircraftRaw> GetAirCraft(string licensePlate);
    }
}
