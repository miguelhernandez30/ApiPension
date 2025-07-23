using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ApiUniRoom.Models
{
    using System;

    public class Perfil
    {
        // Datos del usuario
        public int IdUsuarios { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; } // Podría ser un enum si quieres tiparlo
        public int Edad { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string? FotoPerfil { get; set; }

        // Tipo de usuario
        public string TipoUsuario { get; set; }

        // Ciudad
        public string Ciudad { get; set; }
        public int IdCiudad { get; set; }

        // Departamento
        public string Departamento { get; set; }
        public int IdDepartamento { get; set; }

        // Documento de identidad
        public int? IdDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? CaraDelantera { get; set; }
        public string? CaraTrasera { get; set; }
        public DateTime? FechaSubida { get; set; }
        public bool? Verificado { get; set; }
    }

    public class PerfilPropAdm
    {
        // Datos del usuario
        public int IdUsuarios { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; } // Podría ser un enum si quieres tiparlo
        public int Edad { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string? FotoPerfil { get; set; }

        // Tipo de usuario
        public string TipoUsuario { get; set; }

        // Ciudad
        public string Ciudad { get; set; }
        public int IdCiudad { get; set; }

        // Departamento
        public string Departamento { get; set; }
        public int IdDepartamento { get; set; }
    }
    public class ActualizarUsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty; // 'Masculino', 'Femenino', 'Otro'
        public int Edad { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public int IdCiudadFk { get; set; }
    }

    public class ActualizarDocumentoDto
    {
        public int IdDocumento { get; set; }
        public string TipoDocumento { get; set; } = string.Empty; // "Cedula" o "Tarjeta"
        public string CaraDelantera { get; set; } = string.Empty; // Puede ser una URL base64 o path
        public string CaraTrasera { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
    }

    public class ActualizarContrasenaDto
    {
        public int IdUsuario { get; set; }
        public string ContraseñaActual { get; set; }
        public string NuevaContrasena { get; set; } // Encriptar desde frontend o backend
    }

    public class ActualizarCorreoDto
    {
        public int IdUsuario { get; set; }
        public string NuevoCorreo { get; set; }
    }



}
