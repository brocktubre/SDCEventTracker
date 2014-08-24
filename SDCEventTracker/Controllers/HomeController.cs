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
            var allEvents = from i in db.Events orderby i.Date descending select i; 
            if (!String.IsNullOrEmpty(searchString))
            {
                return View(db.Events.OrderByDescending(item => item.Date).Where(item => item.EventName.Contains(searchString)));
            }
            else
            {
                return View(allEvents.ToList());
            }

        }

        // HttpGet is used to submit upcoming event, date cannot be in the past
        [Authorize] //User must be logged in
        [HttpGet]
        public ActionResult SubmitEvent(string searchString)
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
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // .Find(id) finds the EventId and stores is into @event
            Event @event = db.Events.Find(id);
            if(@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // HttpGet is used to show the Event Results: Place, Handler's Name, and Dog's Name.
        [HttpGet]
        public ActionResult EventResults(int? id)
        {
            ViewBag.Title = "Event Results";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var morningHuntResults = from i in db.Results orderby i.Place where i.EventID == id && i.EventEnum.ID == 1 select i; // Selects the results for EventID == id and the Morning Hunt
            var eveningHuntResults = from i in db.Results orderby i.Place where i.EventID == id && i.EventEnum.ID == 2 select i; // Selects the results for EventID == id and Evening Hunt

            return View(morningHuntResults.ToList());
        }

    }
}