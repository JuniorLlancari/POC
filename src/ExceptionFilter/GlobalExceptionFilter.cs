using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace TipoCambio.ExceptionFilter
{
    public class GlobalExceptionFilter : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error global capturado");

                var httpRequestData = await context.GetHttpRequestDataAsync();
                if (httpRequestData != null)
                {
                    var response = httpRequestData.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                    response.Headers.Add("Content-Type", "application/json");
                    await response.WriteAsJsonAsync(new { success = false, message = "Error interno del servidor" });
                    context.GetInvocationResult().Value = response;
                }
            }
        }
    }
}
