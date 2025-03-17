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
		public int Tmdb_Id { get; set; }
		public Movie Movie { get; set; } = null!;
		public Cast Cast { get; set; } = null!;
	}
}
