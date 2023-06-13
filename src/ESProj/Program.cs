using ESProj.Application.API;
using ESProj.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Register();
builder.Services.AddMediatR(o => 
{
  o.RegisterServicesFromAssembly(typeof(ESProj.Domain.DI).Assembly);
});
builder.Services.AddAutoMapper(typeof(ESProj.Infrastructure.DI).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapWarehouseProductEndpoints();

//app.UseHttpsRedirection();

app.Run();
