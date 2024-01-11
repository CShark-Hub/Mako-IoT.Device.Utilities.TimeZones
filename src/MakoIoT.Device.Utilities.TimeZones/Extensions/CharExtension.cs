namespace MakoIoT.Device.Utilities.TimeZones.Extensions
{
    public static class CharExtension
    {
        public static bool IsLetter(this char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

    }
}
