using System;

namespace MakoIoT.Device.Utilities.TimeZones
{
    /// <summary>
    /// Represents a time zone. Calculates local time & date.
    /// </summary>
    public class TimeZone
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

        /// <summary>
        /// Creates instance of TimeZone.
        /// </summary>
        /// <param name="utcOffset">Time zone offset from UTC</param>
        public TimeZone(TimeSpan utcOffset)
        {
            StandardUtcOffset = utcOffset;
            HasDst = false;
        }

        /// <summary>
        /// Creates instance of TimeZone.
        /// </summary>
        /// <param name="name">Name of the time zone</param>
        /// <param name="utcOffset">Time zone offset from UTC</param>
        public TimeZone(string name, TimeSpan utcOffset) : this(utcOffset)
        {
            StandardName = name;
        }

        /// <summary>
        /// Creates instance of TimeZone.
        /// </summary>
        /// <param name="standardName">Name of the time zone</param>
        /// <param name="dstName">Name of the DST time zone</param>
        /// <param name="standardUtcOffset">Time offset from UTC</param>
        /// <param name="dstUtcOffset">DST time offset from UTC</param>
        /// <param name="dstStarts">Day & time when DST begins</param>
        /// <param name="dstEnds">Day & time when DST ends</param>
        public TimeZone(string standardName, string dstName, TimeSpan standardUtcOffset, TimeSpan dstUtcOffset, ICalendarDate dstStarts,
            ICalendarDate dstEnds) : this(standardUtcOffset, dstUtcOffset, dstStarts, dstEnds)
        {
            StandardName = standardName;
            DstName = dstName;
        }

        /// <summary>
        /// Creates instance of TimeZone.
        /// </summary>
        /// <param name="standardUtcOffset">Time offset from UTC</param>
        /// <param name="dstUtcOffset">DST time offset from UTC</param>
        /// <param name="dstStarts">Day & time when DST begins</param>
        /// <param name="dstEnds">Day & time when DST ends</param>
        public TimeZone(TimeSpan standardUtcOffset, TimeSpan dstUtcOffset, ICalendarDate dstStarts, ICalendarDate dstEnds)
        {
            StandardUtcOffset = standardUtcOffset;
            DstUtcOffset = dstUtcOffset;
            DstStarts = dstStarts;
            DstEnds = dstEnds;
            HasDst = true;
        }

        /// <summary>
        /// Gets local time.
        /// </summary>
        /// <param name="utcDateTime">UTC date and time</param>
        /// <returns>Local date and time</returns>
        public DateTime GetLocalTime(DateTime utcDateTime)
        {
            return utcDateTime.Add(IsDst(utcDateTime) ? DstUtcOffset : StandardUtcOffset);
        }

        /// <summary>
        /// Returns [true] if DST is observed at given date and time.
        /// </summary>
        /// <param name="utcDateTime">UTC date and time</param>
        /// <returns>[true] if DST is observed, otherwise [false]</returns>
        public bool IsDst(DateTime utcDateTime)
        {
            if (!HasDst)
                return false;

            var dstStarts = DstStarts.Kind == DateKind.Utc
                ? DstStarts.GetDate(utcDateTime.Year)
                : DstStarts.GetDate(utcDateTime.Year) - StandardUtcOffset;

            var dstEnds = DstEnds.Kind == DateKind.Utc
                ? DstEnds.GetDate(utcDateTime.Year)
                : DstEnds.GetDate(utcDateTime.Year) - DstUtcOffset;

            return dstEnds < dstStarts //DST spans new year
                ? utcDateTime >= dstStarts || utcDateTime < dstEnds
                : utcDateTime >= dstStarts && utcDateTime < dstEnds;
        }
    }
}
