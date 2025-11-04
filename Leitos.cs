// VidaPlus.Server/Models/Leito.cs
using System.ComponentModel.DataAnnotations;

namespace VidaPlus.Server.Models
{
    public class Leito
    {
        public int Id { get; set; }

        [Required]
        public string Unidade { get; set; } = string.Empty; // ex: "Hospital Central"

        [Required]
        public string Numero { get; set; } = string.Empty;  // ex: "101A"

        // false = livre, true = ocupado
        public bool Ocupado { get; set; } = false;
        public string? Tipo { get; set; }
    }
}
