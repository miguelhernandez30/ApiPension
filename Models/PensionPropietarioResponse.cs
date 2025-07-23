using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace ApiUniRoom.Models
{
    public class Pensions
    {
        [Key]
        [Column("idPension")]
        public int IdPension { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(45)]
        public string Direccion { get; set; }

        public decimal? Latitud { get; set; }

        public decimal? Longitud { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int Capacidad { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } // ENUM: 'Activa', 'Inactiva', 'Reportada'

        [Required]
        public string Logo { get; set; }

        [Required]
        [MaxLength(10)]
        public string GeneroPermitido { get; set; } // ENUM: 'Masculino', 'Femenino', 'Ambos'

        [Required]
        [MaxLength(2)]
        public string PagoAnticipado { get; set; } // ENUM: 'SI', 'NO'

        [Required]
        [Column("Propietario_fk")]
        public int PropietarioFk { get; set; }

        [Required]
        [Column("idCiudad_fk")]
        public int CiudadFk { get; set; }

        public string? Descripcion { get; set; }
    }
    public class PensionPropietarioResponse
    {
        [Key]
        public int IdPension { get; set; }
        public int IdPropietario { get; set; }
        public string? NombrePension { get; set; }
        public string? Direccion { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Logo { get; set; }
        public string TienePagoAnticipado { get; set; }
        public string GeneroPermitido { get; set; }
        public string Ciudad { get; set; }
        public int IdCiudad { get; set; }
        public string Departamento { get; set; }
        public int IdDepartamento { get; set; }
        public bool? OfreceLimpieza { get; set; }
        public bool? AccesoCocina { get; set; }
        public bool? IncluyeWifi { get; set; }
        public string? Comida { get; set; }
        public string? Baños { get; set; }
        public string? Habitaciones { get; set; }
        public string? Parqueadero { get; set; }
        public int Capacidad { get; set; }
        public int ClientesAlojados { get; set; }
        [NotMapped]
        public List<FotoInfo> Fotos { get; set; }
        
    }


    public class FotoInfo
    {
        public int idFotos { get; set; }
        public string Fotos { get; set; }
        public string Estado { get; set; }
        public  DateTime FechaSubida { get; set; }
    }
    public class Pension
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public decimal Precio { get; set; }
        public int Capacidad { get; set; }
        public string Logo { get; set; }
        public string GeneroPermitido { get; set; } // Puede ser 'Masculino', 'Femenino' o 'Ambos'
        public string PagoAnticipado { get; set; } // 'SI' o 'NO'
        public int Propietario_fk { get; set; }
        public int IdCiudad_fk { get; set; }
    }

    public class PensionResponse
    {
        public int IdGenerado { get; set; } 
        public string Mensaje { get; set; }
    }


    public class UpdatePensión
    {
        public int IdPension { get; set; }        // El id de la pensión (este será generado por la base de datos si es necesario)
        public string Nombre { get; set; }        // Nombre de la pensión
        public decimal Precio { get; set; }       // Precio de la pensión
        public int Capacidad { get; set; }        // Capacidad de personas
        public string Logo { get; set; }          // Nombre o ruta del logo
        public string GeneroPermitido { get; set; } // Género permitido ('Masculino', 'Femenino', 'Ambos')
        public string PagoAnticipado { get; set; }  // Si se requiere pago anticipado ('SI', 'NO')
    }


    public class ServiciosPension
    {
        public int IdPensionFk { get; set; }           // El ID de la pensión (relacionado con la pensión principal)
        public byte Limpieza { get; set; }              // 0 o 1 para indicar si se proporciona servicio de limpieza
        public byte AccesoCocina { get; set; }          // 0 o 1 para indicar si hay acceso a la cocina
        public byte Internet { get; set; }              // 0 o 1 para indicar si hay acceso a Internet
        public string Comida { get; set; }              // Puede ser 'Todos los dias' o 'Personalizado'
        public string Banos { get; set; }               // Puede ser 'Compartido' o 'Privado'
        public string Habitaciones { get; set; }        // Puede ser 'Privadas', 'Compartidas' o 'Mixto'
        public string Parqueadero { get; set; }         // Puede ser 'Solo Motos', 'Solo Carros', 'Ambos' o 'No aplica'
    }





}
