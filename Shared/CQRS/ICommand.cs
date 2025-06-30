using MediatR;

namespace Shared.Core.CQRS;

public interface ICommand : IRequest<Unit>
{
}
public interface ICommand<out T> : IRequest<T>
{
}