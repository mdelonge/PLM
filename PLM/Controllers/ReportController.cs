﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLM.Models;
using PLM;
using Microsoft.AspNet.Identity;
using PLM.CutomAttributes;
namespace PLM.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Report/
        public ActionResult YourReports()
        {
            var name = User.Identity.GetUserName();
            ApplicationUser currentUser = (ApplicationUser)db.Users.Single(x => x.UserName == name);//get logged in user
            var Reports = from u in db.Reports
                          where u.userID == currentUser.Id
                          select u;//get all reports for the user that is logged in
            return View(Reports.ToList());
        }
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Index()
        {
            //Gets all reports
            var Reports = from u in db.Reports
                          select u;
            return View(Reports.ToList());
        }
        public ActionResult YourModulesReported()
        {
            //Gets all of your pending reports
            var name = User.Identity.GetUserName();
            var reports = from r in db.Reports
                          join m in db.Modules on r.moduleID equals m.ModuleID
                          where m.User.UserName == name
                          select r;
            return View(reports.ToList());
        }
        // GET: /Report/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ApplicationUser Reporter = (ApplicationUser)db.Users.Single(x => x.Id == report.userID);
            ViewBag.ReporterUserName = Reporter.UserName;
            return View(report);
        }
        // GET: /Report/Create
        public ActionResult Create(int? id)
        {
            if (Request.IsAuthenticated)//If the user isn't logged in make "Guest User" the id of the reporter
            {
                Report placeholder = new Report();
                ViewBag.UserID = User.Identity.Name;
                var name = User.Identity.GetUserName();
                ApplicationUser currentUser = (ApplicationUser)db.Users.Single(x => x.UserName == name);
                placeholder.userID = currentUser.Id;
                placeholder.moduleID = (int)id;

                return View(placeholder);
            }
            else
            {
                Report placeholder = new Report();
                ViewBag.UserID = "guest user";
                placeholder.userID = "guest user"; 
                placeholder.moduleID = (int)id;
                return View(placeholder);
            }
        }
        // POST: /Report/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,moduleID,description,userID,category")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("YourReports");
            }
            return View(report);
        }
        // GET: /Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }
        // POST: /Report/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,moduleID,description,userID,category")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("YourReports");
                    }
            }
            return View(report);
        }
        // GET: /Report/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }
        // POST: /Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
            db.SaveChanges();
            return RedirectToAction("YourReports");
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