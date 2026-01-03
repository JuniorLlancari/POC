using Microsoft.Extensions.DependencyInjection;

namespace TipoCambio.Data;

public static class TipoCambioSeeder
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TipoCambioDbContext>();

        if (context.TiposCambio.Any())
            return;

        context.TiposCambio.AddRange(
            new Models.TipoCambio { Id = Guid.NewGuid(), MonedaOrigen = "USD", MonedaDestino = "PEN", ValorCambio = 3.80m },
            new Models.TipoCambio { Id = Guid.NewGuid(), MonedaOrigen = "EUR", MonedaDestino = "PEN", ValorCambio = 4.00m },
            new Models.TipoCambio { Id = Guid.NewGuid(), MonedaOrigen = "PEN", MonedaDestino = "USD", ValorCambio = 0.26m },
            new Models.TipoCambio { Id = Guid.NewGuid(), MonedaOrigen = "PEN", MonedaDestino = "EUR", ValorCambio = 0.25m }
        );

        context.SaveChanges();
    }
}
