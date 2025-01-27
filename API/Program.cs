using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

/* moved to [Application service extension - application]
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>{

opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(); //angular cors
builder.Services.AddScoped<ITokenService, TokenService>(); //token
*/

builder.Services.AddApplicationServices(builder.Configuration);

/* moved to [Application service extension - identity]
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => {
    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token not found");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey=true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };

});
*/
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();
app.UseCors(x => x.AllowAnyHeader().AllowAnyHeader().WithOrigins("http://localhost:4200","https://localhost:4200")); // added middleware for angular cors

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
