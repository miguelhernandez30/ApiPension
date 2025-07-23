using System.ComponentModel.DataAnnotations;

namespace ApiUniRoom.Models
{
    public class Notificacion
    {
        [Key]
        public int idNotificacion { get; set; }

        public int id_usuario_fk { get; set; }
        public int? id_pension_fk { get; set; }

        public string mensaje { get; set; } = string.Empty;

        public string tipo { get; set; } = string.Empty; // Ejemplo: "reserva", "comentario", "respuesta", etc.

        public bool leida { get; set; }

        public DateTime fecha_creacion { get; set; }

    }

}
