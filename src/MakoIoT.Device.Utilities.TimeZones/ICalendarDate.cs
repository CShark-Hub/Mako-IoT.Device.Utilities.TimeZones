using System;

namespace MakoIoT.Device.Utilities.TimeZones
{
    /// <summary>
    /// Represents a day & time in any year.
    /// </summary>
    public interface ICalendarDate
    {
        /// <summary>
        /// Kind of the date
        /// </summary>
        DateKind Kind { get; }
        /// <summary>
        /// Calculates specific date in the given year.
        /// </summary>
        /// <param name="year">The year</param>
        /// <returns>Date and time</returns>
        DateTime GetDate(int year);
    }
}
