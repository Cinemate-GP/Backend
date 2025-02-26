using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Abstractions.Consts
{
	public static class DefaultRoles
	{
		public const string Admin = nameof(Admin);
		public const string AdminRoleId = "e1940bc8-a54c-494d-9286-a585466c73f0";
		public const string AdminRoleConcurrencyStamp = "f51e5a91-bced-49c2-8b86-c2e170c0846c";


		public const string Member = nameof(Member);
		public const string MemberRoleId = "45781365-655c-42b4-aae7-897e4ea7f834";
		public const string MemberRoleConcurrencyStamp = "86b1eaf4-1099-4170-86d5-b30162221da9";
	}
}
