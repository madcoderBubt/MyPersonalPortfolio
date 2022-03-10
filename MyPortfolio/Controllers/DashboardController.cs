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
    public class DashboardController : Controller
    {
        private BlogContext db_blog = new BlogContext();
        private ApplicationDbContext db_user = new ApplicationDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        #region Blog Posts
        //GET: Post Lists
        public ActionResult Posts()
        {
            var postModels = db_blog.PostModels.Include(p => p.Catagory).ToList();
            return View(postModels);
        }

        // GET: Posts/Create
        public ActionResult AddNew()
        {
            ViewBag.CatagoryID = new SelectList(db_blog.CatagoryModels, "ID", "Catagory");
            ViewBag.TagsID = new MultiSelectList(db_blog.TagModels, "ID", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew([Bind(Include = "ID,Title,PublishedDate,IsModified,ModifiedDate,BodyContent,ShortContent,UrlSlug,CatagoryID,UserID,TagString")] PostModel postModel)
        {
            postModel.PublishedDate = DateTime.Now;
            postModel.IsModified = false;
            postModel.ModifiedDate = DateTime.Today;
            postModel.UrlSlug = postModel.Title == null ? "" : postModel.Title.Replace(' ', '_');

            if (User.Identity.IsAuthenticated)
            {
                postModel.UserID = User.Identity.GetUserId();
            }

            string Tag = postModel.tag;
            string[] tags = Tag == null ? null : Tag.Split(',');

            var Tags = db_blog.TagModels.ToList();
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
                db_blog.PostModels.Add(postModel);
                db_blog.SaveChanges();
                return RedirectToAction("Posts");
            }

            ViewBag.CatagoryID = new SelectList(db_blog.CatagoryModels, "ID", "Catagory", postModel.CatagoryID);
            ViewBag.TagsID = new MultiSelectList(db_blog.TagModels, "ID", "Name",postModel.Tags);
            return View(postModel);
        }

        // GET: Posts/Edit/5
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PostModel postModel = db_blog.PostModels.Find(id);
            if (postModel == null)
            {
                return HttpNotFound();
            }

            //put code here to check is user have permission to change 
            //if (postModel.UserID != User.Identity.GetUserId())
            //{

            //}

            ViewBag.CatagoryID = new SelectList(db_blog.CatagoryModels, "ID", "Catagory", postModel.CatagoryID);
            return View(postModel);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "ID,Title,PublishedDate,IsModified,ModifiedDate,BodyContent,ShortContent,UrlSlug,CatagoryID,UserID,TagString")] PostModel postModel)
        {
            List<TagModel> postTags = new List<TagModel>();

            //Collecting New Tags in TagModel List
            string[] tags = postModel.tag.Split(',');
            for (int i = 0; i < tags.Length; i++)
            {
                TagModel Tag;
                var Tags = db_blog.TagModels.ToList();
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
            var existingPost = db_blog.PostModels.Include(f => f.Tags)
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
                db_blog.Entry(existingPost).State = EntityState.Modified;
                //Setting New Post in Existing Post
                existingPost.BodyContent = postModel.BodyContent;
                existingPost.CatagoryID = postModel.CatagoryID;
                existingPost.IsModified = true;
                existingPost.ModifiedDate = DateTime.Now;
                existingPost.TagString = postModel.TagString;
                existingPost.Title = postModel.Title;
                existingPost.ShortContent = postModel.ShortContent;

                //db.Entry(postModel).State = EntityState.Unchanged;
                db_blog.SaveChanges();
                return RedirectToAction("Posts");
            }

            ViewBag.CatagoryID = new SelectList(db_blog.CatagoryModels, "ID", "Catagory", postModel.CatagoryID);
            return View(postModel);
        }

        // POST: Posts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _DeletePost(int id)
        {
            PostModel postModel = db_blog.PostModels.Find(id);
            db_blog.PostModels.Remove(postModel);
            db_blog.SaveChanges();
            return RedirectToAction("Posts");
        }
        #endregion

        #region Blog Catagories
        //Get: Catagory Dashboard
        public ActionResult Catagories()
        {
            BlogCatagory viewModel = new BlogCatagory();
            viewModel.Catagories = db_blog.CatagoryModels.ToList();
            viewModel.Catagory = new CatagoryModel();

            return View(viewModel);
        }

        //Post: Dashboard/Catagories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Catagories([Bind(Include = "Catagory")] BlogCatagory blogCatagory)
        {
            if (ModelState.IsValid)
            {
                db_blog.CatagoryModels.Add(blogCatagory.Catagory);
                db_blog.SaveChanges();
                return RedirectToAction("Catagories");
            }

            return View(blogCatagory.Catagory);
        }

        // POST: CatagoryModels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteCatagory(int id)
        {
            CatagoryModel catagoryModel = db_blog.CatagoryModels.Find(id);
            db_blog.CatagoryModels.Remove(catagoryModel);
            db_blog.SaveChanges();
            return RedirectToAction("Catagories");
        }
        #endregion

        #region Blog Tags
        //GET: Dashboard/Tags
        //View Tags
        public ActionResult Tags()
        {
            BlogTags viewModel = new BlogTags();
            viewModel.Tags = db_blog.TagModels.ToList();

            return View(viewModel);
        }

        // POST: Dashboard/Tags
        // Adding Tags
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tags([Bind(Include = "Tag")] BlogTags blogTags)
        {
            if (ModelState.IsValid)
            {
                db_blog.TagModels.Add(blogTags.Tag);
                db_blog.SaveChanges();
                return RedirectToAction("Tags");
            }

            return View(blogTags.Tag);
        }

        // POST: Dashboard/_DeleteTag/5
        //[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteTag(int id)
        {
            TagModel tagModel = db_blog.TagModels.Find(id);
            db_blog.TagModels.Remove(tagModel);
            db_blog.SaveChanges();
            return RedirectToAction("Tags");
        }
        #endregion

        #region Blog Comments
        public ActionResult Comments()
        {
            var commentModel = db_blog.CommentModels.Include(q => q.Post).ToList();

            return View(commentModel);
        }

        // POST: Dashboard/_DeleteComment/5
        //[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteComment(int id)
        {
            CommentModel commentModel = db_blog.CommentModels.Find(id);
            db_blog.CommentModels.Remove(commentModel);
            db_blog.SaveChanges();
            return RedirectToAction("Tags");
        }
        #endregion

        #region Others

        #endregion
    }
}
