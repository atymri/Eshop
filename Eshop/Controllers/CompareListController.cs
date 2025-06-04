using DataLayer;
using Eshop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eshop.Controllers
{
    public class CompareListController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        public ActionResult Index()
        {
            List<CompareItemViewModel> list = new List<CompareItemViewModel>();
            List<Features> features = new List<Features>();
            List<ProductFeatures> productFeatures = new List<ProductFeatures>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItemViewModel>;
            }

            foreach (var product in list)
            {
                features.AddRange(db.ProductFeatures.Where(p => p.ProductID == product.ProductId).Select(p => p.Features).ToList());
                productFeatures.AddRange(db.ProductFeatures.Where(p => p.ProductID == product.ProductId).ToList());
            }

            ViewBag.Features = features.Distinct().ToList();
            ViewBag.ProductFeatures = productFeatures;

            return View(list);
        }

        #region Add-Delete
        public ActionResult AddToList(int id)
        {
            List<CompareItemViewModel> list = new List<CompareItemViewModel>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItemViewModel>;
            }

            if (!list.Any(l => l.ProductId == id))
            {
                var product = db.Products.Where(p => p.ProductID == id)
                    .Select(p => new
                    {
                        p.ProductImage,
                        p.ProductTitle,
                    }).Single();

                list.Add(new CompareItemViewModel()
                {
                    ProductId = id,
                    ProductTitle = product.ProductTitle,
                    ProductImageName = product.ProductImage
                });
            }

            Session["Compare"] = list;
            return PartialView("GetList", list);
        }

        public ActionResult GetList()
        {
            List<CompareItemViewModel> list = new List<CompareItemViewModel>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItemViewModel>;
            }

            return PartialView(list);
        }

        public ActionResult DeleteItem(int id)
        {
            List<CompareItemViewModel> list = new List<CompareItemViewModel>();

            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItemViewModel>;
                int index = list.FindIndex(i => i.ProductId == id);
                list.RemoveAt(index);
                Session["Compare"] = list;
            }

            return PartialView("GetList", list);
        }
        #endregion
    }
}