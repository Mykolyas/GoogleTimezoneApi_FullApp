using GoogleTimezoneSln.Helpers;
using GoogleTimezoneSln.Models;
using GoogleTimezoneSln.Services;
using System.Globalization;


namespace GoogleTimezoneSln.Core
{
    public class TimeZoneApp
    {
        private readonly ITimeZoneService _service;

        public TimeZoneApp(ITimeZoneService service)
        {
            _service = service;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                ConsoleUI.ShowHeader();
                Console.Write("Координати: ");
                var input = Console.ReadLine();

                if (!TryParseCoordinates(input, out double lat, out double lon))
                {
                    ConsoleUI.ShowError("Невірний формат координат.");
                    ConsoleUI.Pause();
                    continue;
                }

                var request = new TimeZoneRequest { Latitude = lat, Longitude = lon, Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

                try
                {
                    var response = await _service.GetTimeZoneAsync(request);

                    if (response.Status == "OK")
                    {
                        ConsoleUI.ShowResult(response.TimeZoneName, response.TimeZoneId, response.RawOffset, response.DstOffset);
                    }
                    else
                    {
                        ConsoleUI.ShowError($"Статус помилки: {response.Status}");
                    }
                }
                catch (Exception ex)
                {
                    ConsoleUI.ShowError($"Виняток: {ex.Message}");
                }
                while (true)
                {
                    ConsoleUI.ShowFooter();
                    var choice = Console.ReadLine()?.Trim();
                    if (choice == "1")
                        break;

                    if (choice == "2")
                        Environment.Exit(0);

                    Console.WriteLine("[!] Невідомий вибір. Введіть 1 або 2.");
                }
            }
        }

        private static bool TryParseCoordinates(string? input, out double lat, out double lon)
        {
            lat = lon = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                return false;

            return double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lat) &&
                   double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lon) &&
                   lat >= -90 && lat <= 90 &&
                   lon >= -180 && lon <= 180;
        }
    }

}

