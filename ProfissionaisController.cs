using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidaPlus.Server.Data;
using VidaPlus.Server.DTOs;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdministradorOnly")]
    public class ProfissionaisController : ControllerBase
    {
        private readonly VidaPlusDbContext _context;
        public ProfissionaisController(VidaPlusDbContext context) => _context = context;

        // GET: api/Profissionais
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Profissionais.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        // GET: api/Profissionais/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _context.Profissionais.FindAsync(id);
            if (p == null) return NotFound(new { mensagem = "Profissional não encontrado na base de dados.." });
            return Ok(p);
        }

        // POST: api/Profissionais
        [HttpPost]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Create([FromBody] ProfissionalDTO dto)
        {
            if (await _context.Profissionais.AnyAsync(x => x.CRM == dto.CRM))
                return BadRequest(new { mensagem = "CRM já cadastrado." });

            if (await _context.Profissionais.AnyAsync(x => x.CPF == dto.CPF))
                return BadRequest(new { mensagem = "CPF já cadastrado." });

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { mensagem = "Email já cadastrado." });

            if (string.IsNullOrWhiteSpace(dto.Senha))
                return BadRequest(new { mensagem = "A senha é obrigatória." });

            var profissional = new Profissional
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                CRM = dto.CRM,
                Especialidade = dto.Especialidade,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Unidade = dto.Unidade,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Perfil = "Profissional",
                DataCadastro = DateTime.UtcNow
            };

            _context.Profissionais.Add(profissional);

            // sincroniza com tabela Usuarios
            _context.Usuarios.Add(new UsuarioBase
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                Telefone = dto.Telefone,
                Email = dto.Email,
                SenhaHash = profissional.SenhaHash,
                Perfil = "Profissional",
                DataCadastro = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = profissional.Id }, profissional);
        }

        // PUT: api/Profissionais/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] ProfissionalDTO dto)
        {
            var prof = await _context.Profissionais.FirstOrDefaultAsync(p => p.Id == id);
            if (prof == null)
                return NotFound(new { mensagem = "Profissional não encontrado na base de dados.." });

            prof.Nome = dto.Nome;
            prof.CPF = dto.CPF;
            prof.CRM = dto.CRM;
            prof.Especialidade = dto.Especialidade;
            prof.Telefone = dto.Telefone;
            prof.Email = dto.Email;
            prof.Unidade = dto.Unidade;

            if (!string.IsNullOrWhiteSpace(dto.Senha))
                prof.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == prof.Email);
            if (usuario != null)
                usuario.SenhaHash = prof.SenhaHash;

            await _context.SaveChangesAsync();
            return Ok(prof);
        }

        // DELETE: api/Profissionais/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var prof = await _context.Profissionais.FindAsync(id);
            if (prof == null) return NotFound(new { mensagem = "Profissional não encontrado na base de dados." });

            _context.Profissionais.Remove(prof);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == prof.Email);
            if (usuario != null) _context.Usuarios.Remove(usuario);

            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Profissional removido com sucesso!" });
        }
    }
}