using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingPlannerContext _context;
		public HomeController(WeddingPlannerContext context)
		{
			_context = context;
		}

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.NotLoggedIn = TempData["NotLoggedIn"];
            return View();
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                List<Wedding> TheseWeddings = _context.weddings.Include(w => w.Attenders).ToList();
                foreach(var wedding in TheseWeddings){
                    if(wedding.Date < DateTime.Today)
                    {
                        Wedding this_wedding = _context.weddings.SingleOrDefault(w => w.WeddingId == wedding.WeddingId);
                        List<WeddingAttendance> weddingattenders = _context.weddingattendance.Where(a => a.WeddingId == wedding.WeddingId).ToList();
                        foreach(var attender in weddingattenders){
                            _context.weddingattendance.Remove(attender);
                        }
                        _context.weddings.Remove(this_wedding);
                        _context.SaveChanges();
                    }
                }
                List<Wedding> AllWeddings = _context.weddings.Include(w => w.Attenders).ToList();
                return View("Dashboard", AllWeddings);
            }
            else
            {
                TempData["NotLoggedIn"] = "You must be logged in to view Wedding Planner";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("planwedding")]
        public IActionResult PlanWedding()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                TempData["NotLoggedIn"] = "You must be logged in to view Wedding Planner";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("weddinginfo/{wid}")]
        public IActionResult WeddingInfo(int wid)
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                IEnumerable<User> AllUsers = _context.users.Include(u => u.Attending);
                List<User> Guests = new List<User>();
                Wedding this_wedding = _context.weddings.SingleOrDefault(w => w.WeddingId == wid);
                foreach(var user in AllUsers)
                {
                    foreach(var wedding in user.Attending)
                    {
                        if(wedding.WeddingId == wid)
                        {
                            Guests.Add(user);
                        }
                    }
                }
                ViewBag.WedderOne = this_wedding.WedderOne;
                ViewBag.WedderTwo = this_wedding.WedderTwo;
                ViewBag.Address = this_wedding.Address;
                ViewBag.Date = String.Format("{0: MMM d, yyyy }", this_wedding.Date);
                return View("WeddingInfo", Guests);
            }
            else{
                TempData["NotLoggedIn"] = "You must be logged in to view Wedding Planner";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("registerprocess")]
        public IActionResult RegisterProcess(UserValidator newuser)
        {
            if(ModelState.IsValid)
            {
                User DBUser = _context.users.SingleOrDefault(u=>u.Email == newuser.Email);
                if(DBUser != null)
                {
                    ViewBag.Error = "Email already exists in Database";
                    return View("Index");
                }
                PasswordHasher<UserValidator> Hasher = new PasswordHasher<UserValidator>();
                newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
                User this_user = new User
                {
                    FirstName = newuser.FirstName,
                    LastName = newuser.LastName,
                    Email = newuser.Email,
                    Password = newuser.Password,
                };
                _context.Add(this_user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", this_user.UserId);
                HttpContext.Session.SetString("UserFirstName", this_user.FirstName);
                return RedirectToAction("Dashboard");
            }
            else{
                return View("Index");
            }
        }

        [HttpPost]
        [Route("loginprocess")]
        public IActionResult LoginProcess(string LEmail, string LPassword)
        {
            User myUser = _context.users.SingleOrDefault(u => u.Email == LEmail);
            if(myUser != null && LPassword != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(myUser, myUser.Password, LPassword))
                {
                    HttpContext.Session.SetInt32("UserId", myUser.UserId);
                    HttpContext.Session.SetString("UserFirstName", myUser.FirstName);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.BadPass = "Password Incorrect.";
                    return View("Index");
                }
            }
            else{
                if(myUser == null)
                {
                    ViewBag.NoUser = "Could not locate user with that email.";
                }
                if(LPassword == null)
                {
                    ViewBag.PassNull = "You must enter a password.";
                }
                return View("Index");
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("addwedding")]
        public IActionResult AddWedding(Wedding new_wedding)
        {
            if(ModelState.IsValid)
            {
                if(new_wedding.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Date", "Date must be in the future.");
                    return View("PlanWedding");
                }
                else
                {
                    Wedding this_wedding = new Wedding
                    {
                        Address = new_wedding.Address,
                        Date = new_wedding.Date,
                        WedderOne = new_wedding.WedderOne,
                        WedderTwo = new_wedding.WedderTwo,
                        UserId = (int) HttpContext.Session.GetInt32("UserId")
                    };
                    _context.Add(this_wedding);
                    _context.SaveChanges();
                    
                    return Redirect("~/weddinginfo/"+this_wedding.WeddingId);
                }
            }
            else
            {
                if(new_wedding.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Date", "Date must be in the future.");
                }
                return View("PlanWedding");
            }
        }

        [HttpPost]
        [Route("rsvp")]
        public IActionResult RSVP(int weddingid)
        {
            WeddingAttendance new_attendance = new WeddingAttendance
            {
                UserId= (int) HttpContext.Session.GetInt32("UserId"),
                WeddingId = weddingid
            };
            _context.Add(new_attendance);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("unrsvp")]
        public IActionResult Un_RSVP(int weddingid)
        {
            WeddingAttendance this_attender = _context.weddingattendance.SingleOrDefault(a => a.UserId == HttpContext.Session.GetInt32("UserId") && a.WeddingId == weddingid);
            _context.weddingattendance.Remove(this_attender);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("deletewedding")]
        public IActionResult DeleteWedding(int weddingid)
        {
            Wedding this_wedding = _context.weddings.SingleOrDefault(w => w.WeddingId == weddingid);
            List<WeddingAttendance> weddingattenders = _context.weddingattendance.Where(a => a.WeddingId == weddingid).ToList();
            foreach(var attender in weddingattenders){
                _context.weddingattendance.Remove(attender);
            }
            _context.weddings.Remove(this_wedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

















namespace WeddingPlanner
{
	public static class SessionExtensions
	{
		public static void SetObjectAsJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		public static T GetObjectFromJson<T>(this ISession session, string key)
		{
			string value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}
