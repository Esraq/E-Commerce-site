using MVCFormData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFormData.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Index(string login_type, string name)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UserName,UserPassword,ConfirmPassword,UserEmail")]User u)
        {
            using (SiliconTechDBContext db = new SiliconTechDBContext())
            {
                if (ModelState.IsValid)
                {
                    u.UserProfilePicture = "dummy_value";
                    u.UserType = "admin";
                    db.Users.Add(u);
                    db.SaveChanges();
                    // u = null;
                    ViewBag.Message = "Registered successfully";
                    //TempData["Message"] = "Registered successfully now you can log in";
                }
                else
                {
                    ModelState.AddModelError("", "Check for incorrect fields");
                }
            }
            return View(u);
        }

    }
}