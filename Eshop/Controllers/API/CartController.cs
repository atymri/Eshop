using Eshop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Eshop.Controllers.API
{
    public class CartController : ApiController
    {

        public int Get()
        {
            List<ShopCartItem> list = new List<ShopCartItem>();
            var sessions = HttpContext.Current.Session;
            if (sessions["ShopCart"] != null)
            {
                list = sessions["ShopCart"] as List<ShopCartItem>;
            }

            return list.Sum(l => l.Count);
        }

        public int Get(int id)
        {
            List<ShopCartItem> list = new List<ShopCartItem>();
            var sessions = HttpContext.Current.Session;
            if (sessions["ShopCart"] != null)
            {
                list = sessions["ShopCart"] as List<ShopCartItem>;
            }

            if (list.Any(l => l.ProductId == id))
            {
                int index = list.FindIndex(l => l.ProductId == id);
                list[index].Count += 1;
            }
            else
            {
                list.Add(new ShopCartItem()
                {
                    ProductId = id,
                    Count = 1
                });
            }

            sessions["ShopCart"] = list;
            return Get();

        }
    }
}
