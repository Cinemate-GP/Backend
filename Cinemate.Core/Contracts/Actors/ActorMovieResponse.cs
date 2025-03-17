using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Actors
{
    public record ActorMovieResponse
    (
        int Id,
        string Name,
		string? ProfilePath,
		string? KnownForDepartment,
        string? Character
	);
}
