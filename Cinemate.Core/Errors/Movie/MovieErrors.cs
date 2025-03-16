using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Errors.Movie
{
	public static class MovieErrors
    {
		public static readonly Error MovieNotFound = new("Movie.NotFound", "No Movie was found with the given id", StatusCodes.Status404NotFound);
	}
}
