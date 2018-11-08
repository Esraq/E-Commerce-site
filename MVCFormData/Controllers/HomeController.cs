using MVCFormData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace MVCFormData.Controllers
{
    public class HomeController : Controller
    {
        SiliconTechDBContext db = new SiliconTechDBContext();
        public JsonResult get_post() 
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var a = db.Posts.Select(i => new { i.PostId, i.PostContent, i.PostDate, i.PostImage, i.PostTitle, i.UserId, i.PostType });
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        // GET: Home
        public ActionResult Index()
        {
            //var posts = db.Posts.Include(p => p.User);
            return View();
        }

    }
}