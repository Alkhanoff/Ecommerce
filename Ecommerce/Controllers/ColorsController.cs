﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    public class ColorsController : Controller
    {
        private ecommerceEntities db = new ecommerceEntities();

        // GET: Colors
        public ActionResult Index()
        {
            return View(db.Colors.ToList());
        }

      

        // GET: Colors/Create
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
    
        public ActionResult Create([Bind(Include = "id,name,hash,status")] Color color)
        {
            if (ModelState.IsValid)
            {
                color.status = true;
                db.Colors.Add(color);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(color);
        }

        // GET: Colors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Color color = db.Colors.Find(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        // POST: Colors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        public ActionResult Edit([Bind(Include = "id,name,hash,status")] Color color)
        {
            if (ModelState.IsValid)
            {
             
                db.Entry(color).State = EntityState.Modified;
                db.Entry(color).Property(t => t.status).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(color);
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
