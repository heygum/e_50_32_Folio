﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FIT5032_Week06.Models;
//using FIT5032_Week06.Models.GoodFormViewModel;

namespace FIT5032_Week06.Controllers
{
    public class GoodController : Controller
    {
        // GET: Good
        public ActionResult Index()
        {
            return View();
        }

        // GET: Good/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Good/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Good/Create
        [HttpPost]
        public ActionResult Create(GoodFormViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                String GoodName = model.GoodName;
                int GoodNumber = model.GoodNumber;
                ViewBag.GoodDetail = GoodName + ":" + GoodNumber;
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Good/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Good/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Good/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Good/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
