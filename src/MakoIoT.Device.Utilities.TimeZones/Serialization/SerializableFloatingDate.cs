using System;

namespace MakoIoT.Device.Utilities.TimeZones.Serialization
{
    public class SerializableFloatingDate
    {
        public int Month { get; set; }
        public int Week { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateKind Kind { get; set; }
        public TimeSpan TimeOfDay { get; set; }

        public SerializableFloatingDate()
        {
            
        }

        public SerializableFloatingDate(FloatingDate d)
        {
            Month = d.Month; 
            Week = d.Week; 
            DayOfWeek = d.DayOfWeek; 
            Kind = d.Kind; 
            TimeOfDay = d.TimeOfDay;
        }

        public FloatingDate GetFloatingDate()
        {
            return new FloatingDate(Month, Week, DayOfWeek, TimeOfDay, Kind);
        }
    }
}
