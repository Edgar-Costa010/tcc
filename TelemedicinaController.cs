using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidaPlus.Server.Data;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemedicinaController : ControllerBase
    {
        private readonly VidaPlusDbContext _context;
        public TelemedicinaController(VidaPlusDbContext context) => _context = context;

        // GET: lista teleconsultas (pode reutilizar Consultas)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.Consultas.Where(c => c.Observacoes.Contains("[TELE]") || true).Include(c => c.Paciente).Include(c => c.Profissional).ToListAsync());

        // POST: marcar teleconsulta (inclui link ou meta)
        [HttpPost("agendar")]
        [Authorize]
        public async Task<IActionResult> Agendar([FromBody] Consulta dto)
        {
            // Para sinalizar teleconsulta, prefixar observacoes com [TELE] ou adicionar campo dedicado
            if (!dto.Observacoes.Contains("[TELE]"))
                dto.Observacoes = "[TELE] " + dto.Observacoes;

            _context.Consultas.Add(dto);
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        // POST: registrar prescrição (vinculado a consulta)
        [HttpPost("prescricao")]
        [Authorize]
        public async Task<IActionResult> Prescrever([FromBody] Prescricao presc)
        {
            presc.Data = DateTime.UtcNow;
            _context.Prescricoes.Add(presc);
            await _context.SaveChangesAsync();
            return Ok(presc);
        }
    }
}