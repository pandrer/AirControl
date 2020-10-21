using AirlineService.Proxies.HangarProxy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineService.Proxies.HangarProxy
{
    public interface IHangarProxy
    {
        Task<HangarAircraftModel> GetAircraft(string licensePlate);
    }
}
