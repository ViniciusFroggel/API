using Microsoft.AspNetCore.Identity;

namespace SistemaBarbearia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; }
        public string TipoUsuario { get; set; } // Cliente ou Funcionario
    }
}
