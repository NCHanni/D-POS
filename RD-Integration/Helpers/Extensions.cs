using RD_INTEGRATION.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RD_INTEGRATION.Helpers
{
    public static class Extensions
    {
        public static string ToStringValue(this Field field, int maxLength = 0)
        {
            if (field == null || field.value == null)
            {
                return string.Empty;
            }
            else
            {
                if (maxLength == 0)
                    return field.value.Trim();
                else
                {
                    var value = field.value.Trim();
                    maxLength = maxLength > value.Length ? value.Length : maxLength;
                    return value.Substring(0, maxLength);
                }
            }
        }

        public static string ToBooleanString(this Field field)
        {
            if (field == null)
            {
                return false.ToString();
            }
            else
            {
                if (bool.TryParse(field.value, out bool result))
                    return result.ToString();
                else
                    return false.ToString();
            }
        }

        public static bool ToBooleanValue(this Field field)
        {
            if (field == null)
            {
                return false;
            }
            else
            {
                if (bool.TryParse(field.value, out bool result))
                    return result;
                else
                    return false;
            }
        }

        public static string ToDateString(this DateTime date, bool dateOnly = false)
        {
            if (date == DateTime.MinValue) //date == null || 
            {
                return string.Empty;
            }
            else
            {
                if (dateOnly)
                    return date.ToString("M/d/yyyy");
                else
                    return date.ToString("M/d/yyyy h:mm tt");
            }
        }

        public static DateTime? ToDateTimeValue(this Field field)
        {
            if (field == null || string.IsNullOrEmpty(field.value))
            {
                return null;
            }
            else
            {
                if (DateTime.TryParse(field.value, out DateTime result))
                    return result;
                else
                    return null;
            }
        }

        public static decimal ToDecimalValue(this Field field)
        {
            if (field == null)
            {
                return 0;
            }
            else
            {
                if (decimal.TryParse(field.value, out decimal result))
                    return result;
                else
                    return 0;
            }
        }

        public static int ToIntValue(this Field field)
        {
            if (field == null)
            {
                return 0;
            }
            else
            {
                if (int.TryParse(field.value, out int result))
                    return result;
                else
                    return 0;
            }
        }

        public static Boolean ToBoolean(this string value)
        {
            if(String.IsNullOrEmpty(value))
            {
                return false;
            }
            else
            {
                if (Boolean.TryParse(value, out Boolean result))
                    return result;
                else
                    return false;
            }
        }

        public static bool IsConnectionError(this Exception ex)
        {
            switch (ex.Source)
            {
                case "System.Net.Http":
                case "System.Private.CoreLib":
                    if (ex.Message.Contains("500") && ex.Message.Contains("Internal Server Error"))
                        return true;
                    if (ex.Message.Contains("connection") && ex.Message.Contains("failed"))
                        return true;
                    if (ex.Message.Contains("No such host is known"))
                        return true;
                    if (ex.Message.Contains("sending the request"))
                        return true;
                    break;
            }
            return false;
        }

        public static void LogError(this Exception ex, Data.DBContext context, string module)
        {
            RD_Error error = new RD_Error
            {
                ErrorDateTime = DateTime.Now,
                Source = ex.Source,
                Message = string.Format("{0} - {1}", module, ex.Message)
            };

            context.rd_error.Add(error);
            context.SaveChanges();
        }

        public async static Task LogErrorAsync(this Exception ex, Data.DBContext context, string module)
        {
            RD_Error error = new RD_Error
            {
                ErrorDateTime = DateTime.Now,
                Source = ex.Source,
                Message = string.Format("{0} - {1}\n{2}", module, ex.Message, ex.StackTrace)
            };

            context.rd_error.Add(error);
            await context.SaveChangesAsync();
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            List<T> nextbatch = new List<T>(batchSize);
            foreach (T item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>(batchSize);
                }
            }
            if (nextbatch.Count > 0)
                yield return nextbatch;
        }

    }
}
