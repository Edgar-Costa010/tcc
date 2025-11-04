using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VidaPlus.Server.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        [Required] public string Nome { get; set; } = string.Empty;
        [Required] public string CPF { get; set; } = string.Empty;
        [JsonConverter(typeof(DateTimeConverter))] public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        [Required][JsonIgnore] public string SenhaHash { get; set; } = string.Empty;
        [JsonIgnore]public ICollection<Consulta>? Consultas { get; set; }
    }
}