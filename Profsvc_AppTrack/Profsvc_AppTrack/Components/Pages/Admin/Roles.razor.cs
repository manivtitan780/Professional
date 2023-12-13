#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Roles.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-13-2023 19:58
// *****************************************/

#endregion

#region Using

using RoleDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.RoleDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the administration roles page in the Professional Services Application Tracker.
/// </summary>
/// <remarks>
///     This class is responsible for managing the roles in the administration section of the application.
///     It includes functionalities such as filtering roles, managing user login cookies, and navigation.
///     It also contains methods that are executed after the page is rendered and when the page is initialized.
/// </remarks>
public partial class Roles
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     Gets or sets the AdminGrid component in the administration roles page.
    /// </summary>
    /// <value>
    ///     The AdminGrid component of type <see cref="Role" />.
    /// </value>
    /// <remarks>
    ///     This property represents the AdminGrid component that is used for managing roles in the administration section of
    ///     the application.
    ///     The AdminGrid component provides functionalities such as sorting, paging, filtering, and other data operations.
    /// </remarks>
    private AdminGrid<Role> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of roles displayed in the administration grid.
    /// </summary>
    /// <value>
    ///     The count of roles.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the number of roles displayed in the administration grid.
    ///     It is updated whenever the data in the grid is handled, for example, when the grid is filtered or refreshed.
    /// </remarks>
    private static int Count
    {
        get;
        set;
    } = 24;

    /// <summary>
    ///     Gets or sets the RoleDialog instance used in the Roles page.
    /// </summary>
    /// <value>
    ///     The RoleDialog instance that provides the user interface for managing roles in the administration section of the
    ///     application.
    /// </value>
    private RoleDialog DialogRole
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter string used in the administration roles page.
    /// </summary>
    /// <value>
    ///     The filter string.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the filter string for the roles displayed in the administration grid.
    ///     It is updated whenever the filter value is changed.
    /// </remarks>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance used for invoking JavaScript functions from .NET code in the
    ///     administration roles page.
    /// </summary>
    /// <value>
    ///     The JavaScript runtime instance is of type <see cref="IJSRuntime" />.
    /// </value>
    /// <remarks>
    ///     This instance is used for executing JavaScript code directly from the .NET code.
    ///     For example, it is used to scroll to a specific grid row in the SaveRole method.
    /// </remarks>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the local storage service used in the administration roles page.
    /// </summary>
    /// <value>
    ///     The local storage service.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the local storage in the administration roles page.
    ///     It is used for storing and retrieving user-specific data like the auto role and user login cookies.
    ///     The local storage service is injected into the page and is used in methods such as OnAfterRenderAsync and
    ///     OnInitializedAsync.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the Roles class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the Roles class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<Roles> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the LoginCooky instance for the user in the administration roles page.
    /// </summary>
    /// <value>
    ///     The LoginCooky instance for the user.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the login cookie for the user in the administration roles page.
    ///     It is used in methods such as OnInitializedAsync to check if the user has administrator rights and to redirect the
    ///     user if necessary.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the NavigationManager instance for the administration roles page.
    /// </summary>
    /// <value>
    ///     The NavigationManager instance.
    /// </value>
    /// <remarks>
    ///     This property is used to manage navigation in the administration roles page.
    ///     It is used in methods such as OnInitializedAsync to redirect the user based on their role and login status.
    /// </remarks>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the RoleID associated with the current user session.
    /// </summary>
    /// <value>
    ///     The RoleID is a string value that represents the role identifier of the logged-in user.
    ///     It is used for role-based navigation and access control within the administration section of the application.
    /// </value>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the current Role instance selected in the administration roles page.
    /// </summary>
    /// <remarks>
    ///     This property holds the data of the selected role in the roles table. It is used for operations such as editing and
    ///     viewing role details.
    /// </remarks>
    private Role RoleRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets a clone of the Role record.
    /// </summary>
    /// <value>
    ///     The clone of the Role record.
    /// </value>
    /// <remarks>
    ///     This property is used to hold a copy of the Role record for operations such as editing or adding a role.
    ///     It is used to prevent direct modifications to the original Role record until the changes are confirmed.
    /// </remarks>
    private Role RoleRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the title for the role dialog in the administration roles page.
    /// </summary>
    /// <value>
    ///     The title of the role dialog. This value is either "Edit" or "Add" depending on the action being performed.
    /// </value>
    private string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data for the roles grid in the administration section of the Professional Services Application Tracker.
    /// </summary>
    /// <remarks>
    ///     This method is responsible for updating the count of roles displayed in the administration grid.
    ///     It is executed whenever the data in the grid is handled, for example, when the grid is filtered or refreshed.
    ///     If there are roles in the grid, it also selects the first role.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    private Task DataHandler()
    {
        return ExecuteMethod(() =>
                             {
                                 Count = AdminGrid.Grid.CurrentViewData.Count();
                                 return Count > 0 ? AdminGrid.Grid.SelectRowAsync(0) : Task.CompletedTask;
                             });
    }

    /// <summary>
    ///     Initiates the process of editing a role in the administration section of the Professional Services Application
    ///     Tracker.
    /// </summary>
    /// <param name="code">
    ///     The unique identifier of the role to be edited. If this parameter is not provided or is whitespace, a new role will
    ///     be created.
    /// </param>
    /// <remarks>
    ///     This method is responsible for preparing the role record for editing or addition. It selects the role in the grid
    ///     control
    ///     based on the provided code, sets the title of the dialog box, and shows the dialog box for editing or adding a
    ///     role.
    ///     If the code is not provided or is whitespace, a new role record is created and the title is set to "Add".
    ///     If the code is provided, the role record is copied from the existing role and the title is set to "Edit".
    /// </remarks>
    private Task EditRole(string code = "")
    {
        return ExecuteMethod(async () =>
                             {
                                 List<Role> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().ID != code)
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(code);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 if (code.NullOrWhiteSpace())
                                 {
                                     Title = "Add";
                                     if (RoleRecordClone == null)
                                     {
                                         RoleRecordClone = new();
                                     }
                                     else
                                     {
                                         RoleRecordClone.Clear();
                                     }

                                     RoleRecordClone.IsAdd = true;
                                 }
                                 else
                                 {
                                     Title = "Edit";
                                     RoleRecordClone = RoleRecord.Copy();
                                     RoleRecordClone.IsAdd = false;
                                 }

                                 StateHasChanged();
                                 await DialogRole.Dialog.ShowAsync();
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
    ///     Asynchronously filters the grid of roles based on the provided role.
    /// </summary>
    /// <param name="role">
    ///     The role used to filter the grid. It is an instance of <see cref="ChangeEventArgs{TValue, TKey}" />
    ///     where TValue is string and TKey is <see cref="ProfSvc_Classes.KeyValues" />.
    /// </param>
    /// <remarks>
    ///     This method sets the filter based on the value of the role, refreshes the grid to reflect the changes,
    ///     and prevents multiple simultaneous filter operations using a toggling mechanism.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    private Task FilterGrid(ChangeEventArgs<string, KeyValues> role)
    {
        return ExecuteMethod(() =>
                             {
                                 FilterSet(role.Value);
                                 return AdminGrid.Grid.Refresh();
                             });
    }

    /// <summary>
    ///     Sets the filter string used in the administration roles page.
    /// </summary>
    /// <param name="value">The new value to be set as the filter.</param>
    /// <remarks>
    ///     This method sets the filter string for the roles displayed in the administration grid.
    ///     It uses the 'FilterSet' method from the 'General' utility class to process and format the filter value.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Asynchronously initializes the administration roles page.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is called when the page is initialized. It retrieves the user's login cookie and role ID.
    ///     If the user is not an administrator, the method redirects the user to the home page.
    /// </remarks>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorageBlazored.GetItemAsStringAsync("autoRole");
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
    ///     Asynchronously refreshes the grid control displaying the roles in the administration section.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is used to update the grid control with the latest roles' data. It is typically called after any
    ///     operation that could modify the roles data, such as adding, editing, or deleting a role.
    /// </remarks>
    private Task RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the roles table on the administration roles page.
    /// </summary>
    /// <param name="role">
    ///     The selected Role instance from the roles table.
    /// </param>
    /// <remarks>
    ///     This method is triggered when a row in the roles table is selected. It sets the RoleRecord property to the selected
    ///     role.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<Role> role) => RoleRecord = role.Data;

    /// <summary>
    ///     Asynchronously saves the current role record to the server.
    /// </summary>
    /// <remarks>
    ///     This method sends a POST request to the server with the current role record clone as the body.
    ///     After the server responds, it refreshes the grid control and updates the selected row if necessary.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    private Task SaveRole()
    {
        return ExecuteMethod(async () =>
                             {
                                 string _response = await General.PostRest<string>("Admin/SaveRole", null, RoleRecordClone);

                                 if (_response.NullOrWhiteSpace())
                                 {
                                     _response = RoleRecordClone.ID;
                                 }

                                 RoleRecord = RoleRecordClone.Copy();
                                 await AdminGrid.Grid.Refresh();
                                 int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(_response);
                                 await AdminGrid.Grid.SelectRowAsync(_index);
                                 await JsRuntime.InvokeVoidAsync("scroll", _index);
                             });
    }

    /// <summary>
    ///     Represents an adaptor for administration roles in the Professional Services Application Tracker.
    /// </summary>
    /// <remarks>
    ///     This class is responsible for managing the data operations related to administration roles.
    ///     It includes functionalities such as reading role data asynchronously.
    /// </remarks>
    public class AdminRoleAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads role data from the server using the provided data manager request and filter string.
        /// </summary>
        /// <param name="dm">The data manager request object containing additional parameters for the server request.</param>
        /// <param name="key">An optional key parameter. It is not used in this method.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains a list of roles or a data result
        ///     object with the list of roles and their count, depending on the requirements of the data manager request.
        /// </returns>
        /// <remarks>
        ///     This method uses the 'GetRoleDataAdaptorAsync' method from the 'General' class to retrieve role data from the
        ///     server.
        ///     If the method is already reading data (_reading is true), it returns null to prevent concurrent reads.
        ///     After the data is read, _reading is set to false and the retrieved data is returned.
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
                object _returnValue = await General.GetRoleDataAdaptorAsync(Filter, dm);
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