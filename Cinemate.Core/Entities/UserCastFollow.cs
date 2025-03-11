using Cinemate.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class UserCastFollow
    {
        public string UserId { get; set; } = string.Empty;
        public int CastId { get; set; }
        public DateTime FollowDate { get; set; } = DateTime.UtcNow;

        public Cast Cast { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

    }
}
