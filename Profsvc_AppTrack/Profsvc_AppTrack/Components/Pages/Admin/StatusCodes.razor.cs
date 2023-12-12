#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           StatusCodes.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-06-2022 18:52
// Last Updated On:     11-04-2023 21:23
// *****************************************/

#endregion

using StatusCodeDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.StatusCodeDialog;

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the StatusCodes page in the Admin section of the application.
/// </summary>
/// <remarks>
///     This page provides an interface for managing status codes. It includes functionalities such as adding new status
///     codes,
///     editing existing ones, and displaying a list of all status codes. The page is accessible only to users with an
///     Administrator role.
/// </remarks>
public partial class StatusCodes
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     Gets or sets the AdminGrid component in the StatusCodes page.
    /// </summary>
    /// <value>
    ///     The AdminGrid component of type <see cref="StatusCode" />.
    /// </value>
    /// <remarks>
    ///     This property represents the AdminGrid component that is used to manage the status codes in the Admin section of
    ///     the application.
    ///     It provides functionalities such as adding, editing, and displaying a list of all status codes.
    /// </remarks>
    private AdminGrid<StatusCode> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of status codes currently displayed in the grid.
    /// </summary>
    /// <value>
    ///     The count of status codes.
    /// </value>
    /// <remarks>
    ///     This property is used to manage and track the number of status codes that are currently being displayed in the grid
    ///     on the StatusCodes page.
    ///     It is updated every time the data in the grid is bound or refreshed.
    /// </remarks>
    private static int Count
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the StatusCodeDialog instance used for editing status codes on the StatusCodes page.
    /// </summary>
    /// <value>
    ///     The StatusCodeDialog instance.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the dialog for editing status codes. It provides functionalities such as handling
    ///     the cancel event,
    ///     setting the header string, managing the status code model, and handling the save event.
    /// </remarks>
    private StatusCodeDialog DialogStatusCode
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter string for the status codes grid.
    /// </summary>
    /// <value>
    ///     The filter string.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the filter string that is applied to the status codes grid on the StatusCodes page.
    ///     It is updated every time a new filter is set.
    /// </remarks>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance for the StatusCodes page.
    /// </summary>
    /// <value>
    ///     The JavaScript runtime instance which provides functionalities for invoking JavaScript functions from .NET methods.
    /// </value>
    /// <remarks>
    ///     The JavaScript runtime is used to invoke JavaScript functions from .NET methods.
    ///     For instance, it is used in the SaveStatusCode method to scroll to a specific index in the grid after saving a
    ///     status code.
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
    /// <value>
    ///     The instance of the local storage service.
    /// </value>
    /// <remarks>
    ///     This property is used to interact with the browser's local storage. It is used for operations such as storing the
    ///     user's login information and managing user-specific settings.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the StatusCodes class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the StatusCodes class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<StatusCodes> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the LoginCooky instance for the current user.
    /// </summary>
    /// <value>
    ///     The LoginCooky instance for the current user.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the login information of the current user.
    ///     It is populated during the initialization of the StatusCodes page and used to check the user's role and manage
    ///     access rights.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the NavigationManager instance for the StatusCodes page.
    /// </summary>
    /// <value>
    ///     The NavigationManager instance for the StatusCodes page.
    /// </value>
    /// <remarks>
    ///     This property is used for managing and performing navigation operations in the StatusCodes page.
    ///     It is used to redirect users based on their roles and to navigate to different sections of the application.
    /// </remarks>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the RoleID of the currently logged in user.
    /// </summary>
    /// <value>
    ///     The RoleID is a string value that represents the role identifier of the user. It is used to determine the user's
    ///     permissions on the StatusCodes page.
    /// </value>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the StatusCodeRecord which represents a single status code record in the StatusCodes page.
    /// </summary>
    /// <value>
    ///     The StatusCodeRecord is of type StatusCode, which encapsulates the properties and methods of a status code record.
    /// </value>
    private StatusCode StatusCodeRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the clone of the StatusCode record.
    /// </summary>
    /// <value>
    ///     The clone of the StatusCode record.
    /// </value>
    /// <remarks>
    ///     This property is used to hold a copy of the StatusCode record for operations such as adding or editing status
    ///     codes.
    /// </remarks>
    private StatusCode StatusCodeRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the title of the status code dialog.
    /// </summary>
    /// <value>
    ///     The title is "Edit" when an existing status code is being modified, and "Add" when a new status code is being
    ///     created.
    /// </value>
    private static string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data binding event of the grid control on the StatusCodes page.
    /// </summary>
    /// <param name="obj">
    ///     The object that contains the event data. This parameter is not currently used.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is triggered when the data in the grid control is bound. It updates the count of status codes currently
    ///     displayed in the grid and selects the first row if the count is greater than zero.
    /// </remarks>
    private async Task DataHandler(object obj)
    {
        await ExecuteMethod(async () =>
                            {
                                Count = AdminGrid.Grid.CurrentViewData.Count();
                                if (Count > 0)
                                {
                                    await AdminGrid.Grid.SelectRowAsync(0);
                                }
                            });
    }

    /// <summary>
    ///     Initiates the process of editing a status code.
    /// </summary>
    /// <param name="id">The ID of the status code to be edited. If the ID is 0, a new status code will be created.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is responsible for preparing the user interface for editing a status code. If the ID parameter is 0,
    ///     it prepares the interface for adding a new status code. Otherwise, it prepares the interface for editing the
    ///     existing status code with the provided ID. After setting up the interface, it opens a dialog for the user to
    ///     perform the add or edit operation.
    /// </remarks>
    private async Task EditStatusCode(int id = 0)
    {
        await ExecuteMethod(async () =>
                            {
                                List<StatusCode> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                if (_selectedList.Any() && _selectedList.First().ID != id)
                                {
                                    int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(id);
                                    await AdminGrid.Grid.SelectRowAsync(_index);
                                }

                                if (id == 0)
                                {
                                    Title = "Add";
                                    if (StatusCodeRecordClone == null)
                                    {
                                        StatusCodeRecordClone = new();
                                    }
                                    else
                                    {
                                        StatusCodeRecordClone.Clear();
                                    }

                                    StatusCodeRecordClone.IsAdd = true;
                                }
                                else
                                {
                                    Title = "Edit";
                                    StatusCodeRecordClone = StatusCodeRecord.Copy();
                                    StatusCodeRecordClone.IsAdd = false;
                                }

                                StateHasChanged();
                                await DialogStatusCode.ShowDialog();
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
    private async Task ExecuteMethod(Func<Task> task)
    {
        await General.ExecuteMethod(_semaphore, task, Logger);
    }

    /// <summary>
    ///     Filters the grid based on the provided tax term.
    /// </summary>
    /// <param name="taxTerm">
    ///     The tax term used to filter the grid. It is an instance of the <see cref="KeyValues" /> class,
    ///     which represents a key-value pair.
    /// </param>
    /// <remarks>
    ///     This method is used to filter the grid of status codes in the Admin section of the application.
    ///     It sets a filter based on the provided tax term, refreshes the grid to apply the filter,
    ///     and then resets the toggle state. The method is asynchronous and returns a Task.
    /// </remarks>
    private async Task FilterGrid(ChangeEventArgs<string, KeyValues> taxTerm)
    {
        await ExecuteMethod(async () =>
                            {
                                FilterSet(taxTerm.Value);
                                await AdminGrid.Grid.Refresh();
                            });
    }

    /// <summary>
    ///     Sets the filter string for the status codes grid.
    /// </summary>
    /// <param name="value">The new value to be set as the filter.</param>
    /// <remarks>
    ///     This method is used to update the filter string that is applied to the status codes grid on the StatusCodes page.
    ///     It calls the FilterSet method from the General class to process and format the filter value before setting it.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Initializes the StatusCodes page.
    /// </summary>
    /// <remarks>
    ///     This method is invoked when the component is first initialized. It retrieves the user's login information
    ///     from the local storage and checks if the user has an Administrator role. If the user is not an Administrator,
    ///     they are redirected to the home page.
    /// </remarks>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorageBlazored.GetItemAsStringAsync("autoStatusCode");
                                FilterSet(_result);
                                if (!LoginCookyUser.IsAdmin()) //Administrator only has the rights.
                                {
                                    NavManager.NavigateTo($"{NavManager.BaseUri}home", true);
                                }
                            });
        _initializationTaskSource.SetResult(true);
        await base.OnInitializedAsync();
    }

    /// <summary>
    ///     Refreshes the grid control on the StatusCodes page.
    /// </summary>
    /// <remarks>
    ///     This method is used to update the grid control, ensuring that it displays the most recent status codes data.
    ///     It is typically called after operations that modify the status codes, such as adding a new status code or editing
    ///     an existing one.
    /// </remarks>
    private async Task RefreshGrid() => await AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the status codes grid.
    /// </summary>
    /// <param name="statusCode">
    ///     Contains the data of the selected row encapsulated in a RowSelectEventArgs object.
    /// </param>
    /// <remarks>
    ///     This method is triggered when a row is selected in the status codes grid. It sets the selected status code record
    ///     to the StatusCodeRecord property.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<StatusCode> statusCode) => StatusCodeRecord = statusCode.Data;

    /// <summary>
    ///     Asynchronously saves the status code.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method sends a POST request to the "Admin/SaveStatusCode" endpoint with the current status code record clone
    ///     as the JSON body.
    ///     After the request is sent, it refreshes the grid and selects the row with the returned ID.
    ///     If the returned ID does not match the ID of the first selected status code, it scrolls to the row with the returned
    ///     ID.
    /// </remarks>
    private async Task SaveStatusCode()
    {
        await ExecuteMethod(async () =>
                            {
                                int _response = await General.PostRest<int>("Admin/SaveStatusCode", null, StatusCodeRecordClone);

                                if (_response.NullOrWhiteSpace())
                                {
                                    _response = StatusCodeRecordClone.ID;
                                }

                                StatusCodeRecord = StatusCodeRecordClone.Copy();

                                await AdminGrid.Grid.Refresh();
                                int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(_response);
                                await AdminGrid.Grid.SelectRowAsync(_index);

                                await JsRuntime.InvokeVoidAsync("scroll", _index);
                            });
    }

    /// <summary>
    ///     The AdminStatusCodeAdaptor class is a custom data adaptor used for handling data operations on the StatusCodes
    ///     page.
    /// </summary>
    /// <remarks>
    ///     This class inherits from the DataAdaptor base class and overrides the ReadAsync method to provide a custom
    ///     implementation for reading data.
    ///     The ReadAsync method is used to fetch status codes from the server. It uses the
    ///     General.GetStatusCodeReadAdaptorAsync method to perform the actual data retrieval.
    ///     The class also includes a private field _reading to prevent simultaneous read operations.
    /// </remarks>
    public class AdminStatusCodeAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads data from the server using the provided DataManagerRequest.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object that specifies the requirements for the returned data.</param>
        /// <param name="key">An optional key to be used in the data retrieval process.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains the data retrieved from the server.
        ///     If a read operation is already in progress, the method returns null.
        /// </returns>
        /// <remarks>
        ///     This method overrides the base ReadAsync method to provide a custom implementation for reading data.
        ///     It uses the General.GetStatusCodeReadAdaptorAsync method to perform the actual data retrieval.
        ///     The method includes a mechanism to prevent simultaneous read operations.
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
                object _returnValue = await General.GetStatusCodeReadAdaptorAsync(Filter, dm);
                return _returnValue;
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