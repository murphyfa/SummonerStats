using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SummonerStats.Models;

namespace SummonerStats.Controllers
{
    public class MatchHistoryController : Controller
    {
        private MatchHistoryDBContext db = new MatchHistoryDBContext();

        // GET: MatchHistory
        public ActionResult Index()
        {
            return View(db.MatchHistory.ToList());
        }

        // GET: MatchHistory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchHistoryModel matchHistoryModel = db.MatchHistory.Find(id);
            if (matchHistoryModel == null)
            {
                return HttpNotFound();
            }
            return View(matchHistoryModel);
        }

        // GET: MatchHistory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MatchHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,timestamp,champion,region,queue,season,matchId,role,platformId,lane")] MatchHistoryModel matchHistoryModel)
        {
            if (ModelState.IsValid)
            {
                db.MatchHistory.Add(matchHistoryModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(matchHistoryModel);
        }

        // GET: MatchHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchHistoryModel matchHistoryModel = db.MatchHistory.Find(id);
            if (matchHistoryModel == null)
            {
                return HttpNotFound();
            }
            return View(matchHistoryModel);
        }

        // POST: MatchHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,timestamp,champion,region,queue,season,matchId,role,platformId,lane")] MatchHistoryModel matchHistoryModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchHistoryModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(matchHistoryModel);
        }

        // GET: MatchHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchHistoryModel matchHistoryModel = db.MatchHistory.Find(id);
            if (matchHistoryModel == null)
            {
                return HttpNotFound();
            }
            return View(matchHistoryModel);
        }

        // POST: MatchHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MatchHistoryModel matchHistoryModel = db.MatchHistory.Find(id);
            db.MatchHistory.Remove(matchHistoryModel);
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
