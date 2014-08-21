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

        [HttpPost]
        public ActionResult Competitions()
        {
            ViewBag.Title = "Competitions";
            // results stores the linq query that produces all the Events in descending order
            var results = from i in db.Events orderby i.Date descending select i;
            return View(results.ToList());  
        }

        [HttpGet]
        public ActionResult Competitions(string searchString)
        {

            ViewBag.Title = "Competitions";
            var results = from i in db.Events orderby i.Date descending select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                return View(db.Events.OrderByDescending(item => item.Date).Where(item => item.EventName.Contains(searchString)));
            }
            else
            {
                return View(results.ToList());
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult Create(string searchString)
        {
            ViewBag.Title = "Submit New Hunt";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventName,Date,Location,City,State,Zip,MorningHunt,EveningHunt,BenchShow,BarkingContest")] Event EventToCreate)
        {
            ViewBag.Title = "Submit New Hunt";
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

        [HttpGet]
        public ActionResult EventDetails(int? id)
        {
            ViewBag.Title = "Event Details";
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if(@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        [HttpGet]
        public ActionResult EventResults(int? id)
        {
            ViewBag.Title = "Event Results";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result @result = db.Results.Find(id);
            if (@result == null)
            {
                return HttpNotFound();
            }
            return View(@result);
        }

    }
}