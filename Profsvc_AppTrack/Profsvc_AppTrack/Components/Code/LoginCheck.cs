#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           LoginCheck.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-01-2023 15:13
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Code;

/// <summary>
///     Provides methods for managing user login redirections.
/// </summary>
/// <remarks>
///     This static class includes methods for redirecting the user to the login page if they are not signed in,
///     and redirecting the user to a role-appropriate page if they are already signed in.
/// </remarks>
public static class LoginCheck
{
	/// <summary>
	///     Redirect the User to Login Page, from Inner Pages, if not signed in.
	/// </summary>
	/// <param name="navigationManager">The Navigation Manager object that is injected into the page.</param>
	/// <param name="localStorage">Blazored.LocalStorage object that is injected into the page.</param>
	/// <returns>A Task.</returns>
	public static async Task<LoginCooky> RedirectInner(this NavigationManager navigationManager, ILocalStorageService localStorage)
	{
		string _cookyString = await localStorage.GetItemAsync<string>("DeliciousCookie");
		LoginCooky _loginCooky = null;
		if (!_cookyString.NullOrWhiteSpace())
		{
			AESCryptography _aes = new();
			string _deserializedText = _aes.Decrypt(_cookyString);
			_loginCooky = JsonConvert.DeserializeObject<LoginCooky>(_deserializedText);
		}

		if (_loginCooky != null && !_loginCooky.UserID.NullOrWhiteSpace())
		{
			return _loginCooky;
		}

		navigationManager.NavigateTo(navigationManager.BaseUri, true);
		return null;
	}

	/// <summary>
	///     Redirect the User to Role-appropriate Page, from Login Page, if already signed in.
	/// </summary>
	/// <param name="navigationManager">The Navigation Manager object that is injected into the page.</param>
	/// <param name="blazoredStorage">Blazored.LocalStorage object that is injected into the page.</param>
	/// <param name="fromLogin">Is method called from Login screen.</param>
	/// <param name="page"></param>
	/// <returns>A Task.</returns>
	public static async Task RedirectLogin(this NavigationManager navigationManager, ILocalStorageService blazoredStorage, bool fromLogin = false, string page = "")
	{
		string _cookyString = await blazoredStorage.GetItemAsync<string>("DeliciousCookie");
		LoginCooky _loginCooky = null;
		if (_cookyString.NullOrWhiteSpace())
		{
			return;
		}

		AESCryptography _aes = new();
		string _deserializedText = _aes.Decrypt(_cookyString);
		_loginCooky = JsonConvert.DeserializeObject<LoginCooky>(_deserializedText);

		if (_loginCooky == null || _loginCooky.UserID.NullOrWhiteSpace())
		{
			//navigationManager.NavigateTo($"{navigationManager.BaseUri}", true);
			return;
		}

		if (fromLogin)
		{
			navigationManager.NavigateTo($"{navigationManager.BaseUri}home", true);
		}
	}
}