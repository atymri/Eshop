using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class ProductCommentsMetaData
    {
        public int CommentID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> ParentID { get; set; }

        [Required(ErrorMessage = "وارد کردن نام الزامی است")]
        public string Name { get; set; }

        [Required(ErrorMessage = "وارد کردن ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "لطفا یک ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        public string Website { get; set; }
        public System.DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "وارد کردن متن نطر الزامی است")]
        public string CommentText { get; set; }
    }

    [MetadataType(typeof(ProductCommentsMetaData))]
    public partial class ProductComments
    {

    }
}
