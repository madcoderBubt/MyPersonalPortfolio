using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers
{
    [Authorize]
    public class CatagoryController : Controller
    {
        private BlogContext db = new BlogContext();

        // GET: CatagoryModels
        public ActionResult Index()
        {
            return View(db.CatagoryModels.ToList());
        }

        // GET: CatagoryModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatagoryModel catagoryModel = db.CatagoryModels.Find(id);
            if (catagoryModel == null)
            {
                return HttpNotFound();
            }
            return View(catagoryModel);
        }

        // GET: CatagoryModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CatagoryModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Catagory")] CatagoryModel catagoryModel)
        {
            if (ModelState.IsValid)
            {
                db.CatagoryModels.Add(catagoryModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catagoryModel);
        }

        // GET: CatagoryModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatagoryModel catagoryModel = db.CatagoryModels.Find(id);
            if (catagoryModel == null)
            {
                return HttpNotFound();
            }
            return View(catagoryModel);
        }

        // POST: CatagoryModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Catagory")] CatagoryModel catagoryModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catagoryModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catagoryModel);
        }

        // GET: CatagoryModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatagoryModel catagoryModel = db.CatagoryModels.Find(id);
            if (catagoryModel == null)
            {
                return HttpNotFound();
            }
            return View(catagoryModel);
        }

        // POST: CatagoryModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CatagoryModel catagoryModel = db.CatagoryModels.Find(id);
            db.CatagoryModels.Remove(catagoryModel);
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
