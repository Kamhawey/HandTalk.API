using MediatR;

namespace Shared.Core.CQRS;

public interface IQuery<out T> :IRequest<T>
{
}