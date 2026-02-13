using Minimal.API.Models;
using MiniValidation;

namespace Minimal.API.EndpointFilters;

/// <summary> Validates the annotations of the specified object in the filter context.</summary>
public class ValidateAnnotationsFilter : IEndpointFilter
{
    /// <summary>Validates the annotations of the specified object in the filter context.</summary>
    /// <param name="context">The context of the endpoint filter invocation, containing parameters and additional information.</param>
    /// <param name="next">The delegate to invoke the next filter in the pipeline.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> representing the result of the filter execution.
    /// If validation fails, it returns a validation problem result; otherwise, it proceeds to the next filter or endpoint.</returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dishForCreationDto = context.GetArgument<DishForCreationDto>(2);

        if (!MiniValidator.TryValidate(dishForCreationDto, out var validationErrors))
            return TypedResults.ValidationProblem(validationErrors);

        return await next(context);
    }
}