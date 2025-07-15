namespace Scratchy.Domain.DTO.Response
{
    public class CollectionResponseDto
    {
        public List<CollectionItem> Albums { get; set; }
    }

    public class CollectionItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string CoverUri { get; set; }
        public double Rating{ get; set; }
    }
}
