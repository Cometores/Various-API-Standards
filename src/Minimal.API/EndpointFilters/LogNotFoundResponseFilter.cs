using System.Net;

namespace Minimal.API.EndpointFilters;

/// <summary>Logs a message if the HTTP response status code is 404 (Not Found).</summary>
public class LogNotFoundResponseFilter : IEndpointFilter
{
    private readonly ILogger<LogNotFoundResponseFilter> _logger;

    public LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Processes an endpoint invocation and logs a message if the HTTP response status code is 404 (Not Found).
    /// </summary>
    /// <param name="context">The context of the endpoint filter invocation, providing access to request-specific data.</param>
    /// <param name="next">A delegate that executes the next filter or the endpoint handler in the pipeline.</param>
    /// <returns>A task that resolves to the result of the endpoint handler or the next filter in the pipeline.</returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        
        var actualResult = result is INestedHttpResult httpResult 
            ? httpResult.Result 
            : (IResult) result!;

        if ((actualResult as IStatusCodeHttpResult)?.StatusCode == (int)HttpStatusCode.NotFound)
        {
            _logger.LogInformation("Resource {RequestPath} was not found.", context.HttpContext.Request.Path);
        }

        return result;
    }
}