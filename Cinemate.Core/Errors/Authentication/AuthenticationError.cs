using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Cinemate.Repository.Errors.Authentication
{
	public static class AuthenticationError
	{
		public static class UserErrors
		{
			public static readonly Error InvalidCredentails = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);
			public static readonly Error InvalidJwtToken = new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);
			public static readonly Error DublicatedEmail = new("User.DublicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);
			public static readonly Error InvalidCode = new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
			public static readonly Error DuplicatedConfirmation = new("User.DuplicatedInformation", "Email Already Confirmed", StatusCodes.Status400BadRequest);
			public static readonly Error EmailNotConfirmed = new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);
			public static readonly Error UserEmailNotFound = new("User.UserEmailNotFound", "User Email NotFound", StatusCodes.Status404NotFound);
			public static readonly Error UserNameNotFound = new("User.UserNameNotFound", "User Name NotFound", StatusCodes.Status404NotFound);
			public static readonly Error UserNotFound = new("User.UserNotFound", "User with this Id NotFound", StatusCodes.Status404NotFound);
			public static readonly Error FollowerNotFound = new("User.FollowerNotFound", "Follower with this Id NotFound", StatusCodes.Status404NotFound);
			public static readonly Error DisabledUser = new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);
			public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
			public static readonly Error EnableRecentActivity = new("User.EnableRecentActivity", "User Hide Recent Activity", StatusCodes.Status200OK);
			public static readonly Error FollowersHidden = new("User.FollowersHidden", "Followers list has been hidden by this user", StatusCodes.Status200OK);
			public static readonly Error FollowingHidden = new("User.FollowingHidden", "Following list has been hidden by this user", StatusCodes.Status200OK);
			public static readonly Error Unauthorized = new("User.Unauthorized", "Unauthorized access", StatusCodes.Status401Unauthorized);
			public static readonly Error FollowNotFound = new("User.FollowNotFound", "The follow relationship does not exist", StatusCodes.Status404NotFound);
			public static readonly Error NotificationGetFailed = new("Notification.GetFailed", "Failed to retrieve notifications: ", StatusCodes.Status500InternalServerError);
		}
	}
}
	