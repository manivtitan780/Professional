#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Leads.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-30-2023 20:28
// *****************************************/

#endregion

#region Using

using Profsvc_AppTrack.Components.Pages.Controls.Leads;

using DocumentsPanel = Profsvc_AppTrack.Components.Pages.Controls.Requisitions.DocumentsPanel;

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     Represents the Leads page in the ProfSvc_AppTrack application.
///     This class contains properties and methods related to managing and displaying leads data.
/// </summary>
/// <remarks>
///     The Leads class is a partial class that includes properties for managing the display of leads data,
///     such as the total count of leads, the index of the last record in the current page of leads,
///     the instance of the EditNotesDialog component, the total number of pages that can be formed from the leads data,
///     and the starting record number for the current page of leads.
///     It also includes methods for collapsing the detail row in the Leads page, handling the post-rendering logic for the
///     Leads page,
///     initializing the Leads page, and a nested class for a custom data adaptor for the Leads page.
/// </remarks>
public partial class Leads
{
	private const string StorageName = "LeadGrid";
	private static TaskCompletionSource<bool> _initializationTaskSource;
	private int _currentPage = 1, _selectedTab;
	private List<ByteValues> _industries, _sources, _status;

	private LeadDetails _leadDetailsObject = new(), _leadDetailsObjectClone = new();
	private List<RequisitionDocuments> _leadDocumentsObject;

	private List<Role> _roles;
	private readonly SemaphoreSlim _semaphoreMainPage = new(1, 1);

	private List<IntValues> _states;
	private LeadClass _target;

	/// <summary>
	///     Gets a MemoryStream instance that represents the document added in the Leads page.
	/// </summary>
	/// <remarks>
	///     This property is used in the process of uploading a document in the Leads page.
	///     The MemoryStream instance is filled with the content of the uploaded file in the `UploadDocument` method,
	///     and then it is used in the `SaveDocument` method where it is converted to a byte array and sent as a part of the
	///     request to the API.
	/// </remarks>
	private MemoryStream AddedDocument
	{
		get;
	} = new();

	/// <summary>
	///     Gets or sets the value for the autocomplete functionality in the Leads page.
	/// </summary>
	private string AutocompleteValue
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the total count of leads.
	/// </summary>
	/// <remarks>
	///     This property is used to store the total count of leads retrieved from the API response in the
	///     `General.GetLeadReadAdaptor()` method.
	/// </remarks>
	internal static decimal Count
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog for adding a requisition document.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the dialog for adding a requisition document in the Leads page.
	///     It is an instance of the `AddRequisitionDocument` component, which allows the user to upload a document to a lead.
	///     The dialog is shown when the `AddDocument` method is called.
	/// </remarks>
	private AddRequisitionDocument DialogDocument
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog for editing lead details.
	/// </summary>
	/// <remarks>
	///     This property is used to handle the dialog box for editing lead details. It is an instance of the `EditLeadDetails`
	///     class.
	///     The dialog is shown when the `ShowDialog()` method is called in the `EditLead()` method of the `Leads` class.
	/// </remarks>
	private EditLeadDetails DialogEditLead
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the display status of the Add button.
	/// </summary>
	/// <remarks>
	///     This property is used to control the visibility of the Add button in the Leads page.
	///     If the user has the right to edit the company, the display status is set to "unset", otherwise it is set to "none".
	/// </remarks>
	private string DisplayAdd
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the index of the last record in the current page of leads.
	///     This value is calculated as the index of the first record of the current page plus the number of leads in the
	///     current page.
	/// </summary>
	public static int EndRecord
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the file being uploaded.
	/// </summary>
	/// <remarks>
	///     This property is used to store the name of the file that is being uploaded in the `Leads.UploadDocument()` method.
	///     It is then used in the `Leads.SaveDocument()` method to add the file to the request for the API call.
	/// </remarks>
	private string FileName
	{
		get;
		set;
	}

	/// <summary>
	///     Represents a static grid of LeadClass objects in the Leads page.
	/// </summary>
	/// <remarks>
	///     This grid is used to display and manipulate data related to leads.
	///     It is refreshed when changes are made to the leads data, such as sorting, filtering, changing item count, or
	///     converting a lead.
	/// </remarks>
	private static SfGrid<LeadClass> Grid
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the IJSRuntime interface.
	/// </summary>
	/// <remarks>
	///     This property is used to invoke JavaScript functions from C# code in the Leads page.
	///     For example, it is used in the `Leads.DownloadDocument()` method to open a new browser tab for the download URL.
	/// </remarks>
	[Inject]
	private IJSRuntime JsRuntime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the list of notes associated with a lead.
	/// </summary>
	/// <remarks>
	///     This property is used to store the list of notes retrieved from the API response in the `Leads.DeleteNotes()`,
	///     `Leads.DetailDataBind()`, and `Leads.SaveNotes()` methods.
	///     It is also used as a model for the notes section in the Leads page UI.
	///     Each note in the list is an instance of the `CandidateNotes` class.
	/// </remarks>
	private List<CandidateNotes> LeadNotesObject
	{
		get;
		set;
	} = [];

