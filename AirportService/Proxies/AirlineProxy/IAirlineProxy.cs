using System.Threading.Tasks;

namespace AirportService.Proxies.AirlineProxy
{
    public interface IAirlineProxy
    {
        Task<string> UpdateFligthStatus(int fligthId, int status);
    }
}
