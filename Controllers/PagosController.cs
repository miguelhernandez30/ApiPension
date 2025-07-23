using ApiUniRoom;
using ApiUniRoom.Models;
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
    public class PagosController : ControllerBase
    {
        private readonly ContextDB _context;

        public PagosController(ContextDB context)
        {
            _context = context;
        }

        [HttpPost("InsertarPagoFlexible")]
        public async Task<IActionResult> PutUpdatePension([FromBody] Pagos pagos)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL RegistrarPagoFlexible ({pagos.IdPension},{pagos.IdCliente},{pagos.IdMetodoPago}, {pagos.Monto}, {pagos.EstadoPago})");
                return Ok(pagos);
            }
            catch
            (Exception ex)
            {
                return StatusCode(500, $"Error al insertar pago: {ex.Message}");
            }
        }
        [HttpPost("InsertarAbono")]
        public async Task<IActionResult> InsertarAbono([FromBody] Abono abono)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL InsertarAbono({abono.MontoAbonado}, {abono.FechaAbono}, {abono.IdPago_fk}, {abono.IdMetodoPago_fk})"
                );

                return Ok(new { mensaje = "Abono registrado correctamente", abono });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar abono: {ex.Message}");
            }
        }


        [HttpGet("ObtenerAbonosPorPago/{idPago}")]
        public async Task<IActionResult> ObtenerAbonosPorabono(int idPago)
        {
            try
            {
                var abonos = await _context.Abonos
                    .FromSqlInterpolated($"CALL ObtenerAbonosPorPago({idPago})")
                    .ToListAsync();

                return Ok(abonos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener abonos: {ex.Message}");
            }
        }
        [HttpGet("ObtenerPagosPension/{idPension}")]
        public async Task<IActionResult> ObtenerAbonosPorPension(int idPension)
        {
            try
            {
                var abonos = await _context.PagosPension
                    .FromSqlInterpolated($"CALL ObtenerPagosPension({idPension})")
                    .ToListAsync();

                return Ok(abonos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener abonos: {ex.Message}");
            }
        }
        [HttpPut("ForzarPagoCompleto")]
        public async Task<IActionResult> ForzarPagoCompleto( int IdPago, decimal TotalPago)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ForzarPagoCompleto({IdPago}, {TotalPago})");

                return Ok("Pago forzado a 'Completo'.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al forzar pago: {ex.Message}");
            }
        }

        [HttpPut("ReabrirPagoAAbonos")]
        public async Task<IActionResult> ReabrirPagoAAbonos(ReabrirAbono abono)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ReabrirPagoAAbonos({abono.IdPago}, {abono.MontoInicial}, {abono.MetodoPago_fk})");

                return Ok("Pago reabierto como abono.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al reabrir pago: {ex.Message}");
            }
        }
        [HttpPut("ActualizarMontoAbono")]
        public async Task<IActionResult> ActualizarMontoAbono(int IdAbono, decimal MontoAbonado)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ActualizarMontoAbono({IdAbono}, {MontoAbonado})");

                return Ok("Monto del abono actualizado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar monto del abono: {ex.Message}");
            }
        }


        [HttpDelete("EliminarPago/{idPago}")]
        public async Task<IActionResult> EliminarPago(int idPago)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL EliminarPago({idPago})");

                return Ok("Pago eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar pago: {ex.Message}");
            }
        }
     
        [HttpDelete("EliminarAbono/{idPago}")]
        public async Task<IActionResult> EliminarTodosAbonos(int idPago)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL EliminaAbono({idPago})");

                return Ok("Abono eliminado satisfactoriamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar: {ex.Message}");
            }
        }
      


    }
}
