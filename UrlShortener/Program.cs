
using DeviceDetectorNET.Parser.Device;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Text;
using UrlShortener.BackgroundServices;
using UrlShortener.Repositories;
using UrlShortener.RepositoryContracts;
using UrlShortener.ServiceContracts;
using UrlShortener.Services;
using UrlShortener.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // Frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])) // Your secret key
    };
});

// SERVICES START
builder.Services.AddSingleton<IBackgroundAnalyticsQueue>( _ = new BackgroundAnalyticsQueue(capacity: 500) );
builder.Services.AddHostedService<QueuedWorker>();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))); // to get ready to use Sql Connection wherever needed

builder.Services.AddScoped<IUserRepository, UserRepository>();
//Whenever a class (like your controller) needs an IUserRepository, DI will create a new UserRepository and automatically pass it any required constructor parameters (like IDbConnection).
builder.Services.AddScoped<IRegisterService, RegisterService>();

builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddScoped<ICreationRepository, CreationRepository>();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IShortenService, ShortenService>();

builder.Services.AddScoped<IGeolocationService, GeolocationService>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

// SERVICES END

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();
});


builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactDev");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
