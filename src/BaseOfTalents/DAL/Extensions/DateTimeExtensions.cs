using System;

namespace DAL.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime input, DateTime date1, DateTime date2)
        {
            return (input >= date1 && input <= date2);
        }
    }
}
