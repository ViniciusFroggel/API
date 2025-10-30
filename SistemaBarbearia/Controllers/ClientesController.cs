    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SistemaBarbearia.Data;
    using SistemaBarbearia.Models;

    namespace SistemaBarbearia.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ClientesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public ClientesController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
            {
                return await _context.Clientes.ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Cliente>> GetCliente(int id)
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                    return NotFound();

                return cliente;
            }

            [HttpPost]
            public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutCliente(int id, Cliente cliente)
            {
                if (id != cliente.Id)
                    return BadRequest();

                _context.Entry(cliente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCliente(int id)
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                    return NotFound();

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }
