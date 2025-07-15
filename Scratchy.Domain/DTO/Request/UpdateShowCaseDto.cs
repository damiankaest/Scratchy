using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.DTO.Request
{
    public class UpdateShowCaseDto
    {
        public ShowCaseType ShowCaseType { get; set; }
        public string ShowCaseId { get; set;}
        public string FirstPlaceEntityId { get; set; }
        public string SecondPlaceEntityId { get; set; }
        public string ThirdPlaceEntityId { get; set; }
    }
}
