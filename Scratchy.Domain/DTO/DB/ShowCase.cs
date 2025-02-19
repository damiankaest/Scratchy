using Scratchy.Domain.DTO.Response;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class ShowCase
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public ShowCaseType Type { get; set; }
        public int FirstPlaceEntityId { get; set; }
        public int SecondPlaceEntityId { get; set; }
        public int ThirdPlaceEntityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }

    }
}
