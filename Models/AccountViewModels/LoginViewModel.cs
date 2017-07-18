using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace M101N.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "test123";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
