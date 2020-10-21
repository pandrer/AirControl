using AirlineService.Storage.Models;
using System.Threading.Tasks;

namespace AirlineService.Storage.Interfaz
{
    public interface IAirlineRepository
    {
        Task<int> AddFligth(FlightRaw model);
        Task<int> AddFligthRoute(FlightRouteRaw model);
    }
}
