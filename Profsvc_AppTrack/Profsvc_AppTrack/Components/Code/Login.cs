#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Login.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-01-2023 15:12
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Code;

/// <summary>
/// </summary>
public class Login
{
	/// <summary>
	///     Gets or sets the password for the login. This property is required and its length should be between 3 and 16
	///     characters.
	/// </summary>
	[Required(ErrorMessage = "Password is required."),
	 StringLength(16, MinimumLength = 3, ErrorMessage = "Password should be between 3 and 16 characters.")]
	public string Password
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the user name for the login. This property is required and its length should be between 3 and 10
	///     characters.
	/// </summary>
	[Required(ErrorMessage = "User Name is required."),
	 StringLength(10, MinimumLength = 3, ErrorMessage = "User Name should be between 3 and 10 characters.")]
	public string UserName
	{
		get;
		set;
	}
}