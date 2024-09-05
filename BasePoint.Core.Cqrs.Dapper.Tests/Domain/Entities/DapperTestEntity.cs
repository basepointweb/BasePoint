
using BasePoint.Core.Domain.Entities;

namespace BasePoint.Core.Cqrs.Dapper.Tests.Domain.Entities
{
    public class DapperTestEntity : BaseEntity
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }

        public virtual EntityList<DapperChildEntityTest> Childs { get; protected set; }

        public DapperTestEntity()
        {
            Childs = new EntityList<DapperChildEntityTest>(this);
        }
    }
}