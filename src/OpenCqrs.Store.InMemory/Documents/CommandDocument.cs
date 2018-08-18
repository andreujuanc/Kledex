using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCQRS.Store.InMemory.Documents
{
    public class CommandDocument
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }
        public string Source { get; set; }
    }
}
