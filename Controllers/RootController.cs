using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiUniRoom;
using ApiUniRoom.Models;

namespace ApiUniRoom.Controllers
{
    [ApiController]
    [Route("/")]
    public class RootController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ContextDB _db;
        public RootController(IConfiguration config, ContextDB db)
        {
            _config = config;
            _db = db;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API Pension en Render está funcionando 🚀");
        }

        [HttpGet("env")]
        public IActionResult GetEnv()
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];

            return Ok(new
            {
                ConnectionString = string.IsNullOrEmpty(connStr) ? "NULL" : "OK",
                JwtKey = string.IsNullOrEmpty(jwtKey) ? "NULL" : "OK",
                JwtIssuer = string.IsNullOrEmpty(jwtIssuer) ? "NULL" : "OK"
            });
        }

        [HttpGet("db")]
        public IActionResult TestDb()
        {
            try
            {
                var users = _db.Perfil.Take(1).ToList();
                return Ok("DB conectada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al conectar DB: " + ex.Message);
            }
        }

    }
}