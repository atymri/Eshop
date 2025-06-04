using DataLayer;
using Eshop.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Eshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            DateTime todaysDate = DateTime.Now.Date;
            DateTime yesterdaysDate = DateTime.Now.AddDays(-1).Date;

            VisitsViewModel visits = new VisitsViewModel();
            visits.TodayVisits = db.Visits.Count(v => v.Date == todaysDate);
            visits.YesterDayVisits = db.Visits.Count(v => v.Date == yesterdaysDate);
            visits.AllTimeVisits = db.Visits.Count();
            return View(visits);
        }
    }
}