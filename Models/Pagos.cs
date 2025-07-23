using System.ComponentModel.DataAnnotations;

namespace ApiUniRoom.Models
{
    public class Pagos
    {
        public int IdPension { get; set; }
        public int IdCliente { get; set; }
        public int IdMetodoPago { get; set; }
        public decimal Monto { get; set; }
        public string EstadoPago { get; set; }
    }
    public class PagosAbonos
    {
        public int idAbono { get; set; }
        public decimal MontoAbonado { get; set; }
        public DateTime FechaAbono { get; set; }
        public string EstadoAbono { get; set; } = string.Empty;
    } 
    public class ReabrirAbono
    {
        public int IdPago { get; set; }
        public decimal MontoInicial { get; set; }
        public int MetodoPago_fk { get; set; }
    }
    public class PagoDetalle
    {
        public DateTime FechaPago { get; set; }
        public decimal? TotalPago { get; set; }
        public string EstadoPago { get; set; }
        public DateTime? FechaLimite { get; set; }
        public int IdPago { get; set; }
        public int IdPension { get; set; }
        public string NombrePension { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Metodo { get; set; }
    }
    public class Abono
    {
        public decimal MontoAbonado { get; set; }
        public DateTime FechaAbono { get; set; }
        public int IdPago_fk { get; set; }
        public int IdMetodoPago_fk { get; set; }
    }


}
