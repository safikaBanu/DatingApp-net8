using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
    IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {

            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddCors(); //angular cors
        services.AddScoped<ITokenService, TokenService>(); //token
        
        return services;
    }
//ctrl + shift + L => replace all at once
//shift +alt + F => format

}
