using BasePoint.Core.Domain.Enumerators;

namespace BasePoint.Core.Application.Dtos.Input
{
    public record SearchFilterInput
    {
        public FilterType FilterType { get; set; }

        public string FilterProperty { get; set; }

        public object FilterValue { get; set; }
    }
}