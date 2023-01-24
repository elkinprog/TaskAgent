using Aplicacion.Services;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Persistencia.Context;



var builder = WebApplication.CreateBuilder(args);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();



try
{
    logger.Debug("Aplicación iniciada...");

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    DotNetEnv.Env.Load();

    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));



    builder.Services.AddCors(opt => {
        opt.AddPolicy(name: myAllowSpecificOrigins,
            builder => {
                builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });


    });


    var connectionstring = Environment.GetEnvironmentVariable("CONNECTION_STRING");


    builder.Services.AddDbContext<SqlLIteContext>(options =>
                           options.UseSqlite(connectionstring),
                ServiceLifetime.Transient);

    var app = builder.Build();



    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<SqlLIteContext>();
            context.Database.Migrate();
        }
        catch (Exception e)
        {
            var loggin = services.GetRequiredService<ILogger<Program>>();
            loggin.LogError(e, "Ocurrio un eror en la migración");
        }
    }



    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Excepción durante la ejecución");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}



