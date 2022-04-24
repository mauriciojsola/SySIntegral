using System;
using System.Collections.Generic;
using System.Text;

namespace SySIntegral.Core.Application.Common.Utils
{
   public static class DateTimeExtensions
    {
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime FirstMonthDay(this DateTime current)
        {
            var first = current.AddDays(1 - current.Day);
            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime LastMonthDay(this DateTime current)
        {
            var daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

            var last = current.FirstMonthDay().AddDays(daysInMonth - 1);
            return last;
        }

        /// <summary>
        /// Returns the seconds in a h:m:s:ms format
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string SecondsToString(this double seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            return $"{time.Hours:D2}h:{time.Minutes:D2}m:{time.Seconds:D2}s:{time.Milliseconds:D3}ms";
        }

        public static int DaysBetweenDates(this DateTime current, DateTime otherDate)
        {
            return Math.Abs((otherDate.AbsoluteStart() - current.AbsoluteStart()).Days);
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        /// <param name="includeCurrent">Indicates that if the current day is the same as the date of week, it can be returned. Otherwise the 'next' day of week is returned.</param>
        public static DateTime NextDayOfWeek(this DateTime current, DayOfWeek dayOfWeek, bool includeCurrent = false)
        {
            var offsetDays = dayOfWeek - current.DayOfWeek;
            if (offsetDays < 0 || (offsetDays == 0 && !includeCurrent))
                offsetDays += 7;

            var result = current.AddDays(offsetDays);
            return result;
        }

        public static long ToUnixTimestampMilliseconds(this DateTime date, bool useMidnightTime = false)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)((useMidnightTime ? date.AbsoluteStart() : date) - unixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// Converts a NON-UTC date to Universal Time and then returns it in a string format yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z' (Example: 2018-10-08T19:58:47.1Z)  
        /// </summary>
        /// <param name="nonUtcDate"></param>
        /// <returns></returns>
        public static string ToUniversalTimeStringFormat(this DateTime nonUtcDate)
        {
            return nonUtcDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f'Z'");
        }

        /// <summary>
        /// Returns the date in string pattern 'yyyyMMddHHmmssfffffff'
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToLongStringPattern(this DateTime date)
        {
            return date.ToString("yyyyMMddHHmmssfffffff");
        }

    }
}
