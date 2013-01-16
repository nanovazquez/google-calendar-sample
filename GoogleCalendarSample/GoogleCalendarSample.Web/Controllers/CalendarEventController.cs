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
    public class CalendarEventController : Controller
    {
        //
        // GET: /CalendarEvent/Crate

        [HttpGet]
        public ActionResult Create()
        {
            string calendarId = string.Empty;
            using (var context = new UsersContext())
            {
                calendarId = context.UserProfiles.FirstOrDefault(c => c.UserName == User.Identity.Name).Email;
            }

            var model = new CalendarEvent()
            {
                CalendarId = calendarId,
                Title = "Stand up meeting",
                Location = "Starbucks",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMinutes(60),
                Description = "Let's start this day with a great cup of coffee"
            };
            var colorList = Enum.GetValues(typeof(GoogleEventColors)).Cast<GoogleEventColors>()
                                .Select(v => new SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() });

            ViewBag.Colors = new SelectList(colorList, "Value", "Text");
            return View(model);
        }

        //
        // POST: /CalendarEvent/Create

        [HttpPost]
        public ActionResult Create(CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                var authenticator = GetAuthenticator();
                var service = new GoogleCalendarServiceProxy(authenticator);

                service.CreateEvent(calendarEvent);
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /CalendarEvent/Edit?calendarId={calendarId}&eventId={eventId}

        public ActionResult Edit(string calendarId, string eventId)
        {
            var authenticator = GetAuthenticator();
            var service = new GoogleCalendarServiceProxy(authenticator);
            var model = service.GetEvent(calendarId, eventId);
            var colorList = Enum.GetValues(typeof(GoogleEventColors)).Cast<GoogleEventColors>()
                                .Select(v => new SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() });

            ViewBag.Colors = new SelectList(colorList, "Value", "Text");
            return View(model);
        }

        //
        // POST: /CalendarEvent/Edit

        [HttpPost]
        public ActionResult Edit(CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                var authenticator = GetAuthenticator();
                var service = new GoogleCalendarServiceProxy(authenticator);

                service.UpdateEvent(calendarEvent);
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /CalendarEvent/Delete?calendarId={calendarId}&eventId={eventId}

        public ActionResult Delete(string calendarId, string eventId)
        {
            var authenticator = GetAuthenticator();
            var service = new GoogleCalendarServiceProxy(authenticator);
            service.DeleteEvent(calendarId, eventId);

            return RedirectToAction("Index", "Home");
        }

        private GoogleAuthenticator GetAuthenticator()
        {
            var authenticator = (GoogleAuthenticator)Session["authenticator"];

            if (authenticator == null || !authenticator.IsValid)
            {
                // Get a new Authenticator using the Refresh Token
                var refreshToken = new UsersContext().GoogleRefreshTokens.FirstOrDefault(c => c.UserName == User.Identity.Name).RefreshToken;
                authenticator = GoogleAuthorizationHelper.RefreshAuthenticator(refreshToken);
                Session["authenticator"] = authenticator;
            }

            return authenticator;
        }
    }
}
