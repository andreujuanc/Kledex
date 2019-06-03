using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using OpenCqrs.Examples.Web.Cosmos.Sql.Test.Fakes;
using OpenCqrs.Store.Cosmos.Sql;
using OpenCqrs.Store.Cosmos.Sql.Configuration;
using OpenCqrs.Store.Cosmos.Sql.Documents.Factories;
using OpenCqrs.Store.Cosmos.Sql.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OpenCqrs.Examples.Web.Cosmos.Sql.Test
{
    public class SqlAggregateRepositoryTest
    {
        [Fact]
        public async Task BasicCommands()
        {
            var options = new DomainDbConfiguration()
            {
                ServiceEndpoint = "https://localhost:8081",
                AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                DatabaseId = "DomainDb",
                AggregateCollectionId = "Aggregates",
                CommandCollectionId = "Commands",
                EventCollectionId = "Events",
                EnablePartitioningByType = true
            };

            var client = new DocumentClient(new Uri(options.ServiceEndpoint), options.AuthKey);
            var factory = new AggregateDocumentFactory();
            var repo = new AggregateRepository(client, Options.Create(options));
            //var store = new AggregateStore(repo, factory);

            var aggregate = new MyAggregate();

            var createdDoc = await repo.CreateDocumentAsync(factory.CreateAggregate<MyAggregate>(aggregate.Id));
            var retrievedDoc = await repo.GetDocumentAsync(aggregate.Id.ToString(), typeof(MyAggregate).AssemblyQualifiedName);
            Assert.NotNull(retrievedDoc);
            Assert.Equal(aggregate.Id, retrievedDoc.Id);
            //await store.SaveAggregateAsync<MyAggregate>(aggregate.Id);
            //var aggregates = await store.GetAggregatesAsync();
            //Assert.Contains(aggregates, x => x.Id == aggregate.Id);

        }
    }
}