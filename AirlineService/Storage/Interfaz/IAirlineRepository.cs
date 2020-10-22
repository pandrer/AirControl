using AirlineService.Storage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineService.Storage.Interfaz
{
    public interface IAirlineRepository
    {
        Task<int> AddFligth(FlightRaw model);
        Task<int> AddFligthRoute(FlightRouteRaw model);
        Task<IList<FlightRaw>> GetFligth(int id, string licensePlate);
        Task<FlightRouteRaw> GetFligthRoute(int source, int target);
        Task<IList<FlightRouteRaw>> GetFligthRoutes();
        Task<IList<FlightRaw>> GetFligths();
        Task<int> UpdateFligthStatus(int fligthId, int status);
    }
}
