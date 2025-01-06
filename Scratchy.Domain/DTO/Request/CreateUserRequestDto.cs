using Newtonsoft.Json;

namespace Scratchy.Domain.DTO.Request
{
    public class CreateUserRequestDto
    {

        [JsonProperty("uid")]
        public string Uid { get; set; } = string.Empty;
        [JsonProperty("username")]
        public string UserName { get; set; } = string.Empty;
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
    }
}
