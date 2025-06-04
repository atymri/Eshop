using DataLayer;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductGroupsController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();


        public ActionResult Index()
        {
            var productGroups = db.ProductGroups.Where(g => g.ParentID == null);
            return View(productGroups.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroups productGroups = db.ProductGroups.Find(id);
            if (productGroups == null)
            {
                return HttpNotFound();
            }
            return View(productGroups);
        }

        public ActionResult Create(int? id)
        {
            return PartialView(new ProductGroups()
            {
                ParentID = id
            });
        }

        public ActionResult ShowList()
        {
            var productGroups = db.ProductGroups.Where(g => g.ParentID == null);
            return PartialView(productGroups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,GroupTitle,ParentID")] ProductGroups productGroups)
        {
            if (ModelState.IsValid)
            {
                db.ProductGroups.Add(productGroups);
                db.SaveChanges();
                return RedirectToAction("ShowList");
            }
            return PartialView(productGroups);
        }

        // GET: Admin/ProductGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroups productGroups = db.ProductGroups.Find(id);
            if (productGroups == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ParentID = new SelectList(db.ProductGroups, "GroupID", "GroupTitle", productGroups.ParentID);
            return PartialView(productGroups);
        }

        // POST: Admin/ProductGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,GroupTitle,ParentID")] ProductGroups productGroups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productGroups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowList");
            }
            ViewBag.ParentID = new SelectList(db.ProductGroups, "GroupID", "GroupTitle", productGroups.ParentID);
            return View(productGroups);
        }

        // GET: Admin/ProductGroups/Delete/5
        public void Delete(int id)
        {
            ProductGroups productGroups = db.ProductGroups.Find(id);
            var childs = db.ProductGroups.Where(p => p.ParentID == productGroups.GroupID);
            foreach (var item in childs)
            {
                db.ProductGroups.Remove(item);
            }
            db.ProductGroups.Remove(productGroups);
            db.SaveChanges();
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
