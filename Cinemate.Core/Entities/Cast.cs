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

        public double? popularity { get; set; }

        public string? knownForDepartment { get; set; }

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();

        public ICollection<UserCastFollow> FollowedByUsers { get; set; } = new HashSet<UserCastFollow>();
    }
}
