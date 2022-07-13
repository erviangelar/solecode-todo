using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoleCode.Api.Common;

namespace SoleCode.Api.Infrastructures;

public class FailureMiddleware
{
    private readonly RequestDelegate _next;

    public FailureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<FailureMiddleware> logger)
    {

        var currentBody = context.Response.Body;
        await using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;
        ErrorCode error = null;
        try
        {
            await _next(context);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            logger.LogError(exception, exception.Message);
            error = ErrorCodes.BadRequest; 
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                error = new ErrorCode(HttpStatusCode.BadRequest.ToString(), e.Message, HttpStatusCode.BadRequest);
            }
            else
            {
                error = new ErrorCode("500", "Something went wrong. Please try again in a few minutes or contact your support team", HttpStatusCode.InternalServerError);
            }
        }

        context.Response.Body = currentBody;
        memoryStream.Seek(0, SeekOrigin.Begin);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            var headerWwwAuth = context.Response.Headers["WWW-Authenticate"];
            if (headerWwwAuth.Count > 0)
            {

                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                error = new ErrorCode(ErrorCodes.InvalidToken.Code, headerWwwAuth[0], HttpStatusCode.Unauthorized);

            }

        }

        var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();
        if (error != null)
        {
            context.Response.StatusCode = (int) error.StatusCode;
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            return;
        }


        if (!string.IsNullOrEmpty(context.Response.ContentType))
        {

            if (context.Response.ContentType.Contains("image"))
            {
                var buffer = memoryStream.ToArray();
                await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
                return;
            }

            if (!context.Response.ContentType.Contains("application/json"))
            {
                await context.Response.WriteAsync(readToEnd);
                return;
            }
        }

        await context.Response.WriteAsync(readToEnd);
    }
}