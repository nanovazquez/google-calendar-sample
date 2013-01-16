using System.Web;
using System.Web.Mvc;
using GoogleCalendarSample.Filters;

namespace GoogleCalendarSample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitializeSimpleMembershipAttribute());
        }
    }
}