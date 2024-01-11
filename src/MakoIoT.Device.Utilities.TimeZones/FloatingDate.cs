using System;

namespace MakoIoT.Device.Utilities.TimeZones
{
    /// <summary>
    /// Represents floating day & time in a year.
    /// </summary>
    public class FloatingDate : ICalendarDate
    {
        /// <summary>
        /// Month number
        /// </summary>
        public int Month { get; }
        /// <summary>
        /// Week number in a month. 5 indicates the last week.
        /// </summary>
        public int Week { get; }
        /// <summary>
        /// Day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek { get; }
        /// <summary>
        /// Hour
        /// </summary>
        public int Hour => TimeOfDay.Hours;
        /// <summary>
        /// Minute
        /// </summary>
        public int Minute => TimeOfDay.Minutes;
        /// <summary>
        /// Second
        /// </summary>
        public int Second => TimeOfDay.Seconds;
        /// <summary>
        /// Kind of the date
        /// </summary>
        public DateKind Kind { get; }
        /// <summary>
        /// Time component of the date
        /// </summary>
        public TimeSpan TimeOfDay { get; }

        /// <summary>
        /// Creates instance of FloatingDate
        /// </summary>
        /// <param name="month">Month number</param>
        /// <param name="week">Week number in a month. 5 indicates the last week.</param>
        /// <param name="dayOfWeek">Day of the week.</param>
        /// <param name="hour">Hour</param>
        /// <param name="minute">Minute</param>
        /// <param name="second">Second</param>
        /// <param name="kind">Kind of the date</param>
        public FloatingDate(int month, int week, DayOfWeek dayOfWeek, int hour, int minute, int second, DateKind kind = DateKind.Local)
        {
            Month = month;
            Week = week;
            DayOfWeek = dayOfWeek;
            Kind = kind;
            TimeOfDay = new TimeSpan(hour, minute, second);
        }

        /// <summary>
        /// Creates instance of FloatingDate
        /// </summary>
        /// <param name="month">Month number</param>
        /// <param name="week">Week number in a month. 5 indicates the last week.</param>
        /// <param name="dayOfWeek">Day of the week.</param>
        /// <param name="timeOfDay">Time of day.</param>
        /// <param name="kind">Kind of the date</param>
        public FloatingDate(int month, int week, DayOfWeek dayOfWeek, TimeSpan timeOfDay, DateKind kind = DateKind.Local)
        {
            Month = month;
            Week = week;
            DayOfWeek = dayOfWeek;
            Kind = kind;
            TimeOfDay = timeOfDay;
        }

        /// <summary>
        /// Creates instance of FloatingDate
        /// </summary>
        /// <param name="month">Month number</param>
        /// <param name="week">Week number in a month. 5 indicates the last week.</param>
        /// <param name="dayOfWeek">Day of the week.</param>
        /// <param name="kind">Kind of the date</param>
        public FloatingDate(int month, int week, DayOfWeek dayOfWeek, DateKind kind = DateKind.Local) : this(month, week, dayOfWeek, 0, 0, 0, kind)
        {
        }

        /// <inheritdoc />
        public DateTime GetDate(int year)
        {
            var date = new DateTime(year, Month, 1);
            date = date.AddDays((DayOfWeek < date.DayOfWeek ? 7 : 0) + DayOfWeek - date.DayOfWeek + (Week - 1) * 7);

            if (Week == 5 && date.Month > Month) // if there's no 5th week, return the 4th
                date = date.AddDays(-7);

            return date.Add(TimeOfDay);
        }

        /// <summary>
        /// Determines if the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The other object</param>
        /// <returns>[true] if the specified object is equal to the current object; otherwise [false]</returns>
        public bool Equals(FloatingDate other)
        {
            return Month == other.Month
                   && Week == other.Week
                   && DayOfWeek == other.DayOfWeek
                   && TimeOfDay == other.TimeOfDay
                   && Kind == other.Kind;
        }

        public override string ToString()
        {
            return $"{nameof(Month)}: {Month}, {nameof(Week)}: {Week}, {nameof(DayOfWeek)}: {DayOfWeek}, {nameof(Hour)}: {Hour}, {nameof(Minute)}: {Minute}, {nameof(Second)}: {Second}, {nameof(Kind)}: {Kind}";
        }
    }
}
