using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Recent_Activity
{
    public record UserRecentActivityResponse
    {
        public string UserId { get; init; }
        public int TMDBId { get; init; }
        public string Type { get; init; }
        public string Id { get; init; }
        public string? PosterPath { get; init; }
        public string? Name { get; init; }
        public int? Stars { get; init; } = 0;
        public string? Description { get; init; } = null;
        public DateTime? CreatedOn { get; init; }
    }
}
