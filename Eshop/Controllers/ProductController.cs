using DataLayer;
using Eshop.ViewModels;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Eshop.Controllers
{
    public class ProductController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        #region ProductGroupAndDetails

        public PartialViewResult ProductGroups()
        {
            var data = db.ProductGroups;
            ViewBag.Products = db.Products.ToList();
            return PartialView(data);
        }

        public ActionResult ProductDetails(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            ViewBag.Features = db.ProductFeatures.Where(f => f.ProductID == id).DistinctBy(f => f.FeatureID).Select(f => new SingleProductViewModel()
            {
                FeatureTitle = f.Features.FeatureName,
                Values = db.ProductFeatures.Where(fe => fe.FeatureID == f.FeatureID).Select(fe => fe.Value).ToList()
            }).ToList();

            return View(product);

        }
        #endregion

        #region Comments

        public PartialViewResult ShowComment(int id)
        {
            var comments = db.ProductComments.Where(c => c.ProductID == id).OrderByDescending(c => c.CreateDate);
            return PartialView(comments);
        }

        public PartialViewResult AddComment(int id)
        {

            return PartialView(new ProductComments()
            {
                ProductID = id
            });
        }

        [HttpPost]
        public ActionResult AddComment(ProductComments comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreateDate = DateTime.Now;
                db.ProductComments.Add(comment);
                db.SaveChanges();
                return PartialView("ShowComment", db.ProductComments.Where(c => c.ProductID == comment.ProductID).OrderByDescending(c => c.CreateDate));
            }

            return PartialView(comment);
        }

        #endregion

        #region Archive&Filter

        /*
            t  : title
            mp : min price
            xp : max price
            g  : group
         
         */

        [Route("Archive")]
        public ActionResult Archive(int pageNum = 1, List<int> g = null, string t = "", int mp = 0, int xp = 0)
        {
            ViewBag.PageNumebr = pageNum;
            ViewBag.Groups = db.ProductGroups.ToList();
            ViewBag.ProductTitle = t;
            ViewBag.MinPrice = mp;
            ViewBag.MaxPrice = xp;
            ViewBag.SelectedGroups = g;

            // if you assign the list like this, you can use AddRange!
            List<Products> products = new List<Products>();
            if (g != null && g.Any())
            {
                foreach (var item in g)
                {
                    products.AddRange(db.SelectedProductGruop.Where(p => p.GroupID == item).Select(gr => gr.Products).ToList());
                }
            }
            else
            {
                products.AddRange(db.Products.ToList());
            }

            if (!String.IsNullOrEmpty(t))
            {
                products = db.Products.Where(p => p.ProductTitle.Contains(t)).ToList();
            }

            if (mp > 0)
            {
                products = db.Products.Where(p => p.ProductPrice >= mp).ToList();
            }

            if (xp > 0)
            {
                products = db.Products.Where(p => p.ProductPrice <= xp && p.ProductPrice >= mp).ToList();
            }

            // Pagination, save this somewhere, this is important.
            var take = 9; // How many product you want in a single page?
            var skip = (pageNum - 1) * take; // how many pages do you want?
            ViewBag.PageCount = products.Count / take; // like if we have 18 products, we have 2 pages.


            return View(products.OrderByDescending(p => p.ProductCreateDate).Skip(skip).Take(take).ToList());
        }

        #endregion

    }
}