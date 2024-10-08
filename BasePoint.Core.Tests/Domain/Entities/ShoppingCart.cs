using BasePoint.Core.Domain.Entities;
using BasePoint.Core.Domain.Entities.Interfaces;

namespace BasePoint.Core.Tests.Domain.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public IEntityList<ShoppingCartItem> Items { get; set; }

        public ShoppingCart()
        {
            Items = new EntityList<ShoppingCartItem>(this);
        }
    }
}
