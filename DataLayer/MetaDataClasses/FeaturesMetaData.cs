using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class FeaturesMetaData
    {
        [Key]
        public int FeatureID { get; set; }

        [Display(Name = "نام ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureName { get; set; }

        [Display(Name = "گروه")]
        public int? GroupID { get; set; }

    }

    [MetadataType(typeof(FeaturesMetaData))]
    public partial class Features
    {

    }
}
