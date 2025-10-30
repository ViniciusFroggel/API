using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBarbearia.Data;
using SistemaBarbearia.Models;

namespace SistemaBarbearia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AgendamentosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agendamento>>> GetAgendamentos()
        {
            return await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Agendamento>> GetAgendamento(int id)
        {
            var agendamento = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                return NotFound();

            return agendamento;
        }

        [HttpPost]
        public async Task<ActionResult<Agendamento>> PostAgendamento(Agendamento agendamento)
        {
            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgendamento), new { id = agendamento.Id }, agendamento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgendamento(int id, Agendamento agendamento)
        {
            if (id != agendamento.Id)
                return BadRequest();

            var existingAgendamento = await _context.Agendamentos
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingAgendamento == null)
                return NotFound();

            // Atualiza os campos
            existingAgendamento.ClienteId = agendamento.ClienteId;
            existingAgendamento.ServicoId = agendamento.ServicoId;
            existingAgendamento.DataHora = agendamento.DataHora;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgendamento(int id)
        {
            var agendamento = await _context.Agendamentos
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
                return NotFound();

            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
