// VidaPlus.Server/Models/Financeiro.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VidaPlus.Server.Models
{
    public class Financeiro
    {
        public int Id { get; set; }

        [Required]
        public DateTime? Data { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal Valor { get; set; }

        [Required]
        // "Entrada" ou "Saida"
        public string Tipo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        // opcional: unidade/centro de custo
        public string? Unidade { get; set; }
    }
}
