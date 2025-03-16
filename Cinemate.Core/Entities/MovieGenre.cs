using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class MovieGenre
    {
		public int MovieId { get; set; }
		public int GenreId { get; set; }
		public Movie Movie { get; set; } = null!;
		public Genre Genre { get; set; } = null!;
	}
}
