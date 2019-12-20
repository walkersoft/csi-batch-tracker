using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
