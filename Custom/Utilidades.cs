using ApiUniRoom.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiUniRoom.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _config;
        public Utilidades(IConfiguration config)
        {
            _config = config;
        }

        public string encriptarSHA256(string input)
        {
            using (SHA256 SHA = SHA256.Create())
            {
                byte[] bytes = SHA.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public string generarJWT(LoginResponse user)
        {
            var userClaims = new[]
   {
        new Claim(ClaimTypes.NameIdentifier, user.IdUsuarios.ToString()),
        new Claim(ClaimTypes.Name, user.Nombres),
        new Claim(ClaimTypes.Email, user.Correo),
        new Claim(ClaimTypes.Role, user.TipoUsuario) // útil para control de roles
    };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

    }
}
