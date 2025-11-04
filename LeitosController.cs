// VidaPlus.Server/Controllers/LeitosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidaPlus.Server.Data;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdministradorOnly")]
    public class LeitosController : ControllerBase
    {
        private readonly VidaPlusDbContext _context;
        public LeitosController(VidaPlusDbContext context) { _context = context; }

        // GET: api/Leitos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leito>>> GetAll()
        {
            return await _context.Leitos.OrderBy(l => l.Unidade).ThenBy(l => l.Numero).ToListAsync();
        }

        // GET: api/Leitos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Leito>> Get(int id)
        {
            var l = await _context.Leitos.FindAsync(id);
            if (l == null) return NotFound();
            return l;
        }

        // POST: api/Leitos
        [HttpPost]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<ActionResult<Leito>> Post([FromBody] Leito model)
        {
            _context.Leitos.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        // PUT: api/Leitos/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Put(int id, [FromBody] Leito model)
        {
            var l = await _context.Leitos.FindAsync(id);
            if (l == null) return NotFound();
            l.Unidade = model.Unidade;
            l.Numero = model.Numero;
            l.Ocupado = model.Ocupado;
            l.Tipo = model.Tipo;
            _context.Entry(l).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Leitos/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var l = await _context.Leitos.FindAsync(id);
            if (l == null) return NotFound();
            _context.Leitos.Remove(l);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Leitos/relatorio
        [HttpGet("relatorio")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Relatorio()
        {
            var total = await _context.Leitos.CountAsync();
            var ocupados = await _context.Leitos.CountAsync(l => l.Ocupado);
            var livres = total - ocupados;
            return Ok(new { totalLeitos = total, ocupados, livres });
        }
    }
}
