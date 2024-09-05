namespace BasePoint.Core.Application.Dtos.Input
{
    public record GetPaginatedResultsInput
    {
        public IList<SearchFilterInput> Filters { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
    }
}