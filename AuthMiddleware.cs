using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using VidaPlus.Server.Data;
using VidaPlus.Server.Models;

namespace VidaPlus.Server.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        public AuditMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext context, VidaPlusDbContext db)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;
            var user = context.User?.Identity?.Name ?? "anon";

            await _next(context);

            try
            {
                var log = new AuditLog
                {
                    Usuario = user,
                    Action = method,
                    Path = path,
                    Timestamp = DateTime.UtcNow
                };

                db.AuditLogs.Add(log);
                await db.SaveChangesAsync();
            }
            catch
            {
                // não interrompe requisição se falhar logging
            }
        }
    }
}