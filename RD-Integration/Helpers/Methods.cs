using RD_INTEGRATION.Models;
using System;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Helpers
{
    public static class Methods
    {
        public static void Log(string message, Data.DBContext context)
        {
            RD_Logs log = new RD_Logs
            {
                LogDateTime = DateTime.Now,
                Message = message
            };

            context.rd_logs.Add(log);
            context.SaveChanges();
        }

        public async static Task LogAsync(string message, Data.DBContext context)
        {
            RD_Logs log = new RD_Logs
            {
                LogDateTime = DateTime.Now,
                Message = message
            };

            context.rd_logs.Add(log);
            await context.SaveChangesAsync();
        }

        public static string GetTimestampParameter(DateTime? timestamp, int timezoneOffset)
        {
            if (timestamp != null)
            {
                DateTime timestampData = (DateTime)timestamp;
                return $"$filter=LastModifiedDateTime gt datetimeoffset'{timestampData.AddHours(timezoneOffset):yyyy-MM-ddTHH:mm:ss.99}'";
            }
            else
                return string.Empty;
        }
    }
}
