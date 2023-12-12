#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Eligibility.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     11-04-2023 20:27
// *****************************************/

#endregion

using AdminListDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.AdminListDialog;

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the administration page for managing eligibility records.
/// </summary>
/// <remarks>
///     This class is responsible for the functionality of the Eligibility administration page. It includes properties for
///     managing local storage, navigation, and user login information. It also includes methods for initializing the
///     component and executing tasks after the component has finished rendering. Only administrators have access to this
///     page.
/// </remarks>
public partial class Eligibility
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private int _selectedID;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private byte _toggleValue;

    /// <summary>
    ///     Gets or sets the AdminListDialog instance used for managing administrative tasks in the Eligibility administration
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
    ///     Gets or sets the AdminGrid property, which represents a generic admin grid component for managing data of type
    ///     AdminList.
    /// </summary>
    /// <value>
    ///     The AdminGrid property gets/sets the value of the field, <see cref="AdminGrid{AdminList}" />.
    /// </value>
    /// <remarks>
    ///     This property is used to manage and manipulate the administrative grid on the Eligibility administration page.
    ///     It provides various parameters for customization and includes a ConfirmDialog and a SfGrid for user interactions.
    /// </remarks>
    private AdminGrid<AdminList> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of current view data in the grid on the Eligibility administration page.
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
    ///     Represents a single eligibility record in the Eligibility administration page of the Professional Services
    ///     Application Tracking system.
    /// </summary>
    /// <remarks>
    ///     This property is of type <see cref="AdminList" /> and is used to hold the data of the selected row in the
    ///     eligibility admin list.
    ///     It is also used to create a clone of the eligibility record when editing.
    /// </remarks>
    private AdminList EligibilityRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets a clone of an eligibility record in the administration list.
    /// </summary>
    /// <remarks>
    ///     This property is used to hold a copy of an eligibility record from the administration list.
    ///     It is utilized during the editing process, where a copy of the record is made and manipulated before being saved
    ///     back to the list. This ensures that the original record remains unaltered in case the editing process is cancelled
    ///     or an error occurs.
    /// </remarks>
    private AdminList EligibilityRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the filter value for the Eligibility administration page.
    /// </summary>
    /// <value>
    ///     The filter value used to filter the list of eligibility records displayed on the page.
    /// </value>
    /// <remarks>
    ///     This property is used to store the current filter value for the list of eligibility records.
    ///     The filter value is used in the ReadAsync method of the AdminEducationAdaptor class to filter the data retrieved
    ///     for the Eligibility administration page.
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
    ///     This property is injected into the Eligibility class and is used to interact with JavaScript from C# code.
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
    ///     Gets or sets the local storage service used by the Eligibility administration page.
    /// </summary>
    /// <remarks>
    ///     This property is injected into the class and is used to interact with the browser's local storage. It is used to
    ///     retrieve user login information and check the user's role to ensure that only administrators have access to this
    ///     page.
    ///     It is also used to remember the filter settings for the list of eligibility records.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the Eligibility class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the Eligibility class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<Eligibility> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the login information for the current user.
    /// </summary>
    /// <remarks>
    ///     This property is used to store the login information of the current user, which is retrieved from local storage.
    ///     It is used to check the user's role and ensure that only administrators have access to the Eligibility
    ///     administration page.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the NavigationManager service used by the Eligibility administration page.
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
    ///     It is used to determine the user's access level to the Eligibility administration page.
    /// </remarks>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title for the Eligibility administration page.
    /// </summary>
    /// <value>
    ///     The title is a string that represents the current action (either "Add" or "Edit") being performed on the
    ///     eligibility records.
    /// </value>
    /// <remarks>
    ///     The title is used in the header of the `AdminListDialog` component in the Eligibility administration page.
    ///     It changes based on whether a new eligibility record is being added or an existing one is being edited.
    /// </remarks>
    private static string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data for the Eligibility class. This method updates the Count property with the current count of the
    ///     Grid's data and selects the first row if the count is greater than zero.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private async Task DataHandler()
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
    ///     Asynchronously edits the eligibility record with the specified ID.
    ///     If the ID is 0, a new eligibility record is created.
    ///     The method updates the selected row in the grid, sets the title of the dialog, and prepares the eligibility record
    ///     for editing.
    ///     After all preparations, it opens the Admin Dialog for editing.
    /// </summary>
    /// <param name="id">The ID of the eligibility record to edit. If this parameter is 0, a new record will be created.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private async Task EditEligibility(int id = 0)
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
                                    if (EligibilityRecordClone == null)
                                    {
                                        EligibilityRecordClone = new();
                                    }
                                    else
                                    {
                                        EligibilityRecordClone.Clear();
                                    }
                                }
                                else
                                {
                                    Title = "Edit";
                                    EligibilityRecordClone = EligibilityRecord.Copy();
                                }

                                EligibilityRecordClone.Entity = "Eligibility";

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
    ///     This method is responsible for filtering the grid of the Eligibility page based on the eligibility criteria.
    ///     It sets the filter value and refreshes the grid to reflect the changes.
    ///     The method is asynchronous and is triggered when there is a change in the eligibility criteria.
    /// </summary>
    /// <param name="eligibility">
    ///     The eligibility criteria as a key-value pair. The key represents the field to be filtered and the value represents
    ///     the filter value.
    /// </param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task FilterGrid(ChangeEventArgs<string, KeyValues> eligibility)
    {
        await ExecuteMethod(async () =>
                            {
                                FilterSet(eligibility.Value);
                                await AdminGrid.Grid.Refresh();
                            });
    }

    /// <summary>
    ///     Sets the filter value for the Eligibility class. The method uses the General.FilterSet method to process the passed
    ///     value before setting it to the Filter property.
    /// </summary>
    /// <param name="value">The string value to be set as the filter.</param>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Asynchronously initializes the Eligibility component.
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
                                string _result = await LocalStorage.GetItemAsStringAsync("autoEligibility");
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
    ///     Asynchronously refreshes the data grid on the Eligibility admin page.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task RefreshGrid() => await AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the Eligibility grid.
    /// </summary>
    /// <param name="eligibility">The selected row data of type <see cref="AdminList" />.</param>
    private void RowSelected(RowSelectEventArgs<AdminList> eligibility) => EligibilityRecord = eligibility.Data;

    /// <summary>
    ///     Asynchronously saves the eligibility record.
    /// </summary>
    /// <param name="context">The context for the form being edited.</param>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Yields control to allow the UI to update.
    ///     - Calls the General.SaveAdminListAsync method with the following parameters:
    ///     - "Admin_SaveEligibility" as the method name.
    ///     - "Eligibility" as the parameter name.
    ///     - false for both containDescription and isString.
    ///     - The cloned eligibility record.
    ///     - The grid control.
    ///     - The original eligibility record.
    ///     - The injected JavaScript runtime.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SaveEligibility(EditContext context)
    {
        await ExecuteMethod(async () =>
                            {
                                await General.SaveAdminListAsync("Admin_SaveEligibility", "Eligibility", false, false, EligibilityRecordClone, AdminGrid.Grid, EligibilityRecord, JsRuntime);
                            });
    }

    /// <summary>
    ///     This method is used to toggle the status of a selected record in the Admin List.
    /// </summary>
    /// <param name="id">The ID of the selected record.</param>
    /// <param name="enabled">
    ///     The new status to be set for the record. If true, the status will be set to 2, otherwise it will be set to 1.
    /// </param>
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
    ///     Asynchronously toggles the status of an eligibility record.
    /// </summary>
    /// <param name="eligibilityID">The ID of the eligibility record to toggle.</param>
    /// <remarks>
    ///     This method posts a toggle request to the "Admin_ToggleEligibilityStatus" endpoint.
    ///     The status is toggled only if there is no ongoing toggle operation.
    ///     After the toggle operation, the grid is refreshed to reflect the changes.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    private async Task ToggleStatusAsync(int eligibilityID)
    {
        await ExecuteMethod(async () => { await General.PostToggleAsync("Admin_ToggleEligibilityStatus", eligibilityID, "ADMIN", false, AdminGrid.Grid, runtime: JsRuntime); });
    }

    /// <summary>
    ///     The 'AdminEligibilityAdaptor' class is a custom data adaptor used in the 'Eligibility' admin page.
    ///     It inherits from the 'DataAdaptor' class and overrides the 'ReadAsync' method.
    /// </summary>
    /// <remarks>
    ///     This class is used to asynchronously fetch eligibility data for the admin grid.
    ///     It uses the 'DataManagerRequest' and an optional key to read the data.
    ///     The 'ReadAsync' method checks if a read operation is already in progress, if not, it initiates a new read
    ///     operation.
    ///     The read operation is performed by calling the 'General.GetReadAsync' method with specific parameters.
    /// </remarks>
    public class AdminEligibilityAdaptor : DataAdaptor
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
                object _returnValue = await General.GetReadAsync("Admin_GetEligibility", Filter, dm, false);
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