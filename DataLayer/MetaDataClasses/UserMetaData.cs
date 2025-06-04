using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class UserMetaData
    {
        [Key]
        public int UserID { get; set; }
        public int RoleID { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نمیباشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }

        public string ActiceCode { get; set; }

        [Display(Name = "حساب فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ عضویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public System.DateTime RegisterDate { get; set; }

        public virtual Roles Roles { get; set; }
    }

    [MetadataType(typeof(UserMetaData))]
    public partial class Users
    {

    }

}
