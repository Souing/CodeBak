using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass
{
    public class PersonModel
    {
        [Required]
        [Range(5, 85)]
        public int? Age { set; get; }

        [Range(1, 1000)]
        public long Id { get; set; }

        [Required(ErrorMessage = "手机号必填")]
        //[StringLength(15)]
        //[StringLength(15,MinimumLength =6)]
        public string PhoneNum { get; set; }
         [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "两次输入的密码必须一致")]
        public string Password2 { get; set; }

        [EmailAddress(ErrorMessage = "{0}请输入正确Email地址")]
        public string Email { get; set; }

    }
}
