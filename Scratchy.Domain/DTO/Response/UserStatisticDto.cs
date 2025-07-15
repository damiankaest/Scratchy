
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Models;

namespace Scratchy.Domain.DTO.Response
{
    public class UserStatisticDto
    {
        public UserStatisticDto()
        {
            
        }
        public UserStatisticDto(IEnumerable<ScratchDocument> listOfScratches)
        {
            AlbumCount = CountIndividualAlbums(listOfScratches);
            AvgRating = listOfScratches == null || !listOfScratches.Any() ? 0: CalculateAvgRating(listOfScratches);
            ScratchCount = listOfScratches == null || !listOfScratches.Any() ? 0 : CountScratches(listOfScratches);
        }

        private int CountScratches(IEnumerable<ScratchDocument> listOfScratches) => listOfScratches?.Count() ?? 0;

        private decimal CalculateAvgRating(IEnumerable<ScratchDocument> listOfScratches)=> 
            (decimal)listOfScratches
                .Where(scratch => scratch.Rating > 0) 
                .Average(scratch => scratch.Rating);
        

        private int CountIndividualAlbums(IEnumerable<ScratchDocument> listOfScratches) => 
            listOfScratches
                .Where(scratch => scratch.Album.AlbumId != null)
                .Select(scratch => scratch.Album.AlbumId) 
                .Distinct()                               
                .Count();

        public decimal AvgRating { get; set; }
        public int ScratchCount { get; set; }
        public int AlbumCount { get; set; }
    }
}
