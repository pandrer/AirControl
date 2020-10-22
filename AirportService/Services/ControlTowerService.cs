using AirportService.Proxies.AirlineProxy;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AirportService.Services
{
    public class ControlTowerService : ControlTowerEntry.ControlTowerEntryBase
    {
        private readonly ILogger<ControlTowerService> _logger;
        private readonly IAirlineProxy _airlineProxy;

        public ControlTowerService(ILogger<ControlTowerService> logger, IAirlineProxy airlineProxy)
        {
            _logger = logger;
            _airlineProxy = airlineProxy;
        }

        public override async Task<DefaultResponse> UpdateFligthStatus(ControlTowerRequestModel request, ServerCallContext context)
        {
            var response = new DefaultResponse();
            try
            {
                var result = await _airlineProxy.UpdateFligthStatus(request.FligthId, request.Status);
                response.Message = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Update fligth status");
            }

            return response;
        }
    }
}
