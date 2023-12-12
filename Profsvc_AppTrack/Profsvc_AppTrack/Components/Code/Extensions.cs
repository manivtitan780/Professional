#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Extensions.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     08-31-2023 19:16
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Code;

/// <summary>
///     Provides extension methods for various types used in the application.
/// </summary>
/// <remarks>
///     This static class contains methods that extend the functionality of several types, including IJSRuntime,
///     LoginCooky, and others.
///     These methods provide additional operations such as displaying a confirmation dialog, checking if a user has an
///     Administrator role, and fetching user rights based on roles.
/// </remarks>
public static class Extensions
{
	/// <summary>
	///     Displays a confirmation dialog with a specified message using the JavaScript runtime.
	/// </summary>
	/// <param name="jsRuntime">The JavaScript runtime instance.</param>
	/// <param name="message">The message to display in the confirmation dialog.</param>
	/// <returns>
	///     A ValueTask that completes when the confirmation dialog is closed, yielding a boolean value that indicates
	///     whether the user confirmed the message.
	/// </returns>
	public static async ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message) => await jsRuntime.InvokeAsync<bool>("confirm", message);

	/// <summary>
	///     Retrieves the user rights associated with the given login cookie and roles.
	/// </summary>
	/// <param name="loginCooky">The login cookie of the user.</param>
	/// <param name="roles">The roles to check against the login cookie.</param>
	/// <returns>
	///     A UserRights object representing the rights of the user based on their role.
	/// </returns>
	/// <remarks>
	///     This extension method is used to fetch the rights of a user based on their role.
	///     It iterates over the provided roles, checks if the role ID matches with the role ID in the login cookie,
	///     and if it does, assigns the corresponding rights to the UserRights object.
	/// </remarks>
	public static UserRights GetUserRights(this LoginCooky loginCooky, IEnumerable<Role> roles)
	{
		UserRights _returnValue = new();
		foreach (Role _role in roles.Where(role => role.ID == loginCooky.RoleID))
		{
			_returnValue.ViewCandidate = _role.ViewCandidate;
			_returnValue.ViewRequisition = _role.ViewRequisition;
			_returnValue.ViewCompany = _role.ViewClients;
			_returnValue.EditCandidate = _role.EditCandidate;
			_returnValue.EditRequisition = _role.EditRequisition;
			_returnValue.EditCompany = _role.EditClients;
			_returnValue.ChangeCandidateStatus = _role.ChangeCandidateStatus;
			_returnValue.ChangeRequisitionStatus = _role.ChangeRequisitionStatus;
			_returnValue.SendEmailCandidate = _role.SendEmailCandidate;
			_returnValue.ForwardResume = _role.ForwardResume;
			_returnValue.DownloadResume = _role.DownloadResume;
			_returnValue.SubmitCandidate = _role.SubmitCandidate;
		}

		return _returnValue;
	}

	/// <summary>
	///     Checks if the user has an Administrator role.
	/// </summary>
	/// <param name="loginCooky">The user's login information.</param>
	/// <returns>
	///     Returns true if the user's role is "AD" (Administrator), otherwise false.
	/// </returns>
	public static bool IsAdmin(this LoginCooky loginCooky) => loginCooky is {RoleID: "AD"};
}