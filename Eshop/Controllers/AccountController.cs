using CaptchaMvc.HtmlHelpers;
using DataLayer;
using Eshop.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;


namespace Eshop.Controllers
{
    public class AccountController : Controller
    {

        private Eshop_DBEntities _context = new Eshop_DBEntities();

        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel account)
        {
            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.Error = "پاسخ Captcha اشتباه است, لطفا دوباره امتحان کنید.";
                return View();
            }


            if (!_context.Users.Any(u => u.Email.Trim().ToLower() == account.Email.Trim().ToLower()))
            {

                if (ModelState.IsValid)
                {
                    var user = new Users()
                    {
                        UserName = account.UserName,
                        Email = account.Email.Trim().ToLower(),
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(account.Password, "MD5"),
                        ActiceCode = Guid.NewGuid().ToString(),
                        RegisterDate = DateTime.Now,
                        RoleID = 1,
                        IsActive = false
                    };

                    var body = PartialToStringClass.RenderPartialView("ManageEmail", "ActivationEmail", user);
                    SendEmail.Send(user.Email, "لینک فعالسازی حساب کاربری", body);

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    return View("RegisterSuccess", user);
                }
            }
            else
            {
                ModelState.AddModelError("Email", "حساب کاربری با این ایمیل وجود دارد");

            }

            return View();
        }

        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login, string ReturnUrl = "/")
        {

            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.Error = "پاسخ Captcha اشتباه است, لطفا دوباره امتحان کنید.";
                return View();
            }

            if (ModelState.IsValid)
            {
                var password = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "MD5");
                var user = _context.Users.SingleOrDefault(u => u.Email == login.Email.Trim().ToLower() && u.Password == password);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "کاربری با این ایمیل وجود ندارد");
                }
                else
                {
                    if (user.IsActive)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, login.RememberMe);
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری فعال نشده است");
                    }
                }
            }
            return View();
        }

        [Route("LogOut")]
        [Route("Account/LogOut")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [Route("ForgotPassword")]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ForgotPassword")]
        public ActionResult ForgetPassword(ForgotPasswordViewModel forgot)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == forgot.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "کاربری با این ایمیل وجود ندارد");
                }
                else
                {
                    var body = PartialToStringClass.RenderPartialView("ManageEmail", "ForgetPassword", user);
                    SendEmail.Send(user.Email, "بازیابی کلمه عبور", body);
                    return View("SendPasswordEmailSeccess", user);
                }
            }
            return View();
        }

        public ActionResult RecoverPassword(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecoverPassword(string id, RecoverPasswordViewModel recover)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.ActiceCode == id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var password = FormsAuthentication.HashPasswordForStoringInConfigFile(recover.Password, "MD5");
                user.Password = password;
                user.ActiceCode = Guid.NewGuid().ToString();
                _context.SaveChanges();
                return Redirect("/Login?recover=true");
            }
            return View();
        }

        public ActionResult ActiveAccount(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiceCode == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.ActiceCode = Guid.NewGuid().ToString();
            user.IsActive = true;
            _context.SaveChanges();

            ViewBag.UserName = user.UserName;
            return View();
        }




    }
}