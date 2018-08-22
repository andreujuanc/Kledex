using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class CreateProduct : DomainCommand<Product>
    {
        public string Title { get; set; }
    }
}
