using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Common
{
	public record RequestFilters
	{
		public int PageNumber { get; init; } = 1;
		public int PageSize { get; init; } = 10;
		public string? SearchValue { get; init; }
		public string? Gener { get; init; }
		public string? Year { get; init; }
		public string? MPA { get; init; }
		public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
	}
	public enum SortDirection
	{
		Ascending,
		Descending
	}
}
