using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.DTO.Request
{
    public class UpdateShowCaseDto
    {
        public ShowCaseType ShowCaseType { get; set; }
        public int ShowCaseId { get; set;}
        public int FirstPlaceEntityId { get; set; }
        public int SecondPlaceEntityId { get; set; }
        public int ThirdPlaceEntityId { get; set; }
    }
}
