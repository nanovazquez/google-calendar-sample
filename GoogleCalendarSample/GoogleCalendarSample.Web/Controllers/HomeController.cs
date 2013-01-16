using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoogleApiUtils;
using GoogleApiUtils.GoogleCalendarApi;
using GoogleCalendarSample.Models;

namespace GoogleCalendarSample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // By default, we display all the events from the last 10 days
            return ListEvents(DateTime.Now.Subtract(TimeSpan.FromDays(10)), DateTime.Now.AddHours(22 - DateTime.Now.Hour));
        }

        public ActionResult ListEvents(DateTime startDate, DateTime endDate)
        {
            UserProfile userProfile = null;
            using(var context = new UsersContext())
            {
                userProfile = context.UserProfiles.FirstOrDefault(c => c.UserName == User.Identity.Name);
            }

            if (userProfile == null) return RedirectToAction("Register", "Account");

            var authenticator = GetAuthenticator();

            var service = new GoogleCalendarServiceProxy(authenticator);
            var model = service.GetEvents(userProfile.Email, startDate, endDate);

            ViewBag.StartDate = startDate.ToShortDateString();
            ViewBag.EndDate = endDate.ToShortDateString();
            return View("Index", model);
        }

        private GoogleAuthenticator GetAuthenticator()
        {
            var authenticator = (GoogleAuthenticator)Session["authenticator"];

            if (authenticator == null || !authenticator.IsValid)
            {
                // Get a new Authenticator using the Refresh Token
                var refreshToken = new UsersContext()
                                        .GoogleRefreshTokens
                                        .FirstOrDefault(c => c.UserName == User.Identity.Name)
                                        .RefreshToken;
                authenticator = GoogleAuthorizationHelper.RefreshAuthenticator(refreshToken);
                Session["authenticator"] = authenticator;
            }

            return authenticator;
        }
    }
}
