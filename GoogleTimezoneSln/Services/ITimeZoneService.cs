using GoogleTimezoneSln.Models;
namespace GoogleTimezoneSln.Services
{
    public interface ITimeZoneService
    {
        Task<TimeZoneResponse> GetTimeZoneAsync(TimeZoneRequest request);
    }
}
