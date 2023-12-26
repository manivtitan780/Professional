#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           LeadSource.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-26-2023 16:2
// *****************************************/

#endregion

#region Using

using AdminListDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.AdminListDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents the LeadSource page in the Admin section of the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     This class is responsible for handling the user interactions and rendering the LeadSource page.
///     It includes properties for managing the state of the page, such as the Filter, LocalStorageBlazored,
///     LoginCookyUser, NavManager, and RoleID.
///     It also includes methods for handling the page lifecycle events such as OnAfterRenderAsync and OnInitializedAsync.
/// </remarks>
public partial class LeadSource
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private int _selectedID;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private byte _toggleValue;

    /// <summary>
    ///     Gets or sets the AdminListDialog instance used for managing administrative tasks in the Lead Source
    ///     administration
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
    ///     Gets or sets the AdminGrid component in the LeadSource page.
    /// </summary>
    /// <value>
    ///     The AdminGrid component of type <see cref="AdminGrid{AdminList}" />.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the grid of administrators in the LeadSource page.
    ///     It provides functionalities such as filtering, sorting, and editing of the administrators' data.
    /// </remarks>
    private AdminGrid<AdminList> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of current view data in the grid on the Lead Source administration page.
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
    ///     Gets or sets the filter value for the Lead Source administration page.
    /// </summary>
    /// <value>
    ///     The filter value used to filter the list of lead source records displayed on the page.
    /// </value>
    /// <remarks>
    ///     This property is used to store the current filter value for the list of lead source records.
    ///     The filter value is used in the ReadAsync method of the AdminEducationAdaptor class to filter the data retrieved
    ///     for the Lead Source administration page.
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
    ///     This property is injected into the Lead Source class and is used to interact with JavaScript from C# code.
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
    ///     Gets or sets the local storage service used by the Lead Source administration page.
    /// </summary>
    /// <remarks>
    ///     This property is injected into the class and is used to interact with the browser's local storage. It is used to
    ///     retrieve user login information and check the user's role to ensure that only administrators have access to this
    ///     page.
    ///     It is also used to remember the filter settings for the list of lead source records.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the LeadSource class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the LeadSource class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<LeadSource> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the login information for the current user.
    /// </summary>
    /// <remarks>
    ///     This property is used to store the login information of the current user, which is retrieved from local storage.
    ///     It is used to check the user's role and ensure that only administrators have access to the Lead Source
    ///     administration
    ///     page.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the NavigationManager service used by the Lead Source administration page.
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
    ///     It is used to determine the user's access level to the Education administration page.
    /// </remarks>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Represents a single lead source record in the Lead Source administration page of the Professional Services
    ///     Application Tracking system.
    /// </summary>
    /// <remarks>
    ///     This property is of type <see cref="AdminList" /> and is used to hold the data of the selected row in the lead
    ///     source
    ///     admin list.
    ///     It is also used to create a clone of the lead source record when editing.
    /// </remarks>
    private AdminList SourceRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets a clone of a lead source record in the administration list.
    /// </summary>
    /// <remarks>
    ///     This property is used to hold a copy of a lead source record from the administration list.
    ///     It is utilized during the editing process, where a copy of the record is made and manipulated
    ///     before being saved back to the list. This ensures that the original record remains unaltered
    ///     in case the editing process is cancelled or an error occurs.
    /// </remarks>
    private AdminList SourceRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the title for the Lead Source administration page.
    /// </summary>
    /// <value>
    ///     The title is a string that represents the current action (either "Add" or "Edit") being performed on the lead
    ///     source records.
    /// </value>
    /// <remarks>
    ///     The title is used in the header of the `AdminListDialog` component in the Lead Source administration page.
    ///     It changes based on whether a new lead source record is being added or an existing one is being edited.
    /// </remarks>
    private string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data for the Lead Source page. It counts the number of records in the current view of the grid.
    ///     If there are records, it selects the first row.
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
    ///     Asynchronously edits the lead source with the given ID. If the ID is 0, a new lead source is created.
    /// </summary>
    /// <param name="id">The ID of the lead source to edit. If this parameter is 0, a new lead source is created.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Retrieves the selected records from the grid.
    ///     - If the first selected record's ID does not match the given ID, it selects the row with the given ID in the grid.
    ///     - If the ID is 0, it sets the title to "Add" and initializes a new lead source record clone if it does not exist,
    ///     or clears its data if it does.
    ///     - If the ID is not 0, it sets the title to "Edit" and copies the current lead source record to the clone.
    ///     - Sets the entity of the lead source record clone to "Title".
    ///     - Triggers a state change.
    ///     - Shows the admin dialog.
    /// </remarks>
    private Task EditSource(int id = 0)
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
                                     if (SourceRecordClone == null)
                                     {
                                         SourceRecordClone = new();
                                     }
                                     else
                                     {
                                         SourceRecordClone.Clear();
                                     }
                                 }
                                 else
                                 {
                                     Title = "Edit";
                                     SourceRecordClone = SourceRecord.Copy();
                                 }

                                 SourceRecordClone.Entity = "Lead Source";

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
    ///     Handles the filtering of the grid based on the provided lead source.
    ///     This method is triggered when a lead source is selected in the grid.
    ///     It sets the filter value to the selected lead source and refreshes the grid to update the displayed data.
    ///     The method ensures that the grid is not refreshed multiple times simultaneously by using a toggling flag.
    /// </summary>
    /// <param name="source">The selected lead source in the grid, encapsulated in a ChangeEventArgs object.</param>
    /// <returns>A Task representing the asynchronous operation of refreshing the grid.</returns>
    private Task FilterGrid(ChangeEventArgs<string, KeyValues> source)
    {
        return ExecuteMethod(() =>
                             {
                                 FilterSet(source.Value);
                                 return AdminGrid.Grid.Refresh();
                             });
    }

    /// <summary>
    ///     Sets the filter value for the Lead Source component.
    ///     This method is used to update the static Filter property with the passed value.
    ///     The passed value is processed by the General.FilterSet method before being assigned to the Filter property.
    /// </summary>
    /// <param name="value">The value to be set as the filter.</param>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     This method is called when the component is initialized.
    ///     It retrieves the user's login information from the local storage and checks the user's role.
    ///     If the user's role is not "AD" (Administrator), it redirects the user to the home page.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorageBlazored.GetItemAsStringAsync("autoSource");
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
    ///     Refreshes the grid component of the Lead Source page.
    ///     This method is used to update the grid component and reflect any changes made to the data.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private Task RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event of a row being selected in the Lead Source grid.
    /// </summary>
    /// <param name="source">The selected row data encapsulated in a RowSelectEventArgs object.</param>
    private void RowSelected(RowSelectEventArgs<AdminList> source) => SourceRecord = source.Data;

    /// <summary>
    ///     Saves the changes made to the lead source record.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method calls the General.SaveAdminListAsync method, passing in the necessary parameters to save the changes
    ///     made to the DesignationRecordClone.
    ///     After the save operation, it refreshes the grid and selects the updated row.
    /// </remarks>
    private Task SaveSource()
    {
        return ExecuteMethod(() => General.SaveAdminListAsync("Admin_SaveSource", "Source", false, false, SourceRecordClone, AdminGrid.Grid,
                                                              SourceRecord, JsRuntime));
    }

    /// <summary>
    ///     Toggles the status of an AdminList item and shows a confirmation dialog.
    /// </summary>
    /// <param name="id">The ID of the AdminList item to toggle.</param>
    /// <param name="enabled">
    ///     The new status to set for the AdminList item. If true, the status is set to 2, otherwise it is
    ///     set to 1.
    /// </param>
    /// <returns>A Task representing the asynchronous operation.</returns>
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
    ///     Toggles the status of a lead source asynchronously.
    /// </summary>
    /// <param name="sourceID">The ID of the lead source whose status is to be toggled.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method posts a toggle request to the "Admin_ToggleLeadSourceStatus" endpoint with the provided lead source
    ///     ID.
    ///     The status toggle operation is not performed if it is already in progress.
    ///     After the status is toggled, the method refreshes the grid and selects the row with the toggled lead source.
    /// </remarks>
    private Task ToggleStatusAsync(int sourceID)
    {
        return ExecuteMethod(() => General.PostToggleAsync("Admin_ToggleSourceStatus", sourceID, "ADMIN", false, AdminGrid.Grid, runtime: JsRuntime));
    }

    /// <summary>
    ///     The AdminDesignationAdaptor class is a data adaptor for the Admin Lead Source page.
    ///     It inherits from the DataAdaptor class and overrides the ReadAsync method.
    /// </summary>
    /// <remarks>
    ///     This class is used to handle data operations for the Admin Lead Source page.
    ///     It communicates with the server to fetch data based on the DataManagerRequest and a key.
    ///     The ReadAsync method is used to asynchronously fetch data from the server.
    ///     It uses the General.GetReadAsync method to perform the actual data fetching.
    /// </remarks>
    public class AdminSourceAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously fetches data for the Admin Lead Source page from the server.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object that contains the parameters for the data request.</param>
        /// <param name="key">An optional key used to fetch specific data. Default is null.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task result contains the fetched data as an object.</returns>
        /// <remarks>
        ///     This method uses the General.GetReadAsync method to fetch data from the server.
        ///     It sets a flag to prevent multiple simultaneous reads.
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
                object _returnValue = await General.GetReadAsync("Admin_GetSources", Filter, dm, false);
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