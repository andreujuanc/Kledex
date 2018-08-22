using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Domain;

namespace OpenCqrs.Commands
{
    public interface ICommandHandlerWithDomainEventsAsync<in TCommand> where TCommand : IDomainCommand
    {
        Task<IEnumerable<IDomainEvent>> HandleAsync(TCommand command);
    }


    public interface ICommandHandlerWithDomainEventsAsyncWOOT<TCommand> 
            where TCommand : IDomainCommand<IAggregateRoot>
    {
        Task<IEnumerable<IDomainEvent>> HandleAsync(TCommand command);
    }
}
