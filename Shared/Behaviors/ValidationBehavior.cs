using FluentValidation;
using MediatR;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using System.Reflection;
using Shared.DTOs.Common.Response;

namespace Shared.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures =
            validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            var errorMessage = string.Join("\n", failures.Select(f => f.ErrorMessage));
            var error = new Error(ErrorCode.ValidationError, errorMessage);

            // Handle both Result and Result<T> return types
            if (typeof(TResponse) == typeof(Result))
            {
                // Use a specific method signature to avoid ambiguity
                var failureMethod = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(m => m.Name == "Failure" && !m.IsGenericMethod &&
                           m.GetParameters().Length == 1 &&
                           m.GetParameters()[0].ParameterType == typeof(Error));

                return (TResponse)failureMethod.Invoke(null, new object[] { error })!;
            }
            else if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                // For Result<T>, need to find the specific generic method
                var resultType = typeof(TResponse);
                var dataType = resultType.GetGenericArguments()[0];

                var failureMethod = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(m => m.Name == "Failure" && m.IsGenericMethod &&
                           m.GetParameters().Length == 1 &&
                           m.GetParameters()[0].ParameterType == typeof(Error));

                var genericMethod = failureMethod.MakeGenericMethod(dataType);
                return (TResponse)genericMethod.Invoke(null, new object[] { error })!;
            }

            // If we get here, TResponse is neither Result nor Result<T>
            throw new InvalidOperationException($"Command handler must return Result or Result<T>, but returns {typeof(TResponse).Name}");
        }

        return await next();
    }
}