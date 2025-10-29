using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SalesApp;
using SalesApp.Data;
using SalesApp.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<JwtService>();   //Dependency Injection
builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var config = builder.Configuration;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<UserDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbcon")));
builder.Services.AddDbContext<CustomerDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbcon")));
builder.Services.AddDbContext<PartDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbcon")));
builder.Services.AddDbContext<OrderDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbcon")));
builder.Services.AddDbContext<CustomerAddressDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbcon")));


//CONFIGURE SERILOG TO LOG REQ AND RES TO FILE
//builder.Host.UseSerilog((context, services, configuration) => configuration
//.ReadFrom.Configuration(context.Configuration)
//.ReadFrom.Services(services)
//.Enrich.FromLogContext()
//.WriteTo.Console()
//.WriteTo.File("Logs/app-log.txt", rollingInterval: RollingInterval.Day));

//var logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .CreateLogger();

////MIDDLEWARE LOGGING REQ AND RES
//builder.Services.AddHttpLogging(options =>
//{
//    options.LoggingFields = HttpLoggingFields.ResponseBody;

//    //logger.Information("Configured HttpLogging with fields: {LoggingFields}", 
//    //    options.LoggingFields.ToString());
     
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//use middleware to log req and res
//app.UseHttpLogging();

//app.UseMiddleware();




app.Run();
