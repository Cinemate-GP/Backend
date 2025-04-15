using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Like
{
    public record UserLikeMovieResponse
    {
       public string UserId {  get; init; }
       public int MovieId { get; init; }
      


    }
}
