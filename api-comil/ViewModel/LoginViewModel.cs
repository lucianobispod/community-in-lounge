using System.ComponentModel.DataAnnotations;

namespace api_comil.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [MinLength(10)]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}