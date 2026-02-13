using Microsoft.AspNetCore.Mvc;

namespace Minimal.API.EndpointFilters;

/// <summary> Represents an endpoint filter that restricts operations on a dish based on its guid.</summary>
public class DishIsLockedFilter : IEndpointFilter
{
    private readonly Guid _lockedDishId;

    public DishIsLockedFilter(Guid lockedDishId)
    {
        _lockedDishId = lockedDishId;
    }

    /// <summary>
    /// Invokes the endpoint filter asynchronously and determines whether the dish is locked based on its identifier.
    /// If the dish is locked, it returns a problem result; otherwise, it executes the next filter in the pipeline.
    /// </summary>
    /// <param name="context">The context of the current endpoint invocation, providing access to HTTP request and arguments.</param>
    /// <param name="next">The next delegate in the endpoint pipeline to be invoked if the current filter does not short-circuit.</param>
    /// <returns>A task that represents the asynchronous operation, returning the result of the filter or a problem response if the dish is locked.</returns>
    /// <exception cref="NotSupportedException">Thrown when this filter does not support the HTTP method in the request.</exception>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dishId = context.HttpContext.Request.Method switch
        {
            "PUT" => context.GetArgument<Guid>(2),
            "DELETE" => context.GetArgument<Guid>(1),
            _ => throw new NotSupportedException("This filter is not supported for this scenario.")
        };

        if (dishId == _lockedDishId)
        {
            return TypedResults.Problem(new ProblemDetails
            {
                Status = 400,
                Title = "Dish is perfect and cannot be changed.",
                Detail = "You cannot update or delete perfection."
            });
        }
        
        // invoke the next filter
        var result = await next.Invoke(context);
        return result;
    }
}