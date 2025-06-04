using DataLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eshop.Controllers
{
    public class SearchController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        public ActionResult Index(string q)
        {
            List<Products> products = new List<Products>();

            products.AddRange(db.ProductTags.Where(p => p.Tag == q).Select(p => p.Products));
            products.AddRange(db.Products.Where(p => p.ProductTitle.Contains(q) || p.ProductDescription.Contains(q)));
            ViewBag.q = q;
            return View(products.Distinct());
        }
    }
}