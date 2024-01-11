
using System.Diagnostics;
using MakoIoT.Device.Utilities.TimeZones.Serialization;
using nanoFramework.Json;
using nanoFramework.TestFramework;

namespace MakoIoT.Device.Utilities.TimeZones.Test
{
    [TestClass]
    public class SerializableTimeZoneTest
    {
        [TestMethod]
        public void TimeZone_should_serialize_and_deserialize()
        {

            var tz = new SerializableTimeZone(TimeZoneConverter.FromPosixString("CET-1CEST,M3.5.0,M10.5.0/3"));

            var s = JsonConvert.SerializeObject(tz);

            var stz = ((SerializableTimeZone)JsonConvert.DeserializeObject(s, typeof(SerializableTimeZone))).GetTimeZone();

            Assert.AreEqual(tz.StandardName, stz.StandardName);
            Assert.AreEqual(tz.DstName, stz.DstName);
            Assert.AreEqual(tz.StandardUtcOffset, stz.StandardUtcOffset);
            Assert.AreEqual(tz.DstUtcOffset, stz.DstUtcOffset);
        }
    }
}
