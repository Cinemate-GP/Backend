using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Errors.Notification
{
	public static class NotificationError
	{
		public static class NotifyErrors
		{
			public static readonly Error NotificationNotFound = new("Notification.NotificationNotFound", "Notification Not Found", StatusCodes.Status404NotFound);
			public static readonly Error NotificationIsExsit = new("Notification.NotificationIsExsit", "Notification is already marked as read.", StatusCodes.Status409Conflict);
		}
	}
}
