using DataLayer;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Eshop.Controllers
{
    public class HomeController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        public ActionResult Index()
        {
            var products = db.Products.OrderByDescending(p => p.ProductCreateDate).Take(12);
            return View(products);
        }

        /*
         *
         * برای نمایش اسلایدر توی صفحه اصلی
         * حتما باید بیشتر از یک اسلاید داشته باشیم
         *
         * اگه یکی اضافه کردی و دیدی نمایش داده نمیشه تعجب نکن
         * کد درسته, سیستم نه
         *
         *
         */

        public ActionResult Slider()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            // اسلاید هایی که تاریخ شروعشون کمتر از تاریخ امروز باشه و تاریخ پایانشون بیشتر از تاریخ امروز
            var slider = db.Slider.Where(s => s.IsActive && s.StartDate <= dt && s.EndDate >= dt).ToList();
            return PartialView(slider);
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}