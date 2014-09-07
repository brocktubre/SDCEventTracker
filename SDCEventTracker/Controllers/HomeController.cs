﻿using SDCEventTracker.Models;
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
        public Event objEvent = new Event();
        public Result objResult = new Result();

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
            ViewBag.Title = "Submit New Hunt";
            return View();
        }

        // HttpPost is used to submit upcoming hunt and make sure all fields are correct. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitEvent([Bind(Include = "EventName,Date,Location,City,State,Zip,MorningHunt,EveningHunt,BenchShow,BarkingContest")] Event EventToCreate)
        {
            ViewBag.Title = "Submit New Hunt";
            // if statement passes if Valid and state string has a value
            if (ModelState.IsValid && EventToCreate.State != string.Empty)
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
            ViewBag.EventEnum1 = (from i in db.EventEnums where i.ID == 1 select i.EventType).First(); // "Morning Hunt"
            ViewBag.EventEnum2 = (from i in db.EventEnums where i.ID == 2 select i.EventType).First(); // "Evening Hunt"
            ViewBag.EventEnum3 = (from i in db.EventEnums where i.ID == 3 select i.EventType).First(); // "Barking Contest"
            ViewBag.EventEnum4 = (from i in db.EventEnums where i.ID == 4 select i.EventType).First(); // "Bench Show"
            var allResults = from i in db.Results orderby i.Place where i.EventID == id select i; // Selects all results

            //AllResults rList = new AllResults();
            //rList.MorningHuntData = from i in db.Results orderby i.Place where i.EventID == id && i.EventEnum.ID == 1 select i; // Selects the results for EventID == id and the Morning Hunt
            //rList.EveningHuntData = from i in db.Results orderby i.Place where i.EventID == id && i.EventEnum.ID == 2 select i; // Selects the results for EventID == id and Evening Hunt

            return View(allResults.ToList());
        }

        // User can submit results for completed hunts
        public ActionResult SubmitResults(int id)
        {
            ViewBag.Title = "Submit Results";
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            //Result thisResult = new Result();
            //thisResult.EventID = @event.ID; // stores the id as EventID in Result

            //var q = (from i in db.Events select i.EventName).ToList(); ViewBag.EventNames = q;
            //q = (from i in db.Dogs select i.Name).ToList(); ViewBag.DogNames = q;
            //var q = (from i in db.EventEnums select i.EventType).ToList();
            //List<SelectListItem> eventTypeList = new List<SelectListItem>();
            //eventTypeList.AddRange((from i in db.EventEnums select i.EventType).ToList());
            //ViewBag.EventTypes = eventTypeList;
            //q = (from i in db.Handlers select i.FirstName).ToList(); ViewBag.HandlerName = q;
            //var count = eventList.Count;
            //ViewBag.EventNames = new SelectList(objEvent.EventName, "EventName", "EventName");
            //ViewBag.EventTypes = new SelectList(objResult.EventEnum.EventType, "EventType", "EventType");
            //ViewBag.HandlerNames = new SelectList(objResult.Handler.FirstName, "HandlerName", "HandlerName");
            //ViewBag.HandlerNames = new SelectList(objResult.Dog.Name, "DogName", "DogName");
            return View();
        }

        // HttpPost is used to submit hunting results and make's sure all fields are correct.
        // Eventy Type, Place, Handler's Name/ID, Dog's Name/ID
        [HttpPost]
        public ActionResult SubmitResults([Bind(Include = "EventType,Place,HandlerID,DogID")] Result ResultsToSubmit, int id)
        {
            ViewBag.Title = "Submit Results";
            // TODO must turn FirstName & LastName into HandlerID
            // TODO must turn Name into DogID
            ResultsToSubmit.EventID = id;
            // if statement passes if Valid and state string has a value
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

    }
}