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
    [Authorize]
    public class PostsController : Controller
    {
        private BlogContext db = new BlogContext();
        private ApplicationDbContext userdb = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var postModels = db.PostModels.Include(p => p.Catagory).ToList();

            return View(postModels);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostModel postModel = db.PostModels.Find(id);
            if (postModel == null)
            {
                return HttpNotFound();
            }

            ApplicationUser UserInfo = userdb.Users.Find(postModel.UserID);

            ViewBag.UserName = UserInfo.UserName;

            return View(postModel);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.CatagoryID = new SelectList(db.CatagoryModels, "ID", "Catagory");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,PublishedDate,IsModified,ModifiedDate,BodyContent,ShortContent,UrlSlug,CatagoryID,UserID,TagString")] PostModel postModel)
        {
            postModel.PublishedDate = DateTime.Now;
            postModel.IsModified = false;
            postModel.ModifiedDate = DateTime.Today;
            postModel.UrlSlug = postModel.Title.Replace(' ', '_');

            if (User.Identity.IsAuthenticated)
            {
                postModel.UserID = User.Identity.GetUserId();
            }

            string Tag = postModel.tag;
            string[] tags = Tag.Split(',');

            var Tags = db.TagModels.ToList();
            for (int i = 0; i < tags.Length; i++)
            {
                if (Tags.Exists(t => t.Name == tags[i].Trim()))
                {
                    var tg = Tags.Find(f => f.Name == tags[i].Trim());
                    postModel.Tags.Add(tg);
                }
                else
                {
                    var tg = new TagModel() { Name = tags[i] };
                    postModel.Tags.Add(tg);
                }
            }

            if (ModelState.IsValid)
            {
                db.PostModels.Add(postModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatagoryID = new SelectList(db.CatagoryModels, "ID", "Catagory", postModel.CatagoryID);
            return View(postModel);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PostModel postModel = db.PostModels.Find(id);
            if (postModel == null)
            {
                return HttpNotFound();
            }

            //put code here to check is user have permission to change 
            //if (postModel.UserID != User.Identity.GetUserId())
            //{
                
            //}

            ViewBag.CatagoryID = new SelectList(db.CatagoryModels, "ID", "Catagory", postModel.CatagoryID);
            return View(postModel);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,PublishedDate,IsModified,ModifiedDate,BodyContent,ShortContent,UrlSlug,CatagoryID,UserID,TagString")] PostModel postModel)
        {
            List<TagModel> postTags = new List<TagModel>();

            //Collecting New Tags in TagModel List
            string[] tags = postModel.tag.Split(',');
            for (int i = 0; i < tags.Length; i++)
            {
                TagModel Tag;
                var Tags = db.TagModels.ToList();
                if (Tags.Exists(t => t.Name == tags[i].Trim()))
                {
                    Tag = Tags.Find(t => t.Name == tags[i].Trim());
                }
                else
                {
                    Tag = new TagModel() { Name = tags[i].Trim() };
                }
                postTags.Add(Tag);
            }

            //Getting Existing Post From Database
            var existingPost = db.PostModels.Include(f => f.Tags)
                .Where(p => p.ID == postModel.ID).FirstOrDefault<PostModel>();
            //Checking Deleted Tags from Post
            var deletedTags = existingPost.Tags.Except(postTags).ToList<TagModel>();
            //Checking New Added Tags in Post
            var addedTags = postTags.Except(existingPost.Tags).ToList<TagModel>();
            //Deleting Tags from existing Post
            deletedTags.ForEach(c => existingPost.Tags.Remove(c));
            //Adding Tags in existing Post
            foreach (TagModel item in addedTags)
            {
                existingPost.Tags.Add(item);
            }

            if (ModelState.IsValid)
            {
                //Updating existing Post in database
                db.Entry(existingPost).State = EntityState.Modified;
                //Setting New Post in Existing Post
                existingPost.BodyContent = postModel.BodyContent;
                existingPost.CatagoryID = postModel.CatagoryID;
                existingPost.IsModified = true;
                existingPost.ModifiedDate = DateTime.Now;
                existingPost.TagString = postModel.TagString;
                existingPost.Title = postModel.Title;
                existingPost.ShortContent = postModel.ShortContent;

                //db.Entry(postModel).State = EntityState.Unchanged;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatagoryID = new SelectList(db.CatagoryModels, "ID", "Catagory", postModel.CatagoryID);
            return View(postModel);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostModel postModel = db.PostModels.Find(id);
            if (postModel == null)
            {
                return HttpNotFound();
            }
            return View(postModel);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostModel postModel = db.PostModels.Find(id);
            db.PostModels.Remove(postModel);
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
