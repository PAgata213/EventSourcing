using ESProj.Domain.Repository;
using ESProj.Infrastructure.Repository;
using EventStore.Client;

using Microsoft.Extensions.DependencyInjection;

namespace ESProj.Infrastructure;
public static class DI
{
  public static void Register(this IServiceCollection services)
  {
    var settings = EventStoreClientSettings
    .Create("esdb://localhost:2113?tls=false&keepAliveTimeout=10000&keepAliveInterval=10000");
    var client = new EventStoreClient(settings);

    services
      .AddSingleton(client)
      .AddScoped<IEventRepository, EventRepository>()
      .AddScoped<IWarehouseProductRepository, WarehouseProductRepository>();
  }
}
