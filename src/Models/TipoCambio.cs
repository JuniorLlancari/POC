namespace TipoCambio.Models;

public class TipoCambio
{
    public Guid Id { get; set; }
    public string MonedaOrigen { get; set; }
    public string MonedaDestino { get; set; }
    public decimal ValorCambio { get; set; }
}
