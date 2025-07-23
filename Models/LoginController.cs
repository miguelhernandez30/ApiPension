using System.ComponentModel.DataAnnotations;

namespace ApiUniRoom.Models
{
    public class LoginController
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Contrasena { get; set; }
    }
    public class RegistroRequest
    {
        [Required]
        [MaxLength(45)]
        public string Nombres { get; set; }

        [Required]
        [MaxLength(45)]
        public string Apellidos { get; set; }

        [Required]
        public string Sexo { get; set; }

        [Required]
        [Range(1, 120)]
        public int Edad { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(45)]
        public string Correo { get; set; }

        [Required]
        [MaxLength(150)]
        public string Contrasena { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        [Required]
        public int IdTipo_User_fk { get; set; }

        [Required]
        public int IdCiudad_fk { get; set; }
    }

    public class RegistroResponse
    {
        public int IdNuevoUsuario { get; set; }
        public string Mensaje { get; set; }
    }


    public class LoginResponse
    {
        public int IdUsuarios { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string FotoPerfil { get; set; } = string.Empty;
        public string? TipoUsuario { get; set; }
        public string? Token { get; set; }  // Puedes llenarlo después de generar el JWT
    }


}
