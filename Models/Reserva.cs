using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiUniRoom.Models
{
    public class Reserva
    {
        public int idPension { get; set; }
        public int idCliente { get; set; }
    }
    public class AlojamientoEstadoRequest
    {
        public int IdAlojamiento { get; set; }
        public string NuevoEstado { get; set; }
        public int IdCuarto { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
    }


    public class ReservaCompleta
    {
        public int IdAlojamiento { get; set; }
        public string EstadoReserva { get; set; }
        public DateTime? FechaReserva { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }

        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string? FotoCliente { get; set; }

        public string? CaraTrasera { get; set; }
        public string? CaraDelantera { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }

        public string Ciudad { get; set; }
        public string Departamento { get; set; }

        public int IdPension { get; set; }
        public string NombrePension { get; set; }

        public int? IdCuarto { get; set; }
        public string? CodigoCuarto { get; set; }
    }

    public class HistorialPension
    {
        public int IdAlojamiento { get; set; }
        public string EstadoReserva { get; set; }
        public DateTime? FechaReserva { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }

        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string? FotoCliente { get; set; }

        public string? CaraTrasera { get; set; }
        public string? CaraDelantera { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }

        public string Ciudad { get; set; }
        public string Departamento { get; set; }

        public int IdPension { get; set; }
        public string NombrePension { get; set; }

        public int? IdCuarto { get; set; }
        public string? CodigoCuarto { get; set; }
        public int VecesAlojado { get; set; }
    }

    public class ReservaInfoCliente
    {
        public int IdAlojamiento { get; set; }

        // Relación con el cliente (usuario)
        public int IdCliente { get; set; }
        public string EstadoReserva { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaSalida { get; set; }

        // Datos del cliente
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string? FotoCliente { get; set; }

        // Datos del alojamiento/pensión
        public string NombrePension { get; set; }
        public string Direccion { get; set; }
        public string NomCiudad { get; set; }

        // Datos del cuarto
        public string? NombreCuarto { get; set; }
    }


}
