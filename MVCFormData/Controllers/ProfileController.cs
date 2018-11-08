using MVCFormData.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFormData.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        SiliconTechDBContext db = new SiliconTechDBContext();
        public ActionResult view_profile(int id) {
            ViewBag.id = id;
            return View();
        }
        public JsonResult get_user()
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var a = db.Users.Select(i => new { i.UserId, i.UserName, i.UserEmail, i.UserProfilePicture, i.UserType});
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            int id = -1;
            if (Session["login_type"] != null)
            {
                id = Convert.ToInt32(Session["user_id"]);

            }
            else if (Request.Cookies["user_info"] != null)
            {

                id = Convert.ToInt32(Request.Cookies["user_info"]["user_id"]);
            }
            var user = db.Users.FirstOrDefault(c => c.UserId == id);
            ViewBag.name = user.UserName;
            ViewBag.user_id = user.UserId;
            ViewBag.email = user.UserEmail;
            ViewBag.user_type = user.UserType;
            ViewBag.profile_picture = user.UserProfilePicture;
            return View();
        }
        [HttpPost]
        public ActionResult edit_profile(User u)
        {
            string login_type = "0";
            if (Session["login_type"] != null)
            {
                u.UserId = Convert.ToInt32(Session["user_id"]);
                login_type = "session";

            }
            else if (Request.Cookies["user_info"] != null)
            {
                login_type = "cookie";
                u.UserId = Convert.ToInt32(Request.Cookies["user_info"]["user_id"]);
            }
            var user = db.Users.FirstOrDefault(c => c.UserId == u.UserId);
            if (u.UserName != null)
            {
                user.UserName = u.UserName;
                if (login_type == "session")
                {
                    Session["user_name"] = user.UserName;
                }
                else if (login_type == "cookie")
                {
                    HttpCookie cookie = new HttpCookie("user_info");
                    cookie.Values["user_name"] = user.UserName;
                    cookie.Values["user_type"] = user.UserType;
                    cookie.Values["user_id"] = user.UserId.ToString();
                    cookie.Values["login_type"] = "cookie";
                    cookie.Expires = DateTime.Now.AddDays(10);
                    Response.SetCookie(cookie);
                }
            }
            if (u.UserEmail != null)
                user.UserEmail = u.UserEmail;
            if (u.UserPassword != null)
            {
                user.UserPassword = u.UserPassword;
                user.ConfirmPassword = u.ConfirmPassword;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private bool isValidContentType(string contentType)
        {
            return contentType.Equals("image/png") || contentType.Equals("image/jpg") ||
                contentType.Equals("image/gif") || contentType.Equals("image/jpeg");
        }
        private bool isValidContentLength(double contentLength)
        {
            if (contentLength < 4000000000)
            {
                return true;
            }
            else return false;

        }
        [HttpPost]
        public ActionResult edit_profile_picture(HttpPostedFileBase fileToUpload)
        {

            if (!isValidContentType(fileToUpload.ContentType))
            {
                ViewBag.Error = "Sorry, only JPG, JPEG, PNG & GIF files are allowed.";
                return View();
            }
            else
            {
                if (fileToUpload.ContentLength > 0)
                {
                    int id = -1;
                    if (Session["login_type"] != null)
                    {
                        id = Convert.ToInt32(Session["user_id"]);
                    }
                    else if (Request.Cookies["user_info"] != null)
                    {
                        id = Convert.ToInt32(Request.Cookies["user_info"]["user_id"]);
                    }
                    var user = db.Users.FirstOrDefault(c => c.UserId == id);
                    var fileName = Path.GetFileName(fileToUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    fileToUpload.SaveAs(path);
                    user.UserProfilePicture = "/images/" + fileName;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}