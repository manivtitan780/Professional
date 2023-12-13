#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Experience.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-13-2023 19:51
// *****************************************/

#endregion

#region Using

using AdminListDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.AdminListDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the Experience page in the Admin section of the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     This class is responsible for managing the user experience in the Admin section. It includes functionality for
///     setting a filter, checking user roles, and redirecting non-administrator users. It also interacts with local
///     storage to retrieve user login information and the value of "autoExperience" for first-time rendering.
/// </remarks>
public partial class Experience
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private int _selectedID;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private byte _toggleValue;

    /// <summary>
    ///     Gets or sets the AdminListDialog instance used for managing administrative tasks in the Experience administration
    ///     page.
    /// </summary>
    /// <value>
    ///     The AdminListDialog instance.
    /// </value>
    /// <remarks>
    ///     This property is used to interact with the AdminListDialog control, which provides functionalities for handling
    ///     administrative tasks such as saving and cancelling operations.
    /// </remarks>
    private AdminListDialog AdminDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the AdminGrid component which is a generic admin grid for managing data of type AdminList.
    /// </summary>
    /// <value>
    ///     The AdminGrid component.
    /// </value>
    /// <remarks>
    ///     The AdminGrid component is used to manage the list of administrators in the Admin section of the ProfSvc_AppTrack
    ///     application.
    ///     It provides functionality for filtering, selecting, and editing administrator records.
    /// </remarks>
    private AdminGrid<AdminList> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of current view data in the grid on the Experience administration page.
    /// </summary>
    /// <value>
    ///     The count of current view data in the grid.
    /// </value>
    /// <remarks>
    ///     This property is used to store the number of records currently displayed in the grid. It is updated in the
    ///     DataHandler method.
    /// </remarks>
    private static int Count
    {
        get;
        set;
    } = 24;

    /// <summary>
    ///     Represents a single experience record in the Experience administration page of the Professional Services
    ///     Application Tracking system.
    /// </summary>
    /// <remarks>
    ///     This property is of type <see cref="AdminList" /> and is used to hold the data of the selected row in the
    ///     experience admin list.
    ///     It is also used to create a clone of the experience record when editing.
    /// </remarks>
    private AdminList ExperienceRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets a clone of an experience record in the administration list.
    /// </summary>
    /// <remarks>
    ///     This property is used to hold a copy of an experience record from the administration list.
    ///     It is utilized during the editing process, where a copy of the record is made and manipulated before being saved
    ///     back to the list. This ensures that the original record remains unaltered in case the editing process is cancelled
    ///     or an error occurs.
    /// </remarks>
    private AdminList ExperienceRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the filter value for the Experience administration page.
    /// </summary>
    /// <value>
    ///     The filter value used to filter the list of experience records displayed on the page.
    /// </value>
    /// <remarks>
    ///     This property is used to store the current filter value for the list of experience records.
    ///     The filter value is used in the ReadAsync method of the AdminEducationAdaptor class to filter the data retrieved
    ///     for the Experience administration page.
    ///     The filter value can be set using the FilterSet method, which processes the passed value before setting it as the
    ///     filter.
    /// </remarks>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance.
    /// </summary>
    /// <remarks>
    ///     This property is injected into the Experience class and is used to interact with JavaScript from C# code.
    ///     It is used in methods like 'SaveEducation' to call JavaScript functions for UI manipulations like scrolling to a
    ///     specific row in the grid.
    /// </remarks>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the local storage service used by the Experience administration page.
    /// </summary>
    /// <remarks>
    ///     This property is injected into the class and is used to interact with the browser's local storage. It is used to
    ///     retrieve user login information and check the user's role to ensure that only administrators have access to this
    ///     page.
    ///     It is also used to remember the filter settings for the list of experience records.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the Experience class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the Experience class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<Experience> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the login information for the current user.
    /// </summary>
    /// <remarks>
    ///     This property is used to store the login information of the current user, which is retrieved from local storage.
    ///     It is used to check the user's role and ensure that only administrators have access to the Experience
    ///     administration page.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the NavigationManager service used by the Experience administration page.
    /// </summary>
    /// <remarks>
    ///     This property is injected into the class and is used to manage navigation for the application.
    ///     It is used to redirect the user to the home page if they are not an administrator.
    /// </remarks>
    [Inject]
    private NavigationManager NavManager
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the RoleID of the currently logged-in user.
    /// </summary>
    /// <remarks>
    ///     This property is used to store the RoleID of the user retrieved from the login information.
    ///     It is used to determine the user's access level to the Experience administration page.
    /// </remarks>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title for the Experience administration page.
    /// </summary>
    /// <value>
    ///     The title is a string that represents the current action (either "Add" or "Edit") being performed on the experience
    ///     records.
    /// </value>
    /// <remarks>
    ///     The title is used in the header of the `AdminListDialog` component in the Experience administration page.
    ///     It changes based on whether a new experience record is being added or an existing one is being edited.
    /// </remarks>
    private static string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data for the Experience class. This method updates the Count property with the current count of the
    ///     Grid's data and selects the first row if the count is greater than zero.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private Task DataHandler(object obj)
    {
        return ExecuteMethod(() =>
                             {
                                 Count = AdminGrid.Grid.CurrentViewData.Count();
                                 return Count > 0 ? AdminGrid.Grid.SelectRowAsync(0) : Task.CompletedTask;
                             });
    }

    /// <summary>
    ///     Asynchronously edits the experience record with the specified ID.
    ///     If the ID is 0, a new experience record is created.
    ///     The method updates the selected row in the grid, sets the title of the dialog, and prepares the eligibility record
    ///     for editing.
    ///     After all preparations, it opens the Admin Dialog for editing.
    /// </summary>
    /// <param name="id">The ID of the experience record to edit. If this parameter is 0, a new record will be created.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private Task EditExperience(int id = 0)
    {
        return ExecuteMethod(async () =>
                             {
                                 List<AdminList> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().ID != id)
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(id);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 if (id == 0)
                                 {
                                     Title = "Add";
                                     if (ExperienceRecordClone == null)
                                     {
                                         ExperienceRecordClone = new();
                                     }
                                     else
                                     {
                                         ExperienceRecordClone.Clear();
                                     }
                                 }
                                 else
                                 {
                                     Title = "Edit";
                                     ExperienceRecordClone = ExperienceRecord.Copy();
                                 }

                                 ExperienceRecordClone.Entity = "Experience";

                                 StateHasChanged();
                                 await AdminDialog.ShowDialog();
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
    ///     This method is responsible for filtering the grid of the Experience page based on the experience criteria.
    ///     It sets the filter value and refreshes the grid to reflect the changes.
    ///     The method is asynchronous and is triggered when there is a change in the experience criteria.
    /// </summary>
    /// <param name="experience">
    ///     The experience criteria as a key-value pair. The key represents the field to be filtered and
    ///     the value represents the filter value.
    /// </param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private Task FilterGrid(ChangeEventArgs<string, KeyValues> experience)
    {
        return ExecuteMethod(() =>
                             {
                                 FilterSet(experience.Value);
                                 return AdminGrid.Grid.Refresh();
                             });
    }

    /// <summary>
    ///     Sets the filter value for the Experience class. The method uses the General.FilterSet method to process the passed
    ///     value before setting it to the Filter property.
    /// </summary>
    /// <param name="value">The string value to be set as the filter.</param>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Asynchronously initializes the Experience component.
    /// </summary>
    /// <remarks>
    ///     This method is called when the component is first initialized.
    ///     It retrieves the user's login information from local storage and checks the user's role.
    ///     If the user's role is not "AD" (Administrator), the user is redirected to the home page.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorage);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorage.GetItemAsStringAsync("autoExperience");
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
    ///     Asynchronously refreshes the data grid on the Experience admin page.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private void RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the Experience grid.
    /// </summary>
    /// <param name="experience">The selected row data of type <see cref="AdminList" />.</param>
    private void RowSelected(RowSelectEventArgs<AdminList> experience) => ExperienceRecord = experience.Data;

    /// <summary>
    ///     Asynchronously saves the experience record.
    /// </summary>
    /// <param name="context">The context for the form being edited.</param>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Yields control to allow the UI to update.
    ///     - Calls the General.SaveAdminListAsync method with the following parameters:
    ///     - "Admin_SaveExperience" as the method name.
    ///     - "Experience" as the parameter name.
    ///     - false for both containDescription and isString.
    ///     - The cloned experience record.
    ///     - The grid control.
    ///     - The original eligibility record.
    ///     - The injected JavaScript runtime.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task SaveExperience(EditContext context)
    {
        return ExecuteMethod(() => General.SaveAdminListAsync("Admin_SaveExperience", "Experience", false, false, ExperienceRecordClone, AdminGrid.Grid, ExperienceRecord, JsRuntime));
    }

    /// <summary>
    ///     This method is used to toggle the status of a selected record in the Admin List.
    /// </summary>
    /// <param name="id">The ID of the selected record.</param>
    /// <param name="enabled">
    ///     The new status to be set for the record. If true, the status will be set to 2, otherwise it will
    ///     be set to 1.
    /// </param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private Task ToggleMethod(int id, bool enabled)
    {
        return ExecuteMethod(async () =>
                             {
                                 _selectedID = id;
                                 _toggleValue = enabled ? (byte)2 : (byte)1;
                                 List<AdminList> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().ID != id)
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(id);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 await AdminGrid.DialogConfirm.ShowDialog();
                             });
    }

    /// <summary>
    ///     Asynchronously toggles the status of an experience record.
    /// </summary>
    /// <param name="experienceID">The ID of the experience record to toggle.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method posts a toggle request to the "Admin_ToggleExperienceStatus" endpoint.
    ///     The status is toggled only if there is no ongoing toggle operation.
    ///     After the toggle operation, the grid is refreshed to reflect the changes.
    /// </remarks>
    private Task ToggleStatusAsync(int experienceID)
    {
        return ExecuteMethod(() => General.PostToggleAsync("Admin_ToggleExperienceStatus", experienceID, "ADMIN", false, AdminGrid.Grid, runtime: JsRuntime));
    }

    /// <summary>
    ///     The 'AdminExperienceAdaptor' class is a custom data adaptor used in the 'Experience' admin page.
    ///     It inherits from the 'DataAdaptor' class and overrides the 'ReadAsync' method.
    /// </summary>
    /// <remarks>
    ///     This class is used to asynchronously fetch eligibility data for the admin grid.
    ///     It uses the 'DataManagerRequest' and an optional key to read the data.
    ///     The 'ReadAsync' method checks if a read operation is already in progress, if not, it initiates a new read
    ///     operation.
    ///     The read operation is performed by calling the 'General.GetReadAsync' method with specific parameters.
    /// </remarks>
    public class AdminExperienceAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads data using the DataManagerRequest and a key.
        /// </summary>
        /// <param name="dm">The DataManagerRequest to use for reading data.</param>
        /// <param name="key">An optional key to use for reading data. Default is null.</param>
        /// <returns>
        ///     A Task that represents the asynchronous read operation. The Task's result is the data that was read.
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
                object _returnValue = await General.GetReadAsync("Admin_GetExperience", Filter, dm, false);
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