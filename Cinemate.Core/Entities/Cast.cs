using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class Cast
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public int? Gender { get; set; }
        public string? ProfilePath { get; set; }
		public string? Character { get; set; }
		public double? Popularity { get; set; }
        public string? KnownForDepartment { get; set; }
        public ICollection<UserCastFollow> FollowedByUsers { get; set; } = new HashSet<UserCastFollow>();
		public ICollection<CastMovie> CastMovies { get; set; } = new HashSet<CastMovie>();
	}
}
