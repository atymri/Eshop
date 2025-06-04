using DataLayer;
using Eshop.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Eshop.Areas.User.Controllers
{
    public class AccountController : Controller
    {
        private Eshop_DBEntities _context = new Eshop_DBEntities();

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel chPass)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Single(u => u.UserName == User.Identity.Name);
                var oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(chPass.Password, "MD5");
                if (user.Password == oldPassword)
                {
                    var newPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(chPass.NewPassword, "MD5");
                    user.Password = newPassword;
                    _context.SaveChanges();
                    ViewBag.Success = true;
                    return View();
                }
                ModelState.AddModelError("Password", "کلمه عبور فعلی درست نیست");

            }
            return View();
        }

    }
}