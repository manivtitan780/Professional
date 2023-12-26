#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           States.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-26-2023 16:16
// *****************************************/

#endregion

#region Using

using StateDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.StateDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the administrative page for managing states in the application.
/// </summary>
/// <remarks>
///     This class is responsible for handling user interactions on the states management page.
///     It provides functionalities such as fetching the user's role, redirecting non-admin users,
///     and managing state filters.
/// </remarks>
public partial class States
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     Gets or sets the AdminGrid, a generic admin grid component for managing State data.
    /// </summary>
    /// <value>
    ///     The AdminGrid of type State.
    /// </value>
    /// <remarks>
    ///     This property is used for handling and managing the grid component in the administrative page for states.
    ///     It provides functionalities such as fetching, adding, and filtering State data, as well as handling user
    ///     interactions.
    /// </remarks>
    private AdminGrid<State> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of states currently displayed in the grid on the administrative page.
    /// </summary>
    /// <value>
    ///     The count of states.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the display of the total number of states in the AdminGridFooter component.
    /// </remarks>
    private static int Count
    {
        get;
        set;
    } = 24;

    /// <summary>
    ///     Gets or sets the dialog state for managing State entities in the Admin area.
    /// </summary>
    /// <value>
    ///     The dialog state that represents the state entity being managed in the dialog.
    /// </value>
    /// <remarks>
    ///     This property is used to handle the state management process in the dialog.
    ///     It provides functionality for creating, editing, and saving State entities.
    /// </remarks>
    private StateDialog DialogState
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter string used for state data retrieval in the administrative page.
    /// </summary>
    /// <value>
    ///     The filter string.
    /// </value>
    /// <remarks>
    ///     This property is used to store the filter value that is used when fetching state data in the administrative page.
    ///     The filter value is processed and formatted by the FilterSet method in the General class before being used.
    /// </remarks>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance for the States page.
    /// </summary>
    /// <remarks>
    ///     The JavaScript runtime instance is used to invoke JavaScript functions from the States page.
    ///     It is used, for example, to scroll to a specific index in the Grid component after saving a state.
    /// </remarks>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the local storage service provided by Blazored.
    /// </summary>
    /// <value>
    ///     The instance of the local storage service.
    /// </value>
    /// <remarks>
    ///     This property is used to interact with the browser's local storage. It is used to store and retrieve
    ///     application-specific data like the auto state of the filter. The local storage service is injected into the page
    ///     and is used in methods such as OnAfterRenderAsync and OnInitializedAsync.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the States class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the States class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<States> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the LoginCooky class that represents the current user's login information.
    /// </summary>
    /// <value>
    ///     The instance of the LoginCooky class.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the current user's login information, such as user ID, role, and last
    ///     login date.
    ///     It is populated in the OnInitializedAsync method using the RedirectInner method of the NavigationManager class,
    ///     and used in the same method to check if the user has administrative rights.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the NavigationManager.
    /// </summary>
    /// <value>
    ///     The instance of the NavigationManager.
    /// </value>
    /// <remarks>
    ///     This property is used to manage navigation and URI manipulation within the application.
    ///     It is used in methods such as OnInitializedAsync to redirect non-admin users to the home page.
    /// </remarks>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the role ID associated with the current user.
    /// </summary>
    /// <remarks>
    ///     This property is used to determine the user's permissions on the administrative page for managing states.
    ///     It is set during the initialization of the page and used in the header component for display.
    /// </remarks>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the State class that represents the current state record.
    /// </summary>
    /// <value>
    ///     The instance of the State class.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the current state record being managed on the states management page.
    ///     It is populated in the RowSelected method when a row is selected in the Grid component,
    ///     and used in the EditState method to create a copy of the state record for editing.
    /// </remarks>
    private State StateRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets a clone of the state record.
    /// </summary>
    /// <remarks>
    ///     This property is used to hold a copy of the state record for operations such as add or edit.
    ///     The clone is used to prevent direct modifications to the original state record during these operations.
    /// </remarks>
    private State StateRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the title of the state management dialog.
    /// </summary>
    /// <remarks>
    ///     The title changes based on the action being performed in the dialog.
    ///     It is set to "Add" when a new state is being added and "Edit" when an existing state is being modified.
    /// </remarks>
    private string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data bound event of the Grid component.
    /// </summary>
    /// <remarks>
    ///     This method is invoked when the Grid component has finished binding data.
    ///     It updates the count of states currently displayed in the grid and selects the first row if any states are present.
    /// </remarks>
    private Task DataHandler()
    {
        return ExecuteMethod(() =>
                             {
                                 Count = AdminGrid.Grid.CurrentViewData.Count();
                                 return Count > 0 ? AdminGrid.Grid.SelectRowAsync(0) : Task.CompletedTask;
                             });
    }

    /// <summary>
    ///     Initiates the process of editing a state.
    /// </summary>
    /// <param name="code">
    ///     The ID of the state to be edited. If the code is 0, a new state will be added.
    /// </param>
    /// <remarks>
    ///     This method is responsible for preparing the state record for editing or addition.
    ///     It fetches the selected state from the grid, sets the title based on the operation (Add or Edit),
    ///     and prepares the state record clone for the operation. After the preparation, it triggers the state dialog for the
    ///     user interaction.
    /// </remarks>
    private Task EditState(int code = 0)
    {
        return ExecuteMethod(async () =>
                             {
                                 List<State> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().ID != code)
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(code);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 if (code == 0)
                                 {
                                     Title = "Add";
                                     if (StateRecordClone == null)
                                     {
                                         StateRecordClone = new();
                                     }
                                     else
                                     {
                                         StateRecordClone.Clear();
                                     }

                                     StateRecordClone.IsAdd = true;
                                 }
                                 else
                                 {
                                     Title = "Edit";
                                     StateRecordClone = StateRecord.Copy();
                                     StateRecordClone.IsAdd = false;
                                 }

                                 StateHasChanged();
                                 await DialogState.ShowDialog();
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
    ///     Filters the grid based on the provided tax term.
    /// </summary>
    /// <param name="taxTerm">
    ///     The tax term to filter the grid by. This is an instance of the <see cref="ChangeEventArgs{TValue, TKey}" /> class,
    ///     where TValue is a string representing the tax term and TKey is an instance of the <see cref="KeyValues" /> class.
    /// </param>
    /// <remarks>
    ///     This method is responsible for setting the filter and refreshing the grid. It ensures that the grid is not
    ///     refreshed
    ///     while a previous refresh operation is still in progress by using the <c>_toggling</c> field.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    private Task FilterGrid(ChangeEventArgs<string, KeyValues> taxTerm)
    {
        return ExecuteMethod(() =>
                             {
                                 FilterSet(taxTerm.Value);
                                 return AdminGrid.Grid.Refresh();
                             });
    }

    /// <summary>
    ///     Sets the filter string used for state data retrieval in the administrative page.
    /// </summary>
    /// <param name="value">
    ///     The new value to be set as the filter.
    /// </param>
    /// <remarks>
    ///     This method is used to update the filter value that is used when fetching state data in the administrative page.
    ///     The passed value is processed and formatted by the FilterSet method in the General class before being set.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Initializes the component asynchronously.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is executed when the component is initialized. It retrieves the current user's login information
    ///     and role ID from the local storage. If the user is not an administrator, they are redirected to the home page.
    /// </remarks>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorageBlazored.GetItemAsStringAsync("autoState");
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
    ///     Refreshes the grid component of the States page.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation of refreshing the grid.
    /// </returns>
    /// <remarks>
    ///     This method is used to update the grid component with the latest state data.
    /// </remarks>
    private Task RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the Grid component.
    /// </summary>
    /// <param name="state">
    ///     The event arguments containing the data of the selected state.
    /// </param>
    /// <remarks>
    ///     This method is triggered when a row is selected in the Grid component. It sets the StateRecord property
    ///     to the data of the selected state, which can then be used for editing or other operations.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<State> state) => StateRecord = state.Data;

    /// <summary>
    ///     Saves the current state record to the server.
    /// </summary>
    /// <param name="context">The context of the edit operation.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sends a POST request to the 'Admin/SaveState' endpoint with the current state record as the body.
    ///     After the server responds, it refreshes the Grid component and selects the saved state record.
    ///     If the saved state record is not in the selected records of the Grid, it scrolls the Grid to the saved state
    ///     record.
    /// </remarks>
    private Task SaveState(EditContext context)
    {
        return ExecuteMethod(async () =>
                             {
                                 int _response = await General.PostRest<int>("Admin/SaveState", null, StateRecordClone);

                                 if (_response.NullOrWhiteSpace())
                                 {
                                     _response = StateRecordClone.ID;
                                 }

                                 StateRecord = StateRecordClone.Copy();

                                 await AdminGrid.Grid.Refresh();
                                 int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(_response);
                                 await AdminGrid.Grid.SelectRowAsync(_index);

                                 await JsRuntime.InvokeVoidAsync("scroll", _index);
                             });
    }

    /// <summary>
    ///     Adaptor class for managing state data in the administrative page.
    /// </summary>
    /// <remarks>
    ///     This class extends the DataAdaptor class and overrides the ReadAsync method to fetch state data asynchronously.
    ///     It prevents multiple simultaneous reads by checking the _reading flag.
    /// </remarks>
    public class AdminStateAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads state data using the provided data manager request and an optional key.
        /// </summary>
        /// <param name="dm">The data manager request containing parameters for the server request.</param>
        /// <param name="key">An optional key to be used in the server request.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains the state data retrieved from the
        ///     server.
        /// </returns>
        /// <remarks>
        ///     This method checks the _reading flag before initiating a read operation to prevent multiple simultaneous reads.
        ///     If a read operation is already in progress, the method returns null.
        ///     Otherwise, it sets the _reading flag to true, calls the GetStateDataAdaptorAsync method of the General class to
        ///     fetch the state data,
        ///     sets the _reading flag back to false, and returns the fetched data.
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
                object _returnValue = await General.GetStateDataAdaptorAsync(Filter, dm);
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