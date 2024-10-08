namespace BasePoint.Core.Tests.Application.SampleUseCasesDtos
{
    public record UpdateShoppingCartItemInput
    {
        public Guid ShoppingCartItemId { get; set; }
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
