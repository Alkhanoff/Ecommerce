using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

namespace Ecommerce.Controllers
{
    public class ProductsController : Controller
    {

        private ecommerceEntities db = new ecommerceEntities();
        // GET: Products
        public ActionResult Index()
        {
     
            return View(db.Products.ToList().OrderByDescending(k => k.id));
           
        }

        public ActionResult Create()
        {
            ViewBag.Genders = db.Genders.ToList();
            ViewBag.Sizes = db.Sizes.ToList();
            ViewBag.Colors = db.Colors.Where(f=>f.status == true).ToList();
            ViewBag.Brands = db.Brands.Where(i=>i.status == true).ToList();
            ViewBag.Cats = db.SubCategories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Createnew(Product product , int[] colorId , int[] sizeId , IEnumerable<HttpPostedFileBase> image)
        {
            
                product.date = DateTime.Now;
                product.status = true;
                db.Products.Add(product);
                db.SaveChanges();

            
                for (int i = 0; i < colorId.Count(); i++)
                {
                    ProductColor productColor = new ProductColor();
                    productColor.colorId = colorId[i];
                    productColor.productId = product.id;
                    db.ProductColors.Add(productColor);
                    db.SaveChanges();

                }



                for (int z = 0; z < sizeId.Count(); z++)
                {
                    ProductSize productSize = new ProductSize();
                    productSize.sizeId =sizeId[z];
                    productSize.productId = product.id;
                    db.ProductSizes.Add(productSize);
                    db.SaveChanges();

                }

            var myAccount = new Account { ApiKey = "826331127339442", ApiSecret = "8SQi6Vti80ZwES3LeeBDFnSqBWQ", Cloud = "dycqowxvx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);

            foreach(var img in image)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(img.FileName, img.InputStream)
                };
                var uploadResult = _cloudinary.Upload(uploadParams);
                Image imagenew = new Image();
                imagenew.url = uploadResult.SecureUri.AbsoluteUri;
                imagenew.productId = product.id;
                db.Images.Add(imagenew);
                db.SaveChanges();

            }
            
           


            return RedirectToAction("Index");
        }

        [HttpGet]
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Genders = db.Genders.ToList();
            ViewBag.Sizes = db.Sizes.ToList();
            ViewBag.Colors = db.Colors.Where(f => f.status == true).ToList();
            ViewBag.Brands = db.Brands.Where(i => i.status == true).ToList();
            ViewBag.Cats = db.SubCategories.ToList();
            ProductEdit items = new ProductEdit()
            {
               Product = db.Products.FirstOrDefault(k=>k.id == id),
               Images = db.Images.Where(i=>i.productId == id).ToList()
            };
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }





    }
}