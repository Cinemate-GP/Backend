using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Recent_Activity
{
    public record UserRecentActivityResponse
    (
        string UserId,
        int TMDBId,
        string type,
        string id,
        string? PosterPath,
        string? Name,
        string? Description,
        DateTime? CreatedOn
    );
}
