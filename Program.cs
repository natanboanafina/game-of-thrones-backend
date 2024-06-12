using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ICharacters, Characters>();

// Configura o contexto do Entity Framework para usar o PostgreSQL
builder.Services.AddDbContext<GameOfThronesContext>(options =>
{
    DbContextOptionsBuilder dbContextOptionsBuilder = options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GameOfThronesContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IFileService, FileService>();

// Configuração do JWT
// ?? throw new ... funciona da mesma forma que uma validação com if
var key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY environment variable is not set.");
var keyBytes = Encoding.ASCII.GetBytes(key);

// Adicionando serviços ao container 
// Configura a autenticação JWT, especificando como os tokens devem ser validados
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "issuer",
        ValidAudience = "audience",
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configura o pipeline de processamento de solicitações HTTP, incluindo autenticação e autorização
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();