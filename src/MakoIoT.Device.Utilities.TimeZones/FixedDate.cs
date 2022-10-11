using System;

namespace MakoIoT.Device.Utilities.TimeZones
{
    /// <summary>
    /// Represents specific day & time in a year.
    /// </summary>
    public class FixedDate : ICalendarDate
    {
        /// <summary>
        /// The day & time
        /// </summary>
        public DateTime Date { get; }
        /// <inheritdoc/>
        public DateKind Kind { get; }

        /// <summary>
        /// Creates instance of FixedDate.
        /// </summary>
        /// <param name="date">Day & time in a year (year value is ignored)</param>
        /// <param name="kind">Kind of the date</param>
        public FixedDate(DateTime date, DateKind kind = DateKind.Local)
        {
            Date = date;
            Kind = kind;
        }

        /// <inheritdoc/>
        public DateTime GetDate(int year)
        {
            return new DateTime(year, Date.Month, Date.Day, Date.Hour, Date.Minute, Date.Second);
        }
    }
}
