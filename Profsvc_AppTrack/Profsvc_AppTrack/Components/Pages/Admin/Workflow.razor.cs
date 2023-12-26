#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Workflow.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-26-2023 16:23
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the administrative workflow management page in the Professional Services Application Tracking system.
///     This class provides functionality for managing workflows, including role and status information.
///     It also handles rendering and initialization events for the workflow management page.
/// </summary>
public partial class Workflow
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     Gets or sets the AdminGrid component used for managing workflows in the Professional Services Application Tracking
    ///     system.
    /// </summary>
    /// <value>
    ///     The AdminGrid component of type <see cref="AppWorkflow" />.
    /// </value>
    /// <remarks>
    ///     This property holds the reference to the AdminGrid component which is used for displaying and managing workflows in
    ///     the administrative interface.
    ///     The grid is populated with instances of the <see cref="AppWorkflow" /> class.
    /// </remarks>
    private AdminGrid<AppWorkflow> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of workflow activities currently displayed in the administrative workflow management grid.
    ///     This property is used to update the count displayed in the AdminGridFooter component.
    /// </summary>
    private static int Count
    {
        get;
        set;
    } = 24;

    /// <summary>
    ///     Gets or sets the instance of the WorkflowDialog used in the Workflow page.
    /// </summary>
    /// <remarks>
    ///     The WorkflowDialog is a user interface for creating, editing, and managing workflows in the application.
    ///     It includes parameters for handling events such as saving and cancelling changes, as well as properties for
    ///     managing the dialog's appearance and behavior.
    ///     The dialog uses an instance of the AppWorkflow class as a model to bind the workflow data for editing.
    /// </remarks>
    private WorkflowDialog DialogWorkflow
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter string used for retrieving and displaying workflows in the administrative workflow
    ///     management page.
    ///     This filter is used in conjunction with the <see cref="Code.General.GetWorkflowAsync" /> method to
    ///     filter the workflows based on specific criteria.
    /// </summary>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of IJSRuntime which provides methods for interacting with JavaScript from .NET code.
    ///     This is used in the administrative workflow management page for operations that require JavaScript interop, such as
    ///     scrolling to a specific workflow in the grid.
    /// </summary>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the Blazored LocalStorage service used in the administrative workflow
    ///     management page.
    ///     This service is used for handling local storage operations, such as retrieving and storing the workflow filter
    ///     settings.
    /// </summary>
    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the Workflow class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the Workflow class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<Workflow> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the LoginCooky class representing the current user's login information.
    ///     This property is used to store and retrieve user-specific data such as user ID, role, and last login details.
    /// </summary>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the NavigationManager service used in the administrative workflow
    ///     management page.
    ///     This service is used for handling navigation operations, such as redirecting to different pages
    ///     based on user roles and login status.
    /// </summary>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the Role ID associated with the current user session.
    ///     This ID is used for role-based authorization and navigation within the administrative workflow management page.
    /// </summary>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of roles associated with the workflow in the Professional Services Application Tracking
    ///     system.
    ///     Each role is represented by a KeyValues object.
    /// </summary>
    public static List<KeyValues> Roles
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of statuses associated with the workflow in the Professional Services Application Tracking
    ///     system.
    ///     Each status is represented by a KeyValues object.
    /// </summary>
    public static List<KeyValues> Status
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title of the workflow management page in the Professional Services Application Tracking system.
    ///     This title is used in the header of the workflow dialog and changes depending on the context (e.g., "Add" for
    ///     adding a new workflow, "Edit" for editing an existing workflow).
    /// </summary>
    private static string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Gets or sets the current workflow record in the Professional Services Application Tracking system.
    ///     This record represents a specific workflow instance and is used for operations such as editing or saving workflow
    ///     data.
    /// </summary>
    private AppWorkflow WorkflowRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the clone of the workflow record in the Professional Services Application Tracking system.
    ///     This property is used for creating or editing workflow records. It is initialized with a new instance of
    ///     AppWorkflow.
    /// </summary>
    private AppWorkflow WorkflowRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Handles the data for the administrative workflow management grid in the Professional Services Application Tracking
    ///     system.
    ///     This method updates the count of workflow activities currently displayed in the grid and selects the first row if
    ///     the count is greater than zero.
    ///     It is invoked when the data in the grid is bound.
    /// </summary>
    private Task DataHandler()
    {
        return ExecuteMethod(() =>
                             {
                                 Count = AdminGrid.Grid.CurrentViewData.Count();
                                 return Count > 0 ? AdminGrid.Grid.SelectRowAsync(0) : Task.CompletedTask;
                             });
    }

    /// <summary>
    ///     Initiates the editing process for a workflow. If the ID parameter is zero, a new workflow is created.
    ///     Otherwise, the method retrieves the workflow with the specified ID for editing.
    /// </summary>
    /// <param name="id">The ID of the workflow to edit. If this parameter is zero, a new workflow is created.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private async Task EditWorkflow(int id = 0)
    {
        List<AppWorkflow> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
        if (_selectedList.Any() && _selectedList.First().ID != id)
        {
            int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(id);
            await AdminGrid.Grid.SelectRowAsync(_index);
        }

        await Task.Yield();
        if (id == 0)
        {
            Title = "Add";
            if (WorkflowRecordClone == null)
            {
                WorkflowRecordClone = new();
            }
            else
            {
                WorkflowRecordClone.Clear();
            }
        }
        else
        {
            Title = "Edit";
            WorkflowRecordClone = WorkflowRecord.Copy();
        }

        StateHasChanged();
        await DialogWorkflow.ShowDialog();
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
    ///     Asynchronously filters the grid of workflows based on the provided workflow change event arguments.
    ///     This method is responsible for setting the filter, refreshing the grid, and managing the toggling state.
    /// </summary>
    /// <param name="workflow">The workflow change event arguments containing the new value for the filter.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private async Task FilterGrid(ChangeEventArgs<string, KeyValues> workflow)
    {
        if (await _semaphore.WaitAsync(TimeSpan.Zero))
        {
            try
            {
                FilterSet(workflow.Value);
                await AdminGrid.Grid.Refresh();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    /// <summary>
    ///     Sets the filter string used for retrieving and displaying workflows in the administrative workflow
    ///     management page. The method uses the passed value to update the filter string.
    /// </summary>
    /// <param name="value">The new value to be set as the filter.</param>
    /// <remarks>
    ///     This method internally calls the <see cref="Code.General.FilterSet" /> method to process and
    ///     format the filter value.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Initializes the component asynchronously.
    ///     This method is invoked when the component is first created and before the first render.
    ///     It retrieves the current user's login information and role ID using the NavigationManager and LocalStorageBlazored
    ///     services.
    ///     If the user is not an administrator, they are redirected to the home page.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is called only once, before the first render of the component.
    ///     It uses the NavigationManager and LocalStorageBlazored services to retrieve the current user's login information
    ///     and role ID.
    ///     If the user is not an administrator, they are redirected to the home page.
    /// </remarks>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorageBlazored.GetItemAsStringAsync("autoWorkflow");
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
    ///     Refreshes the Syncfusion grid component for managing AppWorkflow instances in the administrative workflow
    ///     management page.
    ///     This method is asynchronous and returns a Task.
    /// </summary>
    private Task RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the row selection event in the workflow management grid.
    ///     This method is invoked when a row in the grid is selected by the user.
    ///     It sets the selected workflow instance as the current workflow record for further operations.
    /// </summary>
    /// <param name="designation">An object of type RowSelectEventArgs containing the selected workflow instance.</param>
    private void RowSelected(RowSelectEventArgs<AppWorkflow> designation) => WorkflowRecord = designation.Data;

    /// <summary>
    ///     Asynchronously saves the current workflow record clone.
    /// </summary>
    /// <param name="context">The context of the edit operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    /// <remarks>
    ///     This method calls the 'SaveWorkflowAsync' method from the 'General' class, passing the 'WorkflowRecordClone' as the
    ///     workflow to be saved.
    ///     It also passes the 'Grid', 'WorkflowRecord', and 'JsRuntime' to the 'SaveWorkflowAsync' method for further
    ///     processing.
    /// </remarks>
    private Task SaveWorkflow(EditContext context)
    {
        return ExecuteMethod(async () =>
                             {
                                 int _response = await General.PostRest<int>("Admin/SaveWorkflow", null, WorkflowRecordClone);
                                 if (WorkflowRecord != null)
                                 {
                                     WorkflowRecord = WorkflowRecordClone.Copy();
                                 }

                                 await AdminGrid.Grid.Refresh();
                                 int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(_response);
                                 await AdminGrid.Grid.SelectRowAsync(_index);
                                 if (JsRuntime != null)
                                 {
                                     await JsRuntime.InvokeVoidAsync("scroll", _index);
                                 }
                             });
    }

    /// <summary>
    ///     The AdminWorkflowAdaptor class is a custom data adaptor for the administrative workflow management page.
    ///     It extends the DataAdaptor class and overrides the ReadAsync method to fetch workflow data asynchronously.
    ///     The class uses a private boolean field, _reading, to prevent multiple simultaneous read operations.
    /// </summary>
    public class AdminWorkflowAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads workflow data using the current filter and the provided data manager request.
        ///     This method prevents multiple simultaneous read operations by using a private boolean field, _reading.
        ///     If a read operation is already in progress (i.e., _reading is true), this method immediately returns null.
        ///     Otherwise, it sets _reading to true, retrieves the workflow data asynchronously using the General.GetWorkflowAsync
        ///     method,
        ///     sets _reading back to false, and then returns the retrieved data.
        /// </summary>
        /// <param name="dm">The data manager request object containing additional parameters for the workflow retrieval.</param>
        /// <param name="key">An optional key parameter. This parameter is not used in the current implementation.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains the retrieved workflow data,
        ///     or null if a read operation was already in progress.
        /// </returns>
        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            if (!await _semaphoreSlim.WaitAsync(TimeSpan.Zero))
            {
                return null;
            }

            await _initializationTaskSource.Task;
            try
            {
                object _returnValue = await General.GetWorkflowAsync(Filter, dm);
                return _returnValue;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}