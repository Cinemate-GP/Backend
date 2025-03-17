using Cinemate.Core.Contracts.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Service_Contract
{
    public interface IActorService
    {
		Task<Result<ActorDetailsResponse>> GetActorDetailsAsync(int actorid, CancellationToken cancellationToken = default);

	}
}
