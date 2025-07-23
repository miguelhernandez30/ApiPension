using System.ComponentModel.DataAnnotations;

namespace ApiUniRoom.Models
{
    public class Comentarios
    {
        public int IdComentario { get; set; }
        public int IdClienteFk { get; set; }
        public int IdPensionFk { get; set; }
        public string ComentarioR { get; set; }
        public int Puntaje { get; set; }
        public string? Respuesta { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class ComentarioView
    {
        [Key]
        public int IdComentario { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaComentario { get; set; }
        public string Puntaje { get; set; }

        public string? Respuesta { get; set; }
        public DateTime? FechaRespuesta { get; set; }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string? FotoUsuario { get; set; }

        public int IdPension { get; set; }
        public string NombrePension { get; set; }

        public int IdPropietario { get; set; }
        public string NombrePropietario { get; set; }
        public string ApellidoPropietario { get; set; }
        public string? FotoPerfilPropietario { get; set; }
    }

    public class Comentario
    {
        public int IdClienteFk { get; set; }
        public int IdPensionFk { get; set; }
        public string ComentarioTexto { get; set; }
        public string Puntaje { get; set; }  // Puede ser string para que funcione con el ENUM
    }
    public class RespuestaComentarioDTO
    {
        public int IdComentario { get; set; }
        public string Respuesta { get; set; } = string.Empty;
    }


}
