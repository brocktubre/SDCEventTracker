using SDCEventTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net; // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

namespace SDCEventTracker.Controllers
{
    public class HomeController : Controller
    {
        public SDC_databaseEntities db = new SDC_databaseEntities();
        //public Event objEvent = new Event();
        //public Result objResult = new Result();

        // Competitions View produces all the Events, past and present
        public ActionResult Competitions()
        {
            // results stores the linq query that produces all the Events in descending order
            var allEvents = from i in db.Events orderby i.Date descending select i;
            ViewBag.Title = "Competitions";
            return View(allEvents.ToList());// returns the list of all the events  
        }

        // HttpGet is used to search through table for specific hunt using searchString
        // allEvents is used if the searchString is null
        [HttpGet]
        public ActionResult Competitions(string searchString)
        {
            ViewBag.Title = "Competitions";
            if (!String.IsNullOrEmpty(searchString))
            {
                var searchResults = db.Events.OrderByDescending(item => item.Date).Where(item => item.EventName.Contains(searchString));
                //if(searchResults.Count() == 0) // if the search term does not match results
                return View(searchResults.ToList());
            }
            else
            {
                var allEvents = from i in db.Events orderby i.Date descending select i;
                return View(allEvents.ToList());
            }

        }

        // HttpGet is used to submit upcoming event, date cannot be in the past
        [Authorize] //User must be logged in
        [HttpGet]
        public ActionResult SubmitEvent()
        {
            ViewBag.Title = "Submit Upcoming Hunt";
            return View();
        }

        // HttpPost is used to submit upcoming hunt and make sure all fields are correct. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitEvent([Bind(Include = "EventName,Date,Location,City,State,Zip,MorningHunt,EveningHunt,BenchShow,BarkingContest")] Event EventToCreate)
        {
            ViewBag.Title = "Submit Upcoming Hunt";
            // if statement passes if Valid and state string has a value
            if (EventToCreate.State == "0")
                return View(@EventToCreate);

            if (ModelState.IsValid)
            {
                db.Events.Add(@EventToCreate);
                db.SaveChanges();
                return RedirectToAction("Competitions");
            }
            else
            {
                return View(@EventToCreate);
            }
        }

        // HttpGet is used to get the results of a past hunt. The EventID is passed in as '/Home/EventDetails?id=EventID'
        [HttpGet]
        public ActionResult EventDetails(int? id)
        {
            ViewBag.Title = "Event Details";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // .Find(id) finds the EventId and stores is into @event
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // HttpGet is used to show the Event Results: Place, Handler's Name, and Dog's Name.
        // id passed in is equal to the Result.EventID
        [HttpGet]
        public ActionResult EventResults(int? id)
        {
            ViewBag.Title = "Event Results";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FillUpViewBag(id); // function fills up the view bag wiht all the event details
            var allResults = from i in db.Results orderby i.Place where i.EventID == id select i; // Selects all results
            
            return View(allResults.ToList());
        }

        // User can submit results for completed hunts
        [HttpGet]
        public ActionResult SubmitResults(int id)
        {
            ViewBag.Title = "Submit Results";
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            FillUpViewBag(id);

            return View();
        }

        // HttpPost is used to submit hunting results and make's sure all fields are correct.
        // Eventy Type, Place, Handler's Name/ID, Dog's Name/ID
        [HttpPost]
        public ActionResult SubmitResults([Bind(Include = "EventType,Place,HandlerID,DogID")] Result ResultsToSubmit, int id)
        {
            ViewBag.Title = "Submit Results";
            // TODO must turn typing in HandlerID into FirstName & LastName
            // TODO must turn typing in DogID into Dog's Name
            ResultsToSubmit.EventID = id; // stores id passed in (Event.EventID)
            if (ModelState.IsValid)
            {
                db.Results.Add(@ResultsToSubmit);
                db.SaveChanges();
                return RedirectToAction("SubmitResults", new { id = ResultsToSubmit.EventID});
            }
            else
            {
                return View(@ResultsToSubmit);
            }
        }

        // function below stores important values in the ViewBag. Things like EventID, EventName, Location, etc..
        public void FillUpViewBag(int? id)
        {
            ViewBag.EventID = id;
            ViewBag.EventName = (from i in db.Events where i.ID == id select i.EventName).First(); // stores Event.EventName
            ViewBag.Location = (from i in db.Events where i.ID == id select i.Location).First(); // stores Event.Loaction
            ViewBag.City = (from i in db.Events where i.ID == id select i.City).First(); //stores Event.City
            ViewBag.State = (from i in db.Events where i.ID == id select i.State).First(); //stores Event.State
            ViewBag.Date = (from i in db.Events where i.ID == id select i.Date).First();
            ViewBag.MorningHunt = (from i in db.Events where i.ID == id select i.MorningHunt).First(); // Finds out if there was or was not an Morning Hunt
            ViewBag.EveningHunt = (from i in db.Events where i.ID == id select i.EveningHunt).First(); // Finds out if there was or was not an Evening Hunt
            ViewBag.BarkingContest = (from i in db.Events where i.ID == id select i.BarkingContest).First(); // Finds out if there was or was not an Barking Contest
            ViewBag.BenchShow = (from i in db.Events where i.ID == id select i.BenchShow).First(); // Finds out if there was or was not an Bench Show
        }

    }
}