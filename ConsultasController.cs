using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidaPlus.Server.Data;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly VidaPlusDbContext _context;
        public ConsultasController(VidaPlusDbContext context) => _context = context;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.Consultas.Include(c => c.Paciente).Include(c => c.Profissional).OrderByDescending(c => c.Data).ToListAsync());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var c = await _context.Consultas.Include(x => x.Paciente).Include(x => x.Profissional).FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        // marcar consulta (paciente ou admin)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Consulta dto)
        {
            _context.Consultas.Add(dto);
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        // atualizar/encerrar consulta (profissional ou admin)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Consulta dto)
        {
            var c = await _context.Consultas.FindAsync(id);
            if (c == null) return NotFound();
            c.Data = dto.Data;
            c.Observacoes = dto.Observacoes;
            c.Concluida = dto.Concluida;
            await _context.SaveChangesAsync();
            return Ok(c);
        }

        // cancelar (apenas admin)
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Consultas.FindAsync(id);
            if (c == null) return NotFound();
            _context.Consultas.Remove(c);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Consulta cancelada." });
        }
    }
}