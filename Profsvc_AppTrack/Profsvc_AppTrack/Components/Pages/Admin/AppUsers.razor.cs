#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           AppUsers.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-12-2023 15:39
// *****************************************/

#endregion

#region Using

using UserDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.UserDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     The 'AppUsers' class is a part of the 'ProfSvc_AppTrack.Pages.Admin' namespace.
///     This class is responsible for managing the application users in the administrative context.
///     It contains methods for rendering the component and initializing the component.
///     The 'OnAfterRenderAsync' method is called after the component has finished rendering and is used to retrieve the
///     "autoUser" item from the local storage and set the filter accordingly.
///     The 'OnInitializedAsync' method is called when the `AppUsers` component is initialized. It retrieves the
///     `LoginCookyUser` object from the local storage and checks the user's role. If the user's role is not "AD"
///     (Administrator), the user is redirected to the Dashboard page.
///     The class also contains a static property 'Roles' which holds a list of roles.
/// </summary>
public partial class AppUsers
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private string _selectedID;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private byte _toggleValue;

    /// <summary>
    ///     Gets or sets the AdminGrid component in the AppUsers page.
    /// </summary>
    /// <value>
    ///     The AdminGrid component of type <see cref="AdminGrid{User}" />.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the grid of users in the administrative context.
    ///     It provides functionalities such as selecting a row, getting selected records, refreshing the grid, etc.
    /// </remarks>
    private AdminGrid<User> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of users currently displayed in the grid view on the AppUsers page.
    ///     This property is used in the 'DataHandler' method to store the count of users and
    ///     to select the first user if the count is more than zero.
    /// </summary>
    private static int Count
    {
        get;
        set;
    } = 24;

    /// <summary>
    ///     Gets or sets the 'UserDialog' instance used for managing user information in the administrative context.
    ///     This dialog is used for both creating new users and editing existing users.
    /// </summary>
    private UserDialog DialogUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter value for the application users in the administrative context.
    ///     This static property is used to filter the users based on certain criteria in the administrative context.
    /// </summary>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ID of the application user. This private property is used to identify a specific user in the
    ///     administrative context.
    /// </summary>
    private string ID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance. The JavaScript runtime provides a mechanism for running JavaScript in
    ///     the context of the component.
    ///     This property is injected into the component and is used to call JavaScript functions from .NET code.
    ///     For example, it is used in the 'Save' method to scroll to a specific row in the grid, and in the
    ///     'ToggleStatusAsync' method to toggle the status of a user.
    /// </summary>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the ILocalStorageService. This service is used for managing the local storage of the
    ///     browser.
    ///     It is used in this class to retrieve and store user-specific data, such as the "autoUser" item and the
    ///     `LoginCookyUser` object.
    /// </summary>
    [Inject]
    private ILocalStorageService LocalStorage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the AppUsers class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the AppUsers class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<AppUsers> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the `LoginCooky` object for the current user.
    ///     This object contains information about the user's login session, including their ID, name, email address, role,
    ///     last login date, and login IP.
    ///     It is used to manage user authentication and authorization within the application.
    /// </summary>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the NavigationManager. This service is used for managing navigation across the
    ///     application.
    ///     It is used in this class to redirect the user to different pages based on their role and authentication status.
    ///     For example, if the user's role is not "AD" (Administrator), the user is redirected to the Dashboard page.
    /// </summary>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the RoleID for the current user. The RoleID is used to determine the user's permissions within the
    ///     application.
    /// </summary>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of roles for application users. Each role is represented by a 'KeyValues' object.
    ///     This static property is used to manage the roles of users in the administrative context.
    /// </summary>
    public static List<KeyValues> Roles
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title of the User Dialog in the administrative context.
    ///     The title changes based on the action being performed on the user record - "Add" when a new user is being added,
    ///     and "Edit" when an existing user's details are being modified.
    /// </summary>
    private string Title
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the UserRecord property of the AppUsers class.
    ///     The UserRecord property represents a single user in the application.
    ///     It is used to hold the data of the selected user in the user grid.
    ///     The data is encapsulated in a User object, which is defined in the ProfSvc_Classes namespace.
    /// </summary>
    private User UserRecord
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the clone of a User record. This property is used to hold a copy of a User record for operations like
    ///     editing or adding a user.
    ///     When adding a new user, a new instance of User is created and assigned to this property.
    ///     When editing an existing user, a copy of the User record to be edited is created and assigned to this property.
    /// </summary>
    private User UserRecordClone
    {
        get;
        set;
    }

    /// <summary>
    ///     Handles the data processing for the AppUsers page.
    ///     It counts the number of users currently displayed in the grid view and selects the first user if the count is more
    ///     than zero.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private Task DataHandler()
    {
        return ExecuteMethod(() =>
                             {
                                 Count = AdminGrid.Grid.CurrentViewData.Count();
                                 return Count > 0 ? AdminGrid.Grid.SelectRowAsync(0) : Task.CompletedTask;
                             });
    }

    /// <summary>
    ///     This method is used to edit a user record. If an ID is provided, it selects the corresponding user from the grid
    ///     and sets the title to "Edit".
    ///     If no ID is provided, it sets the title to "Add" and prepares for the creation of a new user record.
    ///     After setting up, it opens the UserDialog for user interaction.
    /// </summary>
    /// <param name="id">The ID of the user to be edited. If empty, a new user record will be prepared for creation.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private Task EditUserAsync(string id = "")
    {
        return ExecuteMethod(async () =>
                             {
                                 List<User> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().UserName.ToUpperInvariant().Trim() != id.ToUpperInvariant().Trim())
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(id);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 if (id.NullOrWhiteSpace())
                                 {
                                     Title = "Add";
                                     if (UserRecordClone == null)
                                     {
                                         UserRecordClone = new();
                                     }
                                     else
                                     {
                                         UserRecordClone.Clear();
                                     }

                                     UserRecordClone.IsAdd = true;
                                 }
                                 else
                                 {
                                     Title = "Edit";
                                     UserRecordClone = UserRecord.Copy();
                                     UserRecordClone.IsAdd = false;
                                 }

                                 StateHasChanged();
                                 await DialogUser.Dialog.ShowAsync();
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
    ///     Applies the specified filter to the grid of users.
    /// </summary>
    /// <param name="designation">
    ///     The filter to be applied, represented as a change event argument containing the new filter
    ///     value.
    /// </param>
    /// <remarks>
    ///     This method sets the filter for the grid and then refreshes the grid to apply the filter.
    ///     The filter value is obtained from the 'Value' property of the 'designation' parameter.
    /// </remarks>
    private Task FilterGrid(ChangeEventArgs<string, KeyValues> designation)
    {
        return ExecuteMethod(() =>
                             {
                                 FilterSet(designation.Value);
                                 return AdminGrid.Grid.Refresh();
                             });
    }

    /// <summary>
    ///     Sets the filter for the user grid.
    /// </summary>
    /// <param name="value">The value to be set as the filter.</param>
    /// <remarks>
    ///     This method calls the General.FilterSet method to process the passed value and assigns the result to the Filter
    ///     property.
    ///     If the passed value is null, empty, or whitespace, the Filter property is set to an empty string.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     This method is called when the `AppUsers` component is initialized.
    ///     It retrieves the `LoginCookyUser` object from the local storage and checks the user's role.
    ///     If the user's role is not "AD" (Administrator), the user is redirected to the Dashboard page.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorage);
                                string _result = await LocalStorage.GetItemAsStringAsync("autoUser");
                                FilterSet(_result);
                                RoleID = LoginCookyUser.RoleID;
                                if (!LoginCookyUser.IsAdmin()) //Administrator only has the rights.
                                {
                                    NavManager.NavigateTo($"{NavManager.BaseUri}home", true);
                                }
                            });

        _initializationTaskSource.SetResult(true);
        await base.OnInitializedAsync();
    }

    /// <summary>
    ///     Refreshes the user grid in the AppUsers page.
    /// </summary>
    /// <remarks>
    ///     This method is used to update the user grid by calling the Refresh method of the SfGrid component.
    ///     It should be called whenever a change is made that needs to be reflected in the grid.
    /// </remarks>
    private void RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event of a row being selected in the user grid.
    ///     It sets the UserRecord property to the data of the selected user.
    /// </summary>
    /// <param name="user">The selected user's data encapsulated in a RowSelectEventArgs object.</param>
    private void RowSelected(RowSelectEventArgs<User> user) => UserRecord = user.Data;

    /// <summary>
    ///     This method is used to save the changes made to a user's record.
    ///     It first hashes the password of the user, then makes a POST request to the 'Admin/SaveUser' endpoint with the
    ///     user's record and hashed password.
    ///     After the request is completed, it refreshes the grid and selects the row of the updated user.
    /// </summary>
    /// <param name="arg">The EditContext of the form.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private Task Save(EditContext arg)
    {
        return ExecuteMethod(async () =>
                             {
                                 await General.PostRest<string>("Admin/SaveUser", null, UserRecordClone);

                                 ID = UserRecordClone.UserName;
                                 UserRecord = UserRecordClone.Copy();
                                 await AdminGrid.Grid.Refresh();
                                 int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(ID);
                                 await AdminGrid.Grid.SelectRowAsync(_index);
                                 await JsRuntime.InvokeVoidAsync("scroll", _index);
                             });
    }

    /// <summary>
    ///     Toggles the status of a user in the application.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="enabled">The new status of the user. If true, the user is enabled; if false, the user is disabled.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private Task ToggleMethod(string id, bool enabled)
    {
        return ExecuteMethod(async () =>
                             {
                                 _selectedID = id;
                                 _toggleValue = enabled ? (byte)2 : (byte)1;
                                 List<User> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any())
                                 {
                                     if (_selectedList.First().UserName != id)
                                     {
                                         int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(id);
                                         await AdminGrid.Grid.SelectRowAsync(_index);
                                     }
                                 }

                                 await AdminGrid.DialogConfirm.ShowDialog();
                             });
    }

    /// <summary>
    ///     Toggles the status of a user asynchronously.
    /// </summary>
    /// <param name="userName">The username of the user whose status is to be toggled.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sends a request to the "Admin_ToggleUserStatus" endpoint with the provided username.
    ///     The status of the user is toggled only if the method is not already in the process of toggling a user's status.
    ///     After the status is toggled, the method refreshes the grid and selects the row of the user whose status was
    ///     toggled.
    /// </remarks>
    private Task ToggleStatusAsync(string userName)
    {
        return ExecuteMethod(() => General.PostToggleAsync("Admin_ToggleUserStatus", userName, "ADMIN", true, AdminGrid.Grid, true, JsRuntime));
    }

    /// <summary>
    ///     The AdminUserAdaptor class is a custom data adaptor used in the Admin Users page.
    ///     It inherits from the DataAdaptor class and overrides the ReadAsync method.
    /// </summary>
    /// <remarks>
    ///     This class is used in the Syncfusion Data Manager component in the Admin Users page to handle data operations.
    ///     The ReadAsync method is called when the Data Manager component needs to read data.
    ///     It uses the General.GetUserReadAsync method to fetch user data based on the provided filter and DataManagerRequest.
    /// </remarks>
    public class AdminUserAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads data for the Admin Users page.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object that contains the parameters for the data request.</param>
        /// <param name="key">An optional key to identify the data. This parameter is not used in this method.</param>
        /// <returns>
        ///     A Task that represents the asynchronous operation. The Task result contains the data read from the Admin Users
        ///     page.
        /// </returns>
        /// <remarks>
        ///     This method uses the General.GetUserReadAsync method to fetch user data based on the provided filter and
        ///     DataManagerRequest.
        ///     It ensures that only one read operation is in progress at a time.
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
                object _returnValue = await General.GetUserReadAsync(Filter, dm);
                return _returnValue;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}