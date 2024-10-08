using BasePoint.Core.Domain.Entities;

namespace BasePoint.Core.Tests.Domain.Entities
{
    public class ShoppingCartItem : BaseEntity
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}


