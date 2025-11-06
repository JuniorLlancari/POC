using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TipoCambio.Data;
using TipoCambio.Models;

namespace TipoCambio.Functions;

public class ListarTasaFunction
{
    private readonly ILogger<ListarTasaFunction> _logger;
    private readonly TipoCambioDbContext _dbContext;

    public ListarTasaFunction(ILogger<ListarTasaFunction> logger, TipoCambioDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Function("ListarTasaFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get",Route = "listar")] HttpRequest req)
    {
        var response = new BaseResponse<List<Models.TipoCambio>>();
        var listaTipoCambio = await _dbContext.TiposCambio.ToListAsync();
        response.Data = listaTipoCambio;
        response.Success = true;
        return new OkObjectResult(response);
    }
}