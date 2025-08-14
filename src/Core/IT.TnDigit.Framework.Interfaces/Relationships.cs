using System;

namespace IT.TnDigit.ORM.Interfaces
{
    public abstract class BaseRelation
    {
        public IFrameworkCollection collTemplate { get; set; }
        public IFilter filter { get; set; }

        public BaseRelation(IFrameworkCollection type, IFilter filter)
        {
            this.collTemplate = type;
            this.filter = filter;
        }
    }

    public class RelatedRecord : BaseRelation
    {
        public RelatedRecord(IFrameworkCollection type, IFilter filter) : base(type, filter) { }

        public IFrameworkObject data { get; set; }
    }

    public class RelatedCollection : BaseRelation
    {
        public String order { get; set; }

        public RelatedCollection(IFrameworkCollection type, IFilter filter, String order)
            : base(type, filter)
        {
            this.order = order;
        }

        public IFrameworkCollection data { get; set; }

    }
}
