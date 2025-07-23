using ApiUniRoom;
using ApiUniRoom.Models;
using ApiUniRoom.SignalHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiUniRoom.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Propietario, Cliente")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly ContextDB _context;
        private readonly INotificacionService _notificacionService;

        public ReservaController(ContextDB context, INotificacionService notificacionService)
        {
            _context = context;
            _notificacionService = notificacionService;
        }

        [HttpPost("cliente/RealizarReserva")]
        public async Task<ActionResult> PostReservar(Reserva reservas)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ReservarAlojamiento({reservas.idPension}, {reservas.idCliente})"
                );

                // Obtener el propietario de la pensión
                var pension = await _context.Pensiones.FindAsync(reservas.idPension);
                if (pension != null)
                {
                    var idPropietario = pension.PropietarioFk;

                    await _notificacionService.CrearYEnviarNotificacion(
                        idPropietario,
                        reservas.idPension,
                        "Has recibido una nueva solicitud de reserva.",
                        "reserva"
                    );
                }

                return Ok("Reserva realizada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al reservar: {ex.Message}");
            }
        }


        [HttpPut("propietario/CambiarEstadoReserva")]
        public async Task<IActionResult> CambiarEstadoReserva([FromBody] AlojamientoEstadoRequest request)
        {
            if (string.IsNullOrEmpty(request.NuevoEstado) ||
                (request.NuevoEstado != "Aceptado" && request.NuevoEstado != "Rechazado"))
            {
                return BadRequest("Estado no válido. Debe ser 'Aceptado' o 'Rechazado'.");
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ActualizarEstadoReservaAlojamiento({request.IdAlojamiento}, {request.NuevoEstado}, {request.IdCuarto}, {request.FechaIngreso}, {request.FechaSalida})"
                );

                // Obtener cliente que hizo la reserva (desde una vista o método)
                var reserva = await _context.ReservaInfo.FindAsync(request.IdAlojamiento);
                if (reserva != null)
                {
                    var idCliente = reserva.IdCliente;

                    await _notificacionService.CrearYEnviarNotificacion(
                        idCliente,
                        reserva.IdPension,
                        $"Tu reserva ha sido {request.NuevoEstado.ToLower()}.",
                        "estado-reserva"
                    );
                }

                return Ok(new { mensaje = "Estado de reserva actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cambiar estado de la reserva: {ex.Message}");
            }
        }


        [HttpGet("propietario/ReservasPendientePension/{idPension}")]
        public async Task<IActionResult> ObtenerReservasRecibidasPorPension(int idPension)
        {
            try
            {
                var reservas = await _context.HistorialPensions.FromSqlRaw("CALL ObtenerReservasRecibidasPorPension({0})", idPension).ToListAsync();

                if (reservas.Count == 0 || reservas == null)
                {
                    return NotFound("Esta pension no tiene reservas todavia");
                }
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las reserva: {ex.Message}");
            }

        }
        [HttpGet("propietario/HistorialClientesPension")]
        public async Task<IActionResult> HistorialdeClientesporPension(int idPension)
        {
            try
            {
                var historial = await _context.ReservaInfo.FromSqlInterpolated($"CALL ObtenerHistorialClientesPension({idPension})").ToListAsync();
                if (historial.Count == 0 || historial.IsNullOrEmpty())
                {
                    return NotFound("No hay registros todavia");
                }
                return Ok(historial);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, $"Error al obtener las reserva: {ex.Message}");
                }
            }
        }

        [HttpGet("Cliente/{idCliente}/PensionActual")]
        public async Task<IActionResult> ObtenerPensionActual(int idCliente)
        {
            try
            {
                var resultado = await _context.PensionC
                    .FromSqlRaw("CALL ObtenerPensionActualCliente({0})", idCliente)
                    .ToListAsync();

                if (resultado == null || resultado.Count == 0)
                    return NotFound("No estás actualmente alojado en ninguna pensión.");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, $"Error al obtener las reserva: {ex.Message}");
                }
            }
        }

        [HttpGet("cliente/{idCliente}/ReservasPendientes")]
        public async Task<IActionResult> ObtenerReservasPendientes(int idCliente)
        {
            try
            {
                var resultado = await _context.PensionC
                    .FromSqlRaw("CALL ObtenerReservasPendientesCliente({0})", idCliente)
                    .ToListAsync();

                if (resultado == null || resultado.Count == 0)
                    return NotFound("No tienes reservas pendientes.");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, $"Error al obtener las reserva: {ex.Message}");
                }
            }
        }
        [HttpGet("cliente/{idCliente}/HistorialPensiones")]
        public async Task<IActionResult> GetHistorial(int idCliente)
        {
            try
            {
                var resultado = await _context.PensionC
                    .FromSqlRaw("CALL ObtenerHistorialReservasCliente({0})", idCliente)
                    .ToListAsync();

                if (resultado == null || resultado.Count == 0)
                    return NotFound("No has pasado por una pension todavia.");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, $"Error al obtener las reserva: {ex.Message}");
                }
            }
        }


    }
}
