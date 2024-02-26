using Domain.Common;
using Infrastructure;
using Infrastructure.Abstractions;
using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Configuration.GetValue("Provider", string.Empty);

builder.Services.AddDbContext<MySqlDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString(StaticConfiguration.MySql),
                new MySqlServerVersion(StaticConfiguration.MySqlVersion))
        );

builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString(StaticConfiguration.SqlServer))
);

builder.Services.AddTransient<IDbContextFactory>(_ => provider switch
{
    StaticConfiguration.MySql => new MySqlDbContextFactory(builder.Configuration),
    StaticConfiguration.SqlServer => new SqlServerDbContextFactory(builder.Configuration),
    _ => throw new Exception($"Unsupported provider: {provider}")
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Person API", Version = "v1" });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Person API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();