#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Requisition.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-30-2023 20:46
// *****************************************/

#endregion

#region Using

using ActivityPanelRequisition = Profsvc_AppTrack.Components.Pages.Controls.Requisitions.ActivityPanelRequisition;
using AddRequisitionDocument = Profsvc_AppTrack.Components.Pages.Controls.Requisitions.AddRequisitionDocument;
using DocumentsPanel = Profsvc_AppTrack.Components.Pages.Controls.Requisitions.DocumentsPanel;
using RequisitionDetailsPanel = Profsvc_AppTrack.Components.Pages.Controls.Requisitions.RequisitionDetailsPanel;

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     <para>
///         Represents a requisition in the application. This class contains properties and methods related to a
///         requisition,
///         including its count, end record, next steps, page count, row height, sort direction, sort field,
///         start record, associated companies, company contacts, skills, and status list.
///     </para>
///     <para>
///         It also contains a method for handling detail row collapse and a method that is called when the component is
///         initialized.
///     </para>
/// </summary>
public partial class Requisition
{
	private const string StorageName = "RequisitionGrid";
	private static TaskCompletionSource<bool> _initializationTaskSource;

	private static int _currentPage = 1;

	private static bool _loaded;

	private bool _actionProgress, _editInProgress;

	private List<CandidateActivity> _candidateActivityObject = [];
	private readonly List<KeyValues> _companies = [];

	private List<IntValues> _education, _eligibility, _experience, _skills, _states;

	private List<KeyValues> _jobOptions, _recruiters;

	private string _lastValue = "";
	private Preferences _preference;

	private MarkupString _requisitionDetailSkills = "".ToMarkupString();

	private RequisitionDetails _requisitionDetailsObject = new(), _requisitionDetailsObjectClone = new();
	private List<RequisitionDocuments> _requisitionDocumentsObject = [];

	private List<Role> _roles;
	private int _selectedTab;
	private readonly SemaphoreSlim _semaphoreMainPage = new(1, 1);

	private List<StatusCode> _statusCodes;

	private readonly List<KeyValues> _statusSearch = [];

	private Requisitions _target;

	/// <summary>
	///     A collection of toolbar items used in the Requisition page.
	///     These items include commands for text formatting such as bold, italic, underline, strikethrough,
	///     lowercase, uppercase, superscript, subscript, clear format, undo, and redo.
	/// </summary>
	private readonly List<ToolbarItemModel> _tools1 =
	[
		new() {Command = ToolbarCommand.Bold},
		new() {Command = ToolbarCommand.Italic},
		new() {Command = ToolbarCommand.Underline},
		new() {Command = ToolbarCommand.StrikeThrough},
		new() {Command = ToolbarCommand.LowerCase},
		new() {Command = ToolbarCommand.UpperCase},
		new() {Command = ToolbarCommand.SuperScript},
		new() {Command = ToolbarCommand.SubScript},
		new() {Command = ToolbarCommand.Separator},
		new() {Command = ToolbarCommand.ClearFormat},
		new() {Command = ToolbarCommand.Separator},
		new() {Command = ToolbarCommand.Undo},
		new() {Command = ToolbarCommand.Redo}
	];

	private List<AppWorkflow> _workflows;

	/// <summary>
	///     Gets or sets the ActivityPanelRequisition instance associated with the Requisition.
	///     This property is used to manage and manipulate the activity panel of the requisition.
	/// </summary>
	private ActivityPanelRequisition ActivityPanel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets the MemoryStream instance representing the document added to the requisition.
	///     This property is used when a new document is uploaded to the requisition.
	/// </summary>
	private MemoryStream AddedDocument
	{
		get;
	} = new();

	/// <summary>
	///     Gets or sets the AutocompleteValue property of the Requisition.
	/// </summary>
	/// <remarks>
	///     The AutocompleteValue is used to store the title of the SearchModel during the initialization of the Requisition
	///     component.
	///     It is also updated when clearing filters or selecting all alphabet options.
	/// </remarks>
	/// <value>
	///     The title of the SearchModel.
	/// </value>
	private string AutocompleteValue
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the list of companies associated with the requisition.
	/// </summary>
	/// <value>
	///     The list of companies.
	/// </value>
	internal static List<Company> Companies
	{
		get;
		set;
	} = [];

	/// <summary>
	///     Gets or sets a list of company contacts associated with the requisition.
	/// </summary>
	/// <value>
	///     The list of company contacts.
	/// </value>
	internal static List<CompanyContact> CompanyContacts
	{
		get;
		set;
	} = [];

	/// <summary>
	///     Gets or sets the total count of requisitions. This count is used for pagination and display purposes.
	/// </summary>
	internal static int Count
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogActivity of type EditActivityDialog. This dialog is used to edit the activities related to
	///     the requisition.
	///     It is shown when the user initiates an edit operation on a selected activity.
	/// </summary>
	private EditActivityDialog DialogActivity
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog document for adding a requisition. This property is used to manage the dialog
	///     that is displayed when a new requisition document is being added. It provides methods for showing the dialog,
	///     enabling and disabling buttons during the document upload process.
	/// </summary>
	private AddRequisitionDocument DialogDocument
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogEditRequisition of type RequisitionDetailsPanel. This property represents a dialog box for
	///     editing requisition details.
	///     It is used in the EditRequisition method where it is responsible for showing the dialog box.
	/// </summary>
	private RequisitionDetailsPanel DialogEditRequisition
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog for advanced requisition search. This dialog is used to input advanced search parameters
	///     for finding requisitions. It is shown when the AdvancedSearch method is invoked.
	/// </summary>
	private AdvancedRequisitionSearch DialogSearch
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a string value representing the display status of the Add functionality in the Requisition context.
	/// </summary>
	private string DisplayAdd
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DocumentsPanel associated with the requisition. The DocumentsPanel is used to manage and interact
	///     with the documents related to the requisition.
	///     It is used in the DownloadDocument method to select the document for download.
	/// </summary>
	private DocumentsPanel DocumentsPanel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the end record of the requisition. The end record is calculated as the start record plus the count of
	///     data source items.
	///     This property is used to determine the range of records displayed on the current page of the requisition grid.
	/// </summary>
	internal static int EndRecord
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the file associated with the requisition.
	///     This property is used when uploading a document related to the requisition.
	/// </summary>
	private string FileName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the grid of requisitions. This grid is used to display and manage the list of requisitions in the
	///     application.
	/// </summary>
	private static SfGrid<Requisitions> Grid
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the JavaScript runtime instance. This instance is used to invoke JavaScript functions from C# code.
	///     For example, it is used in the DownloadDocument method to open a new browser tab for document download.
	/// </summary>
	[Inject]
	private IJSRuntime JsRuntime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the local storage service. This service is used for storing data locally in the user's browser.
	///     It provides methods for storing, retrieving, and deleting data.
	/// </summary>
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
	private ILogger<Requisition> Logger
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the LoginCookyUser property in the Requisition class.
	/// </summary>
	/// <remarks>
	///     The LoginCookyUser property is used to store the user's login cookie information.
	///     This information is used for user identification and session management within the application.
	///     It is retrieved and set in the OnInitializedAsync method of the Requisition class.
	///     It is also used in the ClearFilter method to set the User property of the SearchModel.
	/// </remarks>
	private LoginCooky LoginCookyUser
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Mime type of the file associated with the requisition.
	///     This property is used when uploading a document to the requisition.
	/// </summary>
	private string Mime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the NavigationManager that provides navigation and URI manipulation capabilities.
	/// </summary>
	/// <remarks>
	///     This NavigationManager is injected into the Requisition class and is used for handling navigation and URI
	///     manipulation within the application.
	///     It is used in methods such as OnInitializedAsync for redirecting the user, and in methods like DownloadDocument and
	///     SubmitCandidate for creating and opening new URLs.
	/// </remarks>
	[Inject]
	private NavigationManager NavManager
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a new document for the requisition. This property is used to store the details of a new document
	///     that is being added to the requisition. If the new document is null, a new instance of RequisitionDocuments is
	///     created.
	///     If the new document already exists, its data is cleared before adding new data.
	/// </summary>
	private RequisitionDocuments NewDocument
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets the list of next steps for the requisition. Each step is represented by a KeyValues object.
	/// </summary>
	internal List<KeyValues> NextSteps
	{
		get;
	} = [];

	/// <summary>
	///     Gets or sets the total number of pages in the requisition.
	///     This property is calculated by dividing the total count of requisitions by the number of items per page.
	/// </summary>
	internal static int PageCount
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
	///     Gets or sets the ID of the requisition. This ID is used to uniquely identify a requisition in the system.
	/// </summary>
	private static int RequisitionID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the RoleID associated with the Requisition.
	/// </summary>
	private string RoleID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the search model for the requisition. This model is used to store the search parameters for finding
	///     requisitions.
	/// </summary>
	private static RequisitionSearch SearchModel
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets a clone of the search model for the requisition. This clone is used to store a copy of the search
	///     parameters for comparison or backup purposes.
	/// </summary>
	private RequisitionSearch SearchModelClone
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected activity of a candidate in the requisition process.
	/// </summary>
	/// <value>
	///     The selected activity of a candidate.
	/// </value>
	private CandidateActivity SelectedActivity
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected document for download from the requisition's documents panel.
	///     This property is used in the DownloadDocument method to generate a query string for downloading the selected
	///     document.
	/// </summary>
	private RequisitionDocuments SelectedDownload
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the session storage service used for storing and retrieving session data.
	///     This service is used to manage session data such as the requisition grid state and the requisition ID from the
	///     dashboard.
	/// </summary>
	/// <value>
	///     The session storage service.
	/// </value>
	[Inject]
	private ISessionStorageService SessionStorage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the skills associated with the requisition.
	///     This is a list of <see cref="IntValues" /> where each value represents a unique skill.
	/// </summary>
	internal static List<IntValues> Skills
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the sort direction for the requisition grid. This property is used to determine the order in which
	///     requisitions are displayed in the grid.
	///     The sort direction can be either ascending or descending.
	/// </summary>
	private SortDirection SortDirectionProperty
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the field by which the requisition data is sorted.
	/// </summary>
	/// <value>
	///     The field name used for sorting. Possible values include "Code", "Title", "Company", "Option", "Status", "DueEnd",
	///     and "Updated".
	/// </value>
	private string SortField
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Spinner control for the Requisition page. The Spinner control is used to indicate a loading state
	///     while data is being retrieved or processed.
	///     The Spinner is shown when the DetailDataBind or EditRequisition methods are called, and hidden once the data
	///     processing is complete.
	/// </summary>
	private SfSpinner Spinner
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the instance of the SfSpinner control used in the Requisition page.
	///     This control provides a visual indication when an operation is being processed.
	///     It is shown when the EditRequisition method is invoked and hidden when the operation is completed.
	/// </summary>
	private SfSpinner SpinnerTop
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the starting record number in the current page of the requisition grid.
	///     This property is used to calculate the range of records displayed in the requisition grid.
	///     The value of this property is computed based on the current page number and the number of items per page.
	/// </summary>
	internal static int StartRecord
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the list of status values associated with the requisition.
	///     This list is populated with the 'StatusCount' data from the API response.
	///     Each status is represented by a 'KeyValues' object.
	/// </summary>
	internal static List<KeyValues> StatusList
	{
		get;
		set;
	} = [];

	/// <summary>
	///     Gets or sets the title of the requisition. The title is used to distinguish between "Add" and "Edit" modes in the
	///     requisition form.
	///     When a new requisition is being added, the title is set to "Add". When an existing requisition is being edited, the
	///     title is set to "Edit".
	/// </summary>
	private static string Title
	{
		get;
		set;
	} = "Edit";

