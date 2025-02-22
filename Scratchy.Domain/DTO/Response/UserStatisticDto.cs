
using Scratchy.Domain.DTO.DB;

namespace Scratchy.Domain.DTO.Response
{
    public class UserStatisticDto
    {
        public UserStatisticDto()
        {
            
        }
        public UserStatisticDto(IEnumerable<Scratch> listOfScratches)
        {
            AlbumCount = CountIndividualAlbums(listOfScratches);
            AvgRating = listOfScratches == null || !listOfScratches.Any() ? 0: CalculateAvgRating(listOfScratches);
            ScratchCount = listOfScratches == null || !listOfScratches.Any() ? 0 : CountScratches(listOfScratches);
        }

        private int CountScratches(IEnumerable<Scratch> listOfScratches) => listOfScratches?.Count() ?? 0;

        private decimal CalculateAvgRating(IEnumerable<Scratch> listOfScratches)=> 
            (decimal)listOfScratches
                .Where(scratch => scratch.Rating > 0) 
                .Average(scratch => scratch.Rating);
        

        private int CountIndividualAlbums(IEnumerable<Scratch> listOfScratches) => 
            listOfScratches
                .Where(scratch => scratch.AlbumId.HasValue)
                .Select(scratch => scratch.AlbumId.Value) 
                .Distinct()                               
                .Count();

        public decimal AvgRating { get; set; }
        public int ScratchCount { get; set; }
        public int AlbumCount { get; set; }
    }
}