	/// <summary>
	///     Gets or sets the service for interacting with the local storage of the browser.
	///     This service is used to persist the state of the Leads page, including search parameters and pagination.
	/// </summary>
	[Inject]
	private ILocalStorageService LocalStorageBlazored
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
	private ILogger<Candidate> Logger
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the user's login information.
	/// </summary>
	/// <remarks>
	///     This property is used to store the user's login information retrieved from the memory cache in the
	///     `OnInitializedAsync()` method.
	///     It is also used in the `SaveDocument()` method to determine the user ID for the document upload request.
	///     If the user is not logged in, the user ID defaults to "JOLLY".
	/// </remarks>
	private LoginCooky LoginCookyUser
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the MIME type of the file being uploaded.
	/// </summary>
	/// <remarks>
	///     This property is used to store the MIME type of the file being uploaded in the `Leads.UploadDocument()` method.
	///     The MIME type is retrieved from the FileInfo of the uploaded file.
	///     It is then used as a parameter in the API request in the `Leads.SaveDocument()` method.
	/// </remarks>
	private string Mime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the NavigationManager.
	/// </summary>
	/// <remarks>
	///     This property is used to manage navigation tasks such as constructing URIs and navigating to them.
	///     For example, it is used in the `Leads.DownloadDocument()` method to construct a URI for downloading a document and
	///     then navigate to it.
	/// </remarks>
	[Inject]
	private NavigationManager NavManager
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a new instance of the RequisitionDocuments class.
	/// </summary>
	private RequisitionDocuments NewDocument
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the instance of the EditNotesDialog component.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the dialog for editing notes in the Leads page.
	///     The dialog is shown by calling the `ShowDialog` method of this property, as seen in the `Leads.EditNotes()` method.
	/// </remarks>
	private EditNotesDialog NotesDialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the total number of pages that can be formed from the lead's data.
	///     This is calculated by dividing the total count of leads by the number of items per page, and rounding up to the
	///     next integer.
	/// </summary>
	public static int PageCount
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the DocumentsPanel component.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the documents panel in the Leads page.
	///     The documents panel is used for handling operations related to documents in the Leads page, such as downloading a
	///     document.
	///     The selected document for download is obtained from the `SelectedRow` property of this `PanelDocument` instance, as
	///     seen in the `Leads.DownloadDocument()` method.
	/// </remarks>
	private DocumentsPanel PanelDocument
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the LeadNotesPanel component.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the notes panel in the Leads page.
	///     The LeadNotesPanel component provides functionality for displaying and interacting with lead notes.
	///     The selected note for editing is obtained from the `SelectedRow` property of this `PanelNotes` instance, as seen in
	///     the `Leads.EditNotes()` method.
	/// </remarks>
	private LeadNotesPanel PanelNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Redis service used for caching data.
	/// </summary>
	/// <value>
	///     The Redis service.
	/// </value>
	/// <remarks>
	///     This property is injected and used for operations like retrieving or storing data in Redis cache.
	/// </remarks>
	[Inject]
	private RedisService Redis
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the RoleID associated with the Lead.
	/// </summary>
	private string RoleID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SearchModel property of the Leads class. This property is of type LeadSearch and is used to manage
	///     search criteria for leads.
	/// </summary>
	private static LeadSearch SearchModel
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected RequisitionDocuments instance for download.
	/// </summary>
	/// <remarks>
	///     This property is used to store the selected document for download in the `Leads.DownloadDocument()` method.
	///     The selected document is determined by the `PanelDocument.SelectedRow`.
	/// </remarks>
	private RequisitionDocuments SelectedDownload
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the selected notes for a candidate in the Leads page.
	///     This property is of type <see cref="ProfSvc_Classes.CandidateNotes" /> which encapsulates the notes related to a
	///     candidate.
	/// </summary>
	private CandidateNotes SelectedNotes
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the sort direction for the lead's data.
	/// </summary>
	/// <remarks>
	///     This property is used to determine the order in which the leads are displayed.
	///     It is set in the `OnInitializedAsync` method based on the `SearchModel.SortDirection` value.
	///     It is also used in the `BuildRenderTree` method to set the `Direction` attribute of the `GridSortColumn` component.
	/// </remarks>
	private SortDirection SortDirectionProperty
	{
		get;
		set;
	} = SortDirection.Ascending;

	/// <summary>
	///     Gets or sets the field by which the leads data is sorted.
	/// </summary>
	/// <remarks>
	///     This property is used to determine the field (such as "Company", "Location", "Industry", "Status", or "Updated")
	///     by which the leads data is sorted in the Leads page. The sorting field is set in the `OnInitializedAsync` method
	///     based on the `SearchModel.SortField` value.
	/// </remarks>
	private string SortField
	{
		get;
		set;
	} = "Updated";

