using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class SliderMetaData
    {
        [Key]
        public int SliderId { get; set; }

        [Display(Name = "عنوان اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(350, ErrorMessage = "عنوان اسلایدر نمیتواند از 350 کاراکتر بیشتر باشد")]
        public string Title { get; set; }

        [Display(Name = "لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Url(ErrorMessage = "لطفا یک لینک معتبر وارد کنید")]
        public string Url { get; set; }

        [Display(Name = "تاریخ شروع نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public System.DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public System.DateTime EndDate { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "نمایش در صفحه اصلی")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(SliderMetaData))]
    public partial class Slider
    {

    }

}
