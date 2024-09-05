using BasePoint.Core.Domain.Entities;
using BasePoint.Core.Domain.Entities.Interfaces;

namespace BasePoint.Core.Tests.Domain.Entities
{
    public class SampleEntityWithChilds : BaseEntity
    {
        public virtual string SampleCode { get; set; }
        public virtual string SampleName { get; set; }
        public virtual IEntityList<ChildClassListItem> Childs { get; set; }

        public SampleEntityWithChilds()
        {
            Childs = new EntityList<ChildClassListItem>();
        }

        public void AddChild(ChildClassListItem item)
        {
            Childs.Add(item);
        }
    }
}