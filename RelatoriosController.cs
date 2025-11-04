using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidaPlus.Server.Data;

namespace VidaPlus.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly VidaPlusDbContext _context;
        public RelatoriosController(VidaPlusDbContext context) => _context = context;

        [HttpGet("resumo")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Resumo()
        {
            var totalPacientes = await _context.Pacientes.CountAsync();
            var totalProfissionais = await _context.Profissionais.CountAsync();
            var totalConsultas = await _context.Consultas.CountAsync();
            var totalLeitos = await _context.Leitos.CountAsync();

            return Ok(new
            {
                totalPacientes,
                totalProfissionais,
                totalConsultas,
                totalLeitos
            });
        }

        [HttpGet("financeiro")]
        [Authorize(Policy = "AdministradorOnly")]
        public async Task<IActionResult> Financeiro()
        {
            var receitas = await _context.Set<Models.Financeiro>().Where(f => f.Tipo == "Receita").SumAsync(f => (decimal?)f.Valor) ?? 0m;
            var despesas = await _context.Set<Models.Financeiro>().Where(f => f.Tipo == "Despesa").SumAsync(f => (decimal?)f.Valor) ?? 0m;
            return Ok(new { receitas, despesas, saldo = receitas - despesas });
        }
    }
}