using System;
using System.Globalization;

namespace Eshop.Utilities
{
    public static class PersianCalender
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return
                $"{pc.GetYear(value).ToString("0000")}/{pc.GetMonth(value).ToString("00")}/{pc.GetDayOfMonth(value).ToString("00")}";
        }
    }
}