using HangarService.Storage.Models;
using System.Threading.Tasks;

namespace HangarService.Storage.Interfaz
{
    public interface IAircraftRepository
    {
        Task<int> AddAirCraft(AircraftRaw model);
    }
}
