namespace MakoIoT.Device.Utilities.TimeZones
{
    /// <summary>
    /// Specifies whether an object represents a local time or a Coordinated Universal Time (UTC).
    /// </summary>
    public enum DateKind
    {
        /// <summary>The time represented is UTC.</summary>
        Utc = 1,
        /// <summary>The time represented is local time.</summary>
        Local = 2
    }
}
