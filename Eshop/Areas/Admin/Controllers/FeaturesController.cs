using DataLayer;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeaturesController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        // GET: Admin/Features
        public ActionResult Index()
        {
            var features = db.Features.Include(f => f.ProductGroups);
            ViewBag.FeatureID = db.ProductGroups;
            return View(features.ToList());
        }


        public ActionResult ShowList()
        {
            //var data = db.Features.Include(f => f.ProductGroups).ToList();
            //var data = db.ProductGroups.Include(g => g.Features);

            // Now we are talking!
            var data = db.ProductGroups.Where(g => g.Features.Any());

            return PartialView(data);
        }


        // GET: Admin/Features/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Features features = db.Features.Find(id);
            if (features == null)
            {
                return HttpNotFound();
            }
            return View(features);
        }


        public ActionResult Create()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.Add(new SelectListItem() { Text = "همه", Value = "0" });
            foreach (var group in db.ProductGroups)
            {
                groups.Add(new SelectListItem() { Text = group.GroupTitle, Value = group.GroupID.ToString() });
            }
            ViewBag.GroupID = groups;

            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Features features)
        {
            if (ModelState.IsValid)
            {
                if (features.GroupID == 0)
                {
                    foreach (var item in db.ProductGroups)
                    {
                        Features f = new Features() { FeatureName = features.FeatureName, GroupID = item.GroupID };
                        db.Features.Add(f);
                    }

                    db.SaveChanges();
                    return RedirectToAction("ShowList");

                }
                db.Features.Add(features);
                db.SaveChanges();
                return RedirectToAction("ShowList");
            }

            List<SelectListItem> groups = new List<SelectListItem>();
            groups.Add(new SelectListItem() { Text = "همه", Value = null });
            foreach (var group in db.ProductGroups)
            {
                groups.Add(new SelectListItem() { Text = group.GroupTitle, Value = group.GroupID.ToString() });
            }
            ViewBag.GroupID = groups;
            return PartialView(features);
        }

        public void Delete(int? G, int? F)
        {
            if (G != null)
            {
                var group = db.ProductGroups.SingleOrDefault(g => g.GroupID == G).GroupID;

                foreach (var item in db.Features.Where(f => f.GroupID == group))
                {
                    db.Features.Remove(item);
                }

                db.SaveChanges();

            }

            if (F != null)
            {
                var feature = db.Features.Find(F);

                db.Features.Remove(feature);
                db.SaveChanges();
            }

        }

        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Features features = db.Features.Find(id);
        //    if (features == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.GroupID = new SelectList(db.ProductGroups, "GroupID", "GroupTitle", features.GroupID);
        //    return View(features);
        //}

        //// POST: Admin/Features/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "FeatureID,FeatureName,GroupID")] Features features)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(features).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.GroupID = new SelectList(db.ProductGroups, "GroupID", "GroupTitle", features.GroupID);
        //    return View(features);
        //}

        // GET: Admin/Features/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Features features = db.Features.Find(id);
        //    if (features == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(features);
        //}

        //// POST: Admin/Features/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Features features = db.Features.Find(id);
        //    db.Features.Remove(features);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