	/// <summary>
	///     Gets or sets the User of the Requisition. This property is used to store the UserID of the currently logged-in
	///     user.
	///     It is used in the Requisition page to control the display of certain elements based on the user's rights and
	///     whether they are the updater of the record.
	/// </summary>
	/// <remarks>
	///     This property is a string that holds the UserID of the currently logged-in user. It is used in the Requisition page
	///     to control the display of certain elements. If the logged-in user has the right to edit a candidate, and they are
	///     the updater of the record, certain elements on the page will be displayed. Otherwise, those elements will be
	///     hidden.
	/// </remarks>
	private static string User
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the user rights associated with the requisition.
	/// </summary>
	/// <value>
	///     The user rights associated with the requisition.
	/// </value>
	/// <remarks>
	///     This property represents the various permissions a user has in the system related to the requisition.
	///     These permissions include viewing, editing, and changing the status of the requisition.
	/// </remarks>
	private UserRights UserRights
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Asynchronously adds a new document to the requisition.
	///     This method first checks if a new document instance exists. If not, it creates a new instance.
	///     If an instance already exists, it clears the existing data.
	///     After preparing the new document, it opens the dialog for adding a new requisition document.
	/// </summary>
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
	///     Initiates the advanced search process for requisitions. This method is invoked when the advanced search option is
	///     selected.
	///     It creates a copy of the current search model for backup purposes and then opens the advanced search dialog.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the advanced search invocation.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private Task AdvancedSearch(MouseEventArgs args)
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModelClone = SearchModel.Copy();
								 await DialogSearch.ShowDialog();
							 });
	}

	/// <summary>
	///     Handles the actions to be performed after a document has been processed. This method is invoked after a document
	///     has been added to a requisition. It enables the buttons in the 'AddRequisitionDocument' dialog.
	/// </summary>
	/// <param name="args">
	///     The arguments passed when the method is invoked. These arguments provide information about the document processing
	///     event.
	/// </param>
	private void AfterDocument(ActionCompleteEventArgs args) => DialogDocument.EnableButtons();

	/// <summary>
	///     Handles the click event for the "All Alphabet" button in the requisition grid.
	/// </summary>
	/// <remarks>
	///     This method resets the search model's title and page properties, clears the autocomplete value, and refreshes the
	///     grid.
	///     It also ensures that the method's actions are not performed if a previous action is still in progress.
	/// </remarks>
	/// <param name="args">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
	private Task AllAlphabet(MouseEventArgs args)
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModel.Title = "";
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = "";
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     This method is invoked before a document is uploaded. It disables the buttons in the
	///     AddRequisitionDocument dialog to prevent any other actions during the upload process.
	/// </summary>
	/// <param name="arg">The arguments related to the document upload event.</param>
	private void BeforeDocument(BeforeUploadEventArgs arg)
	{
		DialogDocument.DisableButtons();
	}

	/// <summary>
	///     Asynchronously changes the item count of the requisition.
	/// </summary>
	/// <param name="item">The change event arguments containing the new item count.</param>
	/// <remarks>
	///     This method updates the item count of the requisition and refreshes the grid.
	///     It also saves the updated search model to the session storage.
	///     This method is not executed if an action is already in progress.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task ChangeItemCount(ChangeEventArgs<int, IntValues> item)
	{
		if (!_actionProgress)
		{
			_actionProgress = true;
			_currentPage = 1;
			SearchModel.Page = 1;
			SearchModel.ItemCount = item.Value;

			await SessionStorage.SetItemAsync(StorageName, SearchModel);
			await Grid.Refresh();
			_actionProgress = false;
			StateHasChanged();
		}
	}

	/// <summary>
	///     Asynchronously clears the filter applied to the requisition grid. This method is invoked when the filter needs to
	///     be reset.
	///     It resets the search model, current page, and autocomplete value, and then refreshes the grid.
	///     The method is designed to prevent concurrent execution, meaning if the method is already in progress, subsequent
	///     calls will be ignored until the first call is completed.
	/// </summary>
	/// <param name="arg">The mouse event arguments. This parameter is not used in the method.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task ClearFilter(MouseEventArgs arg)
	{
		if (!_actionProgress)
		{
			_actionProgress = true;
			int _currentPageItemCount = SearchModel.ItemCount;
			SearchModel.Clear();
			_currentPage = 1;
			SearchModel.Page = 1;
			SearchModel.ItemCount = _currentPageItemCount;
			SearchModel.User = LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant();
			await SessionStorage.SetItemAsync(StorageName, SearchModel);
			AutocompleteValue = "";
			await Grid.Refresh();
			_actionProgress = false;
		}
	}

	/// <summary>
	///     Handles the click event of the Clear link in the requisition page.
	///     This method resets the search model and refreshes the requisition grid.
	/// </summary>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task ClearLinkClicked()
	{
		if (!_actionProgress)
		{
			_actionProgress = true;
			_currentPage = 1;
			SearchModel.Status = "";
			SearchModel.Page = 1;
			await SessionStorage.SetItemAsync(StorageName, SearchModel);
			await Grid.Refresh();
			_actionProgress = false;
		}
	}

	/// <summary>
	///     Handles the data processing for the requisition. This method is responsible for creating a reference to the current
	///     instance of the Requisition class,
	///     invoking a JavaScript function to manage detail rows, and managing the selection and expansion of rows in the
	///     requisition grid based on the RequisitionID.
	///     If the total item count in the grid is greater than zero, it checks if the RequisitionID is greater than zero. If
	///     so, it selects and expands the corresponding row.
	///     If the RequisitionID is not greater than zero, it selects the first row. After the operations, it resets the
	///     RequisitionID to zero.
	/// </summary>
	/// <param name="obj">The object that triggers the data handling.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task DataHandler(object obj)
	{
		DotNetObjectReference<Requisition> _dotNetReference = DotNetObjectReference.Create(this); // create dotnet ref
		await Runtime.InvokeAsync<string>("detail", _dotNetReference);
		//  send the dotnet ref to JS side
		if (Grid.TotalItemCount > 0)
		{
			if (RequisitionID > 0)
			{
				int _index = await Grid.GetRowIndexByPrimaryKeyAsync(RequisitionID);
				if (_index != Grid.SelectedRowIndex)
				{
					await Grid.SelectRowAsync(_index);
					foreach (Requisitions _requisition in Grid.CurrentViewData.OfType<Requisitions>().Where(requisitions => requisitions.ID == RequisitionID))
					{
						await Grid.ExpandCollapseDetailRowAsync(_requisition);
						break;
					}
				}

				await SessionStorage.SetItemAsync(StorageName, SearchModel);
				await SessionStorage.RemoveItemAsync("RequisitionIDFromDashboard");
			}
			else
			{
				await Grid.SelectRowAsync(0);
			}
		}

		RequisitionID = 0;
	}

	/// <summary>
	///     Asynchronously deletes a document associated with a requisition.
	/// </summary>
	/// <param name="args">The ID of the document to be deleted.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the 'Requisition/DeleteRequisitionDocument' endpoint of the API.
	///     The document ID and user ID are passed as query parameters in the request.
	///     If the deletion is successful, the list of requisition documents is updated.
	///     This method is safe to call concurrently. If the method is already in progress, subsequent calls will return
	///     immediately.
	/// </remarks>
	private async Task DeleteDocument(int args)
	{
		if (!_actionProgress)
		{
			await Task.Yield();
			_actionProgress = true;
			try
			{
				RestClient _client = new($"{Start.ApiHost}");
				RestRequest _request = new("Requisition/DeleteRequisitionDocument", Method.Post)
									   {
										   RequestFormat = DataFormat.Json
									   };
				_request.AddQueryParameter("documentID", args.ToString());
				_request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());

				Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
				if (_response == null)
				{
					return;
				}

				_requisitionDocumentsObject = General.DeserializeObject<List<RequisitionDocuments>>(_response["Document"]);
			}
			catch
			{
				//
			}

			_actionProgress = false;
		}
	}

	/// <summary>
	///     Asynchronously binds the detail data of a requisition.
	/// </summary>
	/// <param name="requisition">The event arguments containing the requisition data to be bound.</param>
	/// <remarks>
	///     This method performs several operations:
	///     - It checks if a previous action is in progress, and if not, sets the flag to indicate that an action is now in
	///     progress.
	///     - It compares the current target requisition with the incoming requisition data. If they are different, it
	///     collapses the detail row of the current target.
	///     - It retrieves the row index of the incoming requisition data in the grid and selects the corresponding row.
	///     - It sets the current target to the incoming requisition data.
	///     - It sends a REST request to the "Requisition/GetRequisitionDetails" endpoint with the ID of the current target as
	///     a query parameter.
	///     - If the REST response is not null, it deserializes the response into the appropriate objects and sets the skills.
	///     - It sets the selected tab based on whether there is any candidate activity.
	///     - Finally, it resets the action progress flag to false.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task DetailDataBind(DetailDataBoundEventArgs<Requisitions> requisition)
	{
		if (!_actionProgress)
		{
			_actionProgress = true;
			if (_target != null && _target != requisition.Data)
			{
				// return when target is equal to args.data
				await Grid.ExpandCollapseDetailRowAsync(_target);
			}

			int _index = await Grid.GetRowIndexByPrimaryKeyAsync(requisition.Data.ID);
			if (_index != Grid.SelectedRowIndex)
			{
				await Grid.SelectRowAsync(_index);
			}

			_target = requisition.Data;

			await Task.Yield();
			try
			{
				await Spinner.ShowAsync();
			}
			catch
			{
				//
			}

			RestClient _restClient = new($"{Start.ApiHost}");
			RestRequest request = new("Requisition/GetRequisitionDetails");
			request.AddQueryParameter("requisitionID", _target.ID);

			Dictionary<string, object> _restResponse = await _restClient.GetAsync<Dictionary<string, object>>(request);

			if (_restResponse != null)
			{
				_requisitionDetailsObject = JsonConvert.DeserializeObject<RequisitionDetails>(_restResponse["Requisition"]?.ToString() ?? string.Empty);
				_candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_restResponse["Activity"]);
				_requisitionDocumentsObject = General.DeserializeObject<List<RequisitionDocuments>>(_restResponse["Documents"]);
				SetSkills();
			}

			_selectedTab = _candidateActivityObject.Count > 0 ? 2 : 0;

			await Task.Yield();
			try
			{
				await Spinner.HideAsync();
			}
			catch
			{
				//
			}

			_actionProgress = false;
		}
	}

	/// <summary>
	///     Collapses the detail row in the requisition grid. This method is invoked when the detail row in the grid is
	///     collapsed.
	///     It sets the target requisition to null.
	/// </summary>
	[JSInvokable("DetailCollapse")]
	private void DetailRowCollapse() => _target = null;

	/// <summary>
	///     Initiates the download of a document associated with a requisition.
	/// </summary>
	/// <param name="args">The identifier of the document to be downloaded.</param>
	/// <remarks>
	///     This method is asynchronous and may not complete immediately. It first checks if a download action is already in
	///     progress,
	///     and if not, it sets the selected download to the document corresponding to the provided identifier.
	///     It then constructs a query string and invokes a JavaScript function to open the download link in a new browser tab.
	///     After the download is initiated, the method resets the action progress indicator.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task DownloadDocument(int args)
	{
		if (!_actionProgress)
		{
			await Task.Yield();
			_actionProgress = true;
			SelectedDownload = DocumentsPanel.SelectedRow;
			string _queryString = (SelectedDownload.DocumentFileName + "^" + _target.ID + "^" + SelectedDownload.OriginalFileName + "^1").ToBase64String();
			await JsRuntime.InvokeVoidAsync("open", $"{NavManager.BaseUri}Download/{_queryString}", "_blank");
			_actionProgress = false;
		}
	}

	/// <summary>
	///     Initiates the editing process for an activity associated with a requisition.
	///     This method is asynchronous and can be awaited. It first checks if an edit is already in progress,
	///     if not, it sets the selected activity for editing, clears the next steps, and populates it with the possible
	///     next steps based on the current status of the activity. It then shows the dialog for editing the activity.
	/// </summary>
	/// <param name="args">
	///     The identifier of the activity to be edited.
	/// </param>
	/// <returns>
	///     A <see cref="Task" /> representing the asynchronous operation.
	/// </returns>
	private async Task EditActivity(int args)
	{
		if (!_editInProgress)
		{
			await Task.Yield();
			_editInProgress = true;
			SelectedActivity = ActivityPanel.SelectedRow;
			NextSteps.Clear();
			NextSteps.Add(new("No Change", ""));
			try
			{
				foreach (string[] _next in _workflows.Where(flow => flow.Step == SelectedActivity.StatusCode).Select(flow => flow.Next.Split(',')))
				{
					foreach (string _nextString in _next)
					{
						foreach (StatusCode _status in _statusCodes.Where(status => status.Code == _nextString && status.AppliesToCode == "SCN"))
						{
							NextSteps.Add(new(_status.Status, _nextString));
							break;
						}
					}

					break;
				}
			}
			catch
			{
				//
			}

			_editInProgress = false;
			await DialogActivity.ShowDialog();
		}
	}

	/// <summary>
	///     Initiates the process of editing a requisition. This method is asynchronous.
	/// </summary>
	/// <param name="isAdd">
	///     A boolean value that determines whether a new requisition is being added or an existing one is being edited.
	///     If true, a new requisition is being added. If false, an existing requisition is being edited.
	/// </param>
	/// <remarks>
	///     This method performs several actions:
	///     - It shows a spinner to indicate that a process is running.
	///     - It sets the title of the dialog box based on whether a new requisition is being added or an existing one is being
	///     edited.
	///     - It creates a new requisition or clears the data of an existing one based on the value of the isAdd parameter.
	///     - It triggers a state change in the component.
	///     - It shows the dialog box for editing the requisition.
	///     - It hides the spinner once the process is complete.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task EditRequisition(bool isAdd)
	{
		await Task.Yield();
		if (isAdd)
		{
			try
			{
				await SpinnerTop.ShowAsync();
			}
			catch
			{
				//
			}
		}
		else
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
			if (_requisitionDetailsObjectClone == null)
			{
				_requisitionDetailsObjectClone = new();
			}
			else
			{
				_requisitionDetailsObjectClone.Clear();
			}
		}
		else
		{
			Title = "Edit";
			_requisitionDetailsObjectClone = _requisitionDetailsObject.Copy();
		}

		StateHasChanged();
		await DialogEditRequisition.ShowDialog();
		if (isAdd)
		{
			try
			{
				await SpinnerTop.HideAsync();
			}
			catch
			{
				//
			}
		}
		else
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
	///     Filters the requisition grid based on the provided requisition value.
	///     The method updates the search model's title with the provided requisition value,
	///     resets the current page to 1, and refreshes the grid to reflect the changes.
	///     If the provided requisition value is the same as the last value, the method returns without making any changes.
	/// </summary>
	/// <param name="requisition">
	///     The requisition value to filter the grid. This is a <see cref="ChangeEventArgs{TValue, TKey}" /> where TValue is
	///     string and TKey is <see cref="KeyValues" />.
	///     The value of the requisition is used to update the search model's title.
	/// </param>
	private void FilterGrid(ChangeEventArgs<string, KeyValues> requisition)
	{
		string _currentValue = requisition.Value ?? "";
		if (_currentValue.Equals(_lastValue))
		{
			return;
		}

		_lastValue = _currentValue;
		SearchModel.Title = requisition.Value ?? "";
		_currentPage = 1;
		SearchModel.Page = 1;
		SessionStorage.SetItemAsync(StorageName, SearchModel);
		AutocompleteValue = SearchModel.Title;
		Grid.Refresh();
	}

	/// <summary>
	///     Handles the first click action in the requisition grid. This method resets the current page to the first page,
	///     updates the search model's page property to 1, and refreshes the grid. This method is designed to prevent multiple
	///     actions from being processed at the same time by using the _actionProgress flag.
	/// </summary>
	private void FirstClick()
	{
		if (_actionProgress)
		{
			return;
		}

		_actionProgress = true;
		if (_currentPage < 1)
		{
			_currentPage = 1;
		}

		_currentPage = 1;
		SearchModel.Page = 1;
		SessionStorage.SetItemAsync(StorageName, SearchModel);
		Grid.Refresh();
		_actionProgress = false;
	}

	/// <summary>
	///     Handles the last click action in the requisition grid. This method sets the current page to the last page,
	///     updates the search model's page property to the last page number, and refreshes the grid. This method is designed
	///     to prevent multiple
	///     actions from being processed at the same time by using the _actionProgress flag.
	/// </summary>
	private void LastClick()
	{
		if (_actionProgress)
		{
			return;
		}

		_actionProgress = true;
		if (_currentPage < 1)
		{
			_currentPage = 1;
		}

		_currentPage = PageCount.ToInt32();
		SearchModel.Page = _currentPage;
		SessionStorage.SetItemAsync(StorageName, SearchModel);
		Grid.Refresh();
		_actionProgress = false;
	}

	/// <summary>
	///     Handles the link click action in the requisition grid. This method resets the current page to the first page,
	///     updates the search model's status and page properties, and refreshes the grid. This method is designed to prevent
	///     multiple
	///     actions from being processed at the same time by using the _actionProgress flag.
	/// </summary>
	/// <param name="args">The status argument passed when the link is clicked.</param>
	private void LinkClicked(string args)
	{
		if (_actionProgress)
		{
			return;
		}

		_actionProgress = true;
		_currentPage = 1;
		SearchModel.Status = args;
		SearchModel.Page = 1;
		SessionStorage.SetItemAsync(StorageName, SearchModel);
		Grid.Refresh();
		_actionProgress = false;
	}

	/// <summary>
	///     Handles the next click action in the requisition grid. This method increments the current page by one,
	///     updates the search model's page property, and refreshes the grid. This method is designed to prevent multiple
	///     actions from being processed at the same time by using the _actionProgress flag.
	/// </summary>
	private void NextClick()
	{
		if (!_actionProgress)
		{
			_actionProgress = true;
			if (_currentPage < 1)
			{
				_currentPage = 1;
			}

			_currentPage = SearchModel.Page >= PageCount.ToInt32() ? PageCount.ToInt32() : SearchModel.Page + 1;
			SearchModel.Page = _currentPage;
			SessionStorage.SetItemAsync(StorageName, SearchModel);
			Grid.Refresh();
			_actionProgress = false;
		}
	}

	/// <summary>
	///     Handles the action begin event in the requisition grid. This method updates the search model's sort field and sort
	///     direction properties based on the
	///     action event arguments, and refreshes the grid. This method is designed to prevent multiple
	///     actions from being processed at the same time by using the _actionProgress flag.
	/// </summary>
	/// <param name="args">The action event arguments.</param>
	private void OnActionBegin(ActionEventArgs<Requisitions> args)
	{
		if (_actionProgress)
		{
			return;
		}

		_actionProgress = true;

		if (args.RequestType == Action.Sorting)
		{
			SearchModel.SortField = args.ColumnName switch
									{
										"Code" => 2,
										"Title" => 3,
										"Company" => 4,
										"Option" => 5,
										"Status" => 6,
										"DueEnd" => 8,
										_ => 1
									};
			SearchModel.SortDirection = args.Direction == SortDirection.Ascending ? (byte)1 : (byte)0;
			SessionStorage.SetItemAsync(StorageName, SearchModel);
			Grid.Refresh();
		}

		_actionProgress = false;
	}

	/// <summary>
	///     Asynchronously initializes the Requisition component.
	/// </summary>
	/// <remarks>
	///     <para>
	///         This method is invoked when the component is first initialized. It performs several operations:
	///     </para>
	///     <para>
	///         - Checks if all required objects are not null, otherwise throws an ArgumentNullException.
	///     </para>
	///     <para>
	///         - Retrieves and sets the LoginCookyUser from the NavigationManager.
	///     </para>
	///     <para>
	///         - Retrieves the RequisitionGrid and RequisitionIDFromDashboard from the SessionStorage.
	///     </para>
	///     <para>
	///         - If the RequisitionID is not set, it deserializes the RequisitionSearch from the RequisitionGrid.
	///     </para>
	///     <para>
	///         - If the RequisitionID is set, it initializes a new RequisitionSearch with default values.
	///     </para>
	///     <para>
	///         - Retrieves various data from the MemoryCache, such as States, Eligibility, Education, Experience, JobOptions,
	///         Recruiters, Skills, StatusCodes, Preferences, Companies,
	///         and Workflow.
	///     </para>
	///     <para>
	///         - Sets the SortDirectionProperty and SortField based on the SearchModel.
	///     </para>
	///     <para>
	///         - Sets the AutocompleteValue to the Title of the SearchModel.
	///     </para>
	///     <para>
	///         - Marks the component as loaded and refreshes the Grid.
	///     </para>
	/// </remarks>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	protected override async Task OnInitializedAsync()
	{
		_loaded = false;
		LoginCookyUser = await NavManager.RedirectInner(LocalStorage);
		//IMemoryCache _memoryCache = Start.MemCache;
		while (_roles == null)
		{
			_roles = await Redis.GetAsync<List<Role>>("Roles");
			//_memoryCache.TryGetValue("Roles", out _roles);
		}

		RoleID = LoginCookyUser.RoleID;
		UserRights = LoginCookyUser.GetUserRights(_roles);
		DisplayAdd = UserRights.EditRequisition ? "unset" : "none";

		if (!UserRights.ViewRequisition) // User doesn't have View Requisition rights. This is done by looping through the Roles of the current user and determining the rights for ViewRequisition.
		{
			NavManager.NavigateTo($"{NavManager.BaseUri}home", true);
		}

		User = LoginCookyUser?.UserID.NullOrWhiteSpace() != false ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant();

		string _cookyString = await SessionStorage.GetItemAsync<string>(StorageName);
		string _tempRequisitionID = await SessionStorage.GetItemAsStringAsync("RequisitionIDFromDashboard");
		if (!_tempRequisitionID.NullOrWhiteSpace())
		{
			RequisitionID = _tempRequisitionID.ToInt32();
		}

		if (!_cookyString.NullOrWhiteSpace() && RequisitionID == 0)
		{
			SearchModel = JsonConvert.DeserializeObject<RequisitionSearch>(_cookyString);
		}
		else
		{
			SearchModel.Company = "%";
			SearchModel.Status = "NEW,OPN,PAR";
			SearchModel.CreatedOn = new(2010, 1, 1);
			SearchModel.CreatedOnEnd = DateTime.Today.AddYears(1);
			SearchModel.Due = new(2010, 1, 1);
			SearchModel.DueEnd = DateTime.Today.AddYears(2);
			SearchModel.CreatedBy = "A";
			SearchModel.ItemCount = 25;
			SearchModel.Page = 1;

			await SessionStorage.SetItemAsync(StorageName, SearchModel);
		}

		_currentPage = SearchModel.Page;
		_lastValue = SearchModel.Title;

		while (_states == null)
		{
			_states = await Redis.GetAsync<List<IntValues>>("States");
		}

		while (_eligibility == null)
		{
			_eligibility = await Redis.GetAsync<List<IntValues>>("Eligibility");
		}

		while (_education == null)
		{
			_education = await Redis.GetAsync<List<IntValues>>("Education");
		}

		while (_experience == null)
		{
			_experience = await Redis.GetAsync<List<IntValues>>("Experience");
		}

		while (_jobOptions == null)
		{
			_jobOptions = await Redis.GetAsync<List<KeyValues>>("JobOptions");
		}

		while (_recruiters == null)
		{
			List<User> _users = await Redis.GetAsync<List<User>>("Users");
			//_memoryCache.TryGetValue("Users", out List<User> _users);
			if (_users == null)
			{
				continue;
			}

			_recruiters = [];
			foreach (User _user in _users.Where(user => user.Role is "Recruiter" or "Recruiter & Sales Manager"))
			{
				_recruiters?.Add(new(_user.UserName, _user.UserName));
			}
		}

		while (_skills == null)
		{
			_skills = await Redis.GetAsync<List<IntValues>>("Skills");
		}

		_statusCodes = await Redis.GetAsync<List<StatusCode>>("StatusCodes");
		_preference = await Redis.GetAsync<Preferences>("Preferences");

		if (_statusCodes is {Count: > 0})
		{
			foreach (StatusCode _statusCode in _statusCodes.Where(statusCode => statusCode.AppliesToCode == "REQ"))
			{
				_statusSearch.Add(new(_statusCode.Status, _statusCode.Code));
			}
		}

		List<Company> _companyList = await Redis.GetAsync<List<Company>>("Companies");
		_companies.Add(new("All Companies", "%"));
		if (_companyList != null)
		{
			foreach (Company _company in _companyList.Where(company => company.Owner == User || company.Owner == "ADMIN"))
			{
				_companies.Add(new(_company.CompanyName, _company.CompanyName));
			}
		}

		_workflows = await Redis.GetAsync<List<AppWorkflow>>("Workflow");

		SortDirectionProperty = SearchModel.SortDirection == 1 ? SortDirection.Ascending : SortDirection.Descending;
		SortField = SearchModel.SortField switch
					{
						2 => "Code",
						3 => "Title",
						4 => "Company",
						5 => "Option",
						6 => "Status",
						8 => "DueEnd",
						_ => "Updated"
					};
		AutocompleteValue = SearchModel.Title;

		_loaded = true;
		await Grid.Refresh();
		await base.OnInitializedAsync();
	}

	/// <summary>
	///     Handles the event of page number change in the requisition grid.
	/// </summary>
	/// <param name="obj">The event arguments containing the new page number.</param>
	/// <remarks>
	///     This method updates the current page number and refreshes the grid to display the requisitions for the new page.
	///     It also prevents multiple simultaneous actions by checking the _actionProgress flag.
	/// </remarks>
	private void PageNumberChanged(ChangeEventArgs obj)
	{
		if (_actionProgress)
		{
			return;
		}

		_actionProgress = true;
		decimal _currentValue = obj.Value.ToInt32();
		if (_currentValue < 1)
		{
			_currentValue = 1;
		}
		else if (_currentValue > PageCount)
		{
			_currentValue = PageCount.ToInt32();
		}

		_currentPage = _currentValue.ToInt32();
		SearchModel.Page = _currentPage;
		SessionStorage.SetItemAsync(StorageName, SearchModel);
		Grid.Refresh();
		_actionProgress = false;
	}

	/// <summary>
	///     Handles the click event for the "Previous" button in the requisition grid.
	///     This method decrements the current page number and refreshes the grid to display the previous page of requisitions.
	///     If the current page is the first page, the page number will not be decremented.
	/// </summary>
	private void PreviousClick()
	{
		if (_actionProgress)
		{
			return;
		}

		_actionProgress = true;
		if (_currentPage < 1)
		{
			_currentPage = 1;
		}

		_currentPage = SearchModel.Page <= 1 ? 1 : SearchModel.Page - 1;
		SearchModel.Page = _currentPage;
		SessionStorage.SetItemAsync(StorageName, SearchModel);
		Grid.Refresh();
		_actionProgress = false;
	}

	/// <summary>
	///     Refreshes the grid of requisitions. This method is used to update the grid display whenever there are changes to
	///     the requisitions data.
	/// </summary>
	private static void RefreshGrid() => Grid.Refresh();

	/// <summary>
	///     Executes an advanced search on requisitions based on the parameters specified in the SearchModelClone.
	///     The search results are then refreshed in the Grid.
	/// </summary>
	/// <param name="args">The context of the edit operation triggering the advanced search.</param>
	private void RequisitionAdvancedSearch(EditContext args)
	{
		SessionStorage.SetItemAsync(StorageName, SearchModelClone);
		SearchModel = SearchModelClone.Copy();
		Grid.Refresh();
	}

	/// <summary>
	///     Asynchronously saves the activity related to a requisition.
	/// </summary>
	/// <param name="activity">
	///     The context of the activity to be saved. This includes the details of the activity.
	/// </param>
	/// <remarks>
	///     This method makes a POST request to the "Candidates/SaveCandidateActivity" endpoint with the activity details.
	///     The request includes additional parameters such as the user, candidate ID, and a flag indicating whether the
	///     request is from the candidate screen.
	///     If the request is successful, the response is deserialized into a list of candidate activities.
	/// </remarks>
	/// <returns>
	///     A task that represents the asynchronous operation.
	/// </returns>
	private async Task SaveActivity(EditContext activity)
	{
		await Task.Yield();

		try
		{
			RestClient _client = new($"{Start.ApiHost}");
			RestRequest _request = new("Candidates/SaveCandidateActivity", Method.Post)
								   {
									   RequestFormat = DataFormat.Json
								   };
			_request.AddJsonBody(activity.Model);
			_request.AddQueryParameter("candidateID", _target.ID);
			_request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
			_request.AddQueryParameter("roleID", LoginCookyUser == null || LoginCookyUser.RoleID.NullOrWhiteSpace() ? "RS" : LoginCookyUser.RoleID.ToUpperInvariant());
			_request.AddQueryParameter("isCandidateScreen", false);
			_request.AddQueryParameter("jsonPath", Start.JsonFilePath);
			_request.AddQueryParameter("emailAddress", LoginCookyUser == null || LoginCookyUser.Email.NullOrWhiteSpace() ? "info@titan-techs.com" : LoginCookyUser.Email.ToUpperInvariant());
			_request.AddQueryParameter("uploadPath", Start.UploadsPath);

			Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
			if (_response == null)
			{
				return;
			}

			_candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_response["Activity"]);
		}
		catch
		{
			//
		}
	}

	/// <summary>
	///     Asynchronously saves the document associated with the requisition.
	/// </summary>
	/// <param name="document">
	///     The <see cref="EditContext" /> instance that contains the document to be saved.
	/// </param>
	/// <returns>
	///     A <see cref="Task" /> that represents the asynchronous operation.
	/// </returns>
	/// <remarks>
	///     This method uses the REST API to upload the document. The document is converted to a byte array and sent as a file
	///     in a multipart form data request. Additional parameters, such as filename, mime type, document name, notes,
	///     requisition ID, user, and path are also sent with the request.
	/// </remarks>
	private async Task SaveDocument(EditContext document)
	{
		await Task.Yield();
		try
		{
			if (document.Model is RequisitionDocuments _document)
			{
				RestClient _client = new($"{Start.ApiHost}");
				RestRequest _request = new("Requisition/UploadDocument", Method.Post)
									   {
										   AlwaysMultipartFormData = true
									   };
				_request.AddFile("file", AddedDocument.ToStreamByteArray(), FileName);
				_request.AddParameter("filename", FileName, ParameterType.GetOrPost);
				_request.AddParameter("mime", Mime, ParameterType.GetOrPost);
				_request.AddParameter("name", _document.DocumentName, ParameterType.GetOrPost);
				_request.AddParameter("notes", _document.DocumentNotes, ParameterType.GetOrPost);
				_request.AddParameter("requisitionID", _target.ID.ToString(), ParameterType.GetOrPost);
				_request.AddParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant(), ParameterType.GetOrPost);
				_request.AddParameter("path", Start.UploadsPath, ParameterType.GetOrPost);
				Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
				if (_response == null)
				{
					return;
				}

				_requisitionDocumentsObject = General.DeserializeObject<List<RequisitionDocuments>>(_response["Document"]);
			}
		}
		catch
		{
			//
		}
	}

	/// <summary>
	///     Asynchronously saves the requisition details.
	/// </summary>
	/// <param name="arg">The edit context of the requisition details.</param>
	/// <remarks>
	///     This method performs the following steps:
	///     1. Creates a new REST client and request.
	///     2. Adds the cloned requisition details object to the request body in JSON format.
	///     3. Adds the user ID, JSON file path, and email address to the request as query parameters.
	///     4. Sends the request to the server.
	///     5. Updates the requisition details object with the cloned object.
	///     6. If the requisition ID is greater than 0, updates the target's title, company, job options, status, and priority
	///     color.
	///     7. If the requisition ID is not greater than 0, clears the search model data and refreshes the grid.
	///     8. Triggers a state change to update the UI.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task SaveRequisition(EditContext arg)
	{
		await Task.Yield();

		RestClient _client = new($"{Start.ApiHost}");
		RestRequest _request = new("Requisition/SaveRequisition", Method.Post)
							   {
								   RequestFormat = DataFormat.Json
							   };
		_request.AddJsonBody(_requisitionDetailsObjectClone);
		_request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
		_request.AddQueryParameter("jsonPath", Start.JsonFilePath);
		_request.AddQueryParameter("emailAddress", LoginCookyUser == null || LoginCookyUser.Email.NullOrWhiteSpace() ? "maniv@titan-techs.com" : LoginCookyUser.Email.ToUpperInvariant());

		await _client.PostAsync<int>(_request);

		_requisitionDetailsObject = _requisitionDetailsObjectClone.Copy();

		if (_requisitionDetailsObject.RequisitionID > 0)
		{
			_target.Title = $"{_requisitionDetailsObject.PositionTitle} ({_candidateActivityObject.Count})";
			_target.Company = _requisitionDetailsObject.CompanyName;
			_target.JobOptions = _requisitionDetailsObject.JobOptions;
			_target.Status = _requisitionDetailsObject.Status;
			_target.PriorityColor = _requisitionDetailsObject.Priority.ToUpperInvariant() switch
									{
										"HIGH" => _preference.HighPriorityColor,
										"LOW" => _preference.LowPriorityColor,
										_ => _preference.NormalPriorityColor
									};
		}
		else
		{
			SearchModel.Clear();
			await Grid.Refresh();
		}

		await Task.Yield();
		StateHasChanged();
	}

	/// <summary>
	///     Sets the alphabet for the search model's title and refreshes the grid.
	/// </summary>
	/// <param name="alphabet">The character to set as the title of the search model.</param>
	/// <remarks>
	///     This method is used to filter the requisitions grid based on the first letter of the requisition's title.
	///     It sets the title of the search model to the provided alphabet character, resets the page count to 1,
	///     and refreshes the grid to display the filtered results.
	/// </remarks>
	private void SetAlphabet(char alphabet)
	{
		if (_actionProgress)
		{
			return;
		}

		_actionProgress = true;
		SearchModel.Title = alphabet.ToString();
		_currentPage = 1;
		SearchModel.Page = 1;
		SessionStorage.SetItemAsync(StorageName, SearchModel);
		AutocompleteValue = alphabet.ToString();
		Grid.Refresh();
		_actionProgress = false;
	}

	/// <summary>
	///     Sets the skills for the requisition details object.
	/// </summary>
	/// <remarks>
	///     This method sets the required and optional skills for the requisition details object.
	///     The skills are retrieved from the SkillsRequired and Optional properties of the requisition details object.
	///     The skills are then formatted into a string and converted to a MarkupString which is stored in the
	///     _requisitionDetailSkills field.
	///     If the requisition details object is null or both the SkillsRequired and Optional properties are null or
	///     whitespace, the method will return without setting the skills.
	/// </remarks>
	private void SetSkills()
	{
		if (_requisitionDetailsObject == null)
		{
			return;
		}

		if (_requisitionDetailsObject.SkillsRequired.NullOrWhiteSpace() && _requisitionDetailsObject.Optional.NullOrWhiteSpace())
		{
			return;
		}

		_requisitionDetailSkills = "".ToMarkupString();

		string[] _skillRequiredStrings = [], _skillOptionalStrings = [];
		if (_requisitionDetailsObject.SkillsRequired != "")
		{
			_skillRequiredStrings = _requisitionDetailsObject.SkillsRequired.Split(',');
		}

		if (_requisitionDetailsObject.Optional != "")
		{
			_skillOptionalStrings = _requisitionDetailsObject.Optional.Split(',');
		}

		string _skillsRequired = "", _skillsOptional = "";
		foreach (string _skillString in _skillRequiredStrings)
		{
			IntValues _skill = _skills.FirstOrDefault(skill => skill.Key == _skillString.ToInt32());
			if (_skill == null)
			{
				continue;
			}

			if (_skillsRequired == "")
			{
				_skillsRequired = _skill.Value;
			}
			else
			{
				_skillsRequired += ", " + _skill.Value;
			}
		}

		foreach (string _skillString in _skillOptionalStrings)
		{
			IntValues _skill = _skills.FirstOrDefault(skill => skill.Key == _skillString.ToInt32());
			if (_skill == null)
			{
				continue;
			}

			if (_skillsOptional == "")
			{
				_skillsOptional = _skill.Value;
			}
			else
			{
				_skillsOptional += ", " + _skill.Value;
			}
		}

		string _skillStringTemp = "";

		if (!_skillsRequired.NullOrWhiteSpace())
		{
			_skillStringTemp = "Required Skills: <br/>" + _skillsRequired + "<br/>";
		}

		if (!_skillsOptional.NullOrWhiteSpace())
		{
			_skillStringTemp += "Optional Skills: <br/>" + _skillsOptional;
		}

		_requisitionDetailSkills = _skillStringTemp.ToMarkupString();
	}

	/// <summary>
	///     Handles the click event of a speed dial item.
	/// </summary>
	/// <param name="args">
	///     The arguments of the speed dial item event, which contains the clicked item's ID.
	/// </param>
	/// <returns>
	///     A <see cref="Task" /> representing the asynchronous operation.
	/// </returns>
	/// <remarks>
	///     Depending on the ID of the clicked item, this method performs different actions:
	///     - "itemEditRequisition": Sets the selected tab to 0 and edits the requisition.
	///     - "itemAddDocument": Sets the selected tab to 1 and adds a document.
	///     - "itemSubmitExisting": Sets the selected tab to 2 and submits a candidate.
	///     - "itemSubmitNew": Sets the selected tab to 2.
	/// </remarks>
	private async Task SpeedDialItemClicked(SpeedDialItemEventArgs args)
	{
		if (!_actionProgress)
		{
			await Task.Yield();
			_actionProgress = true;
			switch (args.Item.ID)
			{
				case "itemEditRequisition":
					_selectedTab = 0;
					await EditRequisition(false);
					break;
				case "itemAddDocument":
					_selectedTab = 1;
					await AddDocument();
					break;
				case "itemSubmitExisting":
					_selectedTab = 2;
					await SubmitCandidate();
					break;
				case "itemSubmitNew":
					_selectedTab = 2;
					//await SubmitCandidate();
					break;
			}

			_actionProgress = false;
		}
	}

	/// <summary>
	///     Navigates to the candidate page for the current requisition.
	/// </summary>
	/// <remarks>
	///     This method is used to submit a candidate for the current requisition. It navigates to the candidate page, passing
	///     the ID of the current requisition as a parameter in the URL.
	/// </remarks>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	private async Task SubmitCandidate()
	{
		await Task.Yield();
		NavManager.NavigateTo($"{NavManager.BaseUri}candidate?requisition={_target.ID}", true);
	}

	/// <summary>
	///     Handles the event when a tab is selected in the user interface.
	/// </summary>
	/// <param name="args">
	///     Contains the event data, which includes the index of the selected tab.
	/// </param>
	/// <returns>
	///     A <see cref="Task" /> representing the asynchronous operation.
	/// </returns>
	private async Task TabSelected(SelectEventArgs args)
	{
		await Task.Yield();
		_selectedTab = args.SelectedIndex;
	}

	/// <summary>
	///     Asynchronously undoes a candidate activity based on the provided activity ID.
	/// </summary>
	/// <param name="activityID">The ID of the candidate activity to undo.</param>
	/// <remarks>
	///     This method sends a POST request to the "Candidates/UndoCandidateActivity" endpoint of the API.
	///     The request includes the activity ID, the user ID (or "JOLLY" if the user ID is null or whitespace), and a flag
	///     indicating that this is not a candidate screen.
	///     If the API response is not null, it deserializes the "Activity" field of the response into a list of
	///     CandidateActivity objects.
	/// </remarks>
	/// <returns>
	///     A task that represents the asynchronous operation.
	/// </returns>
	private async Task UndoActivity(int activityID)
	{
		await Task.Yield();

		try
		{
			RestClient _client = new($"{Start.ApiHost}");
			RestRequest _request = new("Candidates/UndoCandidateActivity", Method.Post)
								   {
									   RequestFormat = DataFormat.Json
								   };
			_request.AddQueryParameter("submissionID", activityID);
			_request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
			_request.AddQueryParameter("isCandidateScreen", false);

			Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
			if (_response == null)
			{
				return;
			}

			_candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_response["Activity"]);
		}
		catch
		{
			//
		}

		await Task.Yield();
	}

	/// <summary>
	///     Handles the upload of a document. It reads the uploaded file, copies it to a memory stream,
	///     and stores the file name and MIME type for further processing.
	/// </summary>
	/// <param name="file">The uploaded file encapsulated in an UploadChangeEventArgs object.</param>
	/// <remarks>
	///     This method is responsible for handling the upload of a document. It takes an UploadChangeEventArgs object as a
	///     parameter, which encapsulates the uploaded file.
	///     The method reads the uploaded file, copies it to a memory stream, and stores the file name and MIME type for
	///     further processing.
	///     The method is asynchronous and returns a Task.
	/// </remarks>
	/// <returns>
	///     A Task that represents the asynchronous operation.
	/// </returns>
	private async Task UploadDocument(UploadChangeEventArgs file)
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
	}

	/// <summary>
	///     The AdminLeadAdaptor class is a custom data adaptor for the Requisitions page.
	///     It inherits from the DataAdaptor class and overrides the ReadAsync method.
	/// </summary>
	/// <remarks>
	///     The ReadAsync method is used to asynchronously read data for the Requisitions page.
	///     It checks if a read operation is already in progress, and if not, it initiates a new read operation.
	///     The method retrieves lead data using the General.GetRequisitionReadAdaptor method and the provided
	///     DataManagerRequest.
	///     If there are any requisitions, it selects the first one. The method returns the retrieved requisitions data.
	/// </remarks>
	internal class AdminRequisitionAdaptor : DataAdaptor
	{
		private bool _reading;

		/// <summary>
		///     Asynchronously reads data from the Requisition data source.
		/// </summary>
		/// <param name="dm">The DataManagerRequest object that contains the parameters for the data request.</param>
		/// <param name="key">An optional key to identify a specific data record.</param>
		/// <remarks>
		///     This method checks if the data is already being read or if it has not been loaded yet. If either of these
		///     conditions is true, it returns null.
		///     Otherwise, it sets the _reading flag to true and proceeds with the data read operation. It also checks if the
		///     Companies list is not null and has more than 0 items.
		///     If so, it sets the _getInformation flag to true. Then it calls the GetRequisitionReadAdaptor method from the
		///     General class, passing the SearchModel, dm, _getInformation, RequisitionID, and true as parameters.
		///     The result of this method call is stored in the _requisitionReturn object. After the data read operation, it sets
		///     the _currentPage to the Page property of the SearchModel and the _reading flag back to false.
		/// </remarks>
		/// <returns>
		///     A Task that represents the asynchronous operation. The task result contains the data retrieved from the data
		///     source.
		/// </returns>
		public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
		{
			if (_reading || !_loaded)
			{
				return null;
			}

			_reading = true;
			bool _getInformation = true;
			if (Companies != null)
			{
				_getInformation = Companies.Count == 0;
			}

			object _requisitionReturn = await General.GetRequisitionReadAdaptor(SearchModel, dm, _getInformation, RequisitionID, true, User);

			_currentPage = SearchModel.Page;
			//if (Count > 0)
			//{
			//    await Grid.SelectRowAsync(0);
			//}

			_reading = false;
			return _requisitionReturn;
		}
	}
}