namespace VidaPlus.Server.Models
{
    public class Internacao
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
        public int LeitoId { get; set; }
        public Leito? Leito { get; set; }
        public DateTime Entrada { get; set; } = DateTime.UtcNow;
        public DateTime? Saida { get; set; }
    }
}