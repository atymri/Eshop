using DataLayer;
using InsertShowImage;
using KooyWebApp_MVC.Classes;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        #region Product

        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        public ActionResult Create()
        {
            ViewBag.Groups = db.ProductGroups.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products, HttpPostedFileBase ProductImage, List<int> ProductGroups, string keywords)
        {
            // If prouct has no group and the image, is not an actual image , we wouldn't let them in.
            if (ProductGroups == null)
            {
                ViewBag.Groups = db.ProductGroups.ToList();
                ViewBag.Error = true;
                return View(products);
            }

            if (ModelState.IsValid)
            {
                // This part is important saving the image
                products.ProductImage = "images.png";
                if (ProductImage != null && ProductImage.IsImage())
                {
                    products.ProductImage = Guid.NewGuid().ToString() + Path.GetExtension(ProductImage.FileName);
                    ProductImage.SaveAs(Server.MapPath("/Images/ProductImages/" + products.ProductImage));

                    //Making a thumbnail version for the index and other pages
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + products.ProductImage),
                        Server.MapPath("/Images/ProductImages/Thumbnails/" + products.ProductImage));
                }
                // END saving the

                products.ProductCreateDate = DateTime.Now;
                db.Products.Add(products);

                foreach (var groupId in ProductGroups)
                {
                    db.SelectedProductGruop.Add(new SelectedProductGruop()
                    {
                        GroupID = groupId,
                        ProductID = products.ProductID
                    });
                }

                if (!String.IsNullOrEmpty(keywords))
                {
                    foreach (var keyword in keywords.Split(','))
                    {
                        db.ProductTags.Add(new ProductTags()
                        {
                            ProductID = products.ProductID,
                            Tag = keyword.Trim()
                        });
                    }
                }


                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Groups = db.ProductGroups.ToList();
            return View(products);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }

            ViewBag.Groups = db.ProductGroups.ToList();
            ViewBag.Tags = String.Join(",", db.ProductTags.Where(t => t.ProductID == products.ProductID).Select(t => t.Tag).ToList());
            ViewBag.SelectedGroups = db.SelectedProductGruop.Where(g => g.ProductID == products.ProductID).ToList();
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products, HttpPostedFileBase ProductImage, List<int> ProductGroups, string keywords)
        {
            if (ProductGroups == null)
            {
                ViewBag.Error = true;
                ViewBag.Groups = db.ProductGroups.ToList();
                ViewBag.Tags = keywords;
                ViewBag.SelectedGroups = db.SelectedProductGruop.Where(g => g.ProductID == products.ProductID).ToList();
                return View(products);
            }

            if (ModelState.IsValid)
            {
                if (ProductImage != null && ProductImage.IsImage())
                {
                    if (products.ProductImage != "images.png")
                    {
                        System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/{products.ProductImage}"));
                        System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/Thumbnails/{products.ProductImage}"));
                    }

                    products.ProductImage = Guid.NewGuid().ToString() + Path.GetExtension(ProductImage.FileName);
                    ProductImage.SaveAs(Server.MapPath($"/Images/ProductImages/{products.ProductImage}"));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath($"/Images/ProductImages/{products.ProductImage}"),
                        Server.MapPath($"/Images/ProductImages/Thumbnails/{products.ProductImage}"));
                }

                db.ProductTags.Where(p => p.ProductID == products.ProductID).ToList().ForEach(t => db.ProductTags.Remove(t));

                if (!String.IsNullOrEmpty(keywords))
                {
                    foreach (var item in keywords.Split(','))
                    {
                        db.ProductTags.Add(new ProductTags()
                        {
                            ProductID = products.ProductID,
                            Tag = item.Trim()
                        });
                    }
                }

                db.SelectedProductGruop.Where(g => g.ProductID == products.ProductID).ToList().ForEach(g => db.SelectedProductGruop.Remove(g));

                if (ProductGroups != null && ProductGroups.Any())
                {
                    foreach (var group in ProductGroups)
                    {
                        db.SelectedProductGruop.Add(new SelectedProductGruop()
                        {
                            GroupID = group,
                            ProductID = products.ProductID
                        });
                    }
                }

                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Error = true;
            ViewBag.Groups = db.ProductGroups.ToList();
            ViewBag.Tags = keywords;
            ViewBag.SelectedGroups = db.SelectedProductGruop.Where(g => g.ProductID == products.ProductID).ToList();

            return View(products);
        }

        // TODO: Fix this shit as soon as possible.
        public JsonResult Delete(int? id)
        {

            if (id == null)
            {
                return Json(new { success = false, error = "آیدی محصول معتبر نیست" });
            }

            Products products = db.Products.Find(id);

            if (products == null)
            {
                return Json(new { success = false, error = "محصول مورد نظر وجود ندارد" });
            }


            if (products.ProductImage.Any() && products.ProductImage != "images.png")
            {
                System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/{products.ProductImage}"));
                System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/Thumbnails/{products.ProductImage}"));
            }

            if (db.ProductGalleries.Any(p => p.ProductID == products.ProductID))
            {
                foreach (var image in db.ProductGalleries.Where(pg => pg.ProductID == products.ProductID))
                {
                    System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/{image.Image}"));
                    System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/Thumbnails/{image.Image}"));
                    db.ProductGalleries.Remove(image);
                }
            }

            if (db.ProductTags.Any(t => t.ProductID == products.ProductID))
            {
                db.ProductTags.Where(t => t.ProductID == products.ProductID)
                    .ForEach(t => db.ProductTags.Remove(t));
            }

            if (db.ProductFeatures.Any(f => f.ProductID == products.ProductID))
            {
                db.ProductFeatures.Where(f => f.ProductID == products.ProductID)
                    .ForEach(f => db.ProductFeatures.Remove(f));
            }

            if (db.SelectedProductGruop.Any(p => p.ProductID == products.ProductID))
            {
                db.SelectedProductGruop.Where(pg => pg.ProductID == products.ProductID)
                    .ForEach(pg => db.SelectedProductGruop.Remove(pg));
            }

            db.Products.Remove(products);
            db.SaveChanges();
            return Json(new { success = true, message = "محصول مورد نظر با موفقیت حذف شد" });
        }


        #endregion

        #region Gallery

        public ActionResult ProductGallery(int id)
        {
            ViewBag.Gallerie = db.ProductGalleries.Where(p => p.ProductID == id).ToList();
            return View(new ProductGalleries() { ProductID = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductGallery(ProductGalleries galleries, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.IsImage())
                {
                    galleries.Image = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    Image.SaveAs(Server.MapPath($"/Images/ProductImages/{galleries.Image}"));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath($"/Images/ProductImages/{galleries.Image}"),
                        Server.MapPath($"/Images/ProductImages/Thumbnails/{galleries.Image}"));

                    db.ProductGalleries.Add(galleries);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ProductGallery", new { id = galleries.ProductID });
        }

        public ActionResult DeleteGallery(int galleryId)
        {
            if (galleryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var image = db.ProductGalleries.SingleOrDefault(g => g.GalleryID == galleryId);
            if (image == null)
            {
                return HttpNotFound();
            }

            var productID = image.ProductID;
            System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/{image.Image}"));
            System.IO.File.Delete(Server.MapPath($"/Images/ProductImages/Thumbnails/{image.Image}"));
            db.ProductGalleries.Remove(image);
            db.SaveChanges();

            return RedirectToAction("ProductGallery", new { id = productID });
        }

        #endregion

        #region Features
        public ActionResult AddFeatureForProduct(int ProductID)
        {
            var group = db.SelectedProductGruop.Where(p => p.ProductID == ProductID).Select(p => p.GroupID).ToList();

            var features = db.Features.Where(g => group.Contains(g.GroupID.Value)).DistinctBy(g => g.FeatureName);

            ViewBag.FeatureID = new SelectList(features, "FeatureID", "FeatureName");
            ViewBag.ProductName = db.Products.Find(ProductID).ProductTitle;
            ViewBag.ProductFeatures = db.ProductFeatures.Where(p => p.ProductID == ProductID).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeatureForProduct(ProductFeatures features)
        {
            if (ModelState.IsValid)
            {
                db.ProductFeatures.Add(features);
                db.SaveChanges();
            }

            return RedirectToAction("AddFeatureForProduct", new { ProductID = features.ProductID });
        }

        public void DeleteFeature(int f)
        {
            var feature = db.ProductFeatures.SingleOrDefault(p => p.PFID == f);
            //if (feature == null)
            //{
            //    // I like this one!
            //    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            //}

            db.ProductFeatures.Remove(feature);
            db.SaveChanges();
            //return RedirectToAction("AddFeatureForProduct", new { ProductID = feature.ProductID });

        }

        #endregion


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
