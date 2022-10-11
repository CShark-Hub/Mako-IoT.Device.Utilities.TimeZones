using System;
// ReSharper disable InconsistentNaming

namespace MakoIoT.Device.Utilities.TimeZones
{
    public static class KnownTimeZones
    {
        public static TimeZone Europe_London => new(
            TimeSpan.Zero,
            TimeSpan.FromHours(1),
            new FloatingDate(3, 5, DayOfWeek.Sunday, 1, 0, 0, DateKind.Utc),
            new FloatingDate(10, 5, DayOfWeek.Sunday, 1, 0, 0, DateKind.Utc));

        public static TimeZone Europe_Warsaw => new(
            TimeSpan.FromHours(1),
            TimeSpan.FromHours(2),
            new FloatingDate(3, 5, DayOfWeek.Sunday, 1, 0, 0, DateKind.Utc),
            new FloatingDate(10, 5, DayOfWeek.Sunday, 1, 0, 0, DateKind.Utc));

        public static TimeZone America_NewYork => new(
            TimeSpan.FromHours(-5),
            TimeSpan.FromHours(-4),
            new FloatingDate(3, 2, DayOfWeek.Sunday, 2, 0, 0, DateKind.Local),
            new FloatingDate(11, 1, DayOfWeek.Sunday, 2, 0, 0, DateKind.Local));

        public static TimeZone America_LosAngeles => new(
            TimeSpan.FromHours(-8),
            TimeSpan.FromHours(-7),
            new FloatingDate(3, 2, DayOfWeek.Sunday, 2, 0, 0, DateKind.Local),
            new FloatingDate(11, 1, DayOfWeek.Sunday, 2, 0, 0, DateKind.Local));
    }
}
