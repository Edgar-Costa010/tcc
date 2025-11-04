using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidaPlus.Server.Data;
using VidaPlus.Server.DTO;
using VidaPlus.Server.Migrations;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly VidaPlusDbContext _context;

        public PacientesController(VidaPlusDbContext context)
        {
            _context = context;
        }

        // ✅ POST: api/Pacientes
        [HttpPost]
        public async Task<ActionResult<Paciente>> PostPaciente(PacienteDTO dto)
        {
            if (await _context.Pacientes.AnyAsync(p => p.CPF == dto.CPF))
                return BadRequest(new { mensagem = "CPF já cadastrado." });

            var paciente = new Paciente
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Endereco = dto.Endereco,
                DataNascimento = dto.DataNascimento,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, paciente);
        }

        // ✅ GET: api/Pacientes
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
        {
            return await _context.Pacientes.ToListAsync();
        }

        // ✅ GET: api/Pacientes/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Paciente")]
        public async Task<ActionResult<Paciente>> GetPaciente(int id)
        {
            // Recupera o ID do paciente autenticado a partir do token JWT
            var userIdClaim = User.FindFirst("PacienteId")?.Value;
            if (userIdClaim == null || int.Parse(userIdClaim) != id)
                return Forbid(); // Não permite acesso a dados de outros pacientes

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return NotFound();

            return paciente;
        }

        // ✅ PUT: api/Pacientes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, PacienteDTO dto)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return NotFound(new { mensagem = "Paciente não encontrado." });

            paciente.Nome = dto.Nome;
            paciente.Telefone = dto.Telefone;
            paciente.Email = dto.Email;
            paciente.Endereco = dto.Endereco;
            paciente.DataNascimento = dto.DataNascimento;

            if (!string.IsNullOrEmpty(dto.Senha))
                paciente.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            _context.Entry(paciente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/Pacientes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return NotFound(new { mensagem = "Paciente não encontrado no banco de dados!" });

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}