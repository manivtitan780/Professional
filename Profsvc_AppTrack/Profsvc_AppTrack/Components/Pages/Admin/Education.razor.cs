#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Education.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     11-04-2023 20:28
// *****************************************/

#endregion

using AdminListDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.AdminListDialog;

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the Education administration page in the Professional Services Application Tracking system.
/// </summary>
/// <remarks>
///     This class is responsible for managing the Education administration interface, which includes displaying a list of
///     education records, providing options to add, edit, and toggle the status of these records.
///     The class interacts with the local storage to retrieve user login information and checks the user's role to ensure
///     that only administrators have access to this page. It also uses local storage to remember the filter settings for
///     the list of education records.
/// </remarks>
public partial class Education
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private int _selectedID;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private byte _toggleValue;

    /// <summary>
    ///     Gets or sets the AdminListDialog instance used for managing administrative tasks in the Education administration
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
    ///     Gets or sets the AdminGrid component used for displaying and managing the list of education records.
    /// </summary>
    /// <value>
    ///     The AdminGrid component of type <see cref="AdminGrid{AdminList}" />.
    /// </value>
    /// <remarks>
    ///     This property is used to interact with the AdminGrid component, which provides functionalities such as displaying a
    ///     list of education records,
    ///     and providing options to add, edit, and toggle the status of these records.
    /// </remarks>
    private AdminGrid<AdminList> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of current view data in the grid on the Education administration page.
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
    ///     Represents a single education record in the Education administration page of the Professional Services Application
    ///     Tracking system.
    /// </summary>
    /// <remarks>
    ///     This property is of type <see cref="AdminList" /> and is used to hold the data of the selected row in the education
    ///     admin list.
    ///     It is also used to create a clone of the education record when editing.
    /// </remarks>
    private AdminList EducationRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets a clone of an education record in the administration list.
    /// </summary>
    /// <remarks>
    ///     This property is used to hold a copy of an education record from the administration list.
    ///     It is utilized during the editing process, where a copy of the record is made and manipulated
    ///     before being saved back to the list. This ensures that the original record remains unaltered
    ///     in case the editing process is cancelled or an error occurs.
    /// </remarks>
    private AdminList EducationRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the filter value for the Education administration page.
    /// </summary>
    /// <value>
    ///     The filter value used to filter the list of education records displayed on the page.
    /// </value>
    /// <remarks>
    ///     This property is used to store the current filter value for the list of education records.
    ///     The filter value is used in the ReadAsync method of the AdminEducationAdaptor class to filter the data retrieved
    ///     for the Education administration page.
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
    ///     This property is injected into the Education class and is used to interact with JavaScript from C# code.
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
    ///     Gets or sets the local storage service used by the Education administration page.
    /// </summary>
    /// <remarks>
    ///     This property is injected into the class and is used to interact with the browser's local storage. It is used to
    ///     retrieve user login information and check the user's role to ensure that only administrators have access to this
    ///     page.
    ///     It is also used to remember the filter settings for the list of education records.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the Education class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the Education class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<Education> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the login information for the current user.
    /// </summary>
    /// <remarks>
    ///     This property is used to store the login information of the current user, which is retrieved from local storage.
    ///     It is used to check the user's role and ensure that only administrators have access to the Education administration
    ///     page.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the NavigationManager service used by the Education administration page.
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
    ///     Gets or sets the RoleID of the currently logged in user.
    /// </summary>
    /// <remarks>
    ///     This property is used to store the RoleID of the user retrieved from the login information.
    ///     It is used to determine the user's access level to the Education administration page.
    /// </remarks>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title for the Education administration page.
    /// </summary>
    /// <value>
    ///     The title is a string that represents the current action (either "Add" or "Edit") being performed on the education
    ///     records.
    /// </value>
    /// <remarks>
    ///     The title is used in the header of the `AdminListDialog` component in the Education administration page.
    ///     It changes based on whether a new education record is being added or an existing one is being edited.
    /// </remarks>
    private string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data for the Education page.
    /// </summary>
    /// <param name="obj">The object that triggers the data handling.</param>
    /// <remarks>
    ///     This method counts the current view data of the grid. If the count is more than 0, it selects the first row of the
    ///     grid.
    /// </remarks>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
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
    ///     Initiates the process of editing an education record.
    /// </summary>
    /// <param name="id">The ID of the education record to be edited. If the ID is 0, a new record will be created.</param>
    /// <remarks>
    ///     This method first checks if the selected record in the grid matches the provided ID. If not, it selects the row
    ///     with the provided ID.
    ///     If the ID is 0, indicating a new record, it prepares a new instance of EducationRecordClone for data entry.
    ///     If the ID is not 0, it makes a copy of the existing record into EducationRecordClone for editing.
    ///     The method then sets the 'Entity' property of EducationRecordClone to "Education", triggers a UI refresh, and opens
    ///     the AdminDialog for data entry.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task EditEducation(int id = 0)
    {
        await ExecuteMethod(async () =>
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
                                    if (EducationRecordClone == null)
                                    {
                                        EducationRecordClone = new();
                                    }
                                    else
                                    {
                                        EducationRecordClone.Clear();
                                    }
                                }
                                else
                                {
                                    Title = "Edit";
                                    EducationRecordClone = EducationRecord.Copy();
                                }

                                EducationRecordClone.Entity = "Education";

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
    private async Task ExecuteMethod(Func<Task> task)
    {
        await General.ExecuteMethod(_semaphore, task, Logger);
    }

    /// <summary>
    ///     Filters the grid of education records based on the provided education key-value pair.
    /// </summary>
    /// <param name="education">The key-value pair representing the education filter.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method sets the filter value and refreshes the grid. It uses a toggling mechanism to prevent multiple
    ///     simultaneous invocations.
    /// </remarks>
    private async Task FilterGrid(ChangeEventArgs<string, KeyValues> education)
    {
        await ExecuteMethod(async () =>
                            {
                                FilterSet(education.Value);
                                await AdminGrid.Grid.Refresh();
                            });
    }

    /// <summary>
    ///     Sets the filter value for the Education component.
    /// </summary>
    /// <param name="value">The value to be set as the filter.</param>
    /// <remarks>
    ///     This method calls the General.FilterSet method to process the passed value before setting it as the filter.
    ///     If the passed value is not null or whitespace and is not equal to "null", it is used as the filter value.
    ///     Otherwise, the filter value is set to an empty string.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Initializes the component asynchronously. This method is invoked when the component is ready to start,
    ///     and is used for two main reasons:
    ///     - To perform an asynchronous operation and avoid blocking the component's rendering until the operation is
    ///     completed.
    ///     - To perform operations that should only occur once in the component's lifecycle.
    /// </summary>
    /// <remarks>
    ///     This method retrieves the user's login information from local storage and checks if the user's role is "AD"
    ///     (Administrator).
    ///     If the user is not an Administrator, they are redirected to the home page.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorage);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorage.GetItemAsStringAsync("autoEducation");
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
    ///     Refreshes the grid data in the Education component.
    /// </summary>
    /// <remarks>
    ///     This method is used to update the grid data by calling the Refresh method of the SfGrid component.
    ///     It should be called whenever there is a change in the data that needs to be reflected in the grid.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task RefreshGrid() => await AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the education admin list.
    /// </summary>
    /// <param name="education">The selected row data of type <see cref="AdminList" />.</param>
    /// <remarks>
    ///     This method sets the selected row data to the EducationRecord property.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<AdminList> education) => EducationRecord = education.Data;

    /// <summary>
    ///     Asynchronously saves the education record.
    /// </summary>
    /// <param name="context">The context for the edit operation.</param>
    /// <remarks>
    ///     This method makes a call to the 'SaveAdminListAsync' method of the 'General' class, passing in specific parameters
    ///     including the cloned education record and the grid.
    ///     The method 'SaveAdminListAsync' is responsible for saving the admin list to the database and refreshing the grid.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task SaveEducation(EditContext context)
    {
        await ExecuteMethod(async () =>
                            {
                                await General.SaveAdminListAsync("Admin_SaveEducation", "Education", false, false, EducationRecordClone, AdminGrid.Grid,
                                                                 EducationRecord, JsRuntime);
                            });
    }

    /// <summary>
    ///     Toggles the status of an entity in the admin list.
    /// </summary>
    /// <param name="id">The ID of the entity to be toggled.</param>
    /// <param name="enabled">
    ///     The new status of the entity. If true, the entity will be enabled; if false, the entity will be
    ///     disabled.
    /// </param>
    /// <remarks>
    ///     This method sets the selected ID and toggle value based on the parameters, then checks if the selected entity in
    ///     the grid matches the provided ID. If not, it selects the row with the matching ID in the grid. Finally, it shows a
    ///     confirmation dialog.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task ToggleMethod(int id, bool enabled)
    {
        await ExecuteMethod(async () =>
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
    ///     Toggles the status of an education item identified by its ID.
    /// </summary>
    /// <param name="educationID">The ID of the education item whose status is to be toggled.</param>
    /// <remarks>
    ///     This method makes use of the PostToggleAsync method from the General class to toggle the status of the education
    ///     item.
    ///     The method is asynchronous and can be awaited. It ensures that the toggling operation is not performed concurrently
    ///     by checking the _toggling flag.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task ToggleStatus(int educationID)
    {
        await ExecuteMethod(async () => { await General.PostToggleAsync("Admin_ToggleEducationStatus", educationID, "ADMIN", false, AdminGrid.Grid, runtime: JsRuntime); });
    }

    /// <summary>
    ///     The AdminEducationAdaptor class is a data adaptor for the Education administration page.
    ///     It inherits from the DataAdaptor base class and overrides the ReadAsync method.
    ///     This class is used to handle the data operations for the Education administration grid in the application.
    /// </summary>
    internal class AdminEducationAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads data for the Education administration page.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object that holds the data operations like sorting and filtering.</param>
        /// <param name="key">An optional key to identify a particular data record. This parameter is not used in this method.</param>
        /// <returns>
        ///     A Task that represents the asynchronous operation. The task result contains the data for the Education
        ///     administration page.
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
                object _returnValue = await General.GetReadAsync("Admin_GetEducation", Filter, dm, false);
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