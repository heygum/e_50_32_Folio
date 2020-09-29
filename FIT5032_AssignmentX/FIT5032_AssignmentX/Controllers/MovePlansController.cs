﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_AssignmentX.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace FIT5032_AssignmentX.Controllers
{
    [Authorize]
    public class MovePlansController : Controller
    {
        private MovesContainer db = new MovesContainer();

        // GET: MovePlans
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.RoundSortParm = sortOrder == "Round" ? "round_desc" : "Round";
            ViewBag.TImeSortParm = sortOrder == "Time" ? "time_desc" : "Time";
            ViewBag.UserIDSortParm = sortOrder == "UserID" ? "userid_desc" : "UserID";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var UserID = User.Identity.GetUserId();
            var moves = from s in db.MovePlans where s.UserID == UserID select s;
            if (User.IsInRole("admin"))
            { 
                moves = from s in db.MovePlans select s; 
            }
            
            if (!String.IsNullOrEmpty(searchString))
            {
                moves = moves.Where(s => s.MoveName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    moves = moves.OrderByDescending(s => s.MoveName);
                    break;
                case "Date":
                    moves = moves.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    moves = moves.OrderByDescending(s => s.Date);
                    break;
                case "Round":
                    moves = moves.OrderBy(s => s.Round);
                    break;
                case "round_desc":
                    moves = moves.OrderByDescending(s => s.Round);
                    break;
                case "Time":
                    moves = moves.OrderBy(s => s.Time);
                    break;
                case "time_desc":
                    moves = moves.OrderByDescending(s => s.Time);
                    break;
                case "UserID":
                    moves = moves.OrderBy(s => s.UserID);
                    break;
                case "userid_desc":
                    moves = moves.OrderByDescending(s => s.UserID);
                    break;
                default:
                    moves = moves.OrderBy(s => s.MoveName);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(moves.ToPagedList(pageNumber, pageSize));
        }

        // GET: MovePlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovePlans movePlans = db.MovePlans.Find(id);
            if (User.Identity.GetUserId() != movePlans.UserID && !User.IsInRole("admin"))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Disable");
            }
            if (movePlans == null)
            {
                return HttpNotFound();
            }
            return View(movePlans);
        }

        // GET: MovePlans/Create
        public ActionResult Create()
        {
            var lstskill = (from a in db.Movements orderby a.MovementName select a).ToList();
            ViewBag.move = ToSelectList(lstskill);
            //ViewBag.move = new SelectList(db.Movements,"MovementName");
            return View();
        }

        public SelectList ToSelectList(List<Movements> lstskill)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Movements item in lstskill)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.MovementName,
                    Value = item.MovementName
                });
            }

            return new SelectList(list, "Value", "Text");
        }
    

    // POST: MovePlans/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlanId,MoveName,Time,Round,Date,UserID")] MovePlans movePlans)
        {
            if (ModelState.IsValid)
            {
                movePlans.UserID = User.Identity.GetUserId();
                db.MovePlans.Add(movePlans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movePlans);
        }

        // GET: MovePlans/Edit/5
        public ActionResult Edit(int? id)
        {
            var lstskill = (from a in db.Movements orderby a.MovementName select a).ToList();
            ViewBag.move = ToSelectList(lstskill);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovePlans movePlans = db.MovePlans.Find(id);
            if (User.Identity.GetUserId() != movePlans.UserID && !User.IsInRole("admin"))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Disable");
            }
            if (movePlans == null)
            {
                return HttpNotFound();
            }
            return View(movePlans);
        }

        // POST: MovePlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlanId,MoveName,Time,Round,Date,UserID")] MovePlans movePlans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movePlans).State = EntityState.Modified;
                movePlans.UserID = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movePlans);
        }

        // GET: MovePlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovePlans movePlans = db.MovePlans.Find(id);
            if (User.Identity.GetUserId() != movePlans.UserID && !User.IsInRole("admin"))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Disable");
            }
            if (movePlans == null)
            {
                return HttpNotFound();
            }
            return View(movePlans);
        }

        // POST: MovePlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovePlans movePlans = db.MovePlans.Find(id);
            if (User.Identity.GetUserId() != movePlans.UserID && !User.IsInRole("admin"))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Disable");
            }
            db.MovePlans.Remove(movePlans);
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