using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DTO
{
    public class CreateMediaDataDTO
    {
        public float imdbRating { get; set; } = 0f;
        public string spotifyUrl { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
}
