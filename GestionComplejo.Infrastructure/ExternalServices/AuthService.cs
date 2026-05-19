using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Exceptions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;
using GestionComplejo.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
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

        public AuthResponse SignUp(SignUpRequest request)
        {
            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidationException($"El email '{request.Email}' no tiene un formato válido.");

            bool emailEnUso = _context.Clientes.Any(c => c.Email == request.Email)
                           || _context.Admins.Any(a => a.Email == request.Email);

            if (emailEnUso)
                throw new ConflictException($"El email '{request.Email}' ya está registrado.");

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

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("Error al guardar el usuario en la base de datos.", ex);
            }

            return new AuthResponse
            {
                Token = GenerarToken(nuevoId, request.Email, rol),
                Rol = rol,
                UserId = nuevoId,
                Email = request.Email
            };
        }

        public AuthResponse SignIn(SignInRequest request)
        {
            Guid userId;
            string rol;

            var cliente = _context.Clientes.FirstOrDefault(c => c.Email == request.Email);
            if (cliente != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, cliente.Contrasena))
                    throw new UnauthorizedException("Credenciales incorrectas.");

                userId = cliente.Id;
                rol = "Cliente";
            }
            else
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Email == request.Email);

                if (admin == null)
                    throw new UnauthorizedException("Credenciales incorrectas.");

                if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, admin.Contrasena))
                    throw new UnauthorizedException("Credenciales incorrectas.");

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
