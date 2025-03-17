using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Errors.Actor
{
    public static class ActorErrors
    {
		public static readonly Error ActorNotFound = new("Actor.NotFound", "No Actor was found with the given id", StatusCodes.Status404NotFound);
	}
}
