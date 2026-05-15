using System.Text;
using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Services;
using GestionComplejo.Infrastructure.ExternalServices;
using GestionComplejo.Infrastructure.Persistance;
using GestionComplejo.Infrastructure.Persistance.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresá el token JWT. No hace falta escribir 'Bearer', Swagger lo agrega solo."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", document), [] }
    });
});

builder.Services.AddDbContext<GestionComplejoDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("GestionComplejoConnectionString")));

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICanchaRepository, CanchaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IVestuarioRepository, VestuarioRepository>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();

builder.Services.AddScoped<ICanchaService, CanchaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IVestuarioService, VestuarioService>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
