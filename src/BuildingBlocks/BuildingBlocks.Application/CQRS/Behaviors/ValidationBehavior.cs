using System.Reflection;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace BuildingBlocks.Application.CQRS.Behaviors;

// could also be done with message preprocessor
public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
    where TResponse : notnull
{
    public async ValueTask<TResponse> Handle(
        TRequest request,
        MessageHandlerDelegate<TRequest, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!validators.Any())
        {
            return await next(request, cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var resultErrors = validationResults.SelectMany(r => r.AsErrors()).ToList();
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count == 0)
        {
            return await next(request, cancellationToken);
        }

        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];
            MethodInfo invalidMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<>.Invalid), [typeof(List<ValidationError>)]);

            if (invalidMethod != null)
            {
                return (TResponse)invalidMethod.Invoke(null, [resultErrors]);
            }
        }
        else if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Invalid(resultErrors);
        }
        else
        {
            throw new ValidationException(failures);
        }

        return await next(request, cancellationToken);
    }
}
