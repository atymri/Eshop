using DataLayer;
using Eshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eshop.Controllers
{
    public class ShopCartController : Controller
    {
        private Eshop_DBEntities db = new Eshop_DBEntities();

        public ActionResult Index()
        {
            return View();
        }

        List<FactorViewModel> GetFactorList()
        {
            List<FactorViewModel> factors = new List<FactorViewModel>();
            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> cartItems = Session["ShopCart"] as List<ShopCartItem>;
                foreach (var item in cartItems)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductId).Select(p =>
                        new
                        {
                            p.ProductImage,
                            p.ProductPrice,
                            p.ProductTitle,
                        }).Single();
                    factors.Add(new FactorViewModel()
                    {
                        ProductId = item.ProductId,
                        ProductTitle = product.ProductTitle,
                        ProductImage = product.ProductImage,
                        ProductPrice = product.ProductPrice,
                        Count = item.Count,
                        TotalPrice = item.Count * product.ProductPrice
                    });
                }
            }

            return factors;
        }

        public ActionResult FactorList()
        {
            return PartialView(GetFactorList());
        }

        public ActionResult FactorListCommands(int productId, int count)
        {
            var factors = Session["ShopCart"] as List<ShopCartItem>;
            int index = factors.FindIndex(p => p.ProductId == productId);
            if (count == 0)
            {
                factors.RemoveAt(index);
            }
            else
            {
                factors[index].Count = count;
            }

            Session["ShopCart"] = factors;
            return PartialView("FactorList", GetFactorList());
        }

        public PartialViewResult ShowCart()
        {
            List<ShopCartItemViewModel> list = new List<ShopCartItemViewModel>();
            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> cartItems = Session["ShopCart"] as List<ShopCartItem>;
                foreach (var item in cartItems)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductId).Select(p =>
                        new
                        {
                            p.ProductImage,
                            p.ProductTitle,
                            p.ProductPrice

                        }).Single();

                    list.Add(new ShopCartItemViewModel()
                    {
                        ProductId = item.ProductId,
                        ProductImage = product.ProductImage,
                        ProductTitle = product.ProductTitle,
                        Count = item.Count
                    });
                }
            }
            return PartialView(list);
        }

        [Authorize]
        public ActionResult SaveAndContinue()
        {
            int userId = db.Users.Single(u => u.UserName == User.Identity.Name).UserID;
            Orders order = new Orders()
            {
                UserID = userId,
                Date = DateTime.Now,
                IsFinally = false,
            };
            db.Orders.Add(order);

            var listDetails = GetFactorList();

            foreach (var item in listDetails)
            {
                var orderDetail = new OrderDetails()
                {
                    Count = item.Count,
                    OrderID = order.OrderID,
                    Price = item.ProductPrice,
                    ProductID = item.ProductId,
                };
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();

            //TODO : Online Payment

            return null;

        }
    }
}