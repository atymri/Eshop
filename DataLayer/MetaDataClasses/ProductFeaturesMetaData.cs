using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    class ProductFeaturesMetaData
    {
        [Key]
        public int PFID { get; set; }

        [Display(Name = "کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductID { get; set; }

        [Display(Name = "ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int FeatureID { get; set; }

        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Value { get; set; }
    }

    [MetadataType(typeof(ProductFeaturesMetaData))]
    public partial class ProductFeatures
    {

    }

}
