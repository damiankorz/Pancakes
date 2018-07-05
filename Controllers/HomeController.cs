using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Activities.Models;

namespace Activities.Controllers
{
    public class HomeController : Controller
    {
        private ActivityContext _context;
        public HomeController(ActivityContext context)
        {
            _context = context;
        }
        // GET: /home
        [HttpGet("home")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("id") != null)
            {
                HomeModels data = new HomeModels
                {
                    AllActivities = _context.Activities.Include(a => a.Participants).Include(a => a.User).OrderBy(a => a.Date).ThenBy(a => a.Time).ToList(),
                    User = _context.Users.Where(u => u.ID == HttpContext.Session.GetInt32("id")).SingleOrDefault(),
                };
                return View(data);
            }
            return RedirectToAction("Index", "User");
        }
        // GET: /new
        [HttpGet("new")]
        public IActionResult New()
        {
            if(HttpContext.Session.GetInt32("id") != null)
            {
                return View();
            }
            return RedirectToAction("Index", "User");
        }
        // GET: /activity/id
        [HttpGet("activity/{id}")]
        public IActionResult Activity(int id)
        {
            if(HttpContext.Session.GetInt32("id") != null)
            {
                ShowActivity data = new ShowActivity
                {
                    Activity = _context.Activities.Where(a => a.ID == id).Include(a => a.User).Include(a => a.Participants).ThenInclude(p => p.User).SingleOrDefault(),
                    User = _context.Users.Where(u => u.ID == HttpContext.Session.GetInt32("id")).SingleOrDefault(),
                };
                return View(data);
            }
            return RedirectToAction("Index", "User");
        }
        // GET: /logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }
        // POST: /create
        [HttpPost("new/create")]
        public IActionResult Create(ActivityModel model, string stringDuration)
        {
            if(ModelState.IsValid)
            {
                ActivityModel newActivity = new ActivityModel
                {
                    UserID = (int)HttpContext.Session.GetInt32("id"),
                    Title = model.Title,
                    Time = model.Time,
                    Date = model.Date,
                    Duration = model.Duration + " " + stringDuration,
                    Description = model.Description,
                };
                _context.Add(newActivity);
                _context.SaveChanges();
                return RedirectToAction("Activity", new{id = newActivity.ID});
            }
            return View("New", model);
        }
        // GET: /activity/id/delete
        [HttpGet("activity/{id}/delete")]
        public IActionResult Delete(int id)
        {
            List<Participant> data = _context.Participants.Where(p => p.ActivityID == id).ToList();
            data.RemoveAll(x => x.ActivityID == id);
            _context.SaveChanges();
            ActivityModel activity = _context.Activities.Where(a => a.ID == id).SingleOrDefault();
            _context.Remove(activity);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: /activity/id/join
        [HttpGet("activity/{id}/join")]
        public IActionResult Join(int id)
        {
            //Check if user joining activity doesn't conflict with another activity
            ActivityModel activity = _context.Activities.Where(a => a.ID == id).SingleOrDefault();
            List<Participant> RSVPS = _context.Participants.Where(p => p.UserID == HttpContext.Session.GetInt32("id")).Include(p => p.Activity).ToList();
            foreach(var schedule in RSVPS)
            {
                if(schedule.Activity.Date == activity.Date && schedule.Activity.Time == activity.Time && schedule.Activity.Duration == activity.Duration)
                {
                    TempData["error"] = "Cannot join activity. Time conflict with other activity";
                    return RedirectToAction("Index");
                }
            }
            Participant data = new Participant
            {
                UserID = (int)HttpContext.Session.GetInt32("id"),
                ActivityID = id,
            };
            _context.Add(data);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: /activity/id/leave
        [HttpGet("activity/{id}/leave")]
        public IActionResult Leave(int id)
        {
            Participant data = _context.Participants.Where(p => p.UserID == HttpContext.Session.GetInt32("id") && p.ActivityID == id).SingleOrDefault();
            _context.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
