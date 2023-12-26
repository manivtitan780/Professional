#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           JobOptions.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-26-2023 16:15
// *****************************************/

#endregion

#region Using

using JobOptionDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.JobOptionDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the administrative page for managing job options in the application.
/// </summary>
/// <remarks>
///     This class provides properties and methods for managing job options, such as filtering and navigation.
///     It includes properties for storing filter settings, local storage service, user login information, navigation
///     manager, and user role ID.
///     The class also contains methods for setting the filter, initializing the page, and rendering the page after
///     initialization.
/// </remarks>
public partial class JobOptions
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     Gets or sets the AdminGrid which is a generic admin grid component for managing JobOption data.
    /// </summary>
    /// <value>
    ///     The AdminGrid of type <see cref="AdminGrid{JobOption}" />.
    /// </value>
    /// <remarks>
    ///     This property is used for managing job options in the administrative page of the application.
    ///     It provides various customization parameters and includes a ConfirmDialog and a SfGrid for user interactions.
    /// </remarks>
    private AdminGrid<JobOption> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of items in the current view of the grid.
    /// </summary>
    /// <value>
    ///     The count of items in the current view of the grid.
    /// </value>
    /// <remarks>
    ///     This property is used to store the count of items in the current view of the grid, which is updated in the
    ///     DataHandler method.
    /// </remarks>
    private static int Count
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JobOptionDialog instance used for managing job options.
    /// </summary>
    /// <value>
    ///     The JobOptionDialog instance.
    /// </value>
    /// <remarks>
    ///     This property is used to interact with the JobOptionDialog, which provides an interface for creating and editing
    ///     job options.
    ///     The dialog includes fields for the job option code, option, description, rate text, percentage text, tax terms,
    ///     cost percent, and several switches for various requirements.
    ///     The dialog also includes a save and cancel button for committing or discarding changes.
    /// </remarks>
    private JobOptionDialog DialogJobOption
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter value used for managing job options.
    /// </summary>
    /// <value>
    ///     The filter value.
    /// </value>
    /// <remarks>
    ///     This property is used to store the current filter value for job options.
    ///     It is used in the 'AdminJobOptionAdaptor.ReadAsync' method where it is added as a query parameter to the REST
    ///     request.
    ///     The 'FilterSet' method is used to set this property's value.
    /// </remarks>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the current JobOption record.
    /// </summary>
    /// <value>
    ///     The JobOption record.
    /// </value>
    /// <remarks>
    ///     This property holds the current JobOption record that is selected or being edited.
    ///     It is used in various operations such as editing a job option.
    /// </remarks>
    private JobOption JobOptionsRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets a clone of the JobOption record.
    /// </summary>
    /// <value>
    ///     The clone of the JobOption record.
    /// </value>
    /// <remarks>
    ///     This property is used to hold a copy of a JobOption record for operations such as editing or adding.
    ///     The clone ensures that the original record is not modified until the changes are saved.
    /// </remarks>
    private JobOption JobOptionsRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance.
    /// </summary>
    /// <remarks>
    ///     This property is used to invoke JavaScript functions from C# code. It is used in various methods within the class
    ///     for operations such as scrolling to a specific index in the grid after saving a job option.
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
    ///     This property is used to interact with the local storage of the browser. It is used in the 'OnAfterRenderAsync'
    ///     method
    ///     where it retrieves the 'autoJobOption' item from the local storage. It is also used in the 'OnInitializedAsync'
    ///     method
    ///     where it is passed as an argument to the 'NavManager.RedirectInner' method.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the JobOptions class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the JobOptions class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<JobOptions> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the user's login information.
    /// </summary>
    /// <value>
    ///     The user's login information.
    /// </value>
    /// <remarks>
    ///     This property is used to store the user's login information after they have been authenticated.
    ///     It is used in the 'OnInitializedAsync' method where it is set to the result of the 'NavManager.RedirectInner'
    ///     method call.
    ///     It is also used to check if the user has administrative rights by calling the 'IsAdmin' extension method on it.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the Navigation Manager.
    /// </summary>
    /// <value>
    ///     The instance of the Navigation Manager.
    /// </value>
    /// <remarks>
    ///     This property is used to manage navigation within the application. It is used in the 'OnInitializedAsync' method
    ///     where it is passed as an argument to the 'RedirectInner' method of the 'LoginCheck' class. This method redirects
    ///     the user
    ///     to the login page if they are not signed in, and to a role-appropriate page if they are already signed in.
    ///     If the user is not an administrator, the 'NavManager' is also used to redirect the user to the home page.
    /// </remarks>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the Role ID associated with the current user.
    /// </summary>
    /// <value>
    ///     The Role ID is a string value that uniquely identifies the role of the current user in the system.
    ///     This property is used for managing access control and permissions in the Job Options administrative page.
    /// </value>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a list of key-value pairs representing tax terms for job options.
    /// </summary>
    /// <remarks>
    ///     This property is used to store tax terms fetched from the server. Each key-value pair represents a tax term,
    ///     where the key is the identifier of the tax term and the value is the display name of the tax term.
    ///     This list is used in the JobOptionDialog component for displaying available tax terms.
    /// </remarks>
    private static List<KeyValues> TaxTermKeyValues
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title for the Job Options page.
    /// </summary>
    /// <value>
    ///     A string representing the title. The default value is "Edit".
    /// </value>
    /// <remarks>
    ///     The title is used to distinguish between "Add" and "Edit" operations on the Job Options page.
    ///     When a new job option is being added, the title is set to "Add".
    ///     When an existing job option is being edited, the title set to "Edit".
    /// </remarks>
    private static string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data for the Syncfusion grid component.
    /// </summary>
    /// <param name="obj">The object to be processed.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is called when the data for the grid is bound. It counts the number of items in the current view of the
    ///     grid.
    ///     If there are any items, it selects the first row asynchronously.
    /// </remarks>
    private Task DataHandler(object obj)
    {
        return ExecuteMethod(() =>
                             {
                                 Count = AdminGrid.Grid.CurrentViewData.Count();
                                 return Count > 0 ? AdminGrid.Grid.SelectRowAsync(0) : Task.CompletedTask;
                             });
    }

    /// <summary>
    ///     Initiates the process to edit or add a job option.
    /// </summary>
    /// <param name="code">The code of the job option to edit. If empty or null, a new job option will be added.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method first checks if there are any selected job options in the grid. If there are, and the first selected
    ///     job option's code
    ///     does not match the provided code, it selects the job option with the provided code in the grid.
    ///     Then, if the provided code is null or white space, it sets the title to "Add" and prepares a new job option record
    ///     for addition.
    ///     If the provided code is not null or white space, it sets the title to "Edit" and prepares the job option record for
    ///     editing.
    ///     Finally, it triggers a state change and shows the job option dialog.
    /// </remarks>
    private Task EditJobOption(string code = "")
    {
        return ExecuteMethod(async () =>
                             {
                                 List<JobOption> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().Code != code)
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(code);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 if (code.NullOrWhiteSpace())
                                 {
                                     Title = "Add";
                                     if (JobOptionsRecordClone == null)
                                     {
                                         JobOptionsRecordClone = new();
                                     }
                                     else
                                     {
                                         JobOptionsRecordClone.Clear();
                                     }

                                     JobOptionsRecordClone.IsAdd = true;
                                 }
                                 else
                                 {
                                     Title = "Edit";
                                     JobOptionsRecordClone = JobOptionsRecord.Copy();
                                     JobOptionsRecordClone.IsAdd = false;
                                 }

                                 StateHasChanged();
                                 await DialogJobOption.Dialog.ShowAsync();
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
    private Task ExecuteMethod(Func<Task> task) => General.ExecuteMethod(_semaphore, task, Logger);

    /// <summary>
    ///     Filters the grid based on the selected job option.
    /// </summary>
    /// <param name="jobOption">
    ///     The job option selected by the user, represented as a <see cref="ChangeEventArgs{TValue, TKey}" />
    ///     where TValue is a string and TKey is a <see cref="KeyValues" /> object.
    /// </param>
    /// <remarks>
    ///     This method sets the filter according to the selected job option, refreshes the grid to reflect the changes,
    ///     and prevents multiple simultaneous filter operations by using a toggling mechanism.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    private Task FilterGrid(ChangeEventArgs<string, KeyValues> jobOption)
    {
        return ExecuteMethod(() =>
                             {
                                 FilterSet(jobOption.Value);
                                 return AdminGrid.Grid.Refresh();
                             });
    }

    /// <summary>
    ///     Sets the filter value for managing job options.
    /// </summary>
    /// <param name="value">The new filter value.</param>
    /// <remarks>
    ///     This method is used to set the filter value for job options. It calls the 'FilterSet' method from the 'General'
    ///     class
    ///     to process and format the passed filter value before setting it as the new filter value.
    ///     This method is called in the 'FilterGrid' and 'OnAfterRenderAsync' methods of this class.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Asynchronously initializes the JobOptions page.
    /// </summary>
    /// <remarks>
    ///     This method is called when the JobOptions page is being initialized. It retrieves the user's login information
    ///     from the local storage and assigns it to the LoginCookyUser property. It also assigns the user's RoleID to the
    ///     RoleID property.
    ///     If the user is not an administrator, the method redirects the user to the home page.
    /// </remarks>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorageBlazored.GetItemAsStringAsync("autoJobOption");
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
    ///     Asynchronously refreshes the grid component displaying job options.
    /// </summary>
    /// <remarks>
    ///     This method is used to update the grid component with the latest job options data.
    ///     It is typically called after changes have been made to the job options, such as adding, editing, or deleting a job
    ///     option.
    /// </remarks>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    private Task RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the job options grid.
    /// </summary>
    /// <param name="jobOption">The selected job option data.</param>
    /// <remarks>
    ///     This method is triggered when a user selects a row in the job options grid.
    ///     The selected job option data is then assigned to the JobOptionsRecord property for further processing.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<JobOption> jobOption) => JobOptionsRecord = jobOption.Data;

    /// <summary>
    ///     Asynchronously saves the changes made to a JobOption record.
    /// </summary>
    /// <param name="context">The context of the edit operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sends a POST request to the 'Admin/SaveJobOptions' endpoint with the modified JobOption record.
    ///     After the server-side operation is completed, it refreshes the grid, selects the modified row, and scrolls to it.
    /// </remarks>
    private Task SaveJobOption(EditContext context)
    {
        return ExecuteMethod(async () =>
                             {
                                 string _response = await General.PostRest<string>("Admin/SaveJobOptions", null, JobOptionsRecordClone);

                                 JobOptionsRecord = JobOptionsRecordClone.Copy();
                                 await AdminGrid.Grid.Refresh();

                                 int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(_response);
                                 await AdminGrid.Grid.SelectRowAsync(_index);
                                 await JsRuntime.InvokeVoidAsync("scroll", _index);
                             });
    }

    /// <summary>
    ///     Acts as a custom data adaptor for the administrative job options page.
    /// </summary>
    /// <remarks>
    ///     This class extends the DataAdaptor base class and overrides the ReadAsync method to provide custom data retrieval
    ///     for the job options page. It makes an asynchronous request to the 'Admin/GetJobOptions' endpoint, passing the
    ///     current filter as a query parameter. The response is then processed and returned in a format suitable for the
    ///     data manager.
    /// </remarks>
    public class AdminJobOptionAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads data for the job options page.
        /// </summary>
        /// <param name="dm">The data manager request.</param>
        /// <param name="key">The key used for the data retrieval (optional).</param>
        /// <returns>
        ///     An object containing the retrieved data. If the '_reading' flag is set, the method returns null.
        /// </returns>
        /// <remarks>
        ///     This method makes an asynchronous request to the 'Admin/GetJobOptions' endpoint, passing the
        ///     current filter as a query parameter. The response is then processed and returned in a format suitable for the
        ///     data manager. If an exception occurs during the process, the method handles it by returning a specific format of
        ///     data.
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
                List<JobOption> _dataSource = [];
                object _returnValue = null;
                try
                {
                    TaxTermKeyValues = [];
                    Dictionary<string, string> _parameters = new()
                                                             {
                                                                 {"filter", HttpUtility.UrlEncode(Filter)}
                                                             };
                    Dictionary<string, object> _jobOptionsItems = await General.GetRest<Dictionary<string, object>>("Admin/GetJobOptions", _parameters);
                    if (_jobOptionsItems == null)
                    {
                        _returnValue = dm.RequiresCounts ? new DataResult {Result = _dataSource, Count = 0} : _dataSource;
                    }
                    else
                    {
                        _dataSource = General.DeserializeObject<List<JobOption>>(_jobOptionsItems["JobOptions"]);
                        int _count = _jobOptionsItems["Count"] as int? ?? 0;
                        TaxTermKeyValues = General.DeserializeObject<List<KeyValues>>(_jobOptionsItems["TaxTerms"]);

                        _returnValue = dm.RequiresCounts ? new DataResult
                                                           {
                                                               Result = _dataSource,
                                                               Count = _count
                                                           } : _dataSource;
                    }
                }
                catch
                {
                    if (_dataSource == null)
                    {
                        _returnValue = dm.RequiresCounts ? new DataResult
                                                           {
                                                               Result = null,
                                                               Count = 1
                                                           } : null;
                    }
                    else
                    {
                        _dataSource.Add(new("", ""));

                        _returnValue = dm.RequiresCounts ? new DataResult {Result = _dataSource, Count = 1} : _dataSource;
                    }
                }

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