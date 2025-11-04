using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VidaPlus.Server.Data;
using VidaPlus.Server.DTO;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly VidaPlusDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(VidaPlusDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ✅ REGISTRO
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { mensagem = "Email já cadastrado." });

            if (await _context.Usuarios.AnyAsync(u => u.CPF == dto.CPF))
                return BadRequest(new { mensagem = "CPF já cadastrado." });

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var novoUsuario = new UsuarioBase
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                CPF = dto.CPF,
                SenhaHash = senhaHash,
                Perfil = string.IsNullOrEmpty(dto.Perfil) ? "Paciente" : dto.Perfil,
                DataCadastro = DateTime.UtcNow
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Usuário cadastrado com sucesso!" });
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 🔍 Tenta encontrar usuário base (Admin / Profissional)
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                    return Unauthorized(new { mensagem = "Credenciais inválidas." });

                var token = GerarToken(usuario);

                return Ok(new
                {
                    token,
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Nome,
                        usuario.Email,
                        usuario.Perfil
                    }
                });
            }

            // 🔍 Se não for usuário base, tenta buscar paciente
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Email == dto.Email);

            if (paciente == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, paciente.SenhaHash))
                return Unauthorized(new { mensagem = "Credenciais inválidas." });

            // ✅ Gera token para paciente com perfil fixo
            var pacToken = GerarToken(new UsuarioBase
            {
                Id = paciente.Id,
                Email = paciente.Email,
                Nome = paciente.Nome,
                Perfil = "Paciente"
            });

            return Ok(new
            {
                token = pacToken,
                usuario = new
                {
                    paciente.Id,
                    paciente.Nome,
                    paciente.Email,
                    Perfil = "Paciente"
                }
            });
        }

        // ✅ TOKEN JWT
        private string GerarToken(UsuarioBase usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}