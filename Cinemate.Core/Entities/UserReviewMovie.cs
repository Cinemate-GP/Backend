using Cinemate.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{

    public class UserReviewMovie
    {
        public int ReviewId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int TMDBId { get; set; }
        public string ReviewBody { get; set; } = string.Empty;
        public DateTime ReviewedOn { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; } = null!;

        public Movie Movie { get; set; } = null!;
    }
}
