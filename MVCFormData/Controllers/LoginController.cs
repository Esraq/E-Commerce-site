using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCFormData.Models;

namespace MVCFormData.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            //ViewBag.Message = TempData["Message"];
            return View();
        }
        [HttpPost]
        public ActionResult Index(User user, bool remember_me=false)
        {
            using(SiliconTechDBContext db=new SiliconTechDBContext()){
                var usr = db.Users.Where(u => u.UserName == user.UserName && u.UserPassword == user.UserPassword).FirstOrDefault();﻿
                
                if (usr != null && remember_me == false)
                {
                    ViewBag.login_info = "Logged in Successfully";
                    Session["user_type"] = usr.UserType;
                    Session["user_id"] = usr.UserId.ToString();
                    Session["user_name"] = usr.UserName;
                    Session["login_type"] = "session";

                    return RedirectToAction("Index", "home");
                    //Session["products"]=null;
                    //var products=Session["products"] as List<Product>;
                    //Session["products"]=products;
                    //var products=Db.GetProducts();
                }
                else if (usr != null && remember_me == true)
                {
                    ViewBag.login_info = "Logged in Successfully";
                    HttpCookie cookie = new HttpCookie("user_info");
                    cookie.Values["user_type"] = usr.UserType;
                    cookie.Values["user_id"] = usr.UserId.ToString();
                    cookie.Values["user_name"] = usr.UserName;
                    cookie.Values["login_type"] = "cookie";

                    cookie.Expires = DateTime.Now.AddDays(10);
                    //cookie.HttpOnly = false;
                    Response.SetCookie(cookie);
                    return RedirectToAction("Index", "home");
                    /*
                     string cookievalue ;
                     if ( Request.Cookies["cookie"] != null )
                     {
                        cookievalue = Request.Cookies["cookie"].ToString();
                     }
                     if (Request.Cookies["cookie"] != null)
                    {
                        Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);
                    }
                    */
                }
                else {
                    ModelState.AddModelError("","USername and password not matching");
                }
            }

            return View();
        }
        public ActionResult Logout(string action)
        {

            if (Session["login_type"] != null)
            {
                Session["user_id"] = null;
                Session["user_name"] = null;
                Session["login_type"] = null;
                Session["user_type"] = null;
            }
            if (Response.Cookies["user_info"] != null)
            {

                Response.Cookies["user_info"].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index", "home");
        }
    }

}