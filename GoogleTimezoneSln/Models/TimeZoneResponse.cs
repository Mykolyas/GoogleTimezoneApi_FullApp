namespace GoogleTimezoneSln.Models
{
    public class TimeZoneResponse
    {
        public string TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
        public int DstOffset { get; set; }
        public int RawOffset { get; set; }
        public string Status { get; set; }
    }

}
