using ApiUniRoom.Custom;
using ApiUniRoom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NuGet.Common;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace ApiUniRoom.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly Utilidades _utilidad;
        private readonly ContextDB _contextdb;
        public AccessController(Utilidades utilidad, ContextDB context)
        {
            _utilidad = utilidad;
            _contextdb = context;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<RegistroResponse>> RegistrarUsuario([FromBody] RegistroRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Campos Incorrectos");

            // Encriptar la contraseña
            string hashPassword = _utilidad.encriptarSHA256(request.Contrasena);

            int idNuevoUsuario = 0;

            var conn = _contextdb.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "InsertarUsuario";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@pNombres", request.Nombres));
            cmd.Parameters.Add(new MySqlParameter("@pApellidos", request.Apellidos));
            cmd.Parameters.Add(new MySqlParameter("@pSexo", request.Sexo));
            cmd.Parameters.Add(new MySqlParameter("@pEdad", request.Edad));
            cmd.Parameters.Add(new MySqlParameter("@pCorreo", request.Correo));
            cmd.Parameters.Add(new MySqlParameter("@pContrasena", hashPassword));
            cmd.Parameters.Add(new MySqlParameter("@pTelefono", request.Telefono));
            cmd.Parameters.Add(new MySqlParameter("@pIdTipo_User_fk", request.IdTipo_User_fk));
            cmd.Parameters.Add(new MySqlParameter("@pIdCiudad_fk", request.IdCiudad_fk));

            var outputParam = new MySqlParameter("@pIdNuevoUsuario", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            await cmd.ExecuteNonQueryAsync();

            idNuevoUsuario = (int)(outputParam.Value ?? 0);

            return Ok(new RegistroResponse
            {
                IdNuevoUsuario = idNuevoUsuario,
                Mensaje = "Usuario registrado correctamente."
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginController request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string hashPassword = _utilidad.encriptarSHA256(request.Contrasena);

            var conn = _contextdb.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "LoginUsuario";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@pCorreo", request.Correo));
            cmd.Parameters.Add(new MySqlParameter("@pContrasena", hashPassword));

            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows)
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });

            await reader.ReadAsync();

            var loginUser = new LoginResponse
            {
                IdUsuarios = reader.GetInt32("idUsuarios"),
                Nombres = reader.GetString("Nombres"),
                Apellidos = reader.GetString("Apellidos"),
                Correo = reader.GetString("Correo"),
                TipoUsuario = reader.GetString("TipoUsuario")
            };

            loginUser.Token = _utilidad.generarJWT(loginUser);

            return Ok(loginUser);
        }


    }


}
