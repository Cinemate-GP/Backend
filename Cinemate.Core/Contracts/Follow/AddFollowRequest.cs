using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Follow
{
    public record AddFollowRequest
    {
        public string UserId { get; init; } 
        public string FollowId { get; init; } 
        public DateTime? FollowedOn { get; init; } = DateTime.UtcNow;
    }
}
