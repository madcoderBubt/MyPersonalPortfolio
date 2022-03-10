using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using MyPortfolio.Models;
using System.Net;
using Microsoft.AspNet.Identity;

namespace MyPortfolio.Controllers
{
    public class DefaultController : Controller
    {
        private BlogContext db = new BlogContext();
        //private ApplicationDbContext userdb = new ApplicationDbContext();

        // GET: Default
        public ActionResult Index()
        {
            ViewBag.SelectedItem = "Home";
            
            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.SelectedItem = "Blog";
            
            var postModel = db.PostModels.Include(p => p.Catagory);
            return View(postModel.ToList());
        }
        
        public ActionResult Portfolio()
        {
            ViewBag.SelectedItem = "Portfolio";
            return View();
        }

        public ActionResult Project()
        {
            ViewBag.SelectedItem = "Project";
            return View();
        }
        
        //Get View Defealt
        public ActionResult Contact()
        {
            ViewBag.SelectedItem = "Contact";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "ID, UserName, Email, Phone, Subject, Message")] ContactModel contactModel)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.ContactModels.Add(contactModel);
                    db.SaveChanges();
                    //return RedirectToAction("Details", contactModel.ID);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Messages(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContactModel contactModel = db.ContactModels.Find(id);
            if (contactModel == null)
            {
                return HttpNotFound();
            }

            return View(contactModel);
        }

        // GET: Comments/Create
        public ActionResult NewComment(int? id)
        {
            //ViewBag.PostID = new SelectList(db.PostModels, "ID", "Title");
            //ViewBag.PostTitle = postModel.Title;
            ViewBag.PostID = id;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewComment([Bind(Include = "ID,UserName,UserEmail,Content,Date,IsUserHaveId,UserID,PostID")] CommentModel commentModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                commentModel.UserID = User.Identity.GetUserId();
                commentModel.IsUserHaveId = true;
            }
            commentModel.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.CommentModels.Add(commentModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.PostID = new SelectList(db.PostModels, "ID", "Title", commentModel.PostID);
            return View(commentModel);
        }

    }
}

