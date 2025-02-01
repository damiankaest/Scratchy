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
        public int firstPlaceEntityId { get; set; }
        public int secondPlaceEntityId { get; set; }
        public int thirdPlaceEntityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }

    }
}
