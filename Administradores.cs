namespace VidaPlus.Server.Models
{
    public class Administradores : UsuarioBase
    {
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}