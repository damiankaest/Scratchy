using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratchy.Domain.DTO
{
    public class MediaDataResponse
    {
        public MediaDataResponse() { }
        public string title { get; set; }
        public string description { get; set; }
        public string spotifyLink { get; set; }
        public string image { get; set; }

    }
}
