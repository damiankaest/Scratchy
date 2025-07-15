using Scratchy.Domain.DTO.Response;

namespace Scratchy.Domain.DTO.Request
{
    public class CreateShowCaseRequestDto
    {
            public ShowCaseType ShowCaseType { get; set; }
            public string FirstPlaceEntityId { get; set; }
            public string SecondPlaceEntityId { get; set; }
            public string ThirdPlaceEntityId { get; set; }
    }
}
