using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TipoCambio.Data;
using TipoCambio.Models;

namespace TipoCambio.Functions;

public class ActualizarTasaFunction
{
    private readonly ILogger<ActualizarTasaFunction> _logger;
    private readonly TipoCambioDbContext _dbContext;

    public ActualizarTasaFunction(ILogger<ActualizarTasaFunction> logger, TipoCambioDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

 
    [Function("ActualizarTasa")]
    public  async Task<IActionResult> Run(
     [HttpTrigger(AuthorizationLevel.Function, "post", Route = "actualizar")] HttpRequest req,
     ILogger log)
    {
        var response = new BaseResponse();
        try
        {

        var origen = req.Form["monedaOrigen"].ToString();
        var destino = req.Form["monedaDestino"].ToString();
        var valorStr = req.Form["valorCambio"].ToString();

        if (string.IsNullOrEmpty(origen))
        {
            response.ErrorMessage = "El parámetro 'monedaOrigen' es requerido.";
            return new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
        }

        if (string.IsNullOrEmpty(destino))
        {
            response.ErrorMessage = "El parámetro 'monedaDestino' es requerido.";
            return new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
        }



        if (!decimal.TryParse(valorStr, out decimal valorCambio))
        {
            response.Success = false;
            response.ErrorMessage = "Valor de cambio inválido";
            return new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
        }

        var cambio =  await _dbContext.TiposCambio.FirstOrDefaultAsync(t => t.MonedaOrigen == origen && t.MonedaDestino == destino);

        if (cambio == null)
        {
            _dbContext.TiposCambio.Add(new Models.TipoCambio
            {
                Id = Guid.NewGuid(),
                MonedaOrigen = origen,
                MonedaDestino = destino,
                ValorCambio = valorCambio
            });
        }
        else
        {
            cambio.ValorCambio = valorCambio;
        }

        _dbContext.SaveChanges();
        response.Success = true;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;

            response.Success = false;

        }
        return new ObjectResult(response);
    }
}