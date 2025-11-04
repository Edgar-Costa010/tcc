namespace VidaPlus.Server.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
        public int ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }
        public bool Concluida { get; set; } = false;
    }
}