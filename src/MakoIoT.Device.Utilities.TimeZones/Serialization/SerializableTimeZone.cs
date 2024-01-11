using System;

namespace MakoIoT.Device.Utilities.TimeZones.Serialization
{
    public class SerializableTimeZone
    {
        public string StandardName { get; set; }
        public string DstName { get; set; }
        public SerializableFixedDate DstStartsFixed { get; set; }
        public SerializableFloatingDate DstStartsFloating { get; set; }
        public SerializableFixedDate DstEndsFixed { get; set; }
        public SerializableFloatingDate DstEndsFloating { get; set; }
        public TimeSpan StandardUtcOffset { get; set; }
        public TimeSpan DstUtcOffset { get; set; }
        public bool HasDst { get; set; }

        public SerializableTimeZone()
        {
            
        }

        public SerializableTimeZone(TimeZone tz)
        {
            StandardName = tz.StandardName;
            DstName = tz.DstName;
            DstStartsFixed = tz.DstStarts is FixedDate ? new SerializableFixedDate((FixedDate)tz.DstStarts) : null;
            DstStartsFloating = tz.DstStarts is FloatingDate ? new SerializableFloatingDate((FloatingDate)tz.DstStarts) : null;
            DstEndsFixed = tz.DstEnds is FixedDate ? new SerializableFixedDate((FixedDate)tz.DstEnds) : null;
            DstEndsFloating = tz.DstEnds is FloatingDate ? new SerializableFloatingDate((FloatingDate)tz.DstEnds) : null;
            StandardUtcOffset = tz.StandardUtcOffset;
            DstUtcOffset = tz.DstUtcOffset;
            HasDst = tz.HasDst;
        }

        public TimeZone GetTimeZone()
        {
            return new TimeZone(StandardName, DstName, StandardUtcOffset, DstUtcOffset, 
                (ICalendarDate)DstStartsFixed?.GetFixedDate() ?? DstStartsFloating.GetFloatingDate(), 
                (ICalendarDate)DstEndsFixed?.GetFixedDate() ?? DstEndsFloating.GetFloatingDate());
        }
    }
}
