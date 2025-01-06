namespace Scratchy.Domain.DTO.Request
{
    public class AcceptFriendRequestDto
    {
        public string RequestId { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;
        public AcceptFriendRequestDto() { }

    }
}
