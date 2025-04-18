using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Common
{
	public record SearchResponse
	(
		string Id,
		string Name,
		string Poster,
		string Type
	);
}
