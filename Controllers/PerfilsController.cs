using ApiUniRoom;
using ApiUniRoom.Custom;
using ApiUniRoom.Models;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiUniRoom.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Propietario, Cliente")]
    [ApiController]
    public class PerfilsController : ControllerBase
    {
        private readonly ContextDB _context;
        private readonly Utilidades _utilidad;

        public PerfilsController(ContextDB context, Utilidades utilidad)
        {
            _context = context;
            _utilidad = utilidad;

        }
        [HttpGet("PerfilUser")]
        public async Task<IActionResult> InformacionPerfil(int idUser)
        {
            try
            {
                var perfiles = await _context.Perfil
                    .FromSqlRaw("CALL InformacionPerfil({0})", idUser)
                    .ToListAsync();

                var perfil = perfiles.FirstOrDefault();

                if (perfil?.TipoUsuario == "Propietario" || perfil?.TipoUsuario == "Admin")
                {
                    var info = new PerfilPropAdm
                    {
                        IdUsuarios = perfil.IdUsuarios,
                        Nombres = perfil.Nombres,
                        Apellidos = perfil.Apellidos,
                        Sexo = perfil.Sexo,
                        Edad = perfil.Edad,
                        Telefono = perfil.Telefono,
                        Correo = perfil.Correo,
                        FotoPerfil = perfil.FotoPerfil,
                        TipoUsuario = perfil.TipoUsuario,
                        Ciudad = perfil.Ciudad,
                        IdCiudad = perfil.IdCiudad
                    };
                    return Ok(info);

                }

                return Ok(perfil);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, $"Error al obtener las reserva: {ex.Message}");
                }
            }
        }
        [HttpPost("usuario/InsertarDocumento")]
        public async Task<IActionResult> InsertarDocumento([FromBody] Documento request)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                         $"CALL InsertarDocumentoUsuario({request.IdUsuario}, {request.TipoDocumento}, {request.CaraDelantera}, {request.CaraTrasera}, {request.FechaSubida}, {request.NumeroDocumento})");


                return Ok(new { mensaje = "Documento insertado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar documento: {ex.Message}");
            }
        }
        [HttpPut("ActualizarPerfil")]
        public async Task<IActionResult> ActualizarUsuario([FromBody] ActualizarUsuarioDto model)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL ActualizarUsuario(
                {model.IdUsuario},
                {model.Nombres},
                {model.Apellidos},
                {model.Sexo},
                {model.Edad},
                {model.Telefono},
                {model.IdCiudadFk}
            )");

                return Ok(new { mensaje = "Usuario actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el usuario: {ex.Message}");
            }
        }

        [HttpPut("ActualizarDocumento")]
        public async Task<IActionResult> ActualizarDocumento([FromBody] ActualizarDocumentoDto model)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL ActualizarDocumentoUsuario(
                {model.IdDocumento},
                {model.TipoDocumento},
                {model.CaraDelantera},
                {model.CaraTrasera},
                {model.NumeroDocumento}
            )");

                return Ok(new { mensaje = "Documento actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el documento: {ex.Message}");
            }
        }
        [HttpPut("ActualizarContrasena")]
        public async Task<IActionResult> ActualizarContrasena([FromBody] ActualizarContrasenaDto model)
        {
            try
            {
                string hashPassword = _utilidad.encriptarSHA256(model.ContraseñaActual);
                string hashPassword2 = _utilidad.encriptarSHA256(model.NuevaContrasena);

                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL ActualizarContrasenaUsuario({model.IdUsuario}, {hashPassword}, {hashPassword2})
        ");
                return Ok(new { mensaje = "Contraseña actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la contraseña: {ex.Message}");
            }
        }

        [HttpPut("ActualizarCorreo")]
        public async Task<IActionResult> ActualizarCorreo([FromBody] ActualizarCorreoDto model)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL ActualizarCorreoUsuario({model.IdUsuario}, {model.NuevoCorreo})
        ");
                return Ok(new { mensaje = "Correo actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el correo: {ex.Message}");
            }
        }
        [HttpDelete("usuario/EliminarCuenta/{idUsuario}")]
        public async Task<IActionResult> EliminarCuenta(int idUsuario)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL EliminarCuenta({idUsuario})");

                return Ok(new { mensaje = "Cuenta eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar cuenta: {ex.Message}");
            }
        }




    }
}
