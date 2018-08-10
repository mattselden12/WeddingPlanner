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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult PlanWedding()
        {
            return View();
        }

        public IActionResult WeddingInfo()
        {
            return View();
        }

        // [HttpPost]
        // [Route("registerprocess")]
        // public IActionResult RegisterProcess(UserValidator newuser)
        // {
        //     if(ModelState.IsValid)
        //     {
        //         PasswordHasher<UserValidator> Hasher = new PasswordHasher<UserValidator>();
        //         newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
        //         User this_user = new User
        //         {
        //             FirstName = newuser.FirstName,
        //             LastName = newuser.LastName,
        //             Email = newuser.Email,
        //             Password = newuser.Password,
        //         };
        //         _context.Add(this_user);
        //         _context.SaveChanges();
        //         HttpContext.Session.SetInt32("UserId", this_user.UserId);
        //         HttpContext.Session.SetString("UserFirstName", this_user.FirstName);

        //         return RedirectToAction("Wall");
        //     }
        //     else{
        //         return View("Index");
        //     }
        // }

        // [HttpPost]
        // [Route("loginprocess")]
        // public IActionResult LoginProcess(string LEmail, string LPassword)
        // {
        //     User myUser = _context.users.SingleOrDefault(u => u.Email == LEmail);
        //     if(myUser != null && LPassword != null)
        //     {
        //         var Hasher = new PasswordHasher<User>();
        //         if(0 != Hasher.VerifyHashedPassword(myUser, myUser.Password, LPassword))
        //         {
        //             HttpContext.Session.SetInt32("UserId", myUser.UserId);
        //             HttpContext.Session.SetString("UserFirstName", myUser.FirstName);
        //             return RedirectToAction("Dashboard");
        //         }
        //         else
        //         {
        //             ViewBag.BadPass = "Password Incorrect.";
        //             return View("Index");
        //         }
        //     }
        //     else{
        //         if(myUser == null)
        //         {
        //             ViewBag.NoUser = "Could not locate user with that email.";
        //         }
        //         if(LPassword == null)
        //         {
        //             ViewBag.PassNull = "You must enter a password.";
        //         }
        //         return View("Index");
        //     }
        // }

        // [HttpGet]
        // [Route("logoff")]
        // public IActionResult Logoff()
        // {
        //     HttpContext.Session.Clear();
        //     return RedirectToAction("Index");
        // }








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
