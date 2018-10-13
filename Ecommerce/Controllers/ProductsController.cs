using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using System.Data.Entity;

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
               Images = db.Images.Where(i=>i.productId == id).ToList(),
               ProductColors = db.ProductColors.Where(p=>p.productId == id).ToList(),
               ProductSizes = db.ProductSizes.Where(s=>s.productId == id).ToList()
            };
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        public ActionResult Deleteimage(int url ,int id , string publicId)
        {
            var myAccount = new Account { ApiKey = "826331127339442", ApiSecret = "8SQi6Vti80ZwES3LeeBDFnSqBWQ", Cloud = "dycqowxvx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);

            //delete
            _cloudinary.DeleteResources(publicId);
            Image del = db.Images.FirstOrDefault(i => i.id == id);
            db.Images.Remove(del);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = url });

        }

        public ActionResult Update(Product product, int[] colorId, int[] sizeId, HttpPostedFileBase[] image)
        {

            db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
            int a = 0;
           for(int i=0; i<image.Count(); i++)
            {
                a++;
            }
            if (a > 1)
            {
                var myAccount = new Account { ApiKey = "826331127339442", ApiSecret = "8SQi6Vti80ZwES3LeeBDFnSqBWQ", Cloud = "dycqowxvx" };
                Cloudinary _cloudinary = new Cloudinary(myAccount);

                foreach (var img in image)
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
            }

                List<ProductColor> colors = db.ProductColors.Where(w => w.productId == product.id).ToList();
                foreach(var c in colors)
                {
                    db.ProductColors.Remove(c);
                    db.SaveChanges();
                }
                for (int i = 0; i < colorId.Count(); i++)
                {
                    ProductColor productColor = new ProductColor();
                    productColor.colorId = colorId[i];
                    productColor.productId = product.id;
                    db.ProductColors.Add(productColor);
                    db.SaveChanges();

                }


                List<ProductSize> sizes = db.ProductSizes.Where(w => w.productId == product.id).ToList();
                foreach (var s in sizes)
                {
                    db.ProductSizes.Remove(s);
                    db.SaveChanges();
                }

                for (int z = 0; z < sizeId.Count(); z++)
                {
                    ProductSize productSize = new ProductSize();
                    productSize.sizeId = sizeId[z];
                    productSize.productId = product.id;
                    db.ProductSizes.Add(productSize);
                    db.SaveChanges();

                }





            return RedirectToAction("Index"); 

        }





    }
}