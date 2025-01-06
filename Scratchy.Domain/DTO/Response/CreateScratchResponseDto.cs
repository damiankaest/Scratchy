using MongoDB.Bson.Serialization.Attributes;

namespace Scratchy.Domain.DTO.Response
{
    public class CreateScratchResponseDto
    {
        public CreateScratchResponseDto(bool _success, string _message)
        {
            Success = _success;
            Message = _message;
        }
        [BsonElement("success")]
        public bool Success { get; set; }
        [BsonElement("message")]
        public string Message { get; set; }
    }
}
