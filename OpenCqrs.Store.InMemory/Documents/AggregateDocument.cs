using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCQRS.Store.InMemory.Documents
{
    public class AggregateDocument
    {
        public AggregateDocument(Guid id, string type)
        {
            Id = id;
            Type = type;
        }

        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}
