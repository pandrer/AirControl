namespace AirlineService.Storage.Models
{
    public class FlightRouteRaw
    {
        public int Id { get; set; }
        public int Source { get; set; }
        public int Target { get; set; }
        public int EstimatedTime { get; set; }
    }
}
