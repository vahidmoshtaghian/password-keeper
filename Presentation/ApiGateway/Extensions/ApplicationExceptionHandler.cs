using Domain.Exceptions.Base;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace ApiGateway.Extensions;

public class ApplicationExceptionHandler
{
    public static ExceptionHandlerOptions Options => new()
    {
        ExceptionHandler = Handle
    };

    private static async Task Handle(HttpContext context)
    {
        string message = null;
        object data = null;

        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

        var error = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        if (error is not null)
        {
            var ex = error as ExceptionBase;
            if (ex is not null)
            {
                message = ex.Message;
                httpStatusCode = ex.StatusCode.Value;
                data = ex.Data;
            }
            else
            {
                message = error.Message;
                data = error.InnerException?.Message;
            }

            await WriteToResponseAsync();
        }

        async Task WriteToResponseAsync()
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException(
                    "Invalid operation.");

            var json = JsonConvert.SerializeObject(new
            {
                data,
                message
            });

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
    }
}