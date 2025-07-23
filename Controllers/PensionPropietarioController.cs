using ApiUniRoom;
using ApiUniRoom.Models;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ApiUniRoom.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Propietario")]
    [ApiController]
    public class PensionPropietarioController : ControllerBase
    {
        private readonly ContextDB _context;

        public PensionPropietarioController(ContextDB context)
        {
            _context = context;
        }
        [HttpGet("MisPensiones/{idPropietario}")]
        public async Task<IActionResult> ObtenerPensionesDelPropietario(int idPropietario)
        {
            var pensiones = await _context.PensionPropietarioResponse
                .FromSqlRaw("CALL ObtenerPensionesPropietario({0})", idPropietario)
                .ToListAsync();

            if (pensiones == null || pensiones.Count == 0)
            {
                return NotFound("Este Usuario No tiene pensiones");
            }

            foreach (var pension in pensiones)
            {
                var fotos = await _context.Foto
                       .FromSqlInterpolated($"CALL ObtenerFotosPension({pension.IdPension})")
                       .ToListAsync();

                pension.Fotos = fotos.Select(f => new FotoInfo
                {
                    idFotos = f.idFotos,
                    Fotos = f.Fotos,
                    Estado = f.Estado,
                    FechaSubida = f.FechaSubida,
                }).ToList();
            }

            return Ok(pensiones);
        }

        [HttpPost("InsertarPension")]  
        public async Task<ActionResult<PensionResponse>> PostInsertarPension([FromBody] Pension pension)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int idNuevaPension = 0;

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "InsertarPension";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@pNombre", pension.Nombre));
            cmd.Parameters.Add(new MySqlParameter("@pDireccion", pension.Direccion));
            cmd.Parameters.Add(new MySqlParameter("@pLatitud", pension.Latitud));
            cmd.Parameters.Add(new MySqlParameter("@pLongitud", pension.Longitud));
            cmd.Parameters.Add(new MySqlParameter("@pPrecio", pension.Precio));
            cmd.Parameters.Add(new MySqlParameter("@pCapacidad", pension.Capacidad));
            cmd.Parameters.Add(new MySqlParameter("@pLogo", pension.Logo));
            cmd.Parameters.Add(new MySqlParameter("@pGeneroPermitido", pension.GeneroPermitido));
            cmd.Parameters.Add(new MySqlParameter("@pPagoAnticipado", pension.PagoAnticipado));
            cmd.Parameters.Add(new MySqlParameter("@pPropietario_fk", pension.Propietario_fk));
            cmd.Parameters.Add(new MySqlParameter("@pIdCiudad_fk", pension.IdCiudad_fk));

            var outputParam = new MySqlParameter("@IdGenerado", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            await cmd.ExecuteNonQueryAsync();

            idNuevaPension = (int)(outputParam.Value ?? 0);         

            return Ok(new PensionResponse
            {
                IdGenerado = idNuevaPension,
                Mensaje = "Usuario registrado correctamente."
            });
        }

        [HttpPut ("ActualizarPension")]
        public async Task<IActionResult> PutUpdatePension([FromBody] UpdatePensión pensionU)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ActualizarPension ({pensionU.IdPension},{pensionU.Nombre},{pensionU.Precio}, {pensionU.Capacidad}, {pensionU.Logo},{pensionU.GeneroPermitido}, {pensionU.PagoAnticipado} )");
                return Ok(pensionU);
            }
            catch
            (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar: {ex.Message}");
            }

        }

        [HttpPut("ActualizarServiciosP")]
        public async Task<IActionResult> PutUpdatePensionServices([FromBody] ServiciosPension services)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ActualizarServiciosPension ({services.IdPensionFk},{services.Limpieza},{services.AccesoCocina}, {services.Internet}, {services.Comida},{services.Banos}, {services.Habitaciones}, {services.Parqueadero} )");
                return Ok(services);
            }
            catch
            (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar: {ex.Message}");
            }

        }

        [HttpPut("EliminarPension")]
        public async Task<IActionResult> DeletePension(int idPension)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL InactivarPension ({idPension})");
                return Ok("Pension eliminada correctamente");
            }
            catch
            (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar: {ex.Message}");
            }
        }

        [HttpPost("SubirFoto")]
        public async Task<IActionResult> SubirFoto( [FromBody] FotoRequest foto)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL InsertarFoto({foto.FotoText}, {foto.IdPensionFk})");

                return Ok("Foto subida exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir foto: {ex.Message}");
            }
        }

        [HttpPut("EliminarFoto/{idFoto}")]
        public async Task<IActionResult> EliminarFoto(int idFoto)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ActualizarEstadoFoto({idFoto})");

                return Ok("Foto eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la foto: {ex.Message}");
            }
        }


        [HttpPost("InsertarCuarto")]
        public async Task<IActionResult> InsertarCuarto([FromBody] CuartoRequest cuarto)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL InsertarCuarto({cuarto.CodCuarto}, {cuarto.Capacidad}, {cuarto.Descripcion},{cuarto.idPension_fk})");

                return Ok("Cuarto insertado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar el cuarto: {ex.Message}");
            }
        }

        [HttpPut("ActualizarCuarto")]
        public async Task<IActionResult> ActualizarCuarto([FromBody] CuartoUpdateRequest cuarto)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ActualizarCuarto({cuarto.idCuarto}, {cuarto.CodCuarto}, {cuarto.Capacidad}, {cuarto.Descripcion})");

                return Ok("Cuarto actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el cuarto: {ex.Message}");
            }
        }

        [HttpPut("EliminarCuarto/{idCuarto}")]
        public async Task<IActionResult> EliminarCuarto(int idCuarto)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL InactivarCuarto({idCuarto})");

                return Ok("Cuarto eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el cuarto: {ex.Message}");
            }
        }

    }
}
