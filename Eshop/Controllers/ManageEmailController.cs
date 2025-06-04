using DataLayer;
using System.Web.Mvc;

namespace Eshop.Controllers
{
    public class ManageEmailController : Controller
    {

        public PartialViewResult ActivationEmail(Users user)
        {
            return PartialView();
        }

        public PartialViewResult ForgetPassword(Users user)
        {
            return PartialView();
        }


    }
}