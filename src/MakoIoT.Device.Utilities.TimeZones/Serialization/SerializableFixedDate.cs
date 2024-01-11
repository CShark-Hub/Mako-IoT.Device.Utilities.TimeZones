using System;
using System.Runtime.CompilerServices;

namespace MakoIoT.Device.Utilities.TimeZones.Serialization
{
    public class SerializableFixedDate
    {
        public DateTime Date { get; set; }
        public DateKind Kind { get; set; }

        public SerializableFixedDate()
        {
            
        }

        public SerializableFixedDate(FixedDate d)
        {
            Date = d.Date;
            Kind = d.Kind;
        }

        public FixedDate GetFixedDate()
        {
            return new FixedDate(Date, Kind);
        }
    }
}
