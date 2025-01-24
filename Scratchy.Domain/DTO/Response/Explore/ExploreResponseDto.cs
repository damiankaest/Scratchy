using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.DTO.Response.Explore
{
    public class ExploreResponseDto
    {

        public List<ExploreAlbumDto> Albums { get; set; }
        public List<ExploreUserDto> Users { get; set; }
        public List<ExploreArtistsDto> Artists { get; set; }


    }
}
