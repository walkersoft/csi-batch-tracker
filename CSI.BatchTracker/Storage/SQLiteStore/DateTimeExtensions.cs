using System;

namespace CSI.BatchTracker.Storage.SQLiteStore
{
    public static class DateTimeExtensions
    {
        public static string FormatForDatabase(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
