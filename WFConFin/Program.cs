using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WFConFin.Data;
using WFConFin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Conexão com Postgres
var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgres");
builder.Services
    .AddDbContext<WFConFinContext>(x => x.UseNpgsql(connectionString));
//Fim

//Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
//Fim

//Configuração de Token de Autenticação
var chave = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Chave").Get<string>());

builder.Services.AddAuthentication(
    x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(
    x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(chave),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

builder.Services.AddSingleton<TokenService>();
//Fim

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Uso do CORS
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

//Uso do Token autenticação
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

