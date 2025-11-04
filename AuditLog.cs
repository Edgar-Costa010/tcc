namespace VidaPlus.Server.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Extra { get; set; }
    }
}