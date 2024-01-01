#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Login.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     1-1-2024 20:52
// *****************************************/

#endregion

//using Profsvc_AppTrack.Components.Code;

#region Using

using AESCryptography = ProfSvc_Classes.AESCryptography;

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     Represents the Login page in the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     This class is responsible for handling user login operations. It includes properties for managing the login model,
///     the edit context, the IP address, local storage, and the login form. It also includes methods for handling the
///     rendering and initialization of the page.
/// </remarks>
public partial class Login
{
	private readonly SemaphoreSlim _semaphoreMainPage = new(1, 1);

	/// <summary>
	///     Gets or sets the EditContext for the Login page.
	/// </summary>
	/// <value>
	///     The EditContext is used for data validation and user interaction within the form on the Login page.
	/// </value>
	private EditContext EditContext
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the local storage service used for storing user session data.
	/// </summary>
	/// <value>
	///     The local storage service.
	/// </value>
	/// <remarks>
	///     This property is used to manage the local storage of the user's browser. It is used to store session data such as
	///     login cookies.
	/// </remarks>
	[Inject]
	private ILocalStorageService LocalStorage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ILogger instance used for logging in the Candidate class.
	/// </summary>
	/// <remarks>
	///     This property is used to log information about the execution of tasks and methods within the Candidate class.
	///     It is injected at runtime by the dependency injection system.
	/// </remarks>
	[Inject]
	private ILogger<Login> Logger
	{
		get;
		set;
	}

	//[Inject]
	//private ProtectedLocalStorage LocalStorage
	//{
	//    get;
	//    set;
	//}

	/// <summary>
	///     Gets or sets the EditForm for the login page.
	/// </summary>
	/// <value>
	///     The EditForm used for the login page.
	/// </value>
	/// <remarks>
	///     This property is used to manage the form used for user login. It includes validation and submission handling for
	///     the login form.
	/// </remarks>
	private EditForm LoginForm
	{
		get;
		set;
	}

	private LoginClass ModelLoginClass
	{
		get;
	} = new();

	/// <summary>
	///     Gets or sets the NavigationManager instance for the Login page.
	/// </summary>
	/// <value>
	///     The NavigationManager instance.
	/// </value>
	/// <remarks>
	///     This property is used to manage navigation for the Login page. It provides methods for navigating to other pages,
	///     constructing URIs, and triggering URI-based operations like redirection.
	/// </remarks>
	[Inject]
	private NavigationManager NavManager
	{
		get;
		set;
	}

	/// <summary>
	///     Checks the login credentials provided by the user.
	/// </summary>
	/// <param name="obj">The EditContext object containing the user's login credentials.</param>
	/// <remarks>
	///     This method is responsible for checking the user's login credentials. It hashes the password provided by the user,
	///     sends a POST request to the Login API with the hashed password and username, and checks the response. If the login
	///     is successful, the method encrypts the login cookie and stores it in local storage. Finally, it redirects the user
	///     to the appropriate page based on their login status.
	/// </remarks>
	private Task CheckLogin(EditContext obj)
	{
		return ExecuteMethod(async () =>
							 {
								 //byte[] _password = General.SHA512PasswordHash(ModelLoginClass.Password.Trim());
								 //RestClient _restClient = new($"{Start.ApiHost}");
								 //RestRequest _request = new("Login/Login", Method.Post);

								 //_request.AddQueryParameter("userName", ModelLoginClass.UserName);
								 //_request.AddQueryParameter("password", Convert.ToBase64String(_password));
								 //_request.AddQueryParameter("ipAddress", IPAddress);

								 //IPHostEntry _host = await Dns.GetHostEntryAsync(Dns.GetHostName());
								 //IPAddress _ip = _host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"userName", ModelLoginClass.UserName},
																			  {"password", Convert.ToBase64String(General.SHA512PasswordHash(ModelLoginClass.Password.Trim()))},
																			  {"ipAddress", "0.0.0.0"}
																		  };
								 LoginCooky _loginCooky = await General.PostRest<LoginCooky>("Login/Login", _parameters);

								 if (_loginCooky == null)
								 {
									 return;
								 }

								 AESCryptography _aes = new();
								 //_tripleDES.Encrypt();
								 string _serializedLogin = JsonConvert.SerializeObject(_loginCooky);
								 string _encryptedText = _aes.Encrypt(_serializedLogin);
								 await LocalStorage.SetItemAsStringAsync("DeliciousCookie", _encryptedText);
								 await NavManager.RedirectLogin(LocalStorage, true);
							 });
	}

	/// <summary>
	///     Executes the provided task within a semaphore lock. If the semaphore is currently locked, the method will return
	///     immediately.
	///     If an exception occurs during the execution of the task, it will be logged using the provided logger.
	/// </summary>
	/// <param name="task">The task to be executed.</param>
	/// <returns>
	///     A task that represents the asynchronous operation.
	/// </returns>
	private Task ExecuteMethod(Func<Task> task) => General.ExecuteMethod(_semaphoreMainPage, task, Logger);

	///// <summary>
	/////     Asynchronously performs operations after the component and its child content have been rendered into the Document
	/////     Object Model (DOM).
	///// </summary>
	///// <param name="firstRender">
	/////     A boolean value that indicates whether this is the first time `OnAfterRenderAsync` has been invoked on this
	/////     component.
	///// </param>
	///// <returns>
	/////     A `Task` that represents the asynchronous operation.
	///// </returns>
	///// <remarks>
	/////     This method is overridden to validate the `EditContext` of the `LoginForm` when the component is first rendered.
	/////     It is part of the component lifecycle in Blazor applications.
	///// </remarks>
	//protected override async Task OnAfterRenderAsync(bool firstRender)
	//{
	//    await base.OnAfterRenderAsync(firstRender);
	//    if (firstRender)
	//    {
	//        LoginForm.EditContext?.Validate();
	//    }
	//}

	/// <summary>
	///     Asynchronously performs initialization operations when the component is first created.
	/// </summary>
	/// <returns>
	///     A `Task` that represents the asynchronous operation.
	/// </returns>
	/// <remarks>
	///     This method is overridden to set up the `EditContext` for the `LoginForm` and to handle redirections based on the
	///     user's login status.
	///     It is part of the component lifecycle in Blazor applications.
	/// </remarks>
	protected override async Task OnInitializedAsync()
	{
		await ExecuteMethod(async () =>
							{
								EditContext = new(ModelLoginClass);
								await NavManager.RedirectLogin(LocalStorage, true);

								LoginForm.EditContext?.Validate();
							});
		await base.OnInitializedAsync();
	}
}