using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_WatchList_Movie
{
    public record UserWatchListMovieResponse
    {
        public string UserId { get; init; }
        public int TMDBId { get; init; }

    }
}
