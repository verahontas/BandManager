using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "A felhasználónév megadása kötelező.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public bool RememberLogin { get; set; }
    }
}
