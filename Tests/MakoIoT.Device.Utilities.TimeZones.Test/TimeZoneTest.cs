using System;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Utilities.TimeZones.Test
{
    [TestClass]
    public class TimeZoneTest
    {
        [DataRow(2, 0, 12, 15, 2, 14, 15)]
        [DataRow(2, 0, 23, 15, 3, 1, 15)]
        [DataRow(-2, 0, 23, 15, 2, 21, 15)]
        [DataRow(5, 30, 23, 35, 3, 5, 5)]
        [DataRow(-10, -30, 5, 25, 1, 18, 55)]
        public void GetLocalTime_should_compute_date_from_utc(int offsetHours, int offsetMinutes, int utcHour,
            int utcMinute, int day, int hour, int minute)
        {
            var sut = new TimeZone(new TimeSpan(offsetHours, offsetMinutes, 0));
            var result = sut.GetLocalTime(new DateTime(2022, 8, 2, utcHour, utcMinute, 0));
            Assert.Equal(new DateTime(2022, 8, day, hour, minute, 0), result);
        }

        [DataRow(2, 0, 23, 15, 2, 21, 15)]
        [DataRow(10, 30, 5, 25, 1, 18, 55)]
        public void GetLocalTime_given_negative_offset_should_compute_date_from_utc(int offsetHours, int offsetMinutes, int utcHour,
            int utcMinute, int day, int hour, int minute)
        {
            var sut = new TimeZone(-new TimeSpan(offsetHours, offsetMinutes, 0));
            var result = sut.GetLocalTime(new DateTime(2022, 8, 2, utcHour, utcMinute, 0));
            Assert.Equal(new DateTime(2022, 8, day, hour, minute, 0), result);
        }

        [DataRow(3, 21, 2, 10, 21, 3, 5, 1, 15, true)]
        [DataRow(3, 21, 2, 10, 21, 3, 2, 1, 15, false)]
        [DataRow(3, 21, 2, 10, 21, 3, 3, 21, 1, true)] //edge case: DST starts
        [DataRow(3, 21, 2, 10, 21, 3, 10, 21, 1, false)] //edge case: DST ends
        [DataRow(10, 21, 2, 3, 21, 3, 5, 1, 15, false)]
        [DataRow(10, 21, 2, 3, 21, 3, 2, 1, 15, true)]
        [DataRow(10, 21, 2, 3, 21, 3, 3, 21, 1, false)] //edge case: DST ends
        [DataRow(10, 21, 2, 3, 21, 3, 10, 21, 1, true)] //edge case: DST starts
        public void IsDst_given_fixed_dates_should_find_out_dst(int startMonth, int startDay, int startHour,
            int endMonth, int endDay, int endHour,
            int utcMonth, int utcDay, int utcHour, bool isDst)
        {
            var sut = new TimeZone(TimeSpan.FromHours(1), TimeSpan.FromHours(2),
                new FixedDate(new DateTime(1990, startMonth, startDay, startHour, 0, 0)),
                new FixedDate(new DateTime(1990, endMonth, endDay, endHour, 0, 0)));
            var result = sut.IsDst(new DateTime(2022, utcMonth, utcDay, utcHour, 0, 0));
            Assert.Equal(isDst, result);
        }

        [TestMethod]
        public void GetLocalTime_should_return_dst_offset_given_dst()
        {
            var sut = new TimeZone(TimeSpan.FromHours(1), TimeSpan.FromHours(2),
                new FixedDate(new DateTime(1990, 3, 21)), new FixedDate(new DateTime(1990, 10, 21)));
            var result = sut.GetLocalTime(new DateTime(2022, 5, 12, 12, 30, 0));
            Assert.Equal(new DateTime(2022, 5, 12, 14, 30, 0), result);
        }
    }
}
