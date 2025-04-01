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
        [JsonProperty("profileImage")]
        public string? ProfileImage { get; set; } // Base64-kodiertes Bild
        [JsonProperty("selectedGenres")]
        public List<string>? SelectedGenres { get; set; } // Base64-kodiertes Bild

    }
}

