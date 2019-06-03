﻿namespace OpenCqrs.Store.Cosmos.Sql.Configuration
{
    public class DomainDbConfiguration
    {
        public string ServiceEndpoint { get; set; }
        public string AuthKey { get; set; }
        public string DatabaseId { get; set; }
        public string AggregateCollectionId { get; set; }
        public string CommandCollectionId { get; set; }
        public string EventCollectionId { get; set; }
        public bool EnablePartitioningByType { get; set; }
    }
}
