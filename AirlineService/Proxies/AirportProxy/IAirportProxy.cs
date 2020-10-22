using AirlineService.Proxies.AirportProxy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineService.Proxies.AirportProxy
{
    public interface IAirportProxy
    {
        Task<IList<AirportProxyModel>> GetAllAirports();
    }
}
