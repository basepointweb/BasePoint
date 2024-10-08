namespace BasePoint.Core.Tests.Application.SampleUseCasesDtos
{
    public record ShoppingCartOutput
    {
        public Guid ShoppingCartId { get; set; }
        public IEnumerable<ShoppingCartItemOutput> Items { get; set; }
    }
}
