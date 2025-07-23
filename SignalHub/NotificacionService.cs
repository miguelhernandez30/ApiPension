using ApiUniRoom.Models;
using Microsoft.AspNetCore.SignalR;
using System;

namespace ApiUniRoom.SignalHub
{
    public class NotificacionService : INotificacionService
    {
        private readonly IHubContext<NotificacionHub> _hub;
        private readonly ContextDB _context;

        public NotificacionService(ContextDB context, IHubContext<NotificacionHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task CrearYEnviarNotificacion(int idUsuario, int? idPension, string mensaje, string tipo)
        {
            var noti = new Notificacion
            {
                id_usuario_fk = idUsuario,
                id_pension_fk = idPension,
                mensaje = mensaje,
                tipo = tipo,
                leida = false,
                fecha_creacion = DateTime.Now
            };
            _context.Notificaciones.Add(noti);
            await _context.SaveChangesAsync();

            await _hub.Clients.Group(idUsuario.ToString()).SendAsync("RecibirNotificacion", new
            {
                mensaje,
                tipo,
                fecha = noti.fecha_creacion
            });
        }
    }

}
