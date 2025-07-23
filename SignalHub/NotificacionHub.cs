using Microsoft.AspNetCore.SignalR;
namespace ApiUniRoom.SignalHub
{
    public class NotificacionHub : Hub
    {
        public async Task UnirseGrupo(string grupoId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, grupoId);
        }
    }
}
