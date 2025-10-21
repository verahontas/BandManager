using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class RegistrationViewModel : UserViewModel
    {
        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [RegularExpression("^[A-Za-z0-9_-]{5,40}$", ErrorMessage = "A jelszó formátuma, vagy hossza nem megfelelő.")]
        [DataType(DataType.Password)]
        public String UserPassword { get; set; }

        [Required(ErrorMessage = "A jelszó ismételt megadása kötelező.")]
        [Compare(nameof(UserPassword), ErrorMessage = "A két jelszó nem egyezik.")]
        [DataType(DataType.Password)]
        public String UserConfirmPassword { get; set; }
    }
}
