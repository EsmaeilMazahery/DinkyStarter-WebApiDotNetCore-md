
using System.ComponentModel.DataAnnotations;

namespace Dinky.WebApi.ViewModels
{
    public class LoginModel: ViewModelBase
    {
        [Display(Name = "موبایل یا ایمیل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }
    }

    public class ForgetPasswordModel : ViewModelBase
    {
        [Display(Name = "موبایل یا ایمیل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }
    }

    public class ChangePasswordModel : ViewModelBase
    {
        [Display(Name = "موبایل یا ایمیل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string verifyCode { set; get; }
    }
}