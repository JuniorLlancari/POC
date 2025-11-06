using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TipoCambio.Data;
using TipoCambio.Models;
using TipoCambio.Services;

namespace TipoCambio.Functions;

public class ConvertirMontoFunction
{
    private readonly ILogger<ConvertirMontoFunction> _logger;
    private readonly JwtService _jwtService;
    private readonly TipoCambioDbContext _dbContext;

    public ConvertirMontoFunction(
        ILogger<ConvertirMontoFunction> logger,
        JwtService jwtService, 
        TipoCambioDbContext dbContext
        )
    {
        _logger = logger;
        _jwtService = jwtService;
        _dbContext = dbContext;
    }

    [Function("ConversionFunction")]
    public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "convertir")] HttpRequest req,
    ILogger log)
    {
        var response = new BaseResponse<ResponseConversion>();

        try
        {
            #region Validar Token

            var authHeader = req.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                response.Success = false;
                response.ErrorMessage = "Usuario no Autorizado";
                return new ObjectResult(response) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var principal = _jwtService.ValidarToken(token);
            if (principal == null)
            {
                response.Success = false;
                response.ErrorMessage = "Usuario no Autorizado";
                return new ObjectResult(response) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            #endregion


            var origen = req.Form["monedaOrigen"].ToString();
            var destino = req.Form["monedaDestino"].ToString();
            var montostr = req.Form["monto"].ToString();

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

            if (!decimal.TryParse(montostr, out decimal monto))
            {
                response.Success = false;
                response.ErrorMessage = "El parámetro 'monto' es inválido";
                return new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
            }




            var cambio = _dbContext.TiposCambio.FirstOrDefault(t => t.MonedaOrigen == origen && t.MonedaDestino == destino);
            if (cambio == null)
            {
                response.Success = false;
                response.ErrorMessage = "Tipo de cambio no registrado";
                return new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
            }

            response.Data = new ResponseConversion
            {
                Monto = monto,
                MonedaOrigen = origen,
                MonedaDestino = destino,
                TipoCambio = cambio.ValorCambio,
                MontoConTipoCambio = monto * cambio.ValorCambio
            };

            response.Success = true;
        }
        catch (Exception ex )
        {
            response.ErrorMessage = ex.Message;

            response.Success = false;
        }





        return new OkObjectResult(response);
    }
}