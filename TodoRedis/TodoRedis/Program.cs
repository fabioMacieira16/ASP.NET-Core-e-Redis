using TodoRedis.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TodoRedis.Infrastructure.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ToDoListDbContext>(o => o.UseInMemoryDatabase("TodoListDb"));

builder.Services.AddScoped<ICachingService, CachingService>();

builder.Services.AddStackExchangeRedisCache(o =>
{
    o.InstanceName = "intance";
    
    // rodando na porta do docker
    o.Configuration = "172.17.0.1:6379"; 

    //o.Configuration = "localhost:6379";
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
