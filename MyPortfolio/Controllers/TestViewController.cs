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
    public class TestViewController : Controller
    {
        private BlogContext db = new BlogContext();
        private ApplicationDbContext userdb = new ApplicationDbContext();


        // GET: TestView/Details/5
        public ActionResult Details()
        {

            var postModel = db.PostModels.Include(p => p.Catagory);
            
            return View(postModel.ToList());
        }

        //GET: Posts/Create
        public ActionResult Index()
        {
            ViewBag.CatagoryID = new SelectList(db.CatagoryModels, "ID", "Catagory");
            ViewBag.Tags = new MultiSelectList(db.TagModels, "ID", "Name", db.PostModels);

            return View();
        }

        //POST: Posts/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ID,Title,PublishedDate,IsModified,ModifiedDate,BodyContent,ShortContent,UrlSlug,CatagoryID,UserID")] PostModel postModel)
        {
            postModel.PublishedDate = DateTime.Now;
            postModel.IsModified = false;
            postModel.ModifiedDate = DateTime.Today;
            postModel.UrlSlug = postModel.Title.Replace(' ', '_');

            if (User.Identity.IsAuthenticated)
            {
                postModel.UserID = User.Identity.GetUserId();
            }

            //INSERT INTO[dbo].[TagModelPostModels] ([TagModel_ID], [PostModel_ID]) VALUES(1, 4)

            if (ModelState.IsValid)
            {
                db.PostModels.Add(postModel);
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            

            ViewBag.CatagoryID = new SelectList(db.CatagoryModels, "ID", "Catagory", postModel.CatagoryID);
            //ViewBag.Tags = new MultiSelectList(db.TagModels, "ID", "Name", db.PostModels);
            
            return View(postModel);
        }
        
    }
}
