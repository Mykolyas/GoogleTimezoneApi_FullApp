using System;

namespace GoogleTimezoneSln.Helpers
{
    public static class ConsoleUI
    {
        public static void ShowHeader()
        {
            Console.Clear();
            WriteLine("\n════════ Введення координат ════════");
            WriteLine("• Введіть ШИРОТУ та ДОВГОТУ через ОДИН пробіл");
            WriteLine("• Десятковий роздільник: лише крапка (.)");
            WriteLine("Приклади: 51.5074 -0.1278   |   48.3794 31.1656   |   40.7128 -74.0060");
            WriteLine(new string('═', 40));
        }

        public static void ShowResult(
            string timeZoneName, string timeZoneId,
            int rawOffset, int dstOffset)
        {
            var totalOffset = rawOffset + dstOffset;
            var offsetTimeSpan = TimeSpan.FromSeconds(totalOffset);
            var rawHours = TimeSpan.FromSeconds(rawOffset).Hours;
            var dstHours = TimeSpan.FromSeconds(dstOffset).Hours;

            WriteLine("\n═══════ Результат ═══════");
            WriteLine($" Часовий пояс              : {timeZoneName} ({timeZoneId})");
            WriteLine($" Загальне зміщення від UTC : {totalOffset} сек ({offsetTimeSpan.Hours:+#;-#;0} год)");
            WriteLine($" Стале зміщення (RAW)      : {rawOffset} сек ({rawHours:+#;-#;0} год)");
            WriteLine($" Літній час (DST)          : {dstOffset} сек ({dstHours:+#;-#;0} год)");
        }

        public static void ShowFooter()
        {
            WriteLine("\n════════════════════════════════════");
            WriteLine("1 – Продовжити  |  2 – Вийти");
            Write("Ваш вибір: ");
        }

        public static void ShowError(string message)
        {
            WriteLine($"\n[!] {message}");
        }

        public static void Pause()
        {
            WriteLine("Натисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }

        private static void Write(string message) => Console.Write(message);
        private static void WriteLine(string message = "") => Console.WriteLine(message);
    }
}
