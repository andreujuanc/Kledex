using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenCqrs.Extensions;
using OpenCqrs.Store.Cosmos.Sql.Configuration;

namespace OpenCqrs.Store.Cosmos.Sql.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IOpenCqrsAppBuilder EnsureCosmosDbSqlDbCreated(this IOpenCqrsAppBuilder builder, IOptions<DomainDbConfiguration> settings)
        {
            var documentClient = builder.App.ApplicationServices.GetService<IDocumentClient>();

            CreateDatabaseIfNotExistsAsync(documentClient, settings.Value.DatabaseId).Wait();
            CreateCollectionIfNotExistsAsync(documentClient, settings.Value.DatabaseId, settings.Value.AggregateCollectionId, settings.Value.EnablePartitioningByType).Wait();
            CreateCollectionIfNotExistsAsync(documentClient, settings.Value.DatabaseId, settings.Value.CommandCollectionId, settings.Value.EnablePartitioningByType).Wait();
            CreateCollectionIfNotExistsAsync(documentClient, settings.Value.DatabaseId, settings.Value.EventCollectionId, settings.Value.EnablePartitioningByType).Wait();

            return builder;
        }

        private static async Task CreateDatabaseIfNotExistsAsync(IDocumentClient documentClient, string databaseId)
        {
            try
            {
                await documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDatabaseAsync(new Database { Id = databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync(IDocumentClient documentClient, string databaseId, string collectionId, bool enablePartitioningByType)
        {
            try
            {
                await documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var collection = new DocumentCollection { Id = collectionId };
                    var requestOptions = new RequestOptions { OfferThroughput = 1000 };

                    if (enablePartitioningByType)
                    {
                        collection.PartitionKey = GetPartitionKeyDefinition(enablePartitioningByType);
                    }

                    await documentClient.CreateDocumentCollectionAsync
                    (
                        UriFactory.CreateDatabaseUri(databaseId),
                        collection,
                        requestOptions
                    );
                }
                else
                {
                    throw;
                }
            }
        }

        private static PartitionKeyDefinition GetPartitionKeyDefinition(bool enablePartitioningByType)
        {
            var partitionKeyDefinition= new PartitionKeyDefinition();
            if(enablePartitioningByType)
                partitionKeyDefinition.Paths.Add("/type");

            return partitionKeyDefinition;
        }
    }
}