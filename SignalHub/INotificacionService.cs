namespace ApiUniRoom.SignalHub
{
    public interface INotificacionService
    {
        Task CrearYEnviarNotificacion(int idUsuario, int? idPension, string mensaje, string tipo);
    }

}
