using MediatR;

namespace Shared.Core.CQRS;

public interface ICommandHandler<in TCommand , TRes> : IRequestHandler<TCommand, TRes>
    where TCommand : ICommand<TRes>
    where TRes: notnull
{
    
}