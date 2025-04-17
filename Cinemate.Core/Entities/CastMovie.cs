using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class CastMovie
    {
		public int CastId { get; set; }
		public int TmdbId { get; set; }
		public string? Role { get; set; }
		public string? Extra { get; set; }
        public Movie Movie { get; set; } = null!;
		public Cast Cast { get; set; } = null!;
	}
}
