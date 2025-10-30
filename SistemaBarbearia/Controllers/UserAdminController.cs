using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaBarbearia.Models;

namespace SistemaBarbearia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Funcionario")] // Somente funcionário (admin)
    public class UserAdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Remover acesso de funcionário (tirar role)
        [HttpPost("remover-acesso")]
        public async Task<IActionResult> RemoverAcesso([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Usuário não encontrado");

            await _userManager.RemoveFromRoleAsync(user, "Funcionario");
            await _userManager.AddToRoleAsync(user, "Cliente");

            user.TipoUsuario = "Cliente";
            await _userManager.UpdateAsync(user);

            return Ok($"O usuário {email} agora é CLIENTE.");
        }

        // Ver roles do usuário
        [HttpGet("roles")]
        public async Task<IActionResult> VerRoles([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Usuário não encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
    }
}