	/// <summary>
	///     Gets or sets the instance of the SfSpinner component.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the spinner in the Leads page.
	///     The spinner is shown by calling the `ShowAsync` method and hidden by calling the `HideAsync` method of this
	///     property.
	///     For example, it is used in the `Leads.DetailDataBind()` and `Leads.EditLead()` methods to indicate a loading state
	///     while performing asynchronous operations.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the starting record number for the current page of leads.
	///     This value is calculated as ((_page - 1) * _itemCount + 1).
	/// </summary>
	public static int StartRecord
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the title for the Leads page.
	/// </summary>
	private string Title
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the User of the Lead. This property is used to store the UserID of the currently logged-in
	///     user.
	///     It is used in the Lead page to control the display of certain elements based on the user's rights and
	///     whether they are the updater of the record.
	/// </summary>
	/// <remarks>
	///     This property is a string that holds the UserID of the currently logged-in user. It is used in the Lead page
	///     to control the display of certain elements. If the logged-in user has the right to edit a candidate, and they are
	///     the updater of the record, certain elements on the page will be displayed. Otherwise, those elements will be
	///     hidden.
	/// </remarks>
	private string User
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the user rights for the Leads page.
	/// </summary>
	/// <value>
	///     The user rights associated with the Leads page.
	/// </value>
	/// <remarks>
	///     This property represents the permissions a user has when interacting with the Leads page.
	///     These permissions are encapsulated in the UserRights class, which includes rights such as viewing, editing, and
	///     changing the status of candidates, requisitions, and companies, among others.
	/// </remarks>
	private UserRights UserRights
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Handles the beginning of an action on the Leads page. This method is triggered when an action begins on the grid.
	///     It checks if an action is not already in progress, then it yields the Task to free up the UI thread.
	///     If the action is a sorting action, it sets the sort field and direction based on the column name and direction
	///     provided in the args.
	///     It then saves the current state of the grid to the local storage and refreshes the grid.
	/// </summary>
	/// <param name="args">
	///     The arguments for the action, containing information such as the request type, column name, and sort
	///     direction.
	/// </param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task ActionBegin(ActionEventArgs<LeadClass> args)
	{
		return ExecuteMethod(async () =>
							 {
								 if (args.RequestType == Action.Sorting)
								 {
									 SearchModel.SortField = args.ColumnName switch
									 {
										 "Company" => 1,
										 "Location" => 2,
										 "Industry" => 3,
										 "Status" => 4,
										 _ => 5
									 };
									 SearchModel.SortDirection = args.Direction == SortDirection.Ascending ? (byte)1 : (byte)0;
									 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
									 await Grid.Refresh();
								 }
							 });
	}

	/// <summary>
	///     Asynchronously adds a new document to the lead.
	/// </summary>
	/// <remarks>
	///     This method first checks if the `NewDocument` property is null. If it is, a new `RequisitionDocuments` instance is
	///     created.
	///     If it's not null, the `ClearData` method of the `NewDocument` is called to reset its data.
	///     After these checks and operations, the `ShowDialog` method of the `DialogDocument` property is called.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private Task AddDocument()
	{
		return ExecuteMethod(async () =>
							 {
								 if (NewDocument == null)
								 {
									 NewDocument = new();
								 }
								 else
								 {
									 NewDocument.Clear();
								 }

								 await DialogDocument.ShowDialog();
							 });
	}

	/// <summary>
	///     Initiates the advanced search functionality in the Leads page.
	///     This method creates a copy of the current search model and opens the advanced search dialog.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private Task AdvancedSearch(MouseEventArgs arg) => Task.Delay(1);

	/// <summary>
	///     Handles the completion of an action by enabling the buttons in the DialogDocument.
	/// </summary>
	/// <param name="arg">The event arguments containing details about the completed action.</param>
	private void AfterDocument(ActionCompleteEventArgs arg) => DialogDocument.EnableButtons();

	/// <summary>
	///     Executes an asynchronous task that resets the search model and refreshes the grid.
	///     This method is intended to be used for displaying all leads without any filters.
	/// </summary>
	private Task AllAlphabet()
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModel.Name = "";
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = "";
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the BeforeUpload event of the Document.
	/// </summary>
	/// <param name="arg">Provides data for the BeforeUpload event.</param>
	private void BeforeDocument(BeforeUploadEventArgs arg) => DialogDocument.DisableButtons();

	/// <summary>
	///     Handles the change in item count for the Leads page.
	/// </summary>
	/// <param name="item">The event arguments containing the new item count.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private Task ChangeItemCount(ChangeEventArgs<int, IntValues> item)
	{
		return ExecuteMethod(async () =>
							 {
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 SearchModel.ItemCount = item.Value;

								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
								 StateHasChanged();
							 });
	}

	/// <summary>
	///     Asynchronously clears the filter applied to the Leads page.
	/// </summary>
	/// <remarks>
	///     This method checks if a filter operation is not already in progress. If not, it sets the current page to 1, clears
	///     the search model data, and refreshes the grid. The method also stores the updated search model in the local
	///     storage.
	/// </remarks>
	private Task ClearFilter()
	{
		return ExecuteMethod(async () =>
							 {
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 int _currentPageItemCount = SearchModel.ItemCount;
								 SearchModel.Clear();
								 SearchModel.ItemCount = _currentPageItemCount;
								 SearchModel.User = User;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = "";
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Asynchronously converts a lead.
	/// </summary>
	/// <remarks>
	///     This method initiates a REST request to the "Lead/ConvertLead" endpoint. The request includes parameters for the
	///     lead ID, user, and path.
	///     The method then awaits the asynchronous POST request to the server and refreshes the grid upon completion.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private Task ConvertLead()
	{
		return ExecuteMethod(async () =>
							 {
								 //RestClient _client = new($"{Start.ApiHost}");
								 //RestRequest _request = new("Lead/ConvertLead", Method.Post)
								 //{
								 // RequestFormat = DataFormat.Json
								 //};
								 //_request.AddQueryParameter("leadID", _target.ID);
								 //_request.AddQueryParameter("user", User);
								 //_request.AddQueryParameter("path", Start.UploadsPath);

								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"leadID", _target.ID.ToString()},
																			  {"user", User},
																			  {"path", Start.UploadsPath}
																		  };

								 await General.PostRest<int>("Lead/ConvertLead", _parameters);

								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the data processing for the Leads page. This method is invoked asynchronously.
	/// </summary>
	/// <param name="obj">The object that triggers the data handling event.</param>
	/// <remarks>
	///     This method creates a .NET reference for the Leads class and invokes a JavaScript function named "detail" with the
	///     created reference.
	///     If the total item count in the Grid is more than 0, it selects the first row asynchronously.
	/// </remarks>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task DataHandler(object obj)
	{
		return ExecuteMethod(async () =>
							 {
								 DotNetObjectReference<Leads> _dotNetReference = DotNetObjectReference.Create(this); // create dotnet ref
								 await Runtime.InvokeAsync<string>("detail", _dotNetReference);
								 //  send the dotnet ref to JS side
								 if (Grid.TotalItemCount > 0)
								 {
									 await Grid.SelectRowAsync(0);
								 }
							 });
	}

	/// <summary>
	///     Asynchronously deletes a document associated with a lead.
	/// </summary>
	/// <param name="args">The ID of the document to be deleted.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the "Lead/DeleteLeadDocument" endpoint of the API host.
	///     The document ID and user are passed as query parameters in the request.
	///     If the deletion is successful, the method updates the list of lead documents.
	/// </remarks>
	private Task DeleteDocument(int args)
	{
		return ExecuteMethod(async () =>
							 {
								 //RestClient _client = new($"{Start.ApiHost}");
								 //RestRequest _request = new("Lead/DeleteLeadDocument", Method.Post)
								 //{
								 // RequestFormat = DataFormat.Json
								 //};
								 //_request.AddQueryParameter("documentID", args.ToString());
								 //_request.AddQueryParameter("user", User);

								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"documentID", args.ToString()},
																			  {"user", User}
																		  };

								 Dictionary<string, object> _response = await General.PostRest("Lead/DeleteLeadDocument", _parameters);
								 if (_response == null)
								 {
									 return;
								 }

								 _leadDocumentsObject = General.DeserializeObject<List<RequisitionDocuments>>(_response["Document"]);
							 });
	}

	/// <summary>
	///     Asynchronously deletes the notes of a lead.
	/// </summary>
	/// <param name="id">The ID of the note to be deleted.</param>
	/// <remarks>
	///     This method sends a POST request to the "Lead/DeleteNotes" endpoint with the note ID, lead ID, and user as
	///     parameters.
	///     If the response is not null, it deserializes the "Notes" field from the response into a list of CandidateNotes and
	///     assigns it to the LeadNotesObject.
	/// </remarks>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task DeleteNotes(int id)
	{
		return ExecuteMethod(async () =>
							 {
								 //using RestClient _client = new($"{Start.ApiHost}");
								 //RestRequest _request = new("Lead/DeleteNotes", Method.Post)
								 //					   {
								 //						   RequestFormat = DataFormat.Json
								 //					   };
								 //_request.AddQueryParameter("id", id.ToString());
								 //_request.AddQueryParameter("leadID", _target.ID.ToString());
								 //_request.AddQueryParameter("user", User);

								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"id", id.ToString()},
																			  {"leadID", _target.ID.ToString()},
																			  {"user", User}
																		  };

								 Dictionary<string, object> _response = await General.PostRest("Lead/DeleteNotes", _parameters);
								 if (_response == null)
								 {
									 return;
								 }

								 LeadNotesObject = General.DeserializeObject<List<CandidateNotes>>(_response["Notes"]);
							 });
	}

