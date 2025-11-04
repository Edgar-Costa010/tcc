using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VidaPlus.Server.Models
{
    public class Profissional
    {
        public int Id { get; set; }

        [Required] public string Nome { get; set; } = string.Empty;
        [Required] public string CPF { get; set; }
        [Required] public string CRM { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Unidade { get; set; } = string.Empty;
        [Required][JsonIgnore]public string SenhaHash { get; set; } = string.Empty;
        public string Perfil { get; set; } = "Profissional";
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
