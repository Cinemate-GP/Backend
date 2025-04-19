using Cinemate.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    //[PrimaryKey(nameof(UserId), nameof(MovieId))]
    public class UserLikeMovie
    {
        public string UserId { get; set; } = string.Empty;
        public int TMDBId { get; set; }
        public DateTime? LikedOn { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; } = null!;

        public Movie Movie { get; set; } = null!;

    }
}
