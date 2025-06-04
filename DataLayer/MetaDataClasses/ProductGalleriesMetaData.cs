using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class ProductGalleriesMetaData
    {
        [Key]
        public int GalleryID { get; set; }
        [Display(Name = "کالا")]
        public int ProductID { get; set; }
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        [Display(Name = "عنوان تصویر")]
        [Required(ErrorMessage = "وارد کردن عنوان تصویر الزامی میباشد")]
        public string Title { get; set; }
    }

    [MetadataType(typeof(ProductGalleriesMetaData))]
    public partial class ProductGalleries
    {

    }
}
