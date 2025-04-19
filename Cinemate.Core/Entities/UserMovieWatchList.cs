using Cinemate.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class UserMovieWatchList
    {
        public string UserId { get; set; } = string.Empty;

        public int TMDBId { get; set; }
        public DateTime AddedOn { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public Movie Movie { get; set; } = null!;
    }
}
