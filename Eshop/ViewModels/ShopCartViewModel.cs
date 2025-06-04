namespace Eshop.ViewModels
{
    public class ShopCartItem
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }

    public class ShopCartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Count { get; set; }
        public string ProductImage { get; set; }
    }

    public class FactorViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Count { get; set; }
        public int ProductPrice { get; set; }
        public int TotalPrice { get; set; }
        public string ProductImage { get; set; }
    }
}