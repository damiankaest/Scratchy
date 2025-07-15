using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class ShowCase : BaseDocument
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public ShowCaseType Type { get; set; }
        public string FirstPlaceEntityId { get; set; }
        public string SecondPlaceEntityId { get; set; }
        public string ThirdPlaceEntityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }

    }
}
