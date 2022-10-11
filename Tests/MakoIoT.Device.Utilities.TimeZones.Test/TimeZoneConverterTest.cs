using System;
using System.Diagnostics;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Utilities.TimeZones.Test
{
    [TestClass]
    public class TimeZoneConverterTest
    {
        [DataRow("12:34:56", 12, 34, 56)]
        [DataRow("2:30", 2, 30, 0)]
        [DataRow("-2:30", -2, -30, 0)]
        [DataRow("3", 3, 0, 0)]
        [DataRow("-3", -3, 0, 0)]
        public void ParseTimeSpan_should_return_correct_value(string input, int hours, int minutes, int seconds)
        {
            var result = TimeZoneConverter.ParseTimeSpan(input);
            Assert.Equal(new TimeSpan(hours, minutes, seconds).Ticks, result.Ticks);
        }

        [DataRow("CET-1CEST,M3.5.0,M10.5.0/3", "CET", "CEST", 1,0,2,0,3,5,0,2,10,5,0,3)]
        [DataRow("EST5EDT,M3.2.0,M11.1.0", "EST", "EDT", -5,0,-4,0,3,2,0,2,11,1,0,2)]
        [DataRow("<+10:30>10:30<+9:30>,M3.2.0,M11.1.0", "<+10:30>", "<+9:30>", -10,-30,-9,-30,3,2,0,2,11,1,0,2)]
        [DataRow("<-10:30>-10:30<-11>-11,M3.2.0,M11.1.0", "<-10:30>", "<-11>", 10,30,11,0,3,2,0,2,11,1,0,2)]
        [DataRow("WGT3WGST,M3.5.0/-2,M10.5.0/-1", "WGT", "WGST", -3,0,-2,0,3,5,0,-2,10,5,0,-1)]
        [DataRow("IST-2IDT,M3.4.4/26,M10.5.0", "IST", "IDT", 2,0,3,0,3,4,4,26,10,5,0,2)]
        // [DataRow("IST-2IDT,M3.4.4/-26:30,M10.5.0", "IST", "IDT", 2,0,3,0,3,4,4,-26,10,5,0,2)]
        public void FromPosix_given_floating_date_should_return_TimeZone(string input, string stdName, string dstName,
            int stdOffsetHours, int stdOffsetMins, int dstOffsetHours, int dstOffsetMins,
            int dstStartMonth, int dstStartWeek, int dstStartDay, int dstStartHour,
            int dstEndMonth, int dstEndWeek, int dstEndDay, int dstEndHour)
        {
            var result = TimeZoneConverter.FromPosixString(input);

            Assert.Equal(stdName, result.StandardName);
            Assert.Equal(dstName, result.DstName);
            Assert.Equal(new TimeSpan(stdOffsetHours, stdOffsetMins,0).Ticks, result.StandardUtcOffset.Ticks);
            Assert.Equal(new TimeSpan(dstOffsetHours, dstOffsetMins, 0).Ticks, result.DstUtcOffset.Ticks);
            Assert.True(new FloatingDate(dstStartMonth, dstStartWeek, (DayOfWeek)dstStartDay, dstStartHour, 0, 0).Equals(result.DstStarts as FloatingDate));
            Assert.True(new FloatingDate(dstEndMonth, dstEndWeek, (DayOfWeek)dstEndDay, dstEndHour, 0, 0).Equals(result.DstEnds as FloatingDate));
        }

        [DataRow("CET-1CEST,J1,J303/3", "CET", "CEST", 1, 0, 2, 0, 1, 1, 2, 10, 30, 3)]
        [DataRow("<+10:30>10:30<+11>11,J314/4,J74/1", "<+10:30>", "<+11>", -10, -30, -11, 0, 11, 10, 4, 3, 15, 1)]

        public void FromPosix_given_fixed_date(string input, string stdName, string dstName,
            int stdOffsetHours, int stdOffsetMins, int dstOffsetHours, int dstOffsetMins,
            int dstStartMonth, int dstStartDay, int dstStartHour,
            int dstEndMonth, int dstEndDay, int dstEndHour)
        {
            var result = TimeZoneConverter.FromPosixString(input);

            Assert.Equal(stdName, result.StandardName);
            Assert.Equal(dstName, result.DstName);
            Assert.Equal(new TimeSpan(stdOffsetHours, stdOffsetMins, 0).Ticks, result.StandardUtcOffset.Ticks);
            Assert.Equal(new TimeSpan(dstOffsetHours, dstOffsetMins, 0).Ticks, result.DstUtcOffset.Ticks);
            Assert.True(new DateTime(1990, dstStartMonth, dstStartDay, dstStartHour, 0, 0).Equals((result.DstStarts as FixedDate).Date));
            Assert.True(new DateTime(1990, dstEndMonth, dstEndDay, dstEndHour, 0, 0).Equals((result.DstEnds as FixedDate).Date));
        }

        [DataRow(5.0, 5)]
        [DataRow(5.2, 5)]
        [DataRow(-5.2, -5)]
        [DataRow(5.5, 5)]
        [DataRow(5.8, 5)]
        [DataRow(-5.8, -5)]
        public void Truncate_should_cut_fractions_off(double d, int i)
        {
            Assert.Equal(i, TimeZoneConverter.Truncate(d));
        }

        [DataRow(1,0,0,false,"1")]
        [DataRow(1,30,0,false,"1:30")]
        [DataRow(1,5,0,false,"1:05")]
        [DataRow(4,5,0,true,"+4:05")]
        [DataRow(-10,0,0,false,"-10")]
        [DataRow(-10,-30,0,true,"-10:30")]
        [DataRow(-10,-5,-9,true,"-10:05:09")]
        public void TimeSpanToString_values(int hours, int minutes, int seconds, bool appendPlus, string output)
        {
            Assert.Equal(output, TimeZoneConverter.TimeSpanToString(new TimeSpan(hours, minutes, seconds), appendPlus));
        }

        [DataRow("CET-1CEST,M3.5.0,M10.5.0/3", "CET", "CEST", 1, 0, 2, 0, 3, 5, 0, 2, 10, 5, 0, 3)]
        [DataRow("EST5EDT,M3.2.0,M11.1.0", "EST", "EDT", -5, 0, -4, 0, 3, 2, 0, 2, 11, 1, 0, 2)]
        [DataRow("<+10:30>10:30<+9:30>,M3.2.0,M11.1.0", "", "", -10, -30, -9, -30, 3, 2, 0, 2, 11, 1, 0, 2)]
        [DataRow("<-10:30>-10:30<-11>-11,M3.2.0,M11.1.0", "", "", 10, 30, 11, 0, 3, 2, 0, 2, 11, 1, 0, 2)]
        [DataRow("WGT3WGST,M3.5.0/-2,M10.5.0/-1", "WGT", "WGST", -3, 0, -2, 0, 3, 5, 0, -2, 10, 5, 0, -1)]
        [DataRow("IST-2IDT,M3.4.4/26,M10.5.0", "IST", "IDT", 2, 0, 3, 0, 3, 4, 4, 26, 10, 5, 0, 2)]
        public void ToPosixString_floating_date(string output, string stdName, string dstName,
            int stdOffsetHours, int stdOffsetMins, int dstOffsetHours, int dstOffsetMins,
            int dstStartMonth, int dstStartWeek, int dstStartDay, int dstStartHour,
            int dstEndMonth, int dstEndWeek, int dstEndDay, int dstEndHour)
        {
            var tz = new TimeZone(stdName, dstName,
                new TimeSpan(stdOffsetHours, stdOffsetMins, 0), new TimeSpan(dstOffsetHours, dstOffsetMins, 0),
                new FloatingDate(dstStartMonth, dstStartWeek, (DayOfWeek)dstStartDay, dstStartHour, 0, 0),
                new FloatingDate(dstEndMonth, dstEndWeek, (DayOfWeek)dstEndDay, dstEndHour, 0, 0));

            var result = TimeZoneConverter.ToPosixString(tz);

            Assert.Equal(output, result);
        }

        [DataRow("CET-1CEST,J1,J303/3", "CET", "CEST", 1, 0, 2, 0, 1, 1, 2, 10, 30, 3)]
        [DataRow("<+10:30>10:30<+11>11,J314/4,J74/1", "<+10:30>", "<+11>", -10, -30, -11, 0, 11, 10, 4, 3, 15, 1)]

        public void ToPosixString_given_fixed_date(string output, string stdName, string dstName,
            int stdOffsetHours, int stdOffsetMins, int dstOffsetHours, int dstOffsetMins,
            int dstStartMonth, int dstStartDay, int dstStartHour,
            int dstEndMonth, int dstEndDay, int dstEndHour)
        {
            var tz = new TimeZone(stdName, dstName,
                new TimeSpan(stdOffsetHours, stdOffsetMins, 0), new TimeSpan(dstOffsetHours, dstOffsetMins, 0),
                new FixedDate(new DateTime(1990, dstStartMonth, dstStartDay, dstStartHour, 0, 0)),
                new FixedDate(new DateTime(1990, dstEndMonth,dstEndDay, dstEndHour, 0, 0)));

            var result = TimeZoneConverter.ToPosixString(tz);

            Assert.Equal(output, result);
        }
    }
}
