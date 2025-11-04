using System.ComponentModel.DataAnnotations;

namespace WebCinemax.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar un nombre de usuario")]
        public string? Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar una contraseña")]
        public string? PasswordHash { get; set; }
    }
}
