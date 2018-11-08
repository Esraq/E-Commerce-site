﻿using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MVCFormData.Models;
using System;
using System.IO;
using System.Web;

namespace MVCFormData.Controllers
{
    public class PostsController : Controller
    {
        SiliconTechDBContext db = new SiliconTechDBContext();
        // GET: Post
        public ActionResult Index(string type)
        {
            ViewBag.type = type;
            return View();
        }
        public ActionResult delete_comment(int delete_id, int post_id)
        {
            var comment = new Comment { CommentId = delete_id };
            db.Comments.Attach(comment);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("post_view/" + post_id);
        }
        public ActionResult post_view(int? id)
        {

            {
                ViewBag.id = id;
                return View();
            }

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
        public ActionResult add_post(Post p, HttpPostedFileBase fileToUpload)
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
                    var fileName = Path.GetFileName(fileToUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    fileToUpload.SaveAs(path);
                    p.PostImage = "/images/" + fileName;
                    p.PostDate = System.DateTime.Now.ToString();
                    if (Session["login_type"] != null)
                    {
                        p.UserId = Convert.ToInt32(Session["user_id"]);

                    }
                    if (Request.Cookies["user_info"] != null)
                    {

                        p.UserId = Convert.ToInt32(Request.Cookies["user_info"]["user_id"]);
                    }
                    db.Posts.Add(p);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult add_post()
        {
            return View();
        }
        public ActionResult edit_post(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult edit_post(Post p, HttpPostedFileBase fileToUpload, string previous_news_picture, int id)
        {
            if (fileToUpload != null)
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
                        var post = db.Posts.FirstOrDefault(c => c.PostId == id);
                        var fileName = Path.GetFileName(fileToUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        fileToUpload.SaveAs(path);
                        post.PostImage = "/images/" + fileName;
                        post.PostDate = System.DateTime.Now.ToString();
                        post.PostContent = p.PostContent;
                        post.PostTitle = p.PostTitle;
                        db.SaveChanges();
                       string fullpath = Server.MapPath(previous_news_picture);
                       FileInfo file = new FileInfo(fullpath);
                          if (file.Exists)//check file exsit or not
                          {
                              file.Delete();
                          }
                    }

                }

            }
            else
            {
                var post = db.Posts.FirstOrDefault(c => c.PostId == id);
                post.PostDate = System.DateTime.Now.ToString();
                post.PostContent = p.PostContent;
                post.PostTitle = p.PostTitle;
                db.SaveChanges();
            }
            return RedirectToAction("post_view/" + id);
            //return View();
        }
        public ActionResult delete_post(int id)
        {
            var post = new Post{ PostId = id };
            db.Posts.Attach(post);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public JsonResult get_post()
        {
            //db.Configuration.ProxyCreationEnabled = false;
            var a = db.Posts.Select(i => new { i.PostId, i.PostContent, i.PostDate, i.PostImage, i.PostTitle, i.UserId, i.PostType, i.User.UserName });
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_comments()
        {
            var a = db.Comments.Select(i => new { i.CommentId, i.CommentContent, i.User.UserId, i.User.UserProfilePicture, i.CommentDate, i.User.UserName, i.Post.PostId });
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult post_view([Bind(Include = "CommentContent")]Comment c, int id)
        {
            // c.CommentContent = "gg";
            c.CommentDate = System.DateTime.Now.ToString();
            c.PostId = id;
            if (Session["login_type"] != null)
            {
                c.UserId = Convert.ToInt32(Session["user_id"]);

            }
            if (Request.Cookies["user_info"] != null)
            {

                c.UserId = Convert.ToInt32(Request.Cookies["user_info"]["user_id"]);
            }
            db.Comments.Add(c);
            db.SaveChanges();

            return RedirectToAction("post_view");

        }
    }
}