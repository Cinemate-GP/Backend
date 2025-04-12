using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Rate_Movie
{
    public record UserRateMovieResponse
    {
        public string UserId { get; init; } = string.Empty;
        public int MovieId { get; init; }
        public int? Stars { get; init; } = 0;




    }
}
