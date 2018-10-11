using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Ecommerce.Controllers
{
    public class BrandsController : Controller
    {
        private ecommerceEntities db = new ecommerceEntities();

        // GET: Brands
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        // GET: Brands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(Brand brand , HttpPostedFileBase image)
        {
            var myAccount = new Account { ApiKey = "826331127339442", ApiSecret = "8SQi6Vti80ZwES3LeeBDFnSqBWQ", Cloud = "dycqowxvx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.FileName , image.InputStream)
            };
        var uploadResult = _cloudinary.Upload(uploadParams);
            brand.image = uploadResult.SecureUri.AbsoluteUri;
            brand.status = true;
            if (ModelState.IsValid)
            {

                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        public ActionResult Edit( HttpPostedFileBase image , Brand brand)
        {

           
            db.Entry(brand).State = EntityState.Modified;
            db.Entry(brand).Property(t => t.status).IsModified = false;
            if (image == null)
                    db.Entry(brand).Property(m => m.image).IsModified = false;
                   
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
