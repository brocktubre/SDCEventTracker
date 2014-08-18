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

        public ActionResult Competitions()
        {
            var results = from i in db.Events orderby i.Date descending select i;
            ViewBag.Title = "Competitions";
            return View(results.ToList());
            //List<db> EventList = new List<db>;
            //var query = from item in db.Events where item.ID == 1 select item;
            //var theEvent = query.FirstOrDefault();
            //theEvent.EventName = "Shit Stain Event";
            //db.SaveChanges(); 
            //var query = from item in db.Events where item.ID >= 0 select item;
            //var theEvent = query.FirstOrDefault();            
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
    }
}