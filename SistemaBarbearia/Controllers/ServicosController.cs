using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBarbearia.Data;
using SistemaBarbearia.Models;

namespace SistemaBarbearia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servico>>> GetServicos()
        {
            return await _context.Servicos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Servico>> GetServico(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
                return NotFound();

            return servico;
        }

        [HttpPost]
        public async Task<ActionResult<Servico>> PostServico(Servico servico)
        {
            _context.Servicos.Add(servico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServico), new { id = servico.Id }, servico);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServico(int id, Servico servico)
        {
            if (id != servico.Id)
                return BadRequest();

            _context.Entry(servico).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServico(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null)
                return NotFound();

            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
