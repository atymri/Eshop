using DataLayer;
using KooyWebApp_MVC.Classes;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Eshop.Areas.Admin.Controllers
{
    public class SlidersController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        public ActionResult Index()
        {
            return View(db.Slider.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider slider, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp == null || !imgUp.IsImage())
                {
                    ModelState.AddModelError("ImageName", "لطفا یک تصویر برای اسلایدر انتخاب کنید");
                    return View(slider);
                }

                // تبدیل تاریخ شمسی به تاریخ میلادی
                PersianCalendar persianCalendar = new PersianCalendar();
                slider.StartDate = persianCalendar.ToDateTime(slider.StartDate.Year, slider.StartDate.Month, slider.StartDate.Day, 0, 0, 0, 0);
                slider.EndDate = persianCalendar.ToDateTime(slider.EndDate.Year, slider.EndDate.Month, slider.EndDate.Day, 0, 0, 0, 0);
                slider.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imgUp.FileName);
                imgUp.SaveAs(Server.MapPath("/Images/Sliders/" + slider.ImageName));

                db.Slider.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider slider, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {

                if (imgUp != null)
                {
                    System.IO.File.Delete(Server.MapPath("/Images/Sliders/" + slider.ImageName));
                    slider.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/Sliders/" + slider.ImageName));
                }


                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Slider.Find(id);

            System.IO.File.Delete(Server.MapPath("/Images/Sliders/" + slider.ImageName));

            db.Slider.Remove(slider);
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
