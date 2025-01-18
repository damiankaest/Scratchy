using System.ComponentModel.DataAnnotations.Schema;

namespace Scratchy.Domain.DTO.DB
{
    public class ScratchesTags
    {
        [ForeignKey(nameof(Scratch))]
        public int ScratchId { get; set; }
        public virtual Scratch Scratch { get; set; }

        [ForeignKey(nameof(Tag))]
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
