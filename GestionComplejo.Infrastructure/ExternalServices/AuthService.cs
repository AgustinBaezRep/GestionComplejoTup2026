using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;
using GestionComplejo.Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GestionComplejo.Infrastructure.ExternalServices
{
    public class AuthService : IAuthService
    {
        private readonly GestionComplejoDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(GestionComplejoDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public AuthResponse? SignUp(SignUpRequest request)
        {
            bool emailEnUso = _context.Clientes.Any(c => c.Email == request.Email)
                           || _context.Admins.Any(a => a.Email == request.Email);

            if (emailEnUso)
                return null;

            string contrasenaHasheada = BCrypt.Net.BCrypt.HashPassword(request.Contrasena);
            Guid nuevoId = Guid.NewGuid();
            string rol;

            if (request.Rol == "Admin")
            {
                var admin = new Admin
                {
                    Id = nuevoId,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Email = request.Email,
                    Contrasena = contrasenaHasheada,
                    Telefono = request.Telefono,
                    Cargo = request.Cargo ?? string.Empty
                };
                _context.Admins.Add(admin);
                rol = "Admin";
            }
            else
            {
                var cliente = new Cliente
                {
                    Id = nuevoId,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Email = request.Email,
                    Contrasena = contrasenaHasheada,
                    Telefono = request.Telefono
                };
                _context.Clientes.Add(cliente);
                rol = "Cliente";
            }

            _context.SaveChanges();

            return new AuthResponse
            {
                Token = GenerarToken(nuevoId, request.Email, rol),
                Rol = rol,
                UserId = nuevoId,
                Email = request.Email
            };
        }

        public AuthResponse? SignIn(SignInRequest request)
        {
            Guid userId;
            string rol;
            string contrasenaHasheada;

            var cliente = _context.Clientes.FirstOrDefault(c => c.Email == request.Email);
            if (cliente != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, cliente.Contrasena))
                    return null;

                userId = cliente.Id;
                contrasenaHasheada = cliente.Contrasena;
                rol = "Cliente";
            }
            else
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Email == request.Email);
                if (admin == null)
                    return null;

                if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, admin.Contrasena))
                    return null;

                userId = admin.Id;
                rol = "Admin";
            }

            return new AuthResponse
            {
                Token = GenerarToken(userId, request.Email, rol),
                Rol = rol,
                UserId = userId,
                Email = request.Email
            };
        }

        private string GenerarToken(Guid userId, string email, string rol)
        {
            string key = _configuration["Jwt:Key"]!;
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;
            int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
