using AirportService.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportService.Storage.Interfaz
{
    public interface IAirportRepository
    {
        Task<int> AddAirport(AirportRaw model);
    }
}
