using System;

namespace Rabbitr.Utilities
{
    public static class UnixTime
    {
        public static int To(DateTime now)
        {
            long unixTimestamp = now.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return (int)unixTimestamp;
        }

        public static DateTime From(long unixString)
        {
            DateTime unixYear0 = new DateTime(1970, 1, 1);
            long unixTimeStampInTicks = unixString * TimeSpan.TicksPerSecond;
            DateTime dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
            return dtUnix;
        }
    }
}