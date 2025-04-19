using Cinemate.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class UserRateMovie
    {
        public string UserId { get; set; } = string.Empty;
        public int TMDBId { get; set; }
        public int? Stars { get; set; }

        public DateTime RatedOn { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; } = null!;

        public Movie Movie { get; set; } = null!;

    }
}
