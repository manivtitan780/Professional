#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           TaxTerm.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-26-2023 16:17
// *****************************************/

#endregion

#region Using

using TaxTermDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.TaxTermDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents a page in the Admin section of the ProfSvc_AppTrack application for managing tax terms.
/// </summary>
/// <remarks>
///     This class provides functionality for retrieving and managing user login information,
///     navigation within the application, and filtering of tax terms. It also includes methods
///     that are executed after the page is rendered and when it is initialized.
/// </remarks>
public partial class TaxTerm
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private string _selectedID;

    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private byte _toggleValue;

    /// <summary>
    ///     Gets or sets the AdminGrid component for managing the list of administrators.
    /// </summary>
    /// <value>
    ///     The AdminGrid component of type <see cref="AdminList" />.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the list of administrators in the Admin section of the ProfSvc_AppTrack
    ///     application.
    ///     It provides functionality for retrieving and managing administrator data, as well as handling user interactions
    ///     with the grid.
    /// </remarks>
    private AdminGrid<AdminList> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of tax terms currently displayed in the grid.
    /// </summary>
    /// <value>
    ///     The count of tax terms.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the number of tax terms displayed in the grid.
    ///     It is updated every time the data in the grid is bound or refreshed.
    /// </remarks>
    private static int Count
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the TaxTermDialog used within the TaxTerm page.
    /// </summary>
    /// <value>
    ///     The TaxTermDialog instance.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the dialog for creating, editing, and saving Tax Terms within the TaxTerm page.
    ///     It provides functionality for handling events such as Cancel and Save, and properties for setting the HeaderString
    ///     and the Model.
    /// </remarks>
    private TaxTermDialog DialogTaxTerm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter value for tax terms in the Admin section of the ProfSvc_AppTrack application.
    /// </summary>
    /// <remarks>
    ///     This property is used to store the current filter value for tax terms. It is used in conjunction with the
    ///     'FilterSet' method in the 'General' class to process and format the filter value before it is used to filter data.
    /// </remarks>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance for invoking JavaScript functions from .NET code in the context of
    ///     this page.
    /// </summary>
    /// <value>
    ///     The JavaScript runtime instance.
    /// </value>
    /// <remarks>
    ///     This instance is used to interact with JavaScript functions from .NET code. It is injected into the page using the
    ///     [Inject] attribute.
    ///     For example, it is used in the SaveTaxTerm method to scroll to the saved row in the grid after saving the tax term
    ///     record.
    /// </remarks>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the local storage service for the Admin section of the ProfSvc_AppTrack application.
    /// </summary>
    /// <remarks>
    ///     This property is used to interact with the local storage of the browser. It is used to store and retrieve data
    ///     such as user preferences and settings. For example, it is used to get the 'autoTaxTerm' item from the local storage
    ///     in the 'OnAfterRenderAsync' method and to get the user login information in the 'OnInitializedAsync' method.
    /// </remarks>
    [Inject]
    private ILocalStorageService LocalStorageBlazored
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the TaxTerm class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the TaxTerm class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<TaxTerm> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the LoginCooky instance representing the current user's login information.
    /// </summary>
    /// <remarks>
    ///     This property is used to store and retrieve the current user's login information,
    ///     which includes details such as user ID, first name, last name, email address, role,
    ///     role ID, last login date, and login IP. It is initialized in the 'OnInitializedAsync'
    ///     method by calling the 'RedirectInner' method of the 'NavManager' object, which redirects
    ///     the user to the login page if they are not signed in, and to a role-appropriate page
    ///     if they are already signed in.
    /// </remarks>
    private LoginCooky LoginCookyUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the NavigationManager instance for the Admin section of the ProfSvc_AppTrack application.
    /// </summary>
    /// <remarks>
    ///     This property is used for managing navigation within the application. It is used in the 'OnInitializedAsync'
    ///     method to redirect the user to the login page if they are not signed in, and to a role-appropriate page
    ///     if they are already signed in. The redirection is done by calling the 'RedirectInner' method of the 'NavManager'
    ///     object.
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
    ///     It is used for role-based operations and access control within the application.
    /// </value>
    private string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of AdminList representing a record of tax terms in the Admin section of the
    ///     ProfSvc_AppTrack application.
    /// </summary>
    /// <value>
    ///     The instance of AdminList representing a record of tax terms.
    /// </value>
    private AdminList TaxTermRecord
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the clone of a tax term record for the Admin List.
    /// </summary>
    /// <value>
    ///     The clone of a tax term record.
    /// </value>
    /// <remarks>
    ///     This property is used to hold a copy of a tax term record for operations such as editing or adding a new tax term.
    ///     It is initialized as a new instance of the AdminList class and can be cleared or copied from another tax term
    ///     record.
    /// </remarks>
    private AdminList TaxTermRecordClone
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the title of the TaxTerm page.
    /// </summary>
    /// <value>
    ///     A string representing the title of the TaxTerm page. This value is used to set the header of the TaxTermDialog.
    /// </value>
    /// <remarks>
    ///     The title is set to "Edit" by default. It changes to "Add" when a new tax term is being added, and reverts back to
    ///     "Edit" when an existing tax term is being edited.
    /// </remarks>
    private static string Title
    {
        get;
        set;
    } = "Edit";

    /// <summary>
    ///     Handles the data processing for the Syncfusion grid control.
    /// </summary>
    /// <param name="obj">
    ///     The object that triggers the event. In this context, it is not used.
    /// </param>
    /// <remarks>
    ///     This method is called when the grid's data is bound. It updates the count of tax terms displayed in the grid
    ///     and selects the first row if the count is greater than zero.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
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
    ///     Initiates the process of editing a tax term.
    /// </summary>
    /// <param name="code">
    ///     The code of the tax term to be edited. If the code is not provided or is whitespace,
    ///     the method will initiate the process to add a new tax term.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     The method first checks if there are any selected records in the grid and if the first selected record's code
    ///     does not match the provided code, it selects the row with the provided code.
    ///     If the provided code is null or whitespace, it sets up the state for adding a new tax term.
    ///     Otherwise, it sets up the state for editing the existing tax term with the provided code.
    ///     Finally, it triggers a state change and shows the dialog for editing or adding a tax term.
    /// </remarks>
    private async Task EditTaxTerm(string code = "")
    {
        await ExecuteMethod(async () =>
                            {
                                List<AdminList> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                if (_selectedList.Any() && _selectedList.First().Code != code)
                                {
                                    int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(code);
                                    await AdminGrid.Grid.SelectRowAsync(_index);
                                }

                                if (code.NullOrWhiteSpace())
                                {
                                    Title = "Add";
                                    if (TaxTermRecordClone == null)
                                    {
                                        TaxTermRecordClone = new();
                                    }
                                    else
                                    {
                                        TaxTermRecordClone.Clear();
                                    }

                                    TaxTermRecordClone.IsAdd = true;
                                }
                                else
                                {
                                    Title = "Edit";
                                    TaxTermRecordClone = TaxTermRecord.Copy();

                                    TaxTermRecordClone.IsAdd = false;
                                }

                                TaxTermRecordClone.Entity = "Tax Term";

                                StateHasChanged();
                            });
        await DialogTaxTerm.ShowDialog();
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
    ///     Filters the grid of tax terms based on the provided tax term.
    /// </summary>
    /// <param name="taxTerm">
    ///     The tax term to filter the grid by. This is an instance of the <see cref="ProfSvc_Classes.KeyValues" /> class.
    /// </param>
    /// <remarks>
    ///     This method sets the filter based on the value of the provided tax term, refreshes the grid to apply the filter,
    ///     and prevents multiple simultaneous filter operations by using a toggling mechanism.
    /// </remarks>
    /// <returns>
    ///     A <see cref="System.Threading.Tasks.Task" /> that represents the asynchronous operation.
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
    ///     Sets the filter value for tax terms in the Admin section of the ProfSvc_AppTrack application.
    /// </summary>
    /// <param name="value">The new value to be set as the filter.</param>
    /// <remarks>
    ///     This method sets the filter value by calling the 'FilterSet' method in the 'General' class,
    ///     which processes and formats the filter value before it is used to filter data.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Asynchronously initializes the TaxTerm page.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is called when the TaxTerm page is initialized. It retrieves the user's login information from the
    ///     local storage,
    ///     checks if the user has an Administrator role, and redirects the user to the home page if they do not have the
    ///     necessary rights.
    ///     The user's RoleID is also set during this process.
    /// </remarks>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorageBlazored);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorageBlazored.GetItemAsStringAsync("autoTaxTerm");
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
    ///     Asynchronously refreshes the grid that displays the list of tax terms in the Admin section.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is called when the grid needs to be refreshed, for example, after a tax term has been added, edited, or
    ///     deleted.
    ///     It uses the Syncfusion grid control's Refresh method to update the grid's content.
    /// </remarks>
    private Task RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event that occurs when a row is selected in the AdminList grid.
    /// </summary>
    /// <param name="designation">
    ///     Contains the data of the selected row in the AdminList grid.
    /// </param>
    /// <remarks>
    ///     This method sets the TaxTermRecord property to the data of the selected row.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<AdminList> designation) => TaxTermRecord = designation.Data;

    /// <summary>
    ///     Saves the changes made to a tax term in the Admin section of the ProfSvc_AppTrack application.
    /// </summary>
    /// <remarks>
    ///     This method is called when the user clicks the Save button in the TaxTermDialog. It uses the
    ///     General.SaveAdminListAsync method
    ///     to save the changes to the database. The method is asynchronous and returns no value.
    /// </remarks>
    private async void SaveTaxTerm()
    {
        await ExecuteMethod(() => General.SaveAdminListAsync("Admin_SaveTaxTerm", "TaxTerm", true, true, TaxTermRecordClone,
                                                             AdminGrid.Grid, TaxTermRecord, JsRuntime));
    }

    /// <summary>
    ///     Toggles the status of a tax term in the Admin section of the ProfSvc_AppTrack application.
    /// </summary>
    /// <param name="code">The code of the tax term to be toggled.</param>
    /// <param name="enabled">
    ///     A boolean value indicating the new status of the tax term. If true, the tax term is enabled;
    ///     otherwise, it is disabled.
    /// </param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method sets the selected tax term code and its new status. If the selected tax term is not the same as the one
    ///     currently selected in the grid,
    ///     the method selects the correct row in the grid. After these operations, the method triggers a confirmation dialog.
    /// </remarks>
    private Task ToggleMethod(string code, bool enabled)
    {
        return ExecuteMethod(async () =>
                             {
                                 _selectedID = code;
                                 _toggleValue = enabled ? (byte)2 : (byte)1;
                                 List<AdminList> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().Code != code)
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(code);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 await AdminGrid.DialogConfirm.ShowDialog();
                             });
    }

    /// <summary>
    ///     Toggles the status of a tax term.
    /// </summary>
    /// <param name="taxTermCode">The code of the tax term to toggle.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method posts a toggle request to the 'Admin_ToggleTaxTermStatus' endpoint with the provided tax term code.
    ///     The method ensures that only one toggle operation can be in progress at a time by using the '_toggling' flag.
    /// </remarks>
    private Task ToggleStatusTaxTerm(string taxTermCode)
    {
        return ExecuteMethod(() => General.PostToggleAsync("Admin_ToggleTaxTermStatus", taxTermCode, "ADMIN", true, AdminGrid.Grid));
    }

    /// <summary>
    ///     The AdminTaxTermAdaptor class is a custom data adaptor for the TaxTerm page in the Admin section of the
    ///     ProfSvc_AppTrack application.
    /// </summary>
    /// <remarks>
    ///     This class inherits from the DataAdaptor base class and overrides the ReadAsync method to fetch tax terms data
    ///     asynchronously.
    ///     The ReadAsync method is called when the data for the TaxTerm page needs to be fetched. It checks if a read
    ///     operation is already in progress,
    ///     and if not, it initiates a new read operation by calling the General.GetReadAsync method with the
    ///     "Admin_GetTaxTerms" parameter, along with the current filter and data manager request.
    ///     The result of this operation is returned as the data for the TaxTerm page.
    /// </remarks>
    public class AdminTaxTermAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads the tax terms data for the TaxTerm page in the Admin section of the ProfSvc_AppTrack
        ///     application.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object that contains the parameters for the data request.</param>
        /// <param name="key">An optional key to identify a specific data item. Default is null.</param>
        /// <returns>
        ///     A Task that represents the asynchronous operation. The Task result contains the fetched data as an object.
        ///     If a read operation is already in progress, the method returns null.
        ///     Otherwise, it initiates a new read operation by calling the General.GetReadAsync method with the
        ///     "Admin_GetTaxTerms" parameter, along with the current filter and data manager request.
        ///     The result of this operation is returned as the data for the TaxTerm page.
        /// </returns>
        /// <remarks>
        ///     This method checks if a read operation is already in progress, and if not, it initiates a new read operation.
        ///     The 'dm' parameter is used to pass the parameters for the data request.
        ///     The 'key' parameter can be used to identify a specific data item, but it is optional and its default value is null.
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
                object _returnValue = await General.GetReadAsync("Admin_GetTaxTerms", Filter, dm);
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