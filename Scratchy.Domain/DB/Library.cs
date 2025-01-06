namespace Scratchy.Domain.DB
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<LpRecord> Records { get; set; } = new List<LpRecord>();
        public string? Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
