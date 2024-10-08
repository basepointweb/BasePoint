using BasePoint.Core.Domain.Entities;

namespace BasePoint.Core.Tests.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Code { get; set; }

        public decimal Price { get; set; }
    }
}
