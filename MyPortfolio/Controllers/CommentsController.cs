using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPortfolio.Models;
using Microsoft.AspNet.Identity;

namespace MyPortfolio.Controllers
{
    public class CommentsController : Controller
    {
        private BlogContext db = new BlogContext();

        // GET: Comments
        public ActionResult Index()
        {
            var commentModels = db.CommentModels.Include(c => c.Post);
            return View(commentModels.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentModel commentModel = db.CommentModels.Find(id);
            if (commentModel == null)
            {
                return HttpNotFound();
            }
            return View(commentModel);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.PostID = new SelectList(db.PostModels, "ID", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,UserEmail,Content,Date,IsUserHaveId,UserID,PostID")] CommentModel commentModel)
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

            ViewBag.PostID = new SelectList(db.PostModels, "ID", "Title", commentModel.PostID);
            return View(commentModel);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentModel commentModel = db.CommentModels.Find(id);
            if (commentModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostID = new SelectList(db.PostModels, "ID", "Title", commentModel.PostID);
            return View(commentModel);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,UserEmail,Content,Date,IsUserHaveId,UserID,PostID")] CommentModel commentModel)
        {
            commentModel.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(commentModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostID = new SelectList(db.PostModels, "ID", "Title", commentModel.PostID);
            return View(commentModel);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentModel commentModel = db.CommentModels.Find(id);
            if (commentModel == null)
            {
                return HttpNotFound();
            }
            return View(commentModel);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommentModel commentModel = db.CommentModels.Find(id);
            db.CommentModels.Remove(commentModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
