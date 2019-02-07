
using System;
using System.ComponentModel.DataAnnotations;
using Dinky.Domain.ViewModels;
using Dinky.Infrastructure.Constants;

namespace Dinky.DomainLayer.Models
{
    public class Media
    {
        public int mediaId { set; get; }

        [Display(Name = "عنوان")]
        [MaxLength(ConstantValidations.TitleLength)]
        public string title { set; get; }

        [Display(Name = "آدرس")]
        [Required]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string address { set; get; }

        [Display(Name = "تاریخ")]
        [Required]
        public DateTime date { set; get; } = DateTime.Now;

        [Display(Name = "کاربر")]
        [Required]
        public int userId { set; get; }
        public virtual User user { set; get; }
    }
}
