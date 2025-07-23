using ApiUniRoom;
using ApiUniRoom.Models;
using ApiUniRoom.SignalHub;
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
    public class ComentariosController : ControllerBase
    {
        private readonly ContextDB _context;
        private readonly INotificacionService _notificacionService;
        public ComentariosController(ContextDB context, INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
            _context = context;
        }

        // Insertar comentario
        [HttpPost("InsertarComentario")]
        public async Task<IActionResult> InsertarComentario([FromBody] Comentario comentario)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL InsertarComentario({comentario.IdClienteFk}, {comentario.IdPensionFk}, {comentario.ComentarioTexto}, {comentario.Puntaje})"
                );

                var pension = await _context.Pensiones.FindAsync(comentario.IdPensionFk);
                if (pension != null)
                {
                    var idPropietario = pension.PropietarioFk;

                    await _notificacionService.CrearYEnviarNotificacion(
                        idPropietario,
                        comentario.IdPensionFk,
                        "Has recibido un nuevo comentario en una de tus pensiones.",
                        "comentario"
                    );
                }

                return Ok("Comentario insertado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar comentario: {ex.Message}");
            }
        }

        // Obtener todos los comentarios desde la vista
        [HttpGet("ListaComentarios")]
        public async Task<IActionResult> ListarComentarios(int idUser, string tipoUser)
        {
            try
            {
                string query;

                if (tipoUser.Equals("cliente", StringComparison.OrdinalIgnoreCase))
                {
                    query = "SELECT * FROM VistaComentarios WHERE idUsuario = {0}";
                }
                else
                {
                    query = "SELECT * FROM VistaComentarios WHERE idPropietario = {0}";
                }

                var comentarios = await _context.Set<ComentarioView>()
                    .FromSqlRaw(query, idUser)
                    .ToListAsync();

                return Ok(comentarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los comentarios: {ex.Message}");
            }
        }


        [HttpPut("ResponderComentario")]
        public async Task<IActionResult> ResponderComentario([FromBody] RespuestaComentarioDTO data)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL ResponderComentario({data.IdComentario}, {data.Respuesta})"
                );

                // Buscar el comentario original para saber quién es el cliente
                var comentario = await _context.Comentarios.FindAsync(data.IdComentario);
                if (comentario != null)
                {
                    var idCliente = comentario.IdClienteFk;
                    var idPension = comentario.IdPensionFk;

                    await _notificacionService.CrearYEnviarNotificacion(
                        idCliente,
                        idPension,
                        "Tu comentario ha sido respondido por el propietario.",
                        "respuesta_comentario"
                    );
                }

                return Ok("Respuesta registrada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al responder comentario: {ex.Message}");
            }
        }


    }
}
