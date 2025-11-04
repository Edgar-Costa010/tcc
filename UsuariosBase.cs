using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VidaPlus.Server.Models
{
    public class UsuarioBase
    {
        public int Id { get; set; }

        [Required] public string Nome { get; set; } = string.Empty;
        [Required] public string CPF { get; set; }
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = "Paciente";
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        [JsonIgnore] public string SenhaHash { get; set; }
        public string Telefone { get; set; }
    }
}   