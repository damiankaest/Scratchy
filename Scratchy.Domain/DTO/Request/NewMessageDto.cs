namespace Scratchy.Domain.DTO.Request
{
    public class NewMessageDto
    {
        public required string Type { get; set; }
        public required string Message { get; set; }
        public required string ReceiverId { get; set; }
        public required string SenderId { get; set; }
    }
}
