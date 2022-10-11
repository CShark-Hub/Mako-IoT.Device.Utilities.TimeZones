using System;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Utilities.TimeZones.Test
{
    [TestClass]
    public class FixedDateTest
    {
        [TestMethod]
        public void GetDate_should_return_date_with_the_given_year()
        {
            var sut = new FixedDate(new DateTime(1990, 8, 2, 13, 23, 56));
            var result = sut.GetDate(2022);
            Assert.Equal(new DateTime(2022, 8, 2, 13, 23, 56), result);
        }
    }
}
