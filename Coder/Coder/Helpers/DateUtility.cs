﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coder.Helpers
{
    public class DateUtility
    {
        /*
        * Takes in the time since comment was made and returns it as a string, rather than a large number.
        */
        public static string TimeAgoFromDateTime(DateTime date)
        {
            const int SECOND = 1;
            const int MINUTE = 60*SECOND;
            const int HOUR = 60*MINUTE;
            const int DAY = 24*HOUR;
            const int MONTH = 30*DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1*MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2*MINUTE)
                return "a minute ago";

            if (delta < 45*MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90*MINUTE)
                return "an hour ago";

            if (delta < 24*HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48*HOUR)
                return "yesterday";

            if (delta < 30*DAY)
                return ts.Days + " days ago";

            if (delta < 12*MONTH)
            {
                var months = Convert.ToInt32(Math.Floor((double) ts.Days/30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            var years = Convert.ToInt32(Math.Floor((double) ts.Days/365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}