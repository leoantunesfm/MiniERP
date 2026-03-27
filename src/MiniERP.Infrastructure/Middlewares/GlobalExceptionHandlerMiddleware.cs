using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniERP.Domain.Exceptions;

namespace MiniERP.Infrastructure.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu uma exceção durante o processamento da requisição: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case DomainException domainEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Erro de Validação de Domínio";
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Detail = domainEx.Message;
                break;

            case UnauthorizedAccessException unauthEx:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                problemDetails.Title = "Acesso Não Autorizado";
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Detail = unauthEx.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "Erro Interno do Servidor";
                problemDetails.Status = context.Response.StatusCode;
                
                problemDetails.Detail = "Ocorreu um erro inesperado no sistema. Tente novamente mais tarde.";
                break;
        }

        var result = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(result);
    }
}