using System;
using System.Text;
using MakoIoT.Device.Utilities.TimeZones.Extensions;

namespace MakoIoT.Device.Utilities.TimeZones
{
    /// <summary>
    /// Converts time zone data
    /// </summary>
    public static class TimeZoneConverter
    {
        /// <summary>
        /// Creates TimeZone object from POSIX string.
        /// <see cref="https://www.iana.org/time-zones"/>
        /// </summary>
        /// <param name="posixTz">POSIX representation of time zone</param>
        /// <returns>TimeZone object</returns>
        /// <exception cref="FormatException"></exception>
        public static TimeZone FromPosixString(string posixTz)
        {
            //time zone database and POSIX format spec: https://www.iana.org/time-zones

            var parts = posixTz.ToUpper().Split(',');

            string standardName = "", dstName = "", stdOffset = "", dstOffset = "";
            TimeSpan standardUtcOffset, dstUtcOffset;

            try
            {
                var i = 0;

                if (ParseFragment(true, parts[0], ref i, out standardName))
                    if (ParseFragment(false, parts[0], ref i, out stdOffset))
                        if (ParseFragment(true, parts[0], ref i, out dstName))
                            ParseFragment(false, parts[0], ref i, out dstOffset);

                standardUtcOffset = -ParseTimeSpan(stdOffset);
                dstUtcOffset = dstOffset == ""
                    ? standardUtcOffset.Add(TimeSpan.FromHours(1))
                    : -ParseTimeSpan(dstOffset);
            }
            catch (Exception ex)
            {
                throw new FormatException(parts[0], ex);
            }

            if (parts.Length == 3)
            {
                return new TimeZone(standardName, dstName, standardUtcOffset, dstUtcOffset,
                    ParsePosixTransition(parts[1]), ParsePosixTransition(parts[2]));
            }

            return new TimeZone(standardName, standardUtcOffset);
        }

        private static bool ParseFragment(bool isName, string input, ref int i, out string fragment)
        {
            fragment = "";
            var c = input[i++];
            bool ltEscape = c == '<';

            while (i < input.Length && IsToken(isName, c, ltEscape))
            {
                fragment += c;
                if (c == '>')
                    ltEscape = false;
                c = input[i++];
            }

            if (i == input.Length && IsToken(isName, c, ltEscape))
            {
                fragment += c;
                return false;
            }

            i--;
            return true;
        }

        private static bool IsToken(bool isName, char c, bool ltEscape)
        {
            return (isName && (c.IsLetter() || ltEscape))
                   || (!isName && !c.IsLetter() && c != '<');
        }

        /// <summary>
        /// Converts the TimeZone to POSIX string representation.
        /// </summary>
        /// <param name="timezone">The TimeZone object</param>
        /// <param name="skipIfDefault">If [true], certain values are not emitted if they equal POSIX defaults (DST offset, DST transition times)</param>
        /// <returns>POSIX string representation of the time zone</returns>
        public static string ToPosixString(TimeZone timezone, bool skipIfDefault = true)
        {
            var builder = new StringBuilder();

            builder.Append(GetNameOrDefault(timezone.StandardName, -timezone.StandardUtcOffset));
            builder.Append(TimeSpanToString(-timezone.StandardUtcOffset, false));
            if (!timezone.HasDst)
                return builder.ToString();

            builder.Append(GetNameOrDefault(timezone.DstName, -timezone.DstUtcOffset));

            if (!skipIfDefault || timezone.DstUtcOffset.Subtract(timezone.StandardUtcOffset) != TimeSpan.FromHours(1))
                builder.Append(TimeSpanToString(-timezone.DstUtcOffset, false));

            builder.Append(',');
            builder.Append(DateToPosixString(timezone.DstStarts, skipIfDefault));
            builder.Append(',');
            builder.Append(DateToPosixString(timezone.DstEnds, skipIfDefault));

            return builder.ToString();
        }

        /// <summary>
        /// Parses TimeSpan from string.
        /// </summary>
        /// <param name="timeSpan">The string representation of TimeSpan</param>
        /// <returns>TimeSpan object</returns>
        /// <exception cref="FormatException"></exception>
        public static TimeSpan ParseTimeSpan(string timeSpan)
        {
            bool negative = timeSpan[0] == '-';

            timeSpan = timeSpan.TrimStart('-', '+');

            var sp = timeSpan.Split(':');

            TimeSpan ts;
            try
            {
                ts = new TimeSpan(int.Parse(sp[0]),
                    sp.Length > 1 ? int.Parse(sp[1]) : 0,
                    sp.Length > 2 ? int.Parse(sp[2]) : 0);
            }
            catch (Exception ex)
            {
                throw new FormatException(timeSpan, ex);
            }

            return negative ? -ts : ts;
        }

        /// <summary>
        /// Converts TimeSpan into string.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan object.</param>
        /// <param name="appendPlus">If [true], prefixes positive TimeSpan with "+"</param>
        /// <returns>String representation of TimeSpan</returns>
        public static string TimeSpanToString(TimeSpan timeSpan, bool appendPlus)
        {
            var builder = new StringBuilder();

            if (timeSpan.Ticks < 0)
                builder.Append('-');
            else if (appendPlus)
                builder.Append('+');

            timeSpan = timeSpan.Duration();

            builder.Append(Truncate(timeSpan.TotalHours));
            if (timeSpan.Minutes == 0 && timeSpan.Seconds == 0)
                return builder.ToString();

            builder.Append(':');
            builder.Append($"{timeSpan.Minutes:d2}");

            if (timeSpan.Seconds == 0)
                return builder.ToString();

            builder.Append(':');
            builder.Append($"{timeSpan.Seconds:d2}");
            return builder.ToString();
        }

        /// <summary>
        /// Truncates fraction part of the number
        /// </summary>
        /// <param name="d">The number</param>
        /// <returns>Integer part of the number</returns>
        public static int Truncate(double d)
        {
            return (int)d;
        }

        private static ICalendarDate ParsePosixTransition(string transition)
        {
            var parts = transition.Split('/');
            try
            {
                if (parts.Length > 2)
                    throw new FormatException();

                var time = TimeSpan.FromHours(2); //default DST transition at 2am
                if (parts.Length == 2)
                    time = ParseTimeSpan(parts[1]);

                if (transition[0] == 'M')
                {
                    var dateParts = parts[0].Substring(1).Split('.');
                    if (dateParts.Length > 3)
                        throw new FormatException();

                    return new FloatingDate(int.Parse(dateParts[0]), int.Parse(dateParts[1]), (DayOfWeek)int.Parse(dateParts[2]),
                        Truncate(time.TotalHours), time.Minutes, time.Seconds, DateKind.Local);
                }

                if (transition[0] == 'J')
                {
                    return new FixedDate(new DateTime(1990, 1, 1)
                        .AddDays(int.Parse(parts[0].Substring(1)) - 1).Add(time), DateKind.Local);
                }

                if (transition[0] == '0' || transition[0] == '1' || transition[0] == '2' || transition[0] == '3')
                    throw new NotSupportedException("The n (0≤n≤365) date form is not supported");

                throw new FormatException();

            }
            catch (Exception ex)
            {
                throw new FormatException(transition, ex);
            }
        }

        private static string DateToPosixString(ICalendarDate date, bool skipIfDefault)
        {
            var builder = new StringBuilder();
            if (date is FloatingDate fd)
            {
                builder.Append($"M{fd.Month}.{fd.Week}.{(int)fd.DayOfWeek}");
                if (!skipIfDefault || fd.TimeOfDay != TimeSpan.FromHours(2))
                    builder.Append($"/{TimeSpanToString(fd.TimeOfDay, false)}");
            }
            else if (date is FixedDate d)
            {
                builder.Append($"J{d.Date.DayOfYear}");
                if (!skipIfDefault || d.Date.TimeOfDay != TimeSpan.FromHours(2))
                    builder.Append($"/{TimeSpanToString(d.Date.TimeOfDay, false)}");
            }
            else
                throw new NotSupportedException();

            return builder.ToString();
        }

        private static string GetNameOrDefault(string name, TimeSpan offset)
        {
            return !string.IsNullOrEmpty(name) ? name.ToUpper() : $"<{TimeSpanToString(offset, true)}>";
        }
    }
}
