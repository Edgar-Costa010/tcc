using System;
using System.ComponentModel.DataAnnotations;

namespace VidaPlus.Server.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty; // Entrada / Saída

        [Required]
        public decimal Valor { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}