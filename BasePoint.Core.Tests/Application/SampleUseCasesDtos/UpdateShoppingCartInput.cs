using System.Text.Json.Serialization;

namespace BasePoint.Core.Tests.Application.SampleUseCasesDtos
{
    public record UpdateShoppingCartInput
    {
        [JsonIgnore]
        public Guid ShoppingCartId { get; set; }


        public Guid CustomerId { get; set; }
        public IEnumerable<UpdateShoppingCartItemInput> Items { get; set; }
    }
}