	/// <summary>
	///     Asynchronously binds data to the detail row of a grid when it is expanded.
	/// </summary>
	/// <param name="lead">The event arguments containing the data item of the detail row.</param>
	/// <remarks>
	///     This method performs the following steps:
	///     - Checks if an action is not already in progress.
	///     - If a target is set and different from the current lead data, it collapses the detail row of the target.
	///     - Gets the row index of the lead data and selects the row in the grid.
	///     - Sets the target to the current lead data.
	///     - Shows a spinner for loading indication.
	///     - Makes a REST API call to "Lead/GetLeadDetails" with the lead ID as a parameter.
	///     - If the API response is not null, it deserializes the lead details, notes, and documents from the response.
	///     - Sets the selected tab to 0.
	///     - Hides the spinner.
	///     - Sets the action progress to false.
	/// </remarks>
	private Task DetailDataBind(DetailDataBoundEventArgs<LeadClass> lead)
	{
		return ExecuteMethod(async () =>
							 {
								 if (_target != null && _target != lead.Data)
								 {
									 // return when target is equal to args.data
									 await Grid.ExpandCollapseDetailRowAsync(_target);
								 }

								 int _index = await Grid.GetRowIndexByPrimaryKeyAsync(lead.Data.ID);
								 if (_index != Grid.SelectedRowIndex)
								 {
									 await Grid.SelectRowAsync(_index);
								 }

								 _target = lead.Data;

								 await Task.Yield();
								 try
								 {
									 await Spinner.ShowAsync();
								 }
								 catch
								 {
									 //
								 }

								 //RestClient _restClient = new($"{Start.ApiHost}");
								 //RestRequest _request = new("Lead/GetLeadDetails");
								 //_request.AddQueryParameter("leadID", _target.ID);

								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"leadID", _target.ID.ToString()}
																		  };

								 Dictionary<string, object> _restResponse = await General.GetRest<Dictionary<string, object>>("Lead/GetLeadDetails", _parameters);

								 if (_restResponse != null)
								 {
									 _leadDetailsObject = JsonConvert.DeserializeObject<LeadDetails>(_restResponse["Lead"]?.ToString() ?? string.Empty);
									 LeadNotesObject = General.DeserializeObject<List<CandidateNotes>>(_restResponse["Notes"]);
									 _leadDocumentsObject = General.DeserializeObject<List<RequisitionDocuments>>(_restResponse["Documents"]);
								 }

								 _selectedTab = 0;

								 await Task.Yield();
								 try
								 {
									 await Spinner.HideAsync();
								 }
								 catch
								 {
									 //
								 }
							 });
	}

	/// <summary>
	///     This method is invoked from JavaScript and is used to collapse the detail row in the Leads page.
	///     It sets the target lead to null, effectively deselecting any selected lead.
	/// </summary>
	[JSInvokable("DetailCollapse")]
	public void DetailRowCollapse() => _target = null;

	/// <summary>
	///     Initiates the download of a selected document associated with a lead.
	/// </summary>
	/// <param name="args">The ID of the document to be downloaded.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if a download is not already in progress. If not, it sets the selected document for
	///     download,
	///     constructs a query string using the document's file name, the ID of the target lead, the original file name, and a
	///     fixed value of 3.
	///     The query string is then Base64 encoded. The method then invokes a JavaScript function to open a new browser tab
	///     for the download URL.
	///     Once the download is initiated, the method resets the download in progress flag.
	/// </remarks>
	private Task DownloadDocument(int args)
	{
		return ExecuteMethod(async () =>
							 {
								 SelectedDownload = PanelDocument.SelectedRow;
								 string _queryString = $"{SelectedDownload.DocumentFileName}^{_target.ID}^{SelectedDownload.OriginalFileName}^3".ToBase64String();
								 await JsRuntime.InvokeVoidAsync("open", $"{NavManager.BaseUri}Download/{_queryString}", "_blank");
							 });
	}

	/// <summary>
	///     Asynchronously edits or adds a lead based on the provided boolean parameter.
	/// </summary>
	/// <param name="isAdd">If true, a new lead is added. If false, an existing lead is edited.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task EditLead(bool isAdd)
	{
		return ExecuteMethod(async () =>
							 {
								 if (Spinner != null)
								 {
									 try
									 {
										 await Spinner.ShowAsync();
									 }
									 catch
									 {
										 //
									 }
								 }

								 if (isAdd)
								 {
									 Title = "Add";
									 if (_leadDetailsObjectClone == null)
									 {
										 _leadDetailsObjectClone = new();
									 }
									 else
									 {
										 _leadDetailsObjectClone.Clear();
									 }

									 _leadDetailsObjectClone.IsAdd = true;
								 }
								 else
								 {
									 Title = "Edit";
									 _leadDetailsObjectClone = _leadDetailsObject.Copy();

									 _leadDetailsObjectClone.IsAdd = false;
								 }

								 StateHasChanged();
								 await DialogEditLead.ShowDialog();
								 if (Spinner != null)
								 {
									 try
									 {
										 await Spinner.HideAsync();
									 }
									 catch
									 {
										 //
									 }
								 }
							 });
	}

	/// <summary>
	///     Asynchronously edits or adds a note for a lead based on the provided note ID.
	/// </summary>
	/// <param name="id">
	///     The ID of the note. If the ID is 0, a new note is added. Otherwise, the existing note with the given
	///     ID is edited.
	/// </param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if an action is already in progress. If not, it sets the action progress flag to true and
	///     yields control back to the caller.
	///     If the ID is 0, it sets the title to "Add" and initializes the SelectedNotes object if it's null, or clears its
	///     data if it's not.
	///     If the ID is not 0, it sets the title to "Edit" and sets the SelectedNotes object to a copy of the selected row in
	///     the PanelNotes.
	///     After setting up the SelectedNotes object, it sets the action progress flag to false and shows the NotesDialog.
	/// </remarks>
	private Task EditNotes(int id)
	{
		return ExecuteMethod(async () =>
							 {
								 if (id == 0)
								 {
									 Title = "Add";
									 if (SelectedNotes == null)
									 {
										 SelectedNotes = new();
									 }
									 else
									 {
										 SelectedNotes.Clear();
									 }
								 }
								 else
								 {
									 Title = "Edit";
									 SelectedNotes = PanelNotes.SelectedRow.Copy();
								 }

								 await NotesDialog.ShowDialog();
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

	/// <summary>
	///     Filters the grid based on the provided lead. The method sets the search model's name to the value of the lead,
	///     resets the current page to 1, and refreshes the grid. The method also stores the updated search model in local
	///     storage
	///     with the key "LeadGrid". The filtering operation is not performed if another action is already in progress.
	/// </summary>
	/// <param name="lead">The lead information used to filter the grid. The lead's value is used as the search model's name.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task FilterGrid(ChangeEventArgs<string, KeyValues> lead)
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModel.Name = lead.Value ?? "";
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = SearchModel.Name;
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the first click action on the Leads page.
	/// </summary>
	/// <remarks>
	///     This asynchronous method checks if an action is not already in progress. If not, it sets the current page to 1,
	///     updates the SearchModel with the current page, stores the updated SearchModel in the local storage with the key
	///     "LeadGrid",
	///     and refreshes the grid. After the operation, it resets the action progress indicator.
	/// </remarks>
	private Task FirstClick()
	{
		return ExecuteMethod(async () =>
							 {
								 if (_currentPage < 1)
								 {
									 _currentPage = 1;
								 }

								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the last click action on the Leads page.
	/// </summary>
	/// <remarks>
	///     This asynchronous method is responsible for managing the pagination of the Leads page.
	///     When the last page button is clicked, it sets the current page to the last page,
	///     updates the SearchModel with the current page number,
	///     saves the updated SearchModel to the local storage,
	///     and refreshes the grid.
	///     It also ensures that the method execution is not overlapped by using the _actionProgress flag.
	/// </remarks>
	private Task LastClick()
	{
		return ExecuteMethod(async () =>
							 {
								 if (_currentPage < 1)
								 {
									 _currentPage = 1;
								 }

								 _currentPage = PageCount.ToInt32();
								 SearchModel.Page = _currentPage;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the click event for the "Next" button in the Leads page.
	/// </summary>
	/// <remarks>
	///     This asynchronous method performs several actions:
	///     - It checks if there is an ongoing action. If there is, it simply returns.
	///     - If there is no ongoing action, it sets the `_actionProgress` flag to true, indicating that an action is in
	///     progress.
	///     - It then checks if the current page is less than 1, and if so, sets it to 1.
	///     - It updates the `_currentPage` and `SearchModel.Page` values based on the current page and total page count.
	///     - It saves the current state of the `SearchModel` to the local storage with the key "LeadGrid".
	///     - It refreshes the grid to reflect the changes.
	///     - Finally, it sets the `_actionProgress` flag back to false, indicating that the action has completed.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private Task NextClick()
	{
		return ExecuteMethod(async () =>
							 {
								 if (_currentPage < 1)
								 {
									 _currentPage = 1;
								 }

								 _currentPage = SearchModel.Page >= PageCount.ToInt32() ? PageCount.ToInt32() : SearchModel.Page + 1;
								 SearchModel.Page = _currentPage;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Asynchronously initializes the Leads page. This method is invoked when the component is first initialized.
	/// </summary>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method retrieves the user's login information and various lead-related data from the memory cache.
	///     If the user is not logged in, it redirects them to the login page.
	///     It also retrieves the lead search model from local storage, or initializes it if it does not exist.
	///     The method sets the sort direction and field based on the search model, and sets the autocomplete value to the name
	///     in the search model.
	/// </remarks>
	protected override async Task OnInitializedAsync()
	{
		_initializationTaskSource = new();
		await ExecuteMethod(async () =>
							{
								LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);

								List<string> _keys =
								[
									CacheObjects.Roles.ToString(),
									CacheObjects.States.ToString(),
									CacheObjects.LeadStatus.ToString(),
									CacheObjects.LeadSources.ToString(),
									CacheObjects.LeadIndustries.ToString()
								];

								Dictionary<string, string> _cacheValues = await Redis.BatchGet(_keys);
								_roles = General.DeserializeObject<List<Role>>(_cacheValues[CacheObjects.Roles.ToString()]);
								_states = General.DeserializeObject<List<IntValues>>(_cacheValues[CacheObjects.States.ToString()]);
								_status = General.DeserializeObject<List<ByteValues>>(_cacheValues[CacheObjects.LeadStatus.ToString()]);
								_sources = General.DeserializeObject<List<ByteValues>>(_cacheValues[CacheObjects.LeadSources.ToString()]);
								_industries = General.DeserializeObject<List<ByteValues>>(_cacheValues[CacheObjects.LeadIndustries.ToString()]);

								RoleID = LoginCookyUser.RoleID;
								UserRights = LoginCookyUser.GetUserRights(_roles);
								DisplayAdd = UserRights.EditCompany ? "unset" : "none";

								if (!UserRights.ViewCompany) // User doesn't have View Company/Leads rights. This is done by looping through the Roles of the current user and determining the rights for ViewCompany.
								{
									NavManager.NavigateTo($"{NavManager.BaseUri}home", true);
								}

								string _cookyString = await LocalStorageBlazored.GetItemAsync<string>(StorageName);
								if (!_cookyString.NullOrWhiteSpace())
								{
									SearchModel = General.DeserializeObject<LeadSearch>(_cookyString);
								}
								else
								{
									await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								}

								SortDirectionProperty = SearchModel.SortDirection == 1 ? SortDirection.Ascending : SortDirection.Descending;
								SortField = SearchModel.SortField switch
								{
									1 => "Company",
									2 => "Location",
									3 => "Industry",
									4 => "Status",
									_ => "Updated"
								};
								User = General.GetUserName(LoginCookyUser);

								AutocompleteValue = SearchModel.Name;
								_currentPage = SearchModel.Page;
								PageCount = _currentPage + 1;
								SearchModel.User = User;
							});

		_initializationTaskSource.SetResult(true);
		await base.OnInitializedAsync();
	}

	/// <summary>
	///     Handles the event when the page number is changed.
	/// </summary>
	/// <param name="obj">The event arguments containing the new page number.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private Task PageNumberChanged(ChangeEventArgs obj)
	{
		return ExecuteMethod(async () =>
							 {
								 decimal _currentValue = obj.Value.ToDecimal();
								 if (_currentValue < 1)
								 {
									 _currentPage = 1;
								 }
								 else if (_currentValue > PageCount)
								 {
									 _currentPage = PageCount.ToInt32();
								 }

								 SearchModel.Page = _currentPage;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the click event for the "Previous" button in the Leads page.
	/// </summary>
	/// <remarks>
	///     This asynchronous method checks if an action is not in progress, then decrements the current page number if it's
	///     greater than 1.
	///     The method also updates the SearchModel and the local storage with the new page number, and refreshes the grid.
	///     If the current page number is less than 1, it sets it to 1.
	/// </remarks>
	private Task PreviousClick()
	{
		return ExecuteMethod(async () =>
							 {
								 if (_currentPage < 1)
								 {
									 _currentPage = 1;
								 }

								 _currentPage = SearchModel.Page <= 1 ? 1 : SearchModel.Page - 1;
								 SearchModel.Page = _currentPage;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Refreshes the grid of leads. This method is used to update the grid display whenever there are changes to
	///     the leads data.
	/// </summary>
	private static Task RefreshGrid() => Grid.Refresh();

	/// <summary>
	///     Asynchronously saves a document related to a lead.
	/// </summary>
	/// <param name="document">The edit context containing the document to be saved.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method uploads the document to a REST API endpoint. The document is sent as a multipart form data.
	///     The method also sends additional parameters such as filename, mime type, document name, notes, lead ID, user ID,
	///     and upload path.
	///     If the document is successfully uploaded, the method updates the list of documents related to the lead.
	/// </remarks>
	private Task SaveDocument(EditContext document)
	{
		return ExecuteMethod(async () =>
							 {
								 if (document.Model is RequisitionDocuments _document)
								 {
									 //RestClient _client = new($"{Start.ApiHost}");
									 //RestRequest _request = new("Lead/UploadDocument", Method.Post)
									 //					   {
									 //						   AlwaysMultipartFormData = true
									 //					   };
									 //_request.AddFile("file", AddedDocument.ToStreamByteArray(), FileName);
									 //_request.AddParameter("filename", FileName, ParameterType.GetOrPost);
									 //_request.AddParameter("mime", Mime, ParameterType.GetOrPost);
									 //_request.AddParameter("name", _document.DocumentName, ParameterType.GetOrPost);
									 //_request.AddParameter("notes", _document.DocumentNotes, ParameterType.GetOrPost);
									 //_request.AddParameter("leadID", _target.ID.ToString(), ParameterType.GetOrPost);
									 //_request.AddParameter("user", User, ParameterType.GetOrPost);
									 //_request.AddParameter("path", Start.UploadsPath, ParameterType.GetOrPost);

									 Dictionary<string, string> _parameters = new()
																			  {
																				  {"filename", FileName},
																				  {"mime", Mime},
																				  {"name", _document.DocumentName},
																				  {"notes", _document.DocumentNotes},
																				  {"leadID", _target.ID.ToString()},
																				  {"user", User},
																				  {"path", Start.UploadsPath}
																			  };

									 Dictionary<string, object> _response =
										 await General.PostRestParameter<Dictionary<string, object>>("Lead/UploadDocument", _parameters, null, AddedDocument.ToStreamByteArray(), FileName);
									 if (_response == null)
									 {
										 return;
									 }

									 _leadDocumentsObject = General.DeserializeObject<List<RequisitionDocuments>>(_response["Document"]);
								 }
							 });
	}

	/// <summary>
	///     Saves the lead information to the server.
	/// </summary>
	/// <param name="lead">The context of the lead to be saved.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method makes a POST request to the "Lead/SaveLeads" endpoint with the lead information.
	///     After the lead information is saved, it updates the local lead details object and refreshes the grid if necessary.
	/// </remarks>
	private Task SaveLead(EditContext lead)
	{
		return ExecuteMethod(async () =>
							 {
								 //RestClient _client = new($"{Start.ApiHost}");
								 //RestRequest _request = new("Lead/SaveLeads", Method.Post)
								 //{
								 // RequestFormat = DataFormat.Json
								 //};
								 //_request.AddJsonBody(lead.Model);
								 //_request.AddQueryParameter("user", User);

								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"user", User}
																		  };

								 await General.PostRest<int>("Lead/SaveLeads", _parameters, lead.Model);

								 _leadDetailsObject = _leadDetailsObjectClone.Copy();

								 if (_leadDetailsObject.ID > 0)
								 {
									 _target.Company = _leadDetailsObject.Company;
									 _target.Phone = _leadDetailsObject.Phone.StripPhoneNumber().FormatPhoneNumber();
									 if (_leadDetailsObject.StateName.Contains("["))
									 {
										 string[] _stateArray = _leadDetailsObject.StateName.Split('-');
										 _target.Location = $"{_leadDetailsObject.City}, {(_stateArray.Length > 1 ? _stateArray[1].Replace("[", "").Replace("]", "").Trim() : _leadDetailsObject.StateName)}";
									 }

									 _target.Industry = _leadDetailsObject.LeadIndustry;
									 _target.Status = _leadDetailsObject.LeadStatus;
									 _target.LastUpdated = $"{DateTime.Today.CultureDate()} [{User}]";
								 }
								 else
								 {
									 SearchModel.Clear();
									 await Grid.Refresh();
								 }

								 StateHasChanged();
							 });
	}

	/// <summary>
	///     Asynchronously saves the notes for a lead.
	/// </summary>
	/// <param name="notes">The edit context containing the notes to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the "Lead/SaveNotes" endpoint of the API.
	///     The notes are sent in the body of the request in JSON format.
	///     Additional query parameters "user" and "leadID" are also sent with the request.
	///     If the response is not null, it deserializes the "Notes" from the response into a list of CandidateNotes objects
	///     and assigns it to LeadNotesObject.
	/// </remarks>
	private Task SaveNotes(EditContext notes)
	{
		return ExecuteMethod(async () =>
							 {
								 //RestClient _client = new($"{Start.ApiHost}");
								 //RestRequest _request = new("Lead/SaveNotes", Method.Post)
								 //{
								 //	RequestFormat = DataFormat.Json
								 //};
								 //_request.AddJsonBody(notes.Model);
								 //_request.AddQueryParameter("user", User);
								 //_request.AddQueryParameter("leadID", _target.ID);

								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"user", User},
																			  {"leadID", _target.ID.ToString()}
																		  };

								 Dictionary<string, object> _response = await General.PostRest("Lead/SaveNotes", _parameters, notes.Model);
								 if (_response == null)
								 {
									 return;
								 }

								 LeadNotesObject = General.DeserializeObject<List<CandidateNotes>>(_response["Notes"]);
							 });
	}

	/// <summary>
	///     Sets the alphabet for the search model and refreshes the grid.
	/// </summary>
	/// <param name="alphabet">The alphabet character to set.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method sets the alphabet character for the search model, updates the current page and the autocomplete value,
	///     and then refreshes the grid. It uses local storage to persist the search model.
	///     The method is asynchronous and will yield if a filter operation is already in progress.
	/// </remarks>
	private Task SetAlphabet(char alphabet)
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModel.Name = alphabet.ToString();
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await LocalStorageBlazored.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = alphabet.ToString();
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the click event of a speed dial item.
	/// </summary>
	/// <param name="args">The arguments of the speed dial item event.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method is invoked when a speed dial item is clicked. It performs different actions based on the ID of the
	///     clicked item.
	///     The possible actions include editing a lead, adding notes, adding a document, or converting a lead.
	///     The method ensures that only one speed dial item action is processed at a time.
	/// </remarks>
	private async Task SpeedDialItemClicked(SpeedDialItemEventArgs args)
	{
		switch (args.Item.ID)
		{
			case "itemEditLead":
				_selectedTab = 0;
				await EditLead(false);
				break;
			case "itemAddNotes":
				_selectedTab = 1;
				await EditNotes(0);
				break;
			case "itemAddDocument":
				_selectedTab = 2;
				await AddDocument();
				break;
			case "itemConvertLead":
				_selectedTab = 2;
				await ConvertLead();
				break;
		}
	}

	/// <summary>
	///     This method is invoked when a tab is selected in the Leads page.
	/// </summary>
	/// <param name="args">An object of type SelectEventArgs that contains the index of the selected tab.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     The method sets the _selectedTab field to the index of the selected tab.
	/// </remarks>
	private void TabSelected(SelectEventArgs args) => _selectedTab = args.SelectedIndex;

	/// <summary>
	///     Handles the upload of a document.
	/// </summary>
	/// <param name="file">The file to be uploaded.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method will open a stream for each file in the UploadChangeEventArgs, copy the stream to the AddedDocument
	///     stream, and then close the original stream.
	///     The file name and MIME type of the uploaded file are also stored.
	/// </remarks>
	private Task UploadDocument(UploadChangeEventArgs file)
	{
		return ExecuteMethod(async () =>
							 {
								 foreach (UploadFiles _file in file.Files)
								 {
									 Stream _str = _file.File.OpenReadStream(20 * 1024 * 1024);
									 await _str.CopyToAsync(AddedDocument);
									 FileName = _file.FileInfo.Name;
									 Mime = _file.FileInfo.MimeContentType;
									 AddedDocument.Position = 0;
									 _str.Close();
								 }
							 });
	}

	/// <summary>
	///     The AdminLeadAdaptor class is a custom data adaptor for the Leads page.
	///     It inherits from the DataAdaptor class and overrides the ReadAsync method.
	/// </summary>
	/// <remarks>
	///     The ReadAsync method is used to asynchronously read data for the Leads page.
	///     It checks if a read operation is already in progress, and if not, it initiates a new read operation.
	///     The method retrieves lead data using the General.GetLeadReadAdaptor method and the provided DataManagerRequest.
	///     If there are any leads, it selects the first one. The method returns the retrieved lead data.
	/// </remarks>
	public class AdminLeadAdaptor : DataAdaptor
	{
		private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

		/// <summary>
		///     Asynchronously reads lead data for the Leads page.
		/// </summary>
		/// <param name="dm">The DataManagerRequest used to retrieve lead data.</param>
		/// <param name="key">An optional key to specify a particular lead data. Default is null.</param>
		/// <returns>A Task that represents the asynchronous operation. The Task result contains the lead data.</returns>
		/// <remarks>
		///     This method checks if a read operation is already in progress, and if not, it initiates a new read operation.
		///     It retrieves lead data using the General.GetLeadReadAdaptor method and the provided DataManagerRequest.
		///     If there are any leads, it selects the first one. The method returns the retrieved lead data.
		/// </remarks>
		public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
		{
			if (!await _semaphoreSlim.WaitAsync(TimeSpan.Zero))
			{
				return null;
			}

			await _initializationTaskSource.Task;
			try
			{
				object _leadReturn = await General.GetLeadReadAdaptor(SearchModel, dm);
				if (Count > 0)
				{
					await Grid.SelectRowAsync(0);
				}

				return _leadReturn;
			}
			catch
			{
				return null;
			}
			finally
			{
				_semaphoreSlim.Release();
			}
		}
	}
}