using Newtonsoft.Json;
using OpenCqrs.Domain;
using OpenCQRS.Store.InMemory.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCqrs.Store.InMemory
{
    public class InMemoryStore :  ICommandStore, IEventStore
    {

        private List<CommandDocument> Commands { get; } = new List<CommandDocument>();
        private List<AggregateDocument> Aggregates { get; } = new List<AggregateDocument>();
        private List<EventDocument> Events { get; } = new List<EventDocument>();
        private readonly IVersionService VersionService;

        public InMemoryStore(IVersionService versionService)
        {
            VersionService = versionService;
        }

        public IEnumerable<DomainCommand> GetCommands(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DomainCommand>> GetCommandsAsync(Guid aggregateId)
        {
            await Task.CompletedTask;
            var result = new List<DomainCommand>();

            var commands = Commands.Where(x => x.AggregateId == aggregateId);

            foreach (var command in commands)
            {
                var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                result.Add((DomainCommand)domainCommand);
            }

            return result;
        }

        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException();
        }

        public async Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            await Task.CompletedTask;

            EnsureAggregateExists<TAggregate>(command.AggregateRootId);

            var commandDocument = new CommandDocument()
            {
                Id = Guid.NewGuid(),
                AggregateId = command.AggregateRootId,
                Type = command.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(command),
                TimeStamp = command.TimeStamp,
                UserId = command.UserId,
                Source = command.Source
            };
            Commands.Add(commandDocument);
        }

        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            await Task.CompletedTask;
            var result = new List<DomainEvent>();

            var events = Events.Where(x => x.AggregateId == aggregateId);

            foreach (var @event in events)
            {
                var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
                result.Add((DomainEvent)domainEvent);
            }

            return result;
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion = null) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException();
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion = null) where TAggregate : IAggregateRoot
        {
            await Task.CompletedTask;
            EnsureAggregateExists<TAggregate>(@event.AggregateRootId);

            var currentVersion = Events.Where(x => x.AggregateId == @event.AggregateRootId).Count();
            var nextVersion = VersionService.GetNextVersion(@event.AggregateRootId, currentVersion, expectedVersion);

            var eventDocument = new EventDocument()
            {
                Id = @event.Id,
                AggregateId = @event.AggregateRootId,
                CommandId = @event.CommandId,
                Sequence = nextVersion,
                Type = @event.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(@event),
                TimeStamp = @event.TimeStamp,
                UserId = @event.UserId,
                Source = @event.Source
            };
            Events.Add(eventDocument);
        }

        private void EnsureAggregateExists<TAggregate>(Guid AggregateRootId) where TAggregate : IAggregateRoot
        {
            var aggregate = Aggregates.Where(x => x.Id == AggregateRootId).FirstOrDefault();
            if (aggregate == null)
            {
                var aggregateDocument = new AggregateDocument(id: AggregateRootId, type: typeof(TAggregate).AssemblyQualifiedName);
                Aggregates.Add(aggregateDocument);
            }
        }
    }
}
