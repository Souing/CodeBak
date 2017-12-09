using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Yst.Framework.Utils
{
    public class TimeHelper
    {
        public TimeHelper()
        {

        }

        public static string TimeTrunTimeZone(DateTime dtime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(dtime.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return timeStamp;
        }

        public static DateTime TimeZoneTrunTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }
    }
}
