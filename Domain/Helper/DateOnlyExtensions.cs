using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class DateOnlyExtensions
    {
        public static DateTime ConvertToDateTime(this DateOnly dateOnly)
        {
            var dateTime = dateOnly.ToDateTime(TimeOnly.MinValue);
            return dateTime;
        }
    }
}
