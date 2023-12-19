#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Candidate.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     10-30-2023 15:11
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     Represents a Candidate page in the ProfSvc_AppTrack application.
///     This class is responsible for handling and processing candidate data.
/// </summary>
/// <remarks>
///     The Candidate class includes properties such as Count, EndRecord, PageCount, StartRecord, and User.
///     It also includes methods for data handling, rendering, initialization, and a specialized data adaptor for candidate
///     data.
/// </remarks>
public partial class Candidate
{
	private const string StorageName = "CandidateGrid";
	private static TaskCompletionSource<bool> _initializationTaskSource;

	private static int _currentPage = 1;

	private List<CandidateActivity> _candidateActivityObject = [];

	private CandidateDetails _candidateDetailsObject = new(), _candidateDetailsObjectClone = new();
	private List<CandidateDocument> _candidateDocumentsObject = [];
	private List<CandidateEducation> _candidateEducationObject = [];
	private List<CandidateExperience> _candidateExperienceObject = [];
	private List<CandidateMPC> _candidateMPCObject = [];
	private List<CandidateNotes> _candidateNotesObject = [];
	private List<CandidateRating> _candidateRatingObject = [];
	private List<CandidateSkills> _candidateSkillsObject = [];
	private List<KeyValues> _communication, _jobOptions, _taxTerms;
	private List<IntValues> _documentTypes = [], _eligibility, _experience, _states;
	private readonly List<IntValues> _eligibilityCopy = [];

	private readonly List<KeyValues> _jobOptionsCopy = [];

	private string _jsonPath;
	private string _lastValue = "";

	private List<Role> _roles;

	private int _selectedTab;
	private readonly SemaphoreSlim _semaphoreMainPage = new(1, 1);
	private readonly List<IntValues> _statesCopy = [];

	private List<StatusCode> _statusCodes;

	private Candidates _target, _targetSelected;

	private readonly List<ToolbarItemModel> _tools1 =
	[
		new()
		{
			Name = "Original", TooltipText = "Show Original Resume"
		},

		new()
		{
			Name = "Formatted", TooltipText = "Show Formatted Resume"
		}
	];

	private List<AppWorkflow> _workflows;

	/// <summary>
	///     Gets or sets the ActivityPanel instance associated with the Candidate.
	///     This property is used to manage and interact with the candidate's activities.
	/// </summary>
	private ActivityPanel ActivityPanel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ShowAddChoice instance associated with the Candidate page.
	///     This instance is used to display a dialog for adding a new candidate.
	/// </summary>
	private ShowAddChoice AddChoice
	{
		get;
		set;
	}

	/// <summary>
	///     Gets the MemoryStream instance that temporarily holds the uploaded document data.
	/// </summary>
	/// <remarks>
	///     This property is used to store the data of a document uploaded by the user.
	///     The data is then used in various operations such as parsing the resume, saving the document,
	///     uploading the resume, and uploading the document.
	/// </remarks>
	private MemoryStream AddedDocument
	{
		get;
	} = new();

	/// <summary>
	///     Gets or sets the address of the candidate in a markup string format.
	/// </summary>
	/// <value>
	///     The address of the candidate.
	/// </value>
	/// <remarks>
	///     The address is set up by the SetupAddress method, which concatenates the address fields of the candidate's details.
	///     Each part of the address is separated by a comma or a line break.
	///     If a part of the address is empty, it is skipped.
	///     If the generated address starts with a comma, it is removed.
	///     The final address is converted to a markup string and stored in this property.
	/// </remarks>
	private MarkupString Address
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the value for the autocomplete functionality in the Candidate page.
	/// </summary>
	private string AutocompleteValue
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the communication rating of the candidate.
	/// </summary>
	/// <value>
	///     The communication rating of the candidate, represented as a <see cref="MarkupString" />.
	/// </value>
	/// <remarks>
	///     This property is used to store the communication rating of the candidate, which is set by the
	///     <see cref="SetCommunication" /> method.
	///     The communication rating is a string that describes the candidate's communication skills, and can be one of the
	///     following values: "Good", "Average", "Excellent", or "Fair".
	/// </remarks>
	private MarkupString CandidateCommunication
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the eligibility status of the candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to store the eligibility status of a candidate. The eligibility status is determined based on
	///     the `EligibilityID` of the candidate.
	///     If the `EligibilityID` is greater than 0, the eligibility status is set to the corresponding value from the
	///     `_eligibility` collection.
	///     If the `EligibilityID` is not greater than 0, the eligibility status is set to an empty string.
	///     This property is used in the `SetEligibility()` method and in the `BuildRenderTree()` method of the `Candidate`
	///     component.
	/// </remarks>
	private MarkupString CandidateEligibility
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Candidate's experience.
	/// </summary>
	/// <remarks>
	///     This property is used to store the Candidate's experience retrieved from the
	///     `_candidateDetailsObject.ExperienceID`.
	///     If the `ExperienceID` is greater than 0, it fetches the corresponding experience from the `_experience` collection.
	///     Otherwise, it is set to an empty string.
	/// </remarks>
	private MarkupString CandidateExperience
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID of the candidate. This ID is used to select and manipulate candidate data in the Candidate
	///     page.
	/// </summary>
	private static int CandidateID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the markup string representation of the candidate's job options.
	/// </summary>
	/// <remarks>
	///     This property is used to store the job options for a candidate in a format that can be directly used in the UI.
	///     The job options are set by the `SetJobOption` method, which processes the job options list and converts it into a
	///     markup string.
	/// </remarks>
	private MarkupString CandidateJobOptions
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the tax terms for the candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to store the tax terms of a candidate. The tax terms are set by the `SetTaxTerm` method,
	///     which checks the tax terms list and appends the corresponding tax term from the list to the return value.
	///     The return value is then converted to a markup string and set as the `CandidateTaxTerms` property.
	/// </remarks>
	private MarkupString CandidateTaxTerms
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the total count of Candidates.
	/// </summary>
	/// <remarks>
	///     This property is used to store the total count of companies retrieved from the API response in the
	///     `General.GetCandidateReadAdaptor()` method.
	/// </remarks>
	internal static decimal Count
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of EditActivityDialog associated with the Candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to manage and interact with the dialog for editing a candidate's activity.
	///     It is used in the `Candidate.EditActivity()` method where the dialog is shown.
	/// </remarks>
	private EditActivityDialog DialogActivity
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog for adding a candidate document.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the dialog for adding a candidate document in the Candidate page.
	///     It is an instance of the `AddCandidateDocument` component, which allows the user to upload a document to a
	///     company.
	///     The dialog is shown when the `AddDocument` method is called.
	/// </remarks>
	private AddDocumentDialog DialogDocument
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog for editing candidate details.
	/// </summary>
	/// <remarks>
	///     This property is used to handle the dialog box for editing candidate details. It is an instance of the
	///     `EditCandidateDetails`
	///     class.
	///     The dialog is shown when the `ShowDialog()` method is called in the `EditCandidate()` method of the `Candidates`
	///     class.
	/// </remarks>
	private EditCandidateDialog DialogEditCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of EditEducationDialog associated with the Candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to manage and interact with the candidate's education details.
	///     It is used in the `Candidate.EditEducation()` method to show the dialog for editing education details.
	/// </remarks>
	private EditEducationDialog DialogEducation
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the EditExperienceDialog associated with the Candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to manage and interact with the dialog for editing a candidate's experience.
	///     It is used in the `Candidate.EditExperience()` method to show the dialog.
	/// </remarks>
	private EditExperienceDialog DialogExperience
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the MPCCandidateDialog component.
	/// </summary>
	/// <remarks>
	///     This property is used to interact with the MPCCandidateDialog component,
	///     which is a dialog used for managing MPC (Most Placeable Candidate) data.
	///     For example, it is used in the `SpeedDialItemClicked` method to show the dialog
	///     when the "itemEditMPC" speed dial item is clicked.
	/// </remarks>
	private MPCCandidateDialog DialogMPC
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the EditNotesDialog instance associated with the Candidate page.
	///     This instance is used to display and manage the dialog for editing candidate notes.
	/// </summary>
	private EditNotesDialog DialogNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogParseCandidate of type ParseCandidateDialog.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the dialog for parsing a candidate.
	///     It is used in the ParseCandidate method to show the dialog when a mouse click event is triggered.
	/// </remarks>
	private ParseCandidateDialog DialogParseCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the RatingCandidateDialog component.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the dialog for rating a candidate. It is used in the `SpeedDialItemClicked` method
	///     where the `ShowDialog` method of the `DialogRating` is called when the "itemEditRating" case is executed.
	///     The dialog provides an interface for rating a candidate and is defined in the RatingCandidateDialog component.
	/// </remarks>
	private RatingCandidateDialog DialogRating
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the AdvancedCandidateSearch dialog.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the advanced search dialog in the Candidate page.
	///     It is used in the AdvancedSearch method to show the dialog.
	/// </remarks>
	private AdvancedCandidateSearch DialogSearch
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the EditSkillDialog instance associated with the Candidate page.
	///     This instance is used to display and manage the dialog for editing a candidate's skills.
	/// </summary>
	private EditSkillDialog DialogSkill
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the SubmitCandidate dialog.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the SubmitCandidate dialog which is shown when a candidate is selected for
	///     submission.
	///     The dialog is used to handle the submission process of the candidate.
	/// </remarks>
	private SubmitCandidate DialogSubmitCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the display status of the Add button.
	/// </summary>
	/// <remarks>
	///     This property is used to control the visibility of the Add button in the Candidate page.
	///     If the user has the right to edit the candidate, the display status is set to "unset", otherwise it is set to
	///     "none".
	/// </remarks>
	private string DisplayAdd
	{
		get;
		set;
	} = "unset";

	/// <summary>
	///     Gets or sets the display status of the Submit Candidate button.
	/// </summary>
	/// <remarks>
	///     This property is used to control the visibility of the Submit Candidate button in the Candidates page.
	///     If the user has the right to submit a candidate to the requisition, the display status is set to "unset", otherwise
	///     it is set to "none".
	/// </remarks>
	private string DisplaySubmit
	{
		get;
		set;
	} = "unset";

	/// <summary>
	///     Gets or sets the DownloadsPanel instance.
	/// </summary>
	/// <remarks>
	///     This property is used to interact with the DownloadsPanel component. It is used to manage and display
	///     the documents related to a candidate. This includes functionalities such as showing a candidate's resume,
	///     and handling document download and deletion events.
	/// </remarks>
	private DownloadsPanel DownloadsPanel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the EducationPanel instance associated with the Candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to manage and interact with the candidate's education details.
	///     For example, in the `Candidate.EditEducation()` method, the `SelectedEducation` is set to the `SelectedRow` of the
	///     `EducationPanel`.
	/// </remarks>
	private EducationPanel EducationPanel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the index of the last record in the current page of candidates.
	///     This value is calculated as the index of the first record of the current page plus the number of candidates in the
	///     current page.
	/// </summary>
	internal static int EndRecord
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog for displaying the details of an existing candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the dialog that shows the details of an existing candidate.
	///     It is used in the `Candidate.OnFileUpload()` method where it is shown after the candidate details are parsed.
	/// </remarks>
	private ShowExistingCandidateDetails ExistingCandidateDetailsDialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a list of existing candidates.
	/// </summary>
	/// <value>
	///     The list of existing candidates.
	/// </value>
	private List<ExistingCandidate> ExistingCandidateList
	{
		get;
		set;
	} = [];

	/// <summary>
	///     Gets or sets the ExperiencePanel instance associated with the Candidate.
	///     This property is used to manage and interact with the candidate's experiences.
	/// </summary>
	private ExperiencePanel ExperiencePanel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the file being uploaded.
	/// </summary>
	/// <remarks>
	///     This property is used to store the name of the file that is being uploaded in the `Candidate.UploadDocument()`
	///     method.
	///     It is then used in the `Candidate.SaveDocument()` method to add the file to the request for the API call.
	/// </remarks>
	private string FileName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the formatted resume of a candidate exists.
	/// </summary>
	/// <remarks>
	///     This property is set in the `DetailDataBind` method after fetching candidate's details from the API.
	///     It is used in the `BuildRenderTree` method to control the availability of the "Show Formatted Resume" button.
	/// </remarks>
	private bool FormattedExists
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the grid of candidates.
	/// </summary>
	/// <remarks>
	///     This grid is used to display the candidates in the Candidate page. It is refreshed when the search model changes,
	///     such as when the number of items per page is changed or the filter settings are cleared. The grid is also refreshed
	///     after the parsing process for a candidate is continued.
	/// </remarks>
	private static SfGrid<Candidates> Grid
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the candidate is from a company.
	/// </summary>
	/// <value>
	///     true if the candidate is from a company; otherwise, false.
	/// </value>
	/// <remarks>
	///     This property is used to determine the source of the candidate.
	///     It is set in the `OnInitializedAsync` method based on the query parameter "company" in the URL.
	///     It is also used in the `SubmitCandidateToRequisition` method to navigate to the appropriate page based on its
	///     value.
	/// </remarks>
	private bool IsFromCompany
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the IJSRuntime instance.
	/// </summary>
	/// <remarks>
	///     This property is used to interact with JavaScript functions from C# code. For example, it is used in the
	///     `DownloadDocument` method to open a new browser tab for document download.
	/// </remarks>
	[Inject]
	private IJSRuntime JsRuntime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the local storage service.
	/// </summary>
	/// <remarks>
	///     This property is used to interact with the browser's local storage. It is used for storing and retrieving data
	///     that needs to be persisted across browser sessions, such as user preferences and session data.
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
	///     This property is used to store the MIME type of the file being uploaded in the `Candidate.UploadDocument()` method.
	///     The MIME type is retrieved from the FileInfo of the uploaded file.
	///     It is then used as a parameter in the API request in the `Candidate.SaveDocument()` method.
	/// </remarks>
	private string Mime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the MPCDate of the Candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to store the most recent date from the CandidateMPC list, converted to a MarkupString.
	///     It is updated by the `GetMPCDate()` method, which retrieves the most recent date from the CandidateMPC list,
	///     formats it, and assigns it to this property. If the MpcNotes property of the CandidateDetails object is empty,
	///     an empty MarkupString is assigned to this property.
	/// </remarks>
	private MarkupString MPCDate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the most recent note from the CandidateMPC object list.
	/// </summary>
	/// <remarks>
	///     This property is used to store the most recent note retrieved from the CandidateMPC object list.
	///     The note is retrieved in the `GetMPCNote()` method, where the CandidateMPC object with the latest date is found
	///     and its Comments property is assigned to the MPCNote property.
	///     If the MpcNotes property of the _candidateDetailsObject is empty, an empty string is converted to a MarkupString
	///     and assigned to the MPCNote property.
	/// </remarks>
	private MarkupString MPCNote
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the NavigationManager instance.
	/// </summary>
	/// <remarks>
	///     This property is injected and used for managing navigation and working with URIs in the application.
	///     For example, it is used in the `DownloadDocument` method to open a new browser tab for downloading a document.
	/// </remarks>
	[Inject]
	private NavigationManager NavManager
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a new instance of the CandidateDocuments class.
	/// </summary>
	private CandidateDocument NewDocument
	{
		get;
	} = new();

	/// <summary>
	///     Gets or sets a list of KeyValues instances representing the next steps for the candidate.
	/// </summary>
	private List<KeyValues> NextSteps
	{
		get;
	} = [];

	/// <summary>
	///     Gets or sets a value indicating whether the original resume of the candidate exists.
	/// </summary>
	/// <remarks>
	///     This property is used to determine if the original resume of a candidate is available.
	///     It is used in the `DetailDataBind` method where it is set based on the `OriginalResume` property of the candidate
	///     data.
	///     It is also used in the `BuildRenderTree` method to disable the "Show Original Resume" button when the original
	///     resume does not exist.
	/// </remarks>
	private bool OriginalExists
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the total number of pages that can be formed from the candidates' data.
	///     This is calculated by dividing the total count of candidates by the number of items per page, and rounding up to
	///     the
	///     next integer.
	/// </summary>
	internal static int PageCount
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the NotesPanel control used in the Candidate page.
	/// </summary>
	/// <remarks>
	///     This property is used to interact with the NotesPanel control, which provides functionality for displaying and
	///     managing candidate notes.
	///     For example, it is used in the `Candidate.EditNotes()` method to access the selected note.
	/// </remarks>
	private NotesPanel PanelNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the MarkupString representation of the candidate's rating date.
	/// </summary>
	/// <remarks>
	///     This property is used to store the formatted string representation of the candidate's rating date.
	///     The value is set in the `GetRatingDate()` method, where the maximum (latest) date from the candidate's
	///     rating list is fetched, formatted into a string, and then converted into a MarkupString.
	/// </remarks>
	private MarkupString RatingDate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the RatingMPC property of the Candidate class. This property represents an instance of the
	///     CandidateRatingMPC class.
	/// </summary>
	private CandidateRatingMPC RatingMPC
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the RatingNote for a candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to store the rating note of a candidate. The rating note is a MarkupString
	///     that is displayed in the user interface. It is set in the `GetRatingNote` method, where it is
	///     either set to an empty string if the candidate's rating notes are empty, or to the most recent
	///     rating note based on the date.
	/// </remarks>
	private MarkupString RatingNote
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Requisition ID for which to submit the Candidate for.
	/// </summary>
	private int RequisitionID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the AddResume component.
	/// </summary>
	/// <remarks>
	///     This property is used to manage the AddResume dialog, which is used to add a resume for a candidate.
	///     The dialog is shown in the AddResume method, where the ResumeType is set and the ResumeObject's ID is updated.
	/// </remarks>
	private AddResume ResumeAdd
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ResumeObject which is an instance of the UploadResume class.
	///     This object is used to handle the functionality related to uploading a resume in the Candidate context.
	/// </summary>
	private UploadResume ResumeObject
	{
		get;
	} = new();

	/// <summary>
	///     Gets or sets the type of the resume for the candidate.
	/// </summary>
	private string ResumeType
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the RoleID associated with the Candidate.
	/// </summary>
	private string RoleID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a clone of the CandidateSearch model. This clone is used to manage the state of the search
	///     functionality
	///     in the Candidate page. It holds the search parameters and criteria used to filter and display the candidate data in
	///     the grid view.
	/// </summary>
	private static CandidateSearch SearchModel
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets a clone of the CandidateSearch model. This clone is used within the Candidate class.
	/// </summary>
	private static CandidateSearch SearchModelClone
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected activity for the candidate.
	/// </summary>
	/// <value>
	///     The selected activity is of type <see cref="ProfSvc_Classes.CandidateActivity" /> and it represents the current
	///     activity selected for the candidate.
	/// </value>
	private CandidateActivity SelectedActivity
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected document for download associated with a candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to store the document selected by the user for download.
	///     The document is selected in the DownloadsPanel and used in the `DownloadDocument` method.
	/// </remarks>
	private CandidateDocument SelectedDownload
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected education for the candidate. This property is of type
	///     <see cref="ProfSvc_Classes.CandidateEducation" />.
	/// </summary>
	private CandidateEducation SelectedEducation
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected experience for the candidate.
	///     This property represents an instance of the CandidateExperience class.
	/// </summary>
	private CandidateExperience SelectedExperience
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected notes for the candidate. This property is of type CandidateNotes.
	/// </summary>
	private CandidateNotes SelectedNotes
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the selected skill for the candidate.
	/// </summary>
	/// <value>
	///     The selected skill of type <see cref="CandidateSkills" />.
	/// </value>
	private CandidateSkills SelectedSkill
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the session storage service used for storing and retrieving session data.
	/// </summary>
	/// <remarks>
	///     This property is used to persist the state of the Candidate Search Model across different methods in the Candidate
	///     class.
	///     It is used to store the SearchModel in the session storage and retrieve it when needed.
	///     The session storage service is injected into the class using the [Inject] attribute.
	/// </remarks>
	[Inject]
	private ISessionStorageService SessionStorage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SkillPanel instance associated with the Candidate.
	/// </summary>
	/// <remarks>
	///     This property is used to interact with the SkillPanel component, which provides functionality for managing
	///     candidate skills.
	///     It includes methods for editing and deleting skills, and properties for managing the display and behavior of the
	///     skill grid.
	/// </remarks>
	private SkillPanel SkillPanel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the sort direction for the candidates' data.
	/// </summary>
	/// <remarks>
	///     This property is used to determine the order in which the candidates are displayed.
	///     It is set in the `OnInitializedAsync` method based on the `SearchModel.SortDirection` value.
	///     It is also used in the `BuildRenderTree` method to set the `Direction` attribute of the `GridSortColumn` component.
	/// </remarks>
	private SortDirection SortDirectionProperty
	{
		get;
		set;
	} = SortDirection.Ascending;

	/// <summary>
	///     Gets or sets the field by which the candidates data is sorted.
	/// </summary>
	/// <remarks>
	///     This property is used to determine the field by which the candidates data is sorted in the grid.
	///     The value of this property is set in the `OnInitializedAsync` method, based on the `SearchModel.SortField` value.
	///     The possible values are "Name", "Phone", "Email", "Location", "Status", and "Updated".
	///     The default value is "Updated".
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
	///     This property is used to manage the spinner in the Candidate page.
	///     The spinner is shown by calling the `ShowAsync` method and hidden by calling the `HideAsync` method of this
	///     property.
	///     For example, it is used in the `Candidate.DetailDataBind()` and `Candidate.EditCandidate()` methods to indicate a
	///     loading state while performing asynchronous operations.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the starting record number for the current page of candidates.
	///     This value is calculated as ((_page - 1) * _itemCount + 1).
	/// </summary>
	internal static int StartRecord
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the model for submitting a candidate requisition.
	/// </summary>
	private SubmitCandidateRequisition SubmitCandidateModel
	{
		get;
	} = new();

	/// <summary>
	///     Gets or sets the username of the currently logged-in user.
	/// </summary>
	/// <remarks>
	///     This property is used to store the username of the currently logged-in user.
	///     It is used in various methods and components for user-specific operations and data handling.
	/// </remarks>
	public string User
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the user rights for the Candidate page.
	/// </summary>
	/// <value>
	///     The user rights associated with the Candidate page.
	/// </value>
	/// <remarks>
	///     This property represents the permissions a user has when interacting with the Candidate page.
	///     These permissions are encapsulated in the UserRights class, which includes rights such as viewing, editing, and
	///     changing the status of candidates, requisitions, and companies, among others.
	/// </remarks>
	private UserRights UserRights
	{
		get;
		set;
	} = new();

	/// <summary>
	///     This method is used to add a new document to the candidate's profile.
	///     It first checks if a new document instance exists, if not, it creates a new one.
	///     If an instance already exists, it clears the data from the previous instance.
	///     After preparing the new document instance, it opens the document dialog for the user to add the document details.
	/// </summary>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task AddDocument()
	{
		return ExecuteMethod(() =>
							 {
								 NewDocument.Clear();
								 return DialogDocument.ShowDialog();
							 });
	}

	/// <summary>
	///     This method is used to add a new candidate in the Candidate page.
	///     It is an asynchronous operation that yields the current thread back to the operating system and then
	///     opens the AddChoice dialog which provides options to add a candidate manually or parse from a resume.
	/// </summary>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task AddNewCandidate()
	{
		return ExecuteMethod(AddChoice.ShowDialog);
	}

	/// <summary>
	///     Initiates the process of adding a resume for a candidate.
	/// </summary>
	/// <param name="typeResume">
	///     A byte value indicating the type of resume. A value of 0 represents an original resume, any
	///     other value represents a formatted resume.
	/// </param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task AddResume(byte typeResume)
	{
		return ExecuteMethod(() =>
							 {
								 ResumeType = typeResume == 0 ? "Original" : "Formatted";
								 ResumeObject.Clear();

								 ResumeObject.ID = _target.ID;
								 return ResumeAdd.ShowDialog();
							 });
	}

	/// <summary>
	///     This method is used to open the advanced search dialog in the Candidate page.
	///     It creates a copy of the current search model and opens the advanced search dialog.
	///     The search model copy is used to restore the original search parameters if the user cancels the advanced search.
	/// </summary>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task AdvancedSearch()
	{
		return ExecuteMethod(() =>
							 {
								 SearchModelClone = SearchModel.Copy();
								 return DialogSearch.ShowDialog();
							 });
	}

	/// <summary>
	///     This method is called after a document action is completed in the Candidate page.
	///     It enables the buttons in the AddDocumentDialog component.
	/// </summary>
	/// <param name="args">The arguments associated with the action completion event.</param>
	private void AfterDocument(ActionCompleteEventArgs args)
	{
		DialogDocument.EnableButtons();
	}

	/// <summary>
	///     Executes the AllAlphabet action asynchronously.
	///     This method resets the search model's Name and Page properties,
	///     clears the autocomplete value, and refreshes the grid.
	///     The action is only performed if no other action is in progress.
	/// </summary>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task AllAlphabet()
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModel.Name = "";
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = "";
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     This method is invoked before a document is uploaded in the Candidate page.
	///     It disables the buttons in the document dialog to prevent further actions during the upload process.
	/// </summary>
	/// <param name="args">Event arguments for the BeforeUpload event.</param>
	private void BeforeDocument(BeforeUploadEventArgs args)
	{
		DialogDocument.DisableButtons();
	}

	/// <summary>
	///     Changes the number of items per page in the candidate grid.
	/// </summary>
	/// <param name="item">An instance of ChangeEventArgs containing the new item count.</param>
	/// <remarks>
	///     This method updates the item count in the SearchModel and refreshes the grid.
	///     If an action is already in progress, this method will return immediately to prevent concurrent updates.
	/// </remarks>
	private Task ChangeItemCount(ChangeEventArgs<int, IntValues> item)
	{
		return ExecuteMethod(async () =>
							 {
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 SearchModel.ItemCount = item.Value;

								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
								 StateHasChanged();
							 });
	}

	/// <summary>
	///     Clears the filter settings of the Candidate Search Model.
	/// </summary>
	/// <remarks>
	///     This method first checks if an action is already in progress, if so, it returns immediately.
	///     If not, it sets the action in progress flag to true and stores the current page item count.
	///     It then calls the ClearData method of the SearchModel to reset its properties to their default values.
	///     The current page is set to 1, and the page and item count of the SearchModel are updated with the current values.
	///     The User property of the SearchModel is set to the UserID of the logged-in user, or to "JOLLY" if no user is logged
	///     in.
	///     The updated SearchModel is then stored in the session storage.
	///     The AutocompleteValue is cleared and the Grid is refreshed.
	///     Finally, the action in progress flag is set to false.
	/// </remarks>
	private Task ClearFilter()
	{
		return ExecuteMethod(async () =>
							 {
								 int _currentPageItemCount = SearchModel.ItemCount;
								 SearchModel.Clear();
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 SearchModel.ItemCount = _currentPageItemCount;
								 SearchModel.User = LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant();
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = "";
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Asynchronously continues the parsing process for a candidate.
	///     This method sends a POST request to the "Candidates/SaveParsedData" endpoint with necessary parameters.
	///     After the request, it updates the SearchModel and refreshes the Grid.
	/// </summary>
	/// <returns>
	///     A Task representing the asynchronous operation.
	/// </returns>
	private Task ContinueParsing()
	{
		return ExecuteMethod(async () =>
							 {
								 int _itemCount = SearchModel.ItemCount;
								 RestClient _client = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/SaveParsedData", Method.Post)
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("jsonFileName", _jsonPath);
								 _request.AddQueryParameter("fileName", FileName);
								 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
								 _request.AddQueryParameter("candidateID", ExistingCandidateDetailsDialog.Value);
								 _request.AddQueryParameter("path", Start.UploadsPath);
								 _request.AddQueryParameter("pageCount", _itemCount);
								 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 int _candidateID = 0;
								 if (_response != null)
								 {
									 SearchModel.Clear();
									 SearchModel.AllCandidates = true;
									 SearchModel.MyCandidates = false;
									 SearchModel.ActiveRequisitionsOnly = false;
									 SearchModel.ItemCount = _itemCount;
									 SearchModel.Page = _response["Page"].ToInt32();
									 _candidateID = _response["CandidateID"].ToInt32();
									 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 }

								 await Grid.Refresh();
								 int _index = await Grid.GetRowIndexByPrimaryKeyAsync(_candidateID);
								 if (_index != Grid.SelectedRowIndex)
								 {
									 await Grid.SelectRowAsync(_index);
								 }

								 StateHasChanged();
							 });
	}

	/// <summary>
	///     Handles the data processing for the Candidate page. This method is invoked asynchronously when the page needs to
	///     process or manipulate data.
	///     It creates a DotNetObjectReference of the current instance of the Candidate class and sends it to the JavaScript
	///     side for further processing.
	///     If there are items in the Grid and a CandidateID is set, it selects the row in the Grid that corresponds to the
	///     CandidateID and expands the detail row for the selected candidate.
	///     If no CandidateID is set, it selects the first row in the Grid.
	///     After processing, it resets the CandidateID to 0.
	/// </summary>
	/// <param name="obj">The object that triggers the data handling.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task DataHandler(object obj)
	{
		return ExecuteMethod(async () =>
							 {
								 using DotNetObjectReference<Candidate> _dotNetReference = DotNetObjectReference.Create(this); // create dotnet ref
								 await Runtime.InvokeAsync<string>("detail", _dotNetReference);
								 //  send the dotnet ref to JS side
								 if (Grid.TotalItemCount > 0)
								 {
									 if (CandidateID > 0)
									 {
										 int _index = await Grid.GetRowIndexByPrimaryKeyAsync(CandidateID);
										 if (_index != Grid.SelectedRowIndex)
										 {
											 await Grid.SelectRowAsync(_index);
											 foreach (Candidates _candid in Grid.CurrentViewData.OfType<Candidates>().Where(candid => candid.ID == CandidateID))
											 {
												 await Grid.ExpandCollapseDetailRowAsync(_candid);
												 break;
											 }
										 }

										 await SessionStorage.SetItemAsync(StorageName, SearchModel);
										 await SessionStorage.RemoveItemAsync("CandidateIDFromDashboard");
									 }
									 else
									 {
										 await Grid.SelectRowAsync(0);
									 }
								 }

								 CandidateID = 0;
							 });
	}

	/// <summary>
	///     Asynchronously deletes a document associated with a candidate.
	/// </summary>
	/// <param name="arg">The ID of the document to be deleted.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the Candidates/DeleteCandidateDocument endpoint with the document ID and user
	///     ID as parameters.
	///     If the action is successful, the candidate's documents are updated.
	/// </remarks>
	private Task DeleteDocument(int arg)
	{
		return ExecuteMethod(async () =>
							 {
								 RestClient _client = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/DeleteCandidateDocument", Method.Post)
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("documentID", arg.ToString());
								 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());

								 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateDocumentsObject = General.DeserializeObject<List<CandidateDocument>>(_response["Document"]);
							 });
	}

	/// <summary>
	///     Asynchronously deletes the education record of a candidate.
	/// </summary>
	/// <param name="id">The identifier of the education record to be deleted.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the "Candidates/DeleteEducation" endpoint with the education record's ID, the
	///     candidate's ID, and the user's ID as parameters.
	///     If the action is not in progress, it sets the action in progress, sends the request, and updates the candidate's
	///     education object with the response.
	///     If the response is null, the method returns immediately. If an exception occurs during the request, it is caught
	///     and ignored.
	///     After the request is completed, the action progress is set to false.
	/// </remarks>
	private Task DeleteEducation(int id)
	{
		return ExecuteMethod(async () =>
							 {
								 RestClient _client = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/DeleteEducation", Method.Post)
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("id", id.ToString());
								 _request.AddQueryParameter("candidateID", _target.ID.ToString());
								 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());

								 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateEducationObject = General.DeserializeObject<List<CandidateEducation>>(_response["Education"]);
							 });
	}

	/// <summary>
	///     Asynchronously deletes the experience of a candidate.
	/// </summary>
	/// <param name="id">The identifier of the experience to be deleted.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the Candidates/DeleteExperience endpoint with the id of the experience,
	///     the candidate's ID, and the user's ID as query parameters. If the response is not null,
	///     it deserializes the "Experience" field of the response into a list of CandidateExperience objects.
	/// </remarks>
	private Task DeleteExperience(int id)
	{
		return ExecuteMethod(async () =>
							 {
								 using RestClient _client = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/DeleteExperience", Method.Post)
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("id", id.ToString());
								 _request.AddQueryParameter("candidateID", _target.ID.ToString());
								 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());

								 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateExperienceObject = General.DeserializeObject<List<CandidateExperience>>(_response["Experience"]);
							 });
	}

	/// <summary>
	///     Asynchronously deletes the notes of a candidate based on the provided identifier.
	/// </summary>
	/// <param name="id">The identifier of the notes to be deleted.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if there is an action in progress. If not, it sends a POST request to the
	///     "Candidates/DeleteNotes" endpoint with the id, candidateID, and user as query parameters.
	///     The user is either the UserID of the logged-in user or "JOLLY" if no user is logged in.
	///     The response is expected to be a dictionary containing the notes of the candidate.
	///     If the response is null, the method returns immediately. Otherwise, it deserializes the "Notes" from the response
	///     into a list of CandidateNotes objects.
	/// </remarks>
	private Task DeleteNotes(int id)
	{
		return ExecuteMethod(async () =>
							 {
								 using RestClient _client = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/DeleteNotes", Method.Post)
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("id", id.ToString());
								 _request.AddQueryParameter("candidateID", _target.ID.ToString());
								 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());

								 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateNotesObject = General.DeserializeObject<List<CandidateNotes>>(_response["Notes"]);
							 });
	}

	/// <summary>
	///     Asynchronously deletes a skill from a candidate's profile.
	/// </summary>
	/// <param name="id">The unique identifier of the skill to be deleted.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the "Candidates/DeleteSkill" endpoint with the skill's ID,
	///     the candidate's ID, and the current user's ID as parameters. If the current user is not logged in,
	///     the user ID is set to "JOLLY". The method also sets a flag to prevent multiple simultaneous requests.
	///     If the request is successful, the method updates the candidate's skills list with the response from the server.
	/// </remarks>
	private Task DeleteSkill(int id)
	{
		return ExecuteMethod(async () =>
							 {
								 using RestClient _client = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/DeleteSkill", Method.Post)
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("id", id.ToString());
								 _request.AddQueryParameter("candidateID", _target.ID);
								 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());

								 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateSkillsObject = General.DeserializeObject<List<CandidateSkills>>(_response["Skills"]);
							 });
	}

	/// <summary>
	///     Asynchronously binds detail data to a candidate.
	///     This method is triggered when a candidate's details need to be displayed.
	///     It fetches the candidate's details, including skills, education, experience, activities, notes, ratings, MPC, and
	///     documents from the API.
	///     It also checks if the candidate's formatted and original resumes exist.
	/// </summary>
	/// <param name="candidate">The event arguments containing the candidate data to be bound.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task DetailDataBind(DetailDataBoundEventArgs<Candidates> candidate)
	{
		return ExecuteMethod(async () =>
							 {
								 if (_target != null && _target != candidate.Data)
								 {
									 // return when target is equal to args.data
									 await Grid.ExpandCollapseDetailRowAsync(_target);
								 }

								 int _index = await Grid.GetRowIndexByPrimaryKeyAsync(candidate.Data.ID);
								 if (_index != Grid.SelectedRowIndex)
								 {
									 await Grid.SelectRowAsync(_index);
								 }

								 _target = candidate.Data;
								 try
								 {
									 await Spinner.ShowAsync();
								 }
								 catch
								 {
									 //Ignore the error.
								 }

								 RestClient _restClient = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/GetCandidateDetails");
								 _request.AddQueryParameter("candidateID", _target.ID);
								 _request.AddQueryParameter("roleID", General.GetRoleID(LoginCookyUser));

								 Dictionary<string, object> _restResponse = await _restClient.GetAsync<Dictionary<string, object>>(_request);

								 if (_restResponse != null)
								 {
									 _candidateDetailsObject = JsonConvert.DeserializeObject<CandidateDetails>(_restResponse["Candidate"]?.ToString() ?? string.Empty);
									 _candidateSkillsObject = General.DeserializeObject<List<CandidateSkills>>(_restResponse["Skills"]);
									 _candidateEducationObject = General.DeserializeObject<List<CandidateEducation>>(_restResponse["Education"]);
									 _candidateExperienceObject = General.DeserializeObject<List<CandidateExperience>>(_restResponse["Experience"]);
									 _candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_restResponse["Activity"]);
									 _candidateNotesObject = General.DeserializeObject<List<CandidateNotes>>(_restResponse["Notes"]);
									 _candidateRatingObject = General.DeserializeObject<List<CandidateRating>>(_restResponse["Rating"]);
									 _candidateMPCObject = General.DeserializeObject<List<CandidateMPC>>(_restResponse["MPC"]);
									 _candidateDocumentsObject = General.DeserializeObject<List<CandidateDocument>>(_restResponse["Document"]);
									 RatingMPC = JsonConvert.DeserializeObject<CandidateRatingMPC>(_restResponse["RatingMPC"]?.ToString() ?? string.Empty) ?? new();
									 GetMPCDate();
									 GetMPCNote();
									 GetRatingDate();
									 GetRatingNote();
									 SetupAddress();
									 SetCommunication();
									 SetEligibility();
									 SetJobOption();
									 SetTaxTerm();
									 SetExperience();
								 }

								 _selectedTab = _candidateActivityObject.Count > 0 ? 7 : 0;
								 FormattedExists = _target.FormattedResume;
								 OriginalExists = _target.OriginalResume;

								 try
								 {
									 await Spinner.HideAsync();
								 }
								 catch
								 {
									 //Ignore the error.
								 }
							 });
	}

	/// <summary>
	///     This method is used to collapse the detail row in the Candidate page.
	///     It is invoked from JavaScript and sets the target candidate to null,
	///     effectively hiding the details of the previously selected candidate.
	/// </summary>
	[JSInvokable("DetailCollapse")]
	private void DetailRowCollapse() => _target = null;

	/// <summary>
	///     Initiates the download of a document associated with a candidate.
	/// </summary>
	/// <param name="arg">The identifier of the document to be downloaded.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if another download operation is in progress. If not, it sets the SelectedDownload
	///     property to the document selected in the DownloadsPanel.
	///     It then constructs a query string by concatenating the internal file name of the document, the ID of the target
	///     candidate, the location of the document, and a zero, separated by "^" characters.
	///     The query string is then Base64 encoded. The method then invokes a JavaScript function to open a new browser tab
	///     with a URL constructed from the BaseUri of the NavigationManager, the string "Download/", and the encoded query
	///     string.
	/// </remarks>
	private Task DownloadDocument(int arg)
	{
		return ExecuteMethod(async () =>
							 {
								 SelectedDownload = DownloadsPanel.SelectedRow;
								 string _queryString = $"{SelectedDownload.InternalFileName}^{_target.ID}^{SelectedDownload.Location}^0".ToBase64String();
								 await JsRuntime.InvokeVoidAsync("open", $"{NavManager.BaseUri}Download/{_queryString}", "_blank");
							 });
	}

	/// <summary>
	///     Asynchronously edits the activity with the specified ID.
	///     If the action is not in progress or is a speed dial, it sets the selected activity to the selected row of the
	///     ActivityPanel,
	///     clears the NextSteps list and adds a new "No Change" option. Then, it iterates through the workflows to find the
	///     next steps
	///     based on the status code of the selected activity. If a next step is found, it is added to the NextSteps list.
	///     After all operations are done, it resets the action progress and shows the DialogActivity.
	/// </summary>
	/// <param name="id">The ID of the activity to be edited.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task EditActivity(int id)
	{
		return ExecuteMethod(() =>
							 {
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
									 //Ignore this error. No need to log this error.
								 }

								 return DialogActivity.ShowDialog();
							 });
	}

	/// <summary>
	///     Asynchronously edits the details of a candidate. If the candidate is not selected or is new,
	///     it prepares the system to add a new candidate. Otherwise, it prepares the system to edit the existing candidate's
	///     details.
	/// </summary>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if an action is already in progress or if the speed dial is active, if so it returns
	///     immediately.
	///     Then it shows a spinner indicating the action is in progress.
	///     If the target candidate is null or new, it prepares the system to add a new candidate.
	///     Otherwise, it prepares the system to edit the existing candidate's details.
	///     Finally, it hides the spinner and shows the dialog to edit the candidate.
	/// </remarks>
	private Task EditCandidate()
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
										 //Ignore the exception.
									 }
								 }

								 if (_target == null || _target.ID == 0)
								 {
									 if (_candidateDetailsObjectClone == null)
									 {
										 _candidateDetailsObjectClone = new();
									 }
									 else
									 {
										 _candidateDetailsObjectClone.Clear();
									 }

									 _candidateDetailsObjectClone.IsAdd = true;
								 }
								 else
								 {
									 _candidateDetailsObjectClone = _candidateDetailsObject.Copy();

									 _candidateDetailsObjectClone.IsAdd = false;
								 }

								 if (Spinner != null)
								 {
									 try
									 {
										 await Spinner.HideAsync();
									 }
									 catch
									 {
										 //Ignore the exception.
									 }
								 }

								 await DialogEditCandidate.ShowDialog();
								 StateHasChanged();
							 });
	}

	/// <summary>
	///     Asynchronously edits the education details of a candidate. If the education record is not selected or is new,
	///     it prepares the system to add a new education record. Otherwise, it prepares the system to edit the existing
	///     education record.
	/// </summary>
	/// <param name="id">The ID of the education record to be edited. If the ID is 0, a new education record will be prepared.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if an action is already in progress or if the speed dial is active, if so it returns
	///     immediately.
	///     Then it sets the action in progress.
	///     If the target education record is null or new, it prepares the system to add a new education record.
	///     Otherwise, it prepares the system to edit the existing education record.
	///     Finally, it resets the action progress and shows the dialog to edit the education record.
	/// </remarks>
	private Task EditEducation(int id)
	{
		return ExecuteMethod(() =>
							 {
								 if (id == 0)
								 {
									 if (SelectedEducation == null)
									 {
										 SelectedEducation = new();
									 }
									 else
									 {
										 SelectedEducation.Clear();
									 }
								 }
								 else
								 {
									 SelectedEducation = EducationPanel.SelectedRow?.Copy();
								 }

								 return DialogEducation.ShowDialog();
							 });
	}

	/// <summary>
	///     Asynchronously edits the experience details of a candidate. If the experience is not selected or is new,
	///     it prepares the system to add a new experience. Otherwise, it prepares the system to edit the existing experience
	///     details.
	/// </summary>
	/// <param name="id">The ID of the experience to edit.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if an action is already in progress or if the speed dial is active, if so it returns
	///     immediately.
	///     Then it sets the action in progress to true.
	///     If the target experience ID is 0, it prepares the system to add a new experience.
	///     Otherwise, it prepares the system to edit the existing experience details.
	///     Finally, it sets the action in progress to false and shows the dialog to edit the experience.
	/// </remarks>
	private Task EditExperience(int id)
	{
		return ExecuteMethod(() =>
							 {
								 if (id == 0)
								 {
									 if (SelectedExperience == null)
									 {
										 SelectedExperience = new();
									 }
									 else
									 {
										 SelectedExperience.Clear();
									 }
								 }
								 else
								 {
									 SelectedExperience = ExperiencePanel.SelectedRow.Copy();
								 }

								 return DialogExperience.ShowDialog();
							 });
	}

	/// <summary>
	///     Asynchronously edits the notes of a candidate. If the note is not selected or is new,
	///     it prepares the system to add a new note. Otherwise, it prepares the system to edit the existing note.
	/// </summary>
	/// <param name="id">The ID of the note to be edited.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if an action is already in progress or if the speed dial is active, if so it returns
	///     immediately.
	///     Then it sets the action progress to true.
	///     If the target note ID is 0, it prepares the system to add a new note.
	///     Otherwise, it prepares the system to edit the existing note.
	///     Finally, it sets the action progress to false and shows the dialog to edit the note.
	/// </remarks>
	private Task EditNotes(int id)
	{
		return ExecuteMethod(() =>
							 {
								 if (id == 0)
								 {
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
									 SelectedNotes = PanelNotes.SelectedRow.Copy();
								 }

								 return DialogNotes.ShowDialog();
							 });
	}

	/// <summary>
	///     Asynchronously edits the skill of a candidate. If the skill is not selected or is new,
	///     it prepares the system to add a new skill. Otherwise, it prepares the system to edit the existing skill.
	/// </summary>
	/// <param name="id">The identifier of the skill to be edited.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if an action is already in progress or if the speed dial is active, if so it returns
	///     immediately.
	///     Then it sets the action in progress.
	///     If the target skill is null or new, it prepares the system to add a new skill.
	///     Otherwise, it prepares the system to edit the existing skill.
	///     Finally, it ends the action in progress and shows the dialog to edit the skill.
	/// </remarks>
	private Task EditSkill(int id)
	{
		return ExecuteMethod(() =>
							 {
								 if (id == 0)
								 {
									 if (SelectedSkill == null)
									 {
										 SelectedSkill = new();
									 }
									 else
									 {
										 SelectedSkill.Clear();
									 }
								 }
								 else
								 {
									 SelectedSkill = SkillPanel.SelectedRow.Copy();
								 }

								 return DialogSkill.ShowDialog();
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
	private Task ExecuteMethod(Func<Task> task)
	{
		return General.ExecuteMethod(_semaphoreMainPage, task, Logger);
	}

	/// <summary>
	///     Handles the filtering of the grid based on the candidate's name.
	/// </summary>
	/// <param name="candidate">The candidate's name and key values.</param>
	/// <remarks>
	///     This method takes a ChangeEventArgs object which contains the candidate's name and key values.
	///     It checks if the current value is the same as the last value, if so, it returns immediately.
	///     If not, it updates the last value and the SearchModel's Name property with the current value.
	///     It also sets the current page and the SearchModel's Page property to 1.
	///     The method then stores the SearchModel in the session storage and updates the AutocompleteValue with the
	///     SearchModel's Name.
	///     Finally, it refreshes the grid to reflect the changes.
	/// </remarks>
	private Task FilterGrid(ChangeEventArgs<string, KeyValues> candidate)
	{
		return ExecuteMethod(async () =>
							 {
								 string _currentValue = candidate.Value ?? "";
								 if (_currentValue.Equals(_lastValue))
								 {
									 return;
								 }

								 _lastValue = _currentValue;
								 SearchModel.Name = candidate.Value ?? "";
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = SearchModel.Name;
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     The FirstClick method is responsible for initiating the first page load action in the Candidate class.
	///     It checks if an action is already in progress, and if so, it returns immediately to prevent overlapping actions.
	///     If no action is in progress, it sets the _actionProgress flag to true and ensures that the _currentPage is at least
	///     1.
	///     It then sets the _currentPage and SearchModel.Page to 1, stores the SearchModel in the session storage, and
	///     refreshes the grid.
	///     After the grid is refreshed, it sets the _actionProgress flag back to false, indicating that the action has
	///     completed.
	/// </summary>
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
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the click event on the Formatted button in the Candidate page.
	///     This method triggers the retrieval of the formatted resume of a candidate.
	/// </summary>
	/// <param name="arg">The mouse event arguments.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task FormattedClick(MouseEventArgs arg)
	{
		return GetResumeOnClick("Formatted");
	}

	/// <summary>
	///     Retrieves the most recent date from the CandidateMPC list and converts it to a MarkupString.
	///     The method first checks if the MpcNotes property of the CandidateDetails object is empty.
	///     If it is, an empty MarkupString is assigned to the MPCDate property.
	///     Then, the method finds the CandidateMPC object with the latest date.
	///     If such an object exists, the method formats its date and assigns it to the MPCDate property.
	/// </summary>
	private void GetMPCDate()
	{
		string _mpcDate = "";
		if (_candidateDetailsObject.MPCNotes == "")
		{
			MPCDate = _mpcDate.ToMarkupString();
		}

		CandidateMPC _candidateMPCObjectFirst = _candidateMPCObject.MaxBy(x => x.Date);
		if (_candidateMPCObjectFirst != null)
		{
			_mpcDate = $"{_candidateMPCObjectFirst.Date.CultureDate()} [{string.Concat(_candidateMPCObjectFirst.User.Where(char.IsLetter))}]";
		}

		MPCDate = _mpcDate.ToMarkupString();
	}

	/// <summary>
	///     The GetMPCNote method is responsible for retrieving the most recent note from the CandidateMPC object list.
	///     If the MpcNotes property of the _candidateDetailsObject is empty, an empty string is converted to a MarkupString
	///     and assigned to the MPCNote property.
	///     The method then finds the CandidateMPC object with the latest date and assigns its Comments property to the
	///     _mpcNote variable.
	///     Finally, the _mpcNote is converted to a MarkupString and assigned to the MPCNote property.
	/// </summary>
	private void GetMPCNote()
	{
		string _mpcNote = "";
		if (_candidateDetailsObject.MPCNotes == "")
		{
			MPCNote = _mpcNote.ToMarkupString();
		}

		CandidateMPC _candidateMPCObjectFirst = _candidateMPCObject.MaxBy(x => x.Date);
		if (_candidateMPCObjectFirst != null)
		{
			_mpcNote = _candidateMPCObjectFirst.Comments;
		}

		MPCNote = _mpcNote.ToMarkupString();
	}

	/// <summary>
	///     Retrieves the rating date for the candidate.
	/// </summary>
	/// <remarks>
	///     This method fetches the maximum (latest) date from the candidate's rating list and formats it into a string.
	///     The formatted string includes the date and the user's initials. If the candidate's rating notes are empty,
	///     an empty string is returned. The result is converted into a MarkupString and stored in the RatingDate property.
	/// </remarks>
	private void GetRatingDate()
	{
		string _ratingDate = "";
		if (_candidateDetailsObject.RateNotes == "")
		{
			RatingDate = _ratingDate.ToMarkupString();
		}

		CandidateRating _candidateRatingObjectFirst = _candidateRatingObject.MaxBy(x => x.Date);
		if (_candidateRatingObjectFirst != null)
		{
			_ratingDate =
				$"{_candidateRatingObjectFirst.Date.CultureDate()} [{string.Concat(_candidateRatingObjectFirst.User.Where(char.IsLetter))}]";
		}

		RatingDate = _ratingDate.ToMarkupString();
	}

	/// <summary>
	///     Retrieves the rating note for a candidate.
	/// </summary>
	/// <remarks>
	///     This method checks if the candidate's rating notes are empty. If they are, it sets the RatingNote property to an
	///     empty string.
	///     If the candidate has rating notes, it retrieves the most recent rating note based on the date and sets the
	///     RatingNote property to this value.
	///     The RatingNote property is then converted to a MarkupString for display purposes.
	/// </remarks>
	private void GetRatingNote()
	{
		string _ratingNote = "";
		if (_candidateDetailsObject.RateNotes == "")
		{
			RatingNote = _ratingNote.ToMarkupString();
		}

		CandidateRating _candidateRatingObjectFirst = _candidateRatingObject.MaxBy(x => x.Date);
		if (_candidateRatingObjectFirst != null)
		{
			_ratingNote = _candidateRatingObjectFirst.Comments;
		}

		RatingNote = _ratingNote.ToMarkupString();
	}

	/// <summary>
	///     Handles the retrieval of a candidate's resume based on the specified resume type.
	/// </summary>
	/// <param name="resumeType">The type of the resume to retrieve. This can be "Original" or "Formatted".</param>
	/// <returns>A Task representing the asynchronous operation of retrieving the resume.</returns>
	/// <remarks>
	///     This method sends a GET request to the "Candidates/DownloadResume" endpoint with the candidate's ID and the resume
	///     type as query parameters.
	///     If the request is successful, it shows the retrieved resume in the DownloadsPanel.
	/// </remarks>
	private Task GetResumeOnClick(string resumeType)
	{
		return ExecuteMethod(async () =>
							 {
								 RestClient _restClient = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/DownloadResume")
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("candidateID", _target.ID);
								 _request.AddQueryParameter("resumeType", resumeType);

								 DocumentDetails _restResponse = await _restClient.GetAsync<DocumentDetails>(_request);

								 if (_restResponse != null)
								 {
									 await DownloadsPanel.ShowResume(_restResponse.DocumentLocation, _target.ID, "Original Resume", _restResponse.InternalFileName);
								 }
							 });
	}

	/// <summary>
	///     Retrieves the state name associated with the given id.
	/// </summary>
	/// <param name="id">The id of the state.</param>
	/// <returns>The name of the state associated with the given id. If no state is found, returns null.</returns>
	private string GetState(int id) => _states.FirstOrDefault(state => state.Key == id)?.Value.Split('-')[1].Replace("[", "").Replace("]", "").Trim();

	/// <summary>
	///     Handles the action when the last page button is clicked in the pagination control.
	///     It sets the current page to the last page, updates the search model with the current page,
	///     stores the updated search model in the session storage, and refreshes the grid.
	///     This method also prevents multiple simultaneous actions by using the _actionProgress flag.
	/// </summary>
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
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Initiates the manual handling of a candidate's data.
	/// </summary>
	/// <param name="arg">The mouse event arguments.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method first yields the current thread to allow other pending work to execute.
	///     Then, it checks if the target candidate is null. If so, it creates a new candidate.
	///     Otherwise, it clears the existing candidate's data.
	///     Finally, it calls the EditCandidate method to initiate the editing process.
	/// </remarks>
	private Task ManualCandidate(MouseEventArgs arg)
	{
		if (_target == null)
		{
			_target = new();
		}
		else
		{
			_target.Clear();
		}

		return EditCandidate();
	}

	/// <summary>
	///     Handles the click event for the "Next" button in the Candidate page.
	///     This method increments the current page number and refreshes the grid to display the next set of candidates.
	///     If the action is already in progress, the method will return immediately to prevent multiple simultaneous requests.
	/// </summary>
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
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the beginning of an action on the Candidates grid.
	/// </summary>
	/// <param name="args">The arguments associated with the action event.</param>
	/// <remarks>
	///     This method is triggered when an action begins on the Candidates grid. If an action is already in progress, the
	///     method returns immediately.
	///     If the action is a sorting operation, the method updates the sort field and sort direction in the SearchModel based
	///     on the column name and sort direction in the event arguments.
	///     The updated SearchModel is then stored in the session storage and the grid is refreshed.
	/// </remarks>
	private Task OnActionBegin(ActionEventArgs<Candidates> args)
	{
		return ExecuteMethod(async () =>
							 {
								 if (args.RequestType == Action.Sorting)
								 {
									 SearchModel.SortField = args.ColumnName switch
															 {
																 "Name" => 2,
																 "Phone" => 3,
																 "Email" => 4,
																 "Location" => 5,
																 "Status" => 8,
																 _ => 1
															 };
									 SearchModel.SortDirection = args.Direction == SortDirection.Ascending ? (byte)1 : (byte)0;
									 await SessionStorage.SetItemAsync(StorageName, SearchModel);
									 await Grid.Refresh();
								 }
							 });
	}
	/*
	/// <summary>
	///     This method is invoked after the component has finished rendering.
	///     It checks if it's the first render, if not, it returns immediately.
	///     If it is the first render, it sets the User property of the SearchModel to the UserID of the logged-in user,
	///     or to "JOLLY" if no user is logged in.
	/// </summary>
	/// <param name="firstRender">A boolean indicating whether this is the first time the component is being rendered.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
		{
			return;
		}

		SearchModel.User = LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant();

		await base.OnAfterRenderAsync(true);
	}*/

	/// <summary>
	///     Handles the file upload event.
	/// </summary>
	/// <param name="file">The file upload event arguments containing the uploaded file(s).</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method is triggered when a file upload event occurs. It first checks if there is no ongoing action. If not, it
	///     processes each uploaded file by opening a stream and copying it to the AddedDocument stream.
	///     The method then makes a POST request to the "Candidates/ParseResume" endpoint with the uploaded file and some
	///     additional parameters.
	///     If the server responds with parsing objects, it processes these objects and shows a dialog with candidate details.
	///     If the server does not respond with parsing objects, it refreshes the grid.
	///     The method is designed to handle multiple file uploads but will stop processing further files if an action is
	///     already in progress.
	/// </remarks>
	private Task OnFileUpload(UploadChangeEventArgs file)
	{
		return ExecuteMethod(async () =>
							 {
								 foreach (UploadFiles _file in file.Files)
								 {
									 Stream _str = _file.File.OpenReadStream(60 * 1024 * 1024);
									 await _str.CopyToAsync(AddedDocument);
									 FileName = _file.FileInfo.Name;
									 //FileSize = _file.FileInfo.Size;
									 //Mime = _file.FileInfo.MimeContentType;
									 AddedDocument.Position = 0;
									 _str.Close();
									 RestClient _client = new(Start.ApiHost);
									 RestRequest _request = new("Candidates/ParseResume", Method.Post)
															{
																AlwaysMultipartFormData = true
															};
									 _request.AddFile("file", AddedDocument.ToArray(), FileName);
									 _request.AddQueryParameter("fileName", FileName);
									 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
									 _request.AddQueryParameter("path", Start.UploadsPath);
									 _request.AddQueryParameter("pageCount", SearchModel.ItemCount);
									 Dictionary<string, object> _parsingObjects = await _client.PostAsync<Dictionary<string, object>>(_request);
									 if (_parsingObjects != null)
									 {
										 _jsonPath = _parsingObjects["Json"].ToString();
										 string _emailAddress = _parsingObjects["Email"].ToString();
										 string _phone = _parsingObjects["Phone"].ToString();
										 ExistingCandidateList = General.DeserializeObject<List<ExistingCandidate>>(_parsingObjects["Candidates"]?.ToString());
										 ExistingCandidateList.Insert(0, new(0, "New Candidate", _emailAddress, _phone));

										 ExistingCandidateDetailsDialog.ShowDialog();
									 }
									 else
									 {
										 await Grid.Refresh();
									 }
								 }
							 });
	}

	[Inject]
	private RedisService Redis
	{
		get;
		set;
	}
	
	/// <summary>
	///     This method is called when the component is first initialized.
	///     It sets up the initial state of the component, including parsing query parameters from the URL,
	///     checking user permissions, loading data from session storage, and initializing various component properties.
	/// </summary>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	protected override async Task OnInitializedAsync()
	{
		//_loaded = false;
		_initializationTaskSource = new();
		await ExecuteMethod(async () =>
							{
								Uri _uri = NavManager.ToAbsoluteUri(NavManager.Uri);
								if (QueryHelpers.ParseQuery(_uri.Query).TryGetValue("requisition", out StringValues _tempRequisitionID))
								{
									RequisitionID = _tempRequisitionID.ToInt32();
								}

								if (QueryHelpers.ParseQuery(_uri.Query).TryGetValue("company", out StringValues _isFromCompany))
								{
									IsFromCompany = _isFromCompany.ToInt32() > 0;
								}

								LoginCookyUser = await NavManager.RedirectInner(LocalStorage);
								
								while (_roles == null)
								{
									_roles = await Redis.GetOrCreateAsync<List<Role>>("Roles");
								}

								RoleID = LoginCookyUser.RoleID;
								UserRights = LoginCookyUser.GetUserRights(_roles);

								DisplayAdd = RequisitionID > 0 ? "none" : UserRights.EditCandidate ? "unset" : "none";
								DisplaySubmit = RequisitionID > 0 ? "unset" : "none";

								if (!UserRights.ViewCandidate) // User doesn't have View Candidate rights. This is done by looping through the Roles of the current user and determining the rights for ViewCandidate.
								{
									NavManager.NavigateTo($"{NavManager.BaseUri}home", true);
								}

								string _cookyString = await SessionStorage.GetItemAsync<string>("CandidateGrid");
								string _tempCandidateID = await SessionStorage.GetItemAsStringAsync("CandidateIDFromDashboard");
								if (!_tempCandidateID.NullOrWhiteSpace())
								{
									CandidateID = _tempCandidateID.ToInt32();
								}

								if (!_cookyString.NullOrWhiteSpace() && CandidateID == 0)
								{
									SearchModel = JsonConvert.DeserializeObject<CandidateSearch>(_cookyString);
								}
								else
								{
									SearchModel.AllCandidates = true;
									SearchModel.IncludeAdmin = true;
									SearchModel.MyCandidates = false;
									SearchModel.ActiveRequisitionsOnly = false;

									await SessionStorage.SetItemAsync(StorageName, SearchModel);
								}

								SearchModel.User = LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant();

								_currentPage = SearchModel.Page;
								_lastValue = SearchModel.Name;

								while (_states == null)
								{
									_states = await Redis.GetOrCreateAsync<List<IntValues>>("States");
								}

								_statesCopy.Clear();
								_statesCopy.Add(new(0, "All"));
								_statesCopy.AddRange(_states);

								while (_eligibility == null)
								{
									_eligibility = await Redis.GetOrCreateAsync<List<IntValues>>("Eligibility");
								}

								_eligibilityCopy.Clear();
								_eligibilityCopy.Add(new(0, "All"));
								_eligibilityCopy.AddRange(_eligibility);

								_experience = await Redis.GetOrCreateAsync<List<IntValues>>("Experience");
								_taxTerms = await Redis.GetOrCreateAsync<List<KeyValues>>("TaxTerms");
								while (_jobOptions == null)
								{
									_jobOptions = await Redis.GetOrCreateAsync<List<KeyValues>>("JobOptions");
								}

								_jobOptionsCopy.Clear();
								_jobOptionsCopy.Add(new("%", "All"));
								_jobOptionsCopy.AddRange(_jobOptions);

									_statusCodes = await Redis.GetOrCreateAsync<List<StatusCode>>("StatusCodes");
									_workflows = await Redis.GetOrCreateAsync<List<AppWorkflow>>("Workflow");
								_communication = await Redis.GetOrCreateAsync<List<KeyValues>>("Communication");
									_documentTypes = await Redis.GetOrCreateAsync<List<IntValues>>("DocumentTypes");

								SortDirectionProperty = SearchModel.SortDirection == 1 ? SortDirection.Ascending : SortDirection.Descending;
								SortField = SearchModel.SortField switch
											{
												2 => "Name",
												3 => "Phone",
												4 => "Email",
												5 => "Location",
												8 => "Status",
												_ => "Updated"
											};
								AutocompleteValue = SearchModel.Name;
								//_loaded = true;
								await Grid.Refresh();
							});

		_initializationTaskSource.SetResult(true);
		await base.OnInitializedAsync();
	}

	/// <summary>
	///     Handles the click event for retrieving the original resume of a candidate.
	/// </summary>
	/// <param name="arg">The Mouse Event Arguments associated with the click event.</param>
	/// <returns>A Task representing the asynchronous operation of retrieving the original resume.</returns>
	/// <remarks>
	///     This method calls the GetResumeOnClick method with "Original" as the argument, which sends a GET request to the
	///     "Candidates/DownloadResume" endpoint to retrieve the original resume.
	/// </remarks>
	private Task OriginalClick(MouseEventArgs arg)
	{
		return GetResumeOnClick("Original");
	}

	/// <summary>
	///     Handles the event when the page number changes in the pagination control.
	/// </summary>
	/// <param name="obj">The event arguments containing the new page number.</param>
	/// <remarks>
	///     This method updates the current page number, stores the updated search model in the session storage, and refreshes
	///     the grid.
	///     If an action is already in progress, this method will return immediately to prevent concurrent operations.
	/// </remarks>
	private Task PageNumberChanged(ChangeEventArgs obj)
	{
		return ExecuteMethod(async () =>
							 {
								 int _currentValue = obj.Value.ToInt32();
								 if (_currentValue < 1)
								 {
									 _currentValue = 1;
								 }
								 else if (_currentValue > PageCount)
								 {
									 _currentValue = PageCount;
								 }

								 _currentPage = _currentValue;
								 SearchModel.Page = _currentPage;
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Handles the mouse click event to parse a candidate.
	///     This method is responsible for showing the ParseCandidateDialog.
	/// </summary>
	/// <param name="arg">The MouseEventArgs associated with the mouse click event.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task ParseCandidate(MouseEventArgs arg)
	{
		return DialogParseCandidate.ShowDialog();
	}

	/// <summary>
	///     Handles the click event for the "Previous" button in the pagination control.
	///     This method decreases the current page number by one, if it's greater than 1.
	///     It also refreshes the grid to display the data for the new page.
	/// </summary>
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
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Refreshes the grid view of the Candidate page.
	/// </summary>
	private static Task RefreshGrid() => Grid.Refresh();

	/// <summary>
	///     Handles the event when a row is selected in the candidate list.
	/// </summary>
	/// <param name="candidate">The selected candidate's data encapsulated in a RowSelectEventArgs object.</param>
	private void RowSelected(RowSelectEventArgs<Candidates> candidate)
	{
		_targetSelected = candidate.Data;
	}

	/// <summary>
	///     Asynchronously saves the activity of a candidate.
	/// </summary>
	/// <param name="activity">The context of the activity to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method creates a new RestClient and RestRequest to send a POST request to the
	///     "Candidates/SaveCandidateActivity" endpoint.
	///     The activity model is serialized into JSON format and added to the body of the request.
	///     Additional parameters such as candidateID, user, roleID, isCandidateScreen, jsonPath, emailAddress, and uploadPath
	///     are added to the request.
	///     If the response is not null, it deserializes the "Activity" from the response into a List of CandidateActivity
	///     objects.
	/// </remarks>
	private Task SaveActivity(EditContext activity)
	{
		return ExecuteMethod(async () =>
							 {
								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"candidateID", _target.ID.ToString()},
																			  {"user", General.GetUserName(LoginCookyUser)},
																			  {"roleID", General.GetRoleID(LoginCookyUser)},
																			  {"isCandidateScreen", true.ToString()},
																			  {"jsonPath", Start.JsonFilePath},
																			  {"emailAddress", General.GetEmail(LoginCookyUser)},
																			  {"uploadPath", Start.UploadsPath}
																		  };

								 Dictionary<string, object> _response = await General.PostRest("Candidates/SaveCandidateActivity", _parameters, activity.Model);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_response["Activity"]);
							 });
	}

	/// <summary>
	///     Asynchronously saves the candidate details.
	/// </summary>
	/// <remarks>
	///     This method performs the following steps:
	///     - Yields the current thread of execution.
	///     - Creates a new RestClient instance with the API host URL.
	///     - Creates a new RestRequest instance for the "Candidates/SaveCandidate" endpoint, using the POST method and JSON
	///     request format.
	///     - Adds the cloned candidate details object to the request body as JSON.
	///     - Adds the JSON file path, username, and email address as query parameters to the request. If the username or
	///     email address is null or whitespace, default values are used.
	///     - Sends the request asynchronously and awaits the response.
	///     - Updates the candidate details object with the cloned object.
	///     - Updates the target candidate's name, phone, email, location, updated date, and status based on the candidate
	///     details object.
	///     - Calls methods to set up the address, communication, eligibility, job option, tax term, and experience.
	///     - Yields the current thread of execution again.
	///     - Triggers a UI refresh by calling StateHasChanged.
	/// </remarks>
	private Task SaveCandidate()
	{
		return ExecuteMethod(async () =>
							 {
								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"jsonPath", Start.JsonFilePath},
																			  {"userName", General.GetUserName(LoginCookyUser)},
																			  {"emailAddress", General.GetEmail(LoginCookyUser)}
																		  };

								 await General.PostRest("Candidates/SaveCandidate", _parameters, _candidateDetailsObjectClone);

								 _candidateDetailsObject = _candidateDetailsObjectClone.Copy();
								 _target.Name = $"{_candidateDetailsObject.FirstName} {_candidateDetailsObject.LastName}";
								 _target.Phone = _candidateDetailsObject.Phone1.FormatPhoneNumber();
								 _target.Email = _candidateDetailsObject.Email;
								 _target.Location = $"{_candidateDetailsObject.City}, {GetState(_candidateDetailsObject.StateID)}, {_candidateDetailsObject.ZipCode}";
								 _target.Updated = DateTime.Today.CultureDate() + "[ADMIN]";
								 _target.Status = "Available";
								 SetupAddress();
								 SetCommunication();
								 SetEligibility();
								 SetJobOption();
								 SetTaxTerm();
								 SetExperience();
								 StateHasChanged();
							 });
	}

	/// <summary>
	///     Asynchronously saves the document related to a candidate.
	/// </summary>
	/// <param name="document">The edit context of the document to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method attempts to save a document related to a candidate. The document is represented by an EditContext.
	///     If the model of the EditContext is a CandidateDocument, it will be uploaded to a specified API endpoint.
	///     The method uses RestSharp to send a POST request to the API, including the document and additional parameters.
	///     If the upload is successful, the response is deserialized into a list of CandidateDocument objects.
	/// </remarks>
	private Task SaveDocument(EditContext document)
	{
		return ExecuteMethod(async () =>
							 {
								 if (document.Model is CandidateDocument _document)
								 {
									 RestClient _client = new(Start.ApiHost);
									 RestRequest _request = new("Candidates/UploadDocument", Method.Post)
															{
																AlwaysMultipartFormData = true
															};
									 _request.AddFile("file", AddedDocument.ToStreamByteArray(), FileName);
									 _request.AddParameter("filename", FileName, ParameterType.GetOrPost);
									 _request.AddParameter("mime", Mime, ParameterType.GetOrPost);
									 _request.AddParameter("name", _document.Name, ParameterType.GetOrPost);
									 _request.AddParameter("notes", _document.Notes, ParameterType.GetOrPost);
									 _request.AddParameter("candidateID", _target.ID.ToString(), ParameterType.GetOrPost);
									 _request.AddParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant(),
														   ParameterType.GetOrPost);
									 _request.AddParameter("path", Start.UploadsPath, ParameterType.GetOrPost);
									 _request.AddParameter("type", _document.DocumentTypeID.ToString(), ParameterType.GetOrPost);
									 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
									 if (_response == null)
									 {
										 return;
									 }

									 _candidateDocumentsObject = General.DeserializeObject<List<CandidateDocument>>(_response["Document"]);
								 }
							 });
	}

	/// <summary>
	///     Asynchronously saves the education details of a candidate.
	/// </summary>
	/// <param name="education">The edit context containing the candidate's education details.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the "Candidates/SaveEducation" endpoint with the candidate's education details.
	///     The user ID of the logged-in user or "JOLLY" (if no user is logged in) and the candidate's ID are added as query
	///     parameters to the request.
	///     If the response is not null, the education details from the response are deserialized and stored in the
	///     _candidateEducationObject.
	/// </remarks>
	private Task SaveEducation(EditContext education)
	{
		return ExecuteMethod(async () =>
							 {
								 if (education.Model is CandidateEducation _candidateEducation)
								 {
									 Dictionary<string, string> _parameters = new()
																			  {
																				  {"candidateID", _target.ID.ToString()},
																				  {"user", General.GetUserName(LoginCookyUser)}
																			  };
									 Dictionary<string, object> _response = await General.PostRest("Candidates/SaveEducation", _parameters, _candidateEducation);
									 if (_response == null)
									 {
										 return;
									 }

									 _candidateEducationObject = General.DeserializeObject<List<CandidateEducation>>(_response["Education"]);
								 }
							 });
	}

	/// <summary>
	///     Asynchronously saves the candidate's experience.
	/// </summary>
	/// <param name="experience">The EditContext object containing the candidate's experience to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sends a POST request to the "Candidates/SaveExperience" endpoint of the API.
	///     The request includes the experience model in the body and user and candidateID as query parameters.
	///     If the response is not null, it deserializes the "Experience" field of the response into a List of
	///     CandidateExperience objects.
	/// </remarks>
	private Task SaveExperience(EditContext experience)
	{
		return ExecuteMethod(async () =>
							 {
								 if (experience.Model is CandidateExperience _candidateExperience)
								 {
									 Dictionary<string, string> _parameters = new()
																			  {
																				  {"candidateID", _target.ID.ToString()},
																				  {"user", General.GetUserName(LoginCookyUser)}
																			  };
									 Dictionary<string, object> _response = await General.PostRest("Candidates/SaveExperience", _parameters, _candidateExperience);
									 if (_response == null)
									 {
										 return;
									 }

									 _candidateExperienceObject = General.DeserializeObject<List<CandidateExperience>>(_response["Experience"]);
								 }
							 });
	}

	/// <summary>
	///     This method is used to save the CandidateMPC object. It makes a POST request to the "Candidates/SaveMPC" endpoint.
	///     The method takes an EditContext object as a parameter, which is used to form the body of the POST request.
	///     The user ID from the LoginCookyUser object is added as a query parameter to the request.
	///     If the request is successful, the response is deserialized into a dictionary and used to update the
	///     _candidateMPCObject and RatingMPC properties.
	///     The GetMPCDate and GetMPCNote methods are then called to update the MPC date and note.
	/// </summary>
	/// <param name="editContext">The EditContext object that contains the data to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task SaveMPC(EditContext editContext)
	{
		return ExecuteMethod(async () =>
							 {
								 if (editContext.Model is CandidateRatingMPC _mpc)
								 {
									 Dictionary<string, string> _parameters = new()
																			  {
																				  {"user", General.GetUserName(LoginCookyUser)}
																			  };
									 Dictionary<string, object> _response = await General.PostRest("Candidates/SaveMPC", _parameters, _mpc);
									 if (_response != null)
									 {
										 _candidateMPCObject = General.DeserializeObject<List<CandidateMPC>>(_response["MPCList"]);
										 RatingMPC = JsonConvert.DeserializeObject<CandidateRatingMPC>(_response["FirstMPC"]?.ToString() ?? string.Empty);
										 GetMPCDate();
										 GetMPCNote();
									 }
								 }
							 });
	}

	/// <summary>
	///     Asynchronously saves the notes for a candidate.
	/// </summary>
	/// <param name="notes">The context for the notes to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method creates a new REST client and sends a POST request to the "Candidates/SaveNotes" endpoint.
	///     The notes are added to the request body in JSON format. The user ID and candidate ID are added as query parameters.
	///     If the user is not logged in, "JOLLY" is used as the user ID. The response is expected to be a dictionary
	///     containing the notes.
	///     If the response is null, the method returns immediately. Otherwise, it deserializes the "Notes" from the response
	///     into a list of CandidateNotes objects.
	///     If any exceptions occur during this process, they are caught and the method returns without doing anything.
	/// </remarks>
	private Task SaveNotes(EditContext notes)
	{
		return ExecuteMethod(async () =>
							 {
								 if (notes.Model is CandidateNotes _notes)
								 {
									 Dictionary<string, string> _parameters = new()
																			  {
																				  {"candidateID", _target.ID.ToString()},
																				  {"user", General.GetUserName(LoginCookyUser)}
																			  };
									 Dictionary<string, object> _response = await General.PostRest("Candidates/SaveNotes", _parameters, _notes);
									 if (_response == null)
									 {
										 return;
									 }

									 _candidateNotesObject = General.DeserializeObject<List<CandidateNotes>>(_response["Notes"]);
								 }
							 });
	}

	/// <summary>
	///     This method is responsible for saving the rating of a candidate.
	///     It sends a POST request to the "Candidates/SaveRating" endpoint with the rating information.
	///     If the operation is successful, it updates the candidate's rating information in the application.
	/// </summary>
	/// <param name="editContext">The context for the form that contains the rating information to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task SaveRating(EditContext editContext)
	{
		return ExecuteMethod(async () =>
							 {
								 if (editContext.Model is CandidateRatingMPC _rating)
								 {
									 Dictionary<string, string> _parameters = new()
																			  {
																				  {"user", General.GetUserName(LoginCookyUser)}
																			  };
									 Dictionary<string, object> _response = await General.PostRest("Candidates/SaveRating", _parameters, _rating);
									 if (_response != null)
									 {
										 _candidateRatingObject = General.DeserializeObject<List<CandidateRating>>(_response["RatingList"]);
										 RatingMPC = JsonConvert.DeserializeObject<CandidateRatingMPC>(_response["FirstRating"]?.ToString() ?? string.Empty);
										 _candidateDetailsObject.RateCandidate = RatingMPC.Rating.ToInt32();
										 GetRatingDate();
										 GetRatingNote();
									 }
								 }
							 });
	}

	/// <summary>
	///     Asynchronously saves the resume of a candidate.
	/// </summary>
	/// <param name="resume">The edit context containing the resume to be saved.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if the model of the resume is of type UploadResume. If it is, it creates a new REST client
	///     and request.
	///     The request is a POST request to the "Candidates/UploadResume" endpoint, and it includes multipart form data.
	///     The form data includes the file to be uploaded, the filename, MIME type, resume type, candidate ID, user, and path.
	///     The request is then sent asynchronously.
	///     If any exceptions occur during this process, they are caught and handled silently.
	/// </remarks>
	private Task SaveResume(EditContext resume)
	{
		return ExecuteMethod(async () =>
							 {
								 if (resume.Model is UploadResume)
								 {
									 RestClient _client = new(Start.ApiHost);
									 RestRequest _request = new("Candidates/UploadResume", Method.Post)
															{
																AlwaysMultipartFormData = true
															};
									 _request.AddFile("file", AddedDocument.ToStreamByteArray(), FileName);
									 _request.AddParameter("filename", FileName, ParameterType.GetOrPost);
									 _request.AddParameter("mime", Mime, ParameterType.GetOrPost);
									 _request.AddParameter("type", ResumeType, ParameterType.GetOrPost);
									 _request.AddParameter("candidateID", _target.ID.ToString(), ParameterType.GetOrPost);
									 _request.AddParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant(),
														   ParameterType.GetOrPost);
									 _request.AddParameter("path", Start.UploadsPath, ParameterType.GetOrPost);
									 await _client.PostAsync(_request);
								 }
							 });
	}

	/// <summary>
	///     Asynchronously saves the skill of a candidate.
	/// </summary>
	/// <param name="skill">The context of the skill to be saved.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method creates a new RestClient and RestRequest to make a POST request to the "Candidates/SaveSkill" endpoint.
	///     The skill model is added to the request body in JSON format.
	///     The user ID from the LoginCookyUser and the candidate ID are added as query parameters.
	///     The response from the server is expected to be a dictionary containing the updated skills.
	///     If the response is not null, it deserializes the "Skills" value from the response into a list of CandidateSkills
	///     and assigns it to the _candidateSkillsObject.
	/// </remarks>
	private Task SaveSkill(EditContext skill)
	{
		return ExecuteMethod(async () =>
							 {
								 if (skill.Model is CandidateSkills _skill)
								 {
									 Dictionary<string, string> _parameters = new()
																			  {
																				  {"candidateID", _target.ID.ToString()},
																				  {"user", General.GetUserName(LoginCookyUser)}
																			  };

									 Dictionary<string, object> _response = await General.PostRest("Candidates/SaveSkill", _parameters, _skill);
									 if (_response == null)
									 {
										 return;
									 }

									 _candidateSkillsObject = General.DeserializeObject<List<CandidateSkills>>(_response["Skills"]);
								 }
							 });
	}

	/// <summary>
	///     Triggers the search operation for candidates based on the current state of the SearchModel.
	/// </summary>
	/// <param name="arg">The EditContext associated with the form submission triggering the search.</param>
	/// <remarks>
	///     This method first checks if a search operation is already in progress, and if so, it immediately returns to prevent
	///     overlapping searches.
	///     If no search operation is in progress, it sets the _actionProgress flag to true, indicating that a search operation
	///     is now in progress.
	///     It then updates the SearchModel with the current state of the SearchModelClone, which represents the user's input.
	///     The updated SearchModel is then stored in the session storage for persistence across multiple requests.
	///     The method then triggers a refresh of the Grid to reflect the new search results.
	///     Finally, it sets the _actionProgress flag back to false, indicating that the search operation has completed.
	/// </remarks>
	private Task SearchCandidate(EditContext arg)
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModel = SearchModelClone.Copy();
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Sets the alphabet for the search model and refreshes the grid.
	/// </summary>
	/// <param name="alphabet">The character to set as the alphabet in the search model.</param>
	/// <remarks>
	///     This method first checks if an action is in progress, if so, it returns immediately.
	///     Otherwise, it sets the action progress to true, sets the Name property of the SearchModel to the provided alphabet
	///     character,
	///     resets the current page to 1, sets the Page property of the SearchModel to 1, and stores the SearchModel in the
	///     session storage.
	///     It also sets the AutocompleteValue to the provided alphabet character, refreshes the grid, and sets the action
	///     progress to false.
	/// </remarks>
	private Task SetAlphabet(char alphabet)
	{
		return ExecuteMethod(async () =>
							 {
								 SearchModel.Name = alphabet.ToString();
								 _currentPage = 1;
								 SearchModel.Page = 1;
								 await SessionStorage.SetItemAsync(StorageName, SearchModel);
								 AutocompleteValue = alphabet.ToString();
								 await Grid.Refresh();
							 });
	}

	/// <summary>
	///     Sets the communication rating of the candidate.
	/// </summary>
	/// <remarks>
	///     This method retrieves the communication rating from the candidate details object and converts it to a more
	///     descriptive string.
	///     The conversion is as follows:
	///     - "G" => "Good"
	///     - "A" => "Average"
	///     - "X" => "Excellent"
	///     - Any other value => "Fair"
	///     The resulting string is then assigned to the CandidateCommunication property.
	/// </remarks>
	private void SetCommunication()
	{
		string _returnValue = _candidateDetailsObject.Communication switch
							  {
								  "G" => "Good",
								  "A" => "Average",
								  "X" => "Excellent",
								  _ => "Fair"
							  };

		CandidateCommunication = _returnValue.ToMarkupString();
	}

	/// <summary>
	///     Sets the eligibility status of the candidate.
	/// </summary>
	/// <remarks>
	///     This method checks if the eligibility list has any items. If it does, it sets the CandidateEligibility property to
	///     the eligibility value of the candidate details object if it exists. If the eligibility ID of the candidate details
	///     object is not greater than 0, it sets the CandidateEligibility property to an empty string.
	/// </remarks>
	private void SetEligibility()
	{
		if (_eligibility is {Count: > 0})
		{
			CandidateEligibility = _candidateDetailsObject.EligibilityID > 0
									   ? _eligibility.FirstOrDefault(eligibility => eligibility.Key == _candidateDetailsObject.EligibilityID)!.Value.ToMarkupString()
									   : "".ToMarkupString();
		}
	}

	/// <summary>
	///     Sets the experience of the candidate.
	/// </summary>
	/// <remarks>
	///     This method checks if the experience list is not null and has more than zero elements.
	///     If the candidate's ExperienceID is greater than zero, it sets the CandidateExperience
	///     to the corresponding experience value from the experience list.
	///     If the ExperienceID is not greater than zero, it sets the CandidateExperience to an empty string.
	/// </remarks>
	private void SetExperience()
	{
		if (_experience is {Count: > 0})
		{
			CandidateExperience = _candidateDetailsObject.ExperienceID > 0
									  ? _experience.FirstOrDefault(experience => experience.Key == _candidateDetailsObject.ExperienceID)!.Value.ToMarkupString()
									  : "".ToMarkupString();
		}
	}

	/// <summary>
	///     Sets the job options for the candidate.
	/// </summary>
	/// <remarks>
	///     This method performs the following steps:
	///     - Checks if the job options list is not null and has more than zero elements.
	///     - Splits the job options from the candidate details object by comma.
	///     - Iterates through each split job option.
	///     - If the split job option is not an empty string, it finds the corresponding job option in the job options list and
	///     appends it to the return value.
	///     - Finally, it converts the return value to a markup string and sets it as the candidate's job options.
	/// </remarks>
	private void SetJobOption()
	{
		string _returnValue = "";
		if (_jobOptions is {Count: > 0})
		{
			string[] _splitJobOptions = _candidateDetailsObject.JobOptions.Split(',');
			foreach (string _str in _splitJobOptions)
			{
				if (_str == "")
				{
					continue;
				}

				if (_returnValue != "")
				{
					_returnValue += ", " + _jobOptions.FirstOrDefault(jobOption => jobOption.Key == _str)?.Value;
				}
				else
				{
					_returnValue = _jobOptions.FirstOrDefault(jobOption => jobOption.Key == _str)?.Value;
				}
			}
		}

		CandidateJobOptions = _returnValue.ToMarkupString();
	}

	/// <summary>
	///     Sets the tax terms for the candidate.
	/// </summary>
	/// <remarks>
	///     This method performs the following steps:
	///     - Checks if the tax terms list is not null and has more than zero items.
	///     - Splits the candidate's tax term string by comma.
	///     - Iterates through each split tax term.
	///     - If the tax term is not an empty string, it finds the corresponding tax term from the tax terms list and appends
	///     it to the return value.
	///     - Sets the `CandidateTaxTerms` property with the return value converted to a markup string.
	/// </remarks>
	private void SetTaxTerm()
	{
		string _returnValue = "";

		if (_taxTerms is {Count: > 0})
		{
			string[] _splitTaxTerm = _candidateDetailsObject.TaxTerm.Split(',');
			foreach (string _str in _splitTaxTerm)
			{
				if (_str == "")
				{
					continue;
				}

				if (_returnValue != "")
				{
					_returnValue += ", " + _taxTerms.FirstOrDefault(taxTerm => taxTerm.Key == _str)?.Value;
				}
				else
				{
					_returnValue = _taxTerms.FirstOrDefault(taxTerm => taxTerm.Key == _str)?.Value;
				}
			}
		}

		CandidateTaxTerms = _returnValue.ToMarkupString();
	}

	/// <summary>
	///     Sets up the address for the candidate by concatenating the address fields.
	/// </summary>
	/// <remarks>
	///     This method concatenates the Address1, Address2, City, StateID, and ZipCode fields of the candidate's details.
	///     Each part of the address is separated by a comma or a line break.
	///     If a part of the address is empty, it is skipped.
	///     If the generated address starts with a comma, it is removed.
	///     The final address is converted to a markup string and stored in the Address field.
	/// </remarks>
	private void SetupAddress()
	{
		string _generateAddress = _candidateDetailsObject.Address1;

		if (_generateAddress == "")
		{
			_generateAddress = _candidateDetailsObject.Address2;
		}
		else
		{
			_generateAddress += _candidateDetailsObject.Address2 == "" ? "" : "<br/>" + _candidateDetailsObject.Address2;
		}

		if (_generateAddress == "")
		{
			_generateAddress = _candidateDetailsObject.City;
		}
		else
		{
			_generateAddress += _candidateDetailsObject.City == "" ? "" : "<br/>" + _candidateDetailsObject.City;
		}

		if (_candidateDetailsObject.StateID > 0)
		{
			if (_generateAddress == "")
			{
				_generateAddress = _states.FirstOrDefault(state => state.Key == _candidateDetailsObject.StateID)?.Value?.Split('-')[0].Trim();
			}
			else
			{
				try //Because sometimes the default values are not getting set. It's so random that it can't be debugged. And it never fails during debugging session.
				{
					_generateAddress += ", " + _states.FirstOrDefault(state => state.Key == _candidateDetailsObject.StateID)?.Value?.Split('-')[0].Trim();
				}
				catch
				{
					//
				}
			}
		}

		if (_candidateDetailsObject.ZipCode != "")
		{
			if (_generateAddress == "")
			{
				_generateAddress = _candidateDetailsObject.ZipCode;
			}
			else
			{
				_generateAddress += ", " + _candidateDetailsObject.ZipCode;
			}
		}

		if (_generateAddress.StartsWith(","))
		{
			_generateAddress = _generateAddress[1..].Trim();
		}

		Address = _generateAddress.ToMarkupString();
	}

	/// <summary>
	///     Handles the event when a speed dial item is clicked.
	/// </summary>
	/// <param name="args">The arguments related to the speed dial item event.</param>
	/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method first checks if an action is already in progress. If not, it sets the action in progress and the speed
	///     dial to active.
	///     Depending on the ID of the clicked speed dial item, it performs different actions such as editing a candidate,
	///     rating, adding a skill, education, experience, notes, attachment, or resume.
	///     After the action is performed, it sets the speed dial to inactive and ends the action in progress.
	/// </remarks>
	private Task SpeedDialItemClicked(SpeedDialItemEventArgs args)
	{
		switch (args.Item.ID)
		{
			case "itemEditCandidate":
				_selectedTab = 0;
				return EditCandidate();
			case "itemEditRating":
				_selectedTab = 0;
				StateHasChanged();
				return DialogRating.ShowDialog();
			case "itemEditMPC":
				_selectedTab = 0;
				StateHasChanged();
				return DialogMPC.ShowDialog();
			case "itemAddSkill":
				_selectedTab = 1;
				return EditSkill(0);
			case "itemAddEducation":
				_selectedTab = 2;
				return EditEducation(0);
			case "itemAddExperience":
				_selectedTab = 3;
				return EditExperience(0);
			case "itemAddNotes":
				_selectedTab = 4;
				return EditNotes(0);
			case "itemAddAttachment":
				_selectedTab = 6;
				return AddDocument();
			case "itemOriginalResume":
				_selectedTab = 5;
				return AddResume(0);
			case "itemFormattedResume":
				_selectedTab = 5;
				return AddResume(1);
		}

		return Task.CompletedTask;
	}

	/// <summary>
	///     Submits a candidate to a requisition.
	/// </summary>
	/// <param name="arg">The context for the form that is being validated.</param>
	/// <remarks>
	///     This method makes a POST request to the "Candidates/SubmitCandidateRequisition" endpoint with various parameters
	///     including requisitionID, candidateID, notes, user, jsonPath, emailAddress, and uploadPath.
	///     The response from the server is deserialized into a list of CandidateActivity objects.
	///     If the RequisitionID is greater than 0, it navigates to either the "company" or "requisition" page, depending on
	///     the value of IsFromCompany.
	/// </remarks>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task SubmitCandidateToRequisition(EditContext arg)
	{
		return ExecuteMethod(async () =>
							 {
								 //RestClient _client = new(Start.ApiHost);
								 //RestRequest _request = new("Candidates/SubmitCandidateRequisition", Method.Post)
								 //					   {
								 //						   RequestFormat = DataFormat.Json
								 //					   };
								 //_request.AddQueryParameter("requisitionID", RequisitionID);
								 //_request.AddQueryParameter("candidateID", _targetSelected.ID);
								 //_request.AddQueryParameter("notes", SubmitCandidateModel.Text);
								 //_request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
								 //_request.AddQueryParameter("jsonPath", Start.JsonFilePath);
								 //_request.AddQueryParameter("emailAddress",
								 //						   LoginCookyUser == null || LoginCookyUser.Email.NullOrWhiteSpace() ? "info@titan-techs.com" : LoginCookyUser.Email.ToUpperInvariant());
								 //_request.AddQueryParameter("uploadPath", Start.UploadsPath);
								 //Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 Dictionary<string, string> _parameters = new()
																		  {
																			  {"requisitionID", RequisitionID.ToString()},
																			  {"candidateID", _targetSelected.ID.ToString()},
																			  {"notes", SubmitCandidateModel.Text},
																			  {"user", General.GetUserName(LoginCookyUser)},
																			  {"jsonPath", Start.JsonFilePath},
																			  {"emailAddress", General.GetEmail(LoginCookyUser)},
																			  {"uploadPath", Start.UploadsPath}
																		  };

								 Dictionary<string, object> _response = await General.PostRest("Candidates/SubmitCandidateRequisition", _parameters);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_response["Activity"]);

								 if (RequisitionID > 0)
								 {
									 NavManager.NavigateTo(NavManager.BaseUri + (IsFromCompany ? "company" : "requisition"));
								 }
							 });
	}

	/// <summary>
	///     Handles the event of candidate selection for submission.
	///     This method is triggered when a candidate is selected for submission.
	///     It prepares the SubmitCandidateModel and opens the DialogSubmitCandidate dialog.
	/// </summary>
	/// <param name="arg">Mouse event arguments, not used in this method.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private Task SubmitSelectedCandidate(MouseEventArgs arg)
	{
		SubmitCandidateModel.Clear();
		return DialogSubmitCandidate.ShowDialog();
	}

	/// <summary>
	///     Handles the event when a tab is selected in the user interface.
	/// </summary>
	/// <param name="args">Contains data about the selected tab, including its index.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private void TabSelected(SelectEventArgs args)
	{
		_selectedTab = args.SelectedIndex;
	}

	/// <summary>
	///     Asynchronously undoes a candidate activity based on the provided activity ID.
	/// </summary>
	/// <param name="activityID">The ID of the candidate activity to undo.</param>
	/// <remarks>
	///     This method sends a POST request to the "Candidates/UndoCandidateActivity" endpoint with the activity ID, user ID,
	///     and a flag indicating it's from the candidate screen.
	///     If the user is not logged in, "JOLLY" is used as the user ID. The response is expected to be a dictionary
	///     containing the activity data, which is then deserialized into a list of CandidateActivity objects.
	///     If the response is null or an exception occurs during the process, the method will return immediately.
	/// </remarks>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private Task UndoActivity(int activityID)
	{
		return ExecuteMethod(async () =>
							 {
								 RestClient _client = new(Start.ApiHost);
								 RestRequest _request = new("Candidates/UndoCandidateActivity", Method.Post)
														{
															RequestFormat = DataFormat.Json
														};
								 _request.AddQueryParameter("submissionID", activityID);
								 _request.AddQueryParameter("user", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
								 _request.AddQueryParameter("isCandidateScreen", true);

								 Dictionary<string, object> _response = await _client.PostAsync<Dictionary<string, object>>(_request);
								 if (_response == null)
								 {
									 return;
								 }

								 _candidateActivityObject = General.DeserializeObject<List<CandidateActivity>>(_response["Activity"]);
							 });
	}

	/// <summary>
	///     Handles the upload of a document for a candidate. It reads the file from the upload event,
	///     copies it into a memory stream, and stores the file name and MIME type for later use.
	/// </summary>
	/// <param name="file">The file upload event arguments which contain the file to be uploaded.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private async Task UploadDocument(UploadChangeEventArgs file)
	{
		foreach (UploadFiles _file in file.Files)
		{
			Stream _str = _file.File.OpenReadStream(60 * 1024 * 1024);
			await _str.CopyToAsync(AddedDocument);
			FileName = _file.FileInfo.Name;
			Mime = _file.FileInfo.MimeContentType;
			AddedDocument.Position = 0;
			_str.Close();
		}
	}

	/// <summary>
	///     The `CandidateAdaptor` class is a specialized data adaptor used for handling candidate data.
	///     It extends the `DataAdaptor` class and overrides the `ReadAsync` method to provide custom data reading logic.
	/// </summary>
	/// <remarks>
	///     This class is specifically designed to work with the `Candidate` page and is used to fetch candidate data based on
	///     the provided `DataManagerRequest`.
	///     It ensures that data reading operations are not performed concurrently by checking the `_reading` flag.
	/// </remarks>
	public class CandidateAdaptor : DataAdaptor
	{
		private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

		/// <summary>
		///     Asynchronously reads candidate data using the provided DataManagerRequest and an optional key.
		/// </summary>
		/// <param name="dm">The DataManagerRequest object that contains the parameters for the data request.</param>
		/// <param name="key">An optional key to further specify the data request.</param>
		/// <returns>A Task that represents the asynchronous operation. The task result contains the candidate data.</returns>
		/// <remarks>
		///     This method first checks if a reading operation is already in progress or if the data has not been loaded yet. If
		///     either condition is true, it returns null.
		///     Otherwise, it sets the _reading flag to true and calls the GetCandidateReadAdaptor method from the General class to
		///     fetch the candidate data.
		///     After the data has been fetched, it resets the _reading flag to false and returns the fetched data.
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
                _currentPage = SearchModel.Page;
                object _candidateReturn = await General.GetCandidateReadAdaptor(SearchModel, dm, CandidateID, true);
                _currentPage = SearchModel.Page;
                return _candidateReturn;
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