#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Dashboard.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-31-2023 19:53
// *****************************************/

#endregion

#region Using

using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Schedule;

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     Represents the Dashboard page in the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     This class is responsible for handling the logic of the Dashboard page,
///     including the retrieval and manipulation of various data related to appointments,
///     submission statuses, candidates, requisitions, and user statuses.
/// </remarks>
public partial class Dashboard
{
    private readonly SemaphoreSlim _semaphoreMainPage = new(1, 1);

    /// <summary>
    ///     Gets or sets the list of candidates that have been submitted.
    /// </summary>
    /// <value>
    ///     The list of submitted candidates.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the list of candidates that have been submitted in the Dashboard page.
    /// </remarks>
    private List<Candidates> CandidatesSubmitted
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the ChartActivities for the Dashboard.
    /// </summary>
    /// <value>
    ///     The SfChart object representing the chart of activities.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the chart of activities displayed on the Dashboard.
    ///     It is updated when the ActivitiesTab is selected.
    /// </remarks>
    private SfChart ChartActivities
    {
        get;
        set;
    }

    private IEnumerable<object> ChartActivitiesDataSource
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the status points for the chart on the Dashboard.
    /// </summary>
    /// <value>
    ///     The status points for the chart.
    /// </value>
    /// <remarks>
    ///     This property is used to define the color and status code for different sections of the chart displayed on the
    ///     Dashboard.
    /// </remarks>
    private ChartStatusPoints ChartStatus
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the current date for the Dashboard.
    /// </summary>
    /// <value>
    ///     The current date value.
    /// </value>
    /// <remarks>
    ///     This property is used to represent the current date on the Dashboard. It is bound to the date selector component on
    ///     the Dashboard.
    /// </remarks>
    private DateTime CurrentDate
    {
        get;
        set;
    } = DateTime.Today;

    /// <summary>
    ///     Gets or sets the list of appointment data for the Dashboard.
    /// </summary>
    /// <value>
    ///     The list of appointment data.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the appointment data that is displayed on the Dashboard.
    ///     Each item in the list represents a single appointment with its details.
    /// </remarks>
    private List<AppointmentData> DataSource
    {
        get;
        set;
    } 
    
    /*= new()
            {
                new()
                {Id = 1, Subject = "Paris", StartTime = new(2023, 5, 15, 10, 0, 0), EndTime = new(2023, 5, 15, 14, 0, 0)},
                new()
                {Id = 2, Subject = "Germany", StartTime = new(2023, 5, 18, 10, 0, 0), EndTime = new(2023, 5, 18, 12, 0, 0)}
            };*/

    ///// <summary>
    /////     Gets or sets the grid of submitted candidates for the Dashboard.
    ///// </summary>
    ///// <value>
    /////     The grid of submitted candidates.
    ///// </value>
    ///// <remarks>
    /////     This property is used to display the list of candidates who have submitted their applications.
    /////     It is a grid view that provides a structured and easy-to-read presentation of the candidates' data.
    ///// </remarks>
    //private SfGrid<Candidates> GridCandidatesSubmitted
    //{
    //    get;
    //    set;
    //}

    ///// <summary>
    /////     Gets or sets the grid that displays the user's requisitions on the Dashboard page.
    ///// </summary>
    ///// <value>
    /////     The grid of requisitions.
    ///// </value>
    ///// <remarks>
    /////     This property is used to bind the data of the user's requisitions to the grid on the Dashboard page.
    /////     The grid is implemented using the Syncfusion Blazor Grids component.
    ///// </remarks>
    //private SfGrid<Requisitions> GridMyRequisitions
    //{
    //    get;
    //    set;
    //}

    ///// <summary>
    /////     Gets or sets the grid of submitted requisitions for the Dashboard.
    ///// </summary>
    ///// <value>
    /////     The grid of submitted requisitions.
    ///// </value>
    ///// <remarks>
    /////     This property is used to display the submitted requisitions in a grid format on the Dashboard.
    ///// </remarks>
    //private SfGrid<Requisitions> GridSubmittedRequisitions
    //{
    //    get;
    //    set;
    //}

    /// <summary>
    ///     Gets or sets the local storage service for the Dashboard.
    /// </summary>
    /// <value>
    ///     The instance of ILocalStorageService.
    /// </value>
    /// <remarks>
    ///     This property is used to manage local storage operations in the Dashboard page,
    ///     including the storage and retrieval of user session data.
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
    private ILogger<Dashboard> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the LoginCookyUser for the Dashboard.
    /// </summary>
    /// <value>
    ///     The LoginCookyUser.
    /// </value>
    /// <remarks>
    ///     This property is used to store the login cookie of the current user. It contains information about the user's
    ///     session,
    ///     including their ID, role, and other related data. This information is used for various operations on the Dashboard,
    ///     such as data retrieval and manipulation.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the maximum date value for the Dashboard.
    /// </summary>
    /// <value>
    ///     The maximum date value.
    /// </value>
    /// <remarks>
    ///     This property is used to limit the date range for the data displayed on the Dashboard.
    /// </remarks>
    private DateTime MaxDate
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the minimum date for the Dashboard.
    /// </summary>
    /// <value>
    ///     The minimum date value.
    /// </value>
    /// <remarks>
    ///     This property is used to limit the range of dates that can be selected or displayed on the Dashboard.
    /// </remarks>
    private DateTime MinDate
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of requisitions associated with the current user.
    /// </summary>
    /// <value>
    ///     The list of requisitions.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the list of requisitions that the current user has access to on the
    ///     Dashboard page.
    /// </remarks>
    private List<Requisitions> MyRequisitions
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the NavigationManager for the Dashboard.
    /// </summary>
    /// <value>
    ///     The NavigationManager instance.
    /// </value>
    /// <remarks>
    ///     This property is used to manage and navigate between different pages in the ProfSvc_AppTrack application.
    /// </remarks>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the RoleID associated with the current user.
    /// </summary>
    /// <value>
    ///     The RoleID is a string that represents the unique identifier for the user's role.
    /// </value>
    /// <remarks>
    ///     This property is used to manage access control and permissions within the Dashboard page.
    /// </remarks>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the role name associated with the current user on the Dashboard.
    /// </summary>
    /// <value>
    ///     The name of the role.
    /// </value>
    /// <remarks>
    ///     This property is used to manage access control and functionality based on the user's role.
    /// </remarks>
    private string RoleName
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SessionStorage service for the Dashboard.
    /// </summary>
    /// <value>
    ///     The ISessionStorageService instance.
    /// </value>
    /// <remarks>
    ///     This property is used to manage session storage in the ProfSvc_AppTrack application.
    ///     It allows for the storage and retrieval of session data such as user preferences and settings.
    /// </remarks>
    [Inject]
    private ISessionStorageService SessionStorage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the Spinner control for the Dashboard.
    /// </summary>
    /// <value>
    ///     The Spinner control.
    /// </value>
    /// <remarks>
    ///     This property is used to control the spinner's visibility and behavior on the Dashboard page.
    ///     It is used to indicate loading status when data is being fetched or processed.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the status of the spinner on the Dashboard.
    /// </summary>
    /// <value>
    ///     The status of the spinner.
    /// </value>
    /// <remarks>
    ///     This property is used to control the visibility and activity of the spinner on the Dashboard.
    ///     The spinner is shown while data is being loaded and hidden once the data is ready.
    /// </remarks>
    private SfSpinner SpinnerStatus
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the StatusChart for the Dashboard.
    /// </summary>
    /// <value>
    ///     The StatusChart of type SfChart.
    /// </value>
    /// <remarks>
    ///     This property is used to display various user status data in a chart format on the Dashboard.
    ///     The data source for the chart is determined by the selected index of the status tab.
    /// </remarks>
    private SfChart StatusChart
    {
        get;
        set;
    }

    private IEnumerable<object> StatusChartDataSource
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of submission statuses for the last 30 days for the Dashboard.
    /// </summary>
    /// <value>
    ///     The list of submission statuses for the last 30 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the submission status data for the last 30 days that is displayed on
    ///     the Dashboard.
    ///     Each item in the list represents a single submission status with its details.
    /// </remarks>
    private List<SubmissionStatus> SubmissionStatus30
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the list of submission statuses for the last 365 days for the Dashboard.
    /// </summary>
    /// <value>
    ///     The list of submission statuses for the last 365 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the submission status data for the last 365 days that is displayed on
    ///     the Dashboard.
    ///     Each item in the list represents a single submission status with its details.
    /// </remarks>
    private List<SubmissionStatus> SubmissionStatus365
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the list of submission statuses for the last 7 days for the Dashboard.
    /// </summary>
    /// <value>
    ///     The list of submission statuses for the last 7 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the submission status data for the last 7 days that is displayed on the
    ///     Dashboard.
    ///     Each item in the list represents a single submission status with its details.
    /// </remarks>
    private List<SubmissionStatus> SubmissionStatus7
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the list of submission statuses for the last 90 days for the Dashboard.
    /// </summary>
    /// <value>
    ///     The list of submission statuses for the last 90 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the submission status data for the last 90 days that is displayed on
    ///     the Dashboard.
    ///     Each item in the list represents a single submission status with its details.
    /// </remarks>
    private List<SubmissionStatus> SubmissionStatus90
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the list of submitted requisitions.
    /// </summary>
    /// <value>
    ///     The list of submitted requisitions.
    /// </value>
    /// <remarks>
    ///     This property is used to hold the data related to the requisitions that have been submitted.
    /// </remarks>
    private List<Requisitions> SubmittedRequisitions
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the list of user statuses for the last 30 days.
    /// </summary>
    /// <value>
    ///     The list of user statuses for the last 30 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the user status data for the last 30 days, which is used in the
    ///     Dashboard page to display user status trends.
    /// </remarks>
    private List<ChartUsersStatus> UserStatus30
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of user statuses for the last 365 days.
    /// </summary>
    /// <value>
    ///     The list of user statuses for the last 365 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the user status data for the last 365 days, which is used in the
    ///     Dashboard page to display user status trends.
    /// </remarks>
    private List<ChartUsersStatus> UserStatus365
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of user statuses for the last 7 days.
    /// </summary>
    /// <value>
    ///     The list of user statuses for the last 7 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the user status data for the last 7 days, which is used in the
    ///     Dashboard page to display user status trends.
    /// </remarks>
    private List<ChartUsersStatus> UserStatus7
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of user statuses for the last 90 days.
    /// </summary>
    /// <value>
    ///     The list of user statuses for the last 90 days.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the user status data for the last 90 days, which is used in the
    ///     Dashboard page to display user status trends.
    /// </remarks>
    private List<ChartUsersStatus> UserStatus90
    {
        get;
        set;
    }

    /// <summary>
    ///     Handles the selection of the Activities tab on the Dashboard.
    /// </summary>
    /// <param name="args">
    ///     The arguments of the selection event, containing the index of the selected tab.
    /// </param>
    /// <returns>
    ///     An asynchronous Task that completes when the method has finished executing.
    /// </returns>
    /// <remarks>
    ///     This method is invoked when the Activities tab is selected on the Dashboard.
    ///     It updates the data source of the ChartActivities based on the selected index and refreshes the chart.
    ///     It also controls the visibility of the Spinner during the data fetching and processing.
    /// </remarks>
    private Task ActivitiesTabSelected(SelectEventArgs args)
    {
        return ExecuteMethod(async () =>
                             {
                                 await Spinner.ShowAsync();
                                 ChartActivitiesDataSource = args.SelectedIndex switch
                                                             {
                                                                 0 => SubmissionStatus7,
                                                                 1 => SubmissionStatus30,
                                                                 2 => SubmissionStatus90,
                                                                 _ => SubmissionStatus365
                                                             };

                                 await ChartActivities.RefreshAsync(false);
                                 StateHasChanged();
                                 await Spinner.HideAsync();
                             });
    }

    /// <summary>
    ///     Handles the click event for a candidate.
    /// </summary>
    /// <param name="candidateID">
    ///     The ID of the candidate that was clicked.
    /// </param>
    /// <remarks>
    ///     This method is called when a candidate is clicked on the Dashboard page.
    ///     It stores the clicked candidate's ID in the session storage and navigates to the candidate's page.
    /// </remarks>
    private Task ClickCandidate(int candidateID)
    {
        return ExecuteMethod(async () =>
                             {
                                 await SessionStorage.SetItemAsync("CandidateIDFromDashboard", candidateID);
                                 NavManager.NavigateTo($"{NavManager.BaseUri}candidate", true);
                             });
    }

    /// <summary>
    ///     Handles the click event for a requisition item on the Dashboard page.
    /// </summary>
    /// <param name="requisitionID">
    ///     The ID of the requisition item that was clicked.
    /// </param>
    /// <remarks>
    ///     This method is invoked when a user clicks on a requisition item on the Dashboard page.
    ///     It stores the ID of the clicked requisition item in the session storage and navigates
    ///     the user to the Requisition page for the clicked item.
    /// </remarks>
    private Task ClickRequisition(int requisitionID)
    {
        return ExecuteMethod(async () =>
                             {
                                 await SessionStorage.SetItemAsync("RequisitionIDFromDashboard", requisitionID);
                                 NavManager.NavigateTo($"{NavManager.BaseUri}requisition", true);
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
    ///     Handles the event that occurs when an appointment is rendered on the Dashboard.
    /// </summary>
    /// <param name="arg">
    ///     Contains the data related to the appointment that is being rendered.
    /// </param>
    /// <remarks>
    ///     This method is used to customize the appearance of appointments on the Dashboard.
    ///     It sets the background color of the appointment to the color specified in the appointment data.
    /// </remarks>
    private static void OnEventRendered(EventRenderedArgs<AppointmentData> arg)
    {
        arg.Attributes = new()
                         {
                             {
                                 "style", $"background: {arg.Data.Color};"
                             }
                         };
    }

    /// <summary>
    ///     Initializes the Dashboard page asynchronously.
    /// </summary>
    /// <remarks>
    ///     This method is called when the Dashboard page is first initialized. It sets the minimum and maximum dates,
    ///     retrieves the LoginCookyUser, and makes an API call to fetch various data related to appointments, submission
    ///     statuses,
    ///     candidates, requisitions, and user statuses. The fetched data is then deserialized and stored in the respective
    ///     properties.
    /// </remarks>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    protected override async Task OnInitializedAsync()
    {
        await ExecuteMethod(async () =>
                            {
                                MinDate = DateTime.Today.AddDays(-7);
                                MaxDate = DateTime.Today.AddYears(1);

                                LoginCookyUser = await NavManager.RedirectInner(LocalStorage);
                                RoleID = RoleName = General.GetRoleID(LoginCookyUser);
                                //RestClient _restClient = new($"{Start.ApiHost}");
                                //RestRequest _request = new("Login/Dashboard");
                                //_request.AddQueryParameter("userName", LoginCookyUser == null || LoginCookyUser.UserID.NullOrWhiteSpace() ? "JOLLY" : LoginCookyUser.UserID.ToUpperInvariant());
                                //_request.AddQueryParameter("roleID", RoleName);

                                Dictionary<string, string> _parameters = new()
                                                                         {
                                                                             {"userName", General.GetUserName(LoginCookyUser)},
                                                                             {"roleID", RoleName}
                                                                         };

                                Dictionary<string, object> _restResponse = await General.GetRest<Dictionary<string, object>>("Login/Dashboard", _parameters);
                                if (_restResponse != null)
                                {
                                    DataSource = General.DeserializeObject<List<AppointmentData>>(_restResponse["Appointments"]);
                                    SubmissionStatus7 = General.DeserializeObject<List<SubmissionStatus>>(_restResponse["SubmissionStatus7"]);
                                    SubmissionStatus30 = General.DeserializeObject<List<SubmissionStatus>>(_restResponse["SubmissionStatus30"]);
                                    SubmissionStatus90 = General.DeserializeObject<List<SubmissionStatus>>(_restResponse["SubmissionStatus90"]);
                                    SubmissionStatus365 = General.DeserializeObject<List<SubmissionStatus>>(_restResponse["SubmissionStatus365"]);
                                    CandidatesSubmitted = General.DeserializeObject<List<Candidates>>(_restResponse["Candidates"]);
                                    MyRequisitions = General.DeserializeObject<List<Requisitions>>(_restResponse["Requisitions"]);
                                    SubmittedRequisitions = General.DeserializeObject<List<Requisitions>>(_restResponse["RequisitionsSubmitted"]);
                                    ChartStatus = General.DeserializeObject<ChartStatusPoints>(_restResponse["ChartStatus"]);
                                    UserStatus7 = General.DeserializeObject<List<ChartUsersStatus>>(_restResponse["UserStatus7"]);
                                    UserStatus30 = General.DeserializeObject<List<ChartUsersStatus>>(_restResponse["UserStatus30"]);
                                    UserStatus90 = General.DeserializeObject<List<ChartUsersStatus>>(_restResponse["UserStatus90"]);
                                    UserStatus365 = General.DeserializeObject<List<ChartUsersStatus>>(_restResponse["UserStatus365"]);
                                }
                            });
    }

    /// <summary>
    ///     Handles the event when a status tab is selected on the Dashboard.
    /// </summary>
    /// <param name="args">
    ///     The arguments of the event, containing the selected index of the status tab.
    /// </param>
    /// <remarks>
    ///     This method updates the data source of the StatusChart based on the selected index of the status tab.
    ///     It shows the spinner while the data is being loaded and hides it once the data is ready.
    /// </remarks>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private Task StatusTabSelected(SelectEventArgs args)
    {
        return ExecuteMethod(async () =>
                             {
                                 await SpinnerStatus.ShowAsync();
                                 StatusChartDataSource = args.SelectedIndex switch
                                                         {
                                                             0 => UserStatus7,
                                                             1 => UserStatus30,
                                                             2 => UserStatus90,
                                                             _ => UserStatus365
                                                         };

                                 await StatusChart.RefreshAsync(false);
                                 StateHasChanged();
                                 await SpinnerStatus.HideAsync();
                             });
    }
}