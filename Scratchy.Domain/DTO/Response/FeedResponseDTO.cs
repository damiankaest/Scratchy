using Scratchy.Domain.DB;

namespace Scratchy.Domain.DTO.Response
{
    public class FeedResponseDTO
    {
        //public List<FeedItem> listOfFeedItems { get; set; }
        public FeedResponseDTO(List<Scratch> listOfScratches)
        {
            foreach (var item in listOfScratches)
            {

            }
        }
    }
}
