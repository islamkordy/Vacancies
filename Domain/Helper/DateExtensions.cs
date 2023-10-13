using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class DateExtensions
    {
        public static bool IsDateInHijriCalendar(this DateTime date)
        {
            HijriCalendar hijriCalendar = new HijriCalendar();
            int hijriYear = hijriCalendar.GetYear(date);
            return date.Year == hijriYear;
        }

        public static DateTime ConvertToHijri(this DateTime date)
        {
            HijriCalendar hijriCalendar = new HijriCalendar();
            int hijriYear = hijriCalendar.GetYear(date);
            int hijriMonth = hijriCalendar.GetMonth(date);
            int hijriDay = hijriCalendar.GetDayOfMonth(date);
            DateTime hijriDate = new DateTime(hijriYear, hijriMonth, hijriDay);
            return hijriDate;
        }
        public static string ConvertToGregorian(this string hijriDateString)
        {
            DateTime hijriDate=DateTime.Parse(hijriDateString);
            HijriCalendar hijriCalendar = new HijriCalendar();
            DateTime gregorianDate = hijriCalendar.ToDateTime(hijriDate.Year, hijriDate.Month, hijriDate.Day, 0, 0, 0, 0);
            return gregorianDate.ToString("yyyy-MM-dd");
        }
    }
}






