using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TipoCambio.Data;
using TipoCambio.Models;
using TipoCambio.Services;

namespace TipoCambio.Functions;

public class GenerarTokenFunction
{
    private readonly ILogger<GenerarTokenFunction> _logger;
    private readonly JwtService _jwtService;

    public GenerarTokenFunction(ILogger<GenerarTokenFunction> logger, JwtService jwtService)
    {
        _logger = logger;
        _jwtService = jwtService;


    }
    



    [Function("TokenFunctionFunction")]
     public  IActionResult Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "token/generar")] HttpRequest req)
    {
        var response = new BaseResponse<string>();

        try
        {
            var usuario = req.Form["usuario"].ToString();
            if (string.IsNullOrEmpty(usuario))
                return new BadRequestObjectResult("Usuario requerido");

            var token = _jwtService.GenerarToken(usuario);
            response.Data = token;
            response.Success = true;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
        }



        return new OkObjectResult(response);
    }

}