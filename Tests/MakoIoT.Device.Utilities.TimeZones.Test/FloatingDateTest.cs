using nanoFramework.TestFramework;
using System;

namespace MakoIoT.Device.Utilities.TimeZones.Test
{
    [TestClass]
    public class FloatingDateTest
    {
        [DataRow(8, 1, (int)DayOfWeek.Tuesday, 2022, 2)]
        [DataRow(6, 3, (int)DayOfWeek.Sunday, 2022, 19)]
        [DataRow(2, 1, (int)DayOfWeek.Monday, 2022, 7)]
        [DataRow(2, 5, (int)DayOfWeek.Thursday, 2024, 29)]
        [DataRow(3, 1, (int)DayOfWeek.Friday, 2024, 1)]
        [DataRow(8, 5, (int)DayOfWeek.Saturday, 2022, 27)] //edge case: last saturday is the 4th week
        public void GetDate_given_year_should_compute_date(int month, int week, DayOfWeek dayOfWeek, int year, int day)
        {
            var sut = new FloatingDate(month, week, dayOfWeek);
            var result = sut.GetDate(year);
            Assert.Equal(new DateTime(year, month, day), result);
        }

        [DataRow(2, 30, 10, 2, 30)]
        [DataRow(26, 30, 11, 2, 30)]
        [DataRow(-2, 0, 9, 22, 0)]
        public void GetDate_given_year_should_compute_date_with_time(int hours, int minutes, int day, int dateHours,
            int dateMinutes)
        {
            var sut = new FloatingDate(8, 2, DayOfWeek.Wednesday, hours, minutes, 0);
            var result = sut.GetDate(2022);
            Assert.Equal(new DateTime(2022, 8, day, dateHours, dateMinutes, 0), result);
        }

        [TestMethod]
        public void GetDate_should_populate_hour_minute_and_second()
        {
            var sut = new FloatingDate(8, 1, DayOfWeek.Tuesday, 13, 23, 56);
            var result = sut.GetDate(2022);
            Assert.Equal(new DateTime(2022, 8, 2, 13, 23, 56), result);
        }
    }
}
