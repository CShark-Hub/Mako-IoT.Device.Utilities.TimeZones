using System;

namespace MakoIoT.Device.Utilities.TimeZones
{
    public class SerializableTimeZone
    {
        /// <summary>
        /// Time zone name
        /// </summary>
        public string StandardName { get; }
        /// <summary>
        /// DST time zone name
        /// </summary>
        public string DstName { get; }
        /// <summary>
        /// Daylight Saving Time begins
        /// </summary>
        public ICalendarDate DstStarts { get; }
        /// <summary>
        /// Daylight Saving Time ends
        /// </summary>
        public ICalendarDate DstEnds { get; }
        /// <summary>
        /// Time offset from UTC
        /// </summary>
        public TimeSpan StandardUtcOffset { get; }
        /// <summary>
        /// DST time offset from UTC
        /// </summary>
        public TimeSpan DstUtcOffset { get; }
        /// <summary>
        /// [true] if time zone observes DST
        /// </summary>
        public bool HasDst { get; }

        public SerializableTimeZone()
        {
            StandardName = "CET";
            DstStarts = new FloatingDate(3, 4, DayOfWeek.Sunday);
        }
    }
}
