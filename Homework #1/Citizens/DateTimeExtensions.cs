using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citizens
{
    public static class DateTimeExtensions
    {
        public static string ToFriendlyDateString(this DateTime Date)
        {
            string formattedDate = "";
            if (Date.Date == SystemDateTime.Now.Invoke().Date)
            {
                formattedDate = "today";
            }
            else if (Date.Date == SystemDateTime.Now.Invoke().AddDays(-1))
            {
                formattedDate = "yesterday";
            }
            else if (Date.Date > SystemDateTime.Now.Invoke().AddDays(-6))
            {
                // *** Show the Day of the week
                formattedDate = Date.ToString("dddd").ToString();
            }
            else
            {
                formattedDate = Date.ToString("MMMM dd, yyyy");
            }

            return formattedDate;
        }
    }
}
