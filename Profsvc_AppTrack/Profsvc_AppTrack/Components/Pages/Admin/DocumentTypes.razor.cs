#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           DocumentTypes.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-12-2023 15:48
// *****************************************/

#endregion

#region Using

using DocumentTypeDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.DocumentTypeDialog;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin;

/// <summary>
///     Represents a page in the application that allows administrators to manage document types.
/// </summary>
/// <remarks>
///     This page includes functionality for filtering the list of document types,
///     and it ensures that only users with administrative rights can access the page.
/// </remarks>
public partial class DocumentTypes
{
    private static TaskCompletionSource<bool> _initializationTaskSource;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     Gets or sets the AdminGrid component used for managing DocumentType entities.
    /// </summary>
    /// <value>
    ///     The AdminGrid component.
    /// </value>
    /// <remarks>
    ///     The AdminGrid component provides functionalities for filtering, selecting, and refreshing the list of DocumentType
    ///     entities.
    ///     It is used in various methods within the DocumentTypes class such as DataHandler, EditDocumentType, FilterGrid,
    ///     RefreshGrid, and SaveDocumentType.
    /// </remarks>
    private AdminGrid<DocumentType> AdminGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the count of document type currently displayed in the grid view on the DocumentTypes page.
    ///     This property is used in the 'DataHandler' method to store the count of document type and
    ///     to select the first document types if the count is more than zero.
    /// </summary>
    private int Count
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DocTypeRecord property of the DocumentTypes class.
    ///     The DocTypeRecord property represents a single document type in the application.
    ///     It is used to hold the data of the selected document type in the document type grid.
    ///     The data is encapsulated in a DocumentType object, which is defined in the ProfSvc_Classes namespace.
    /// </summary>
    private DocumentType DocTypeRecord
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the clone of a DocumentTypes record. This property is used to hold a copy of a DocumentTypes record
    ///     for
    ///     operations like editing or adding a document type.
    ///     When adding a new document type, a new instance of DocumentType is created and assigned to this property.
    ///     When editing an existing document type, a copy of the DocumentType record to be edited is created and assigned to
    ///     this property.
    /// </summary>
    private DocumentType DocTypeRecordClone
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the 'DocumentTypeDialog' instance used for managing document type information in the administrative
    ///     context.
    ///     This dialog is used for both creating new document type and editing existing document type.
    /// </summary>
    private DocumentTypeDialog DocumentTypeDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the filter value for the application document types in the administrative context.
    ///     This static property is used to filter the document types based on certain criteria in the administrative context.
    /// </summary>
    private static string Filter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ID property of the DocumentTypes class.
    ///     The ID property represents the unique identifier of a document type in the application.
    ///     It is used to track the document type being edited or added in the document type grid.
    ///     The default value of ID is -1, indicating that no document type is currently being processed.
    /// </summary>
    private int ID
    {
        get;
        set;
    } = -1;

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance. The JavaScript runtime provides a mechanism for running JavaScript in
    ///     the context of the component.
    ///     This property is injected into the component and is used to call JavaScript functions from .NET code.
    ///     For example, it is used in the 'Save' method to scroll to a specific row in the grid, and in the
    ///     'ToggleStatusAsync' method to toggle the status of a document type.
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
    ///     It is used in this class to retrieve and store document type-specific data, such as the "autoDocument" item and the
    ///     `LoginCookyUser` object.
    /// </summary>
    [Inject]
    private ILocalStorageService LocalStorage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ILogger instance used for logging in the DocumentTypes class.
    /// </summary>
    /// <remarks>
    ///     This property is used to log information about the execution of tasks and methods within the DocumentTypes class.
    ///     It is injected at runtime by the dependency injection system.
    /// </remarks>
    [Inject]
    private ILogger<DocumentTypes> Logger
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the `LoginCooky` object for the current document type.
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
    ///     Gets or sets the document type of the DocumentTypes Dialog in the administrative context.
    ///     The document type changes based on the action being performed on the document type record - "Add" when a new
    ///     document type is being added,
    ///     and "Edit" when an existing document type's details are being modified.
    /// </summary>
    private string Title
    {
        get;
        set;
    }

    /// <summary>
    ///     Handles the data for the DocumentTypes page.
    /// </summary>
    /// <remarks>
    ///     This method is responsible for counting the current view data of the grid and selecting the first row if the count
    ///     is more than zero.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private Task DataHandler()
    {
        return ExecuteMethod(() =>
                             {
                                 Count = AdminGrid.Grid.CurrentViewData.Count();
                                 return Count > 0 ? AdminGrid.Grid.SelectRowAsync(0) : Task.CompletedTask;
                             });
    }

    /// <summary>
    ///     Initiates the process of editing a DocumentType.
    /// </summary>
    /// <param name="id">The ID of the DocumentType to be edited. If the ID is 0, a new DocumentType is created.</param>
    /// <remarks>
    ///     This method retrieves the selected DocumentType from the grid. If the selected DocumentType's ID does not match the
    ///     provided ID,
    ///     it selects the row with the matching ID in the grid. If the ID is 0, it prepares for adding a new DocumentType.
    ///     Otherwise, it prepares for editing the existing DocumentType. Finally, it opens the DocumentTypeDialog for user
    ///     interaction.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private Task EditDocumentType(int id = 0)
    {
        return ExecuteMethod(async () =>
                             {
                                 List<DocumentType> _selectedList = await AdminGrid.Grid.GetSelectedRecordsAsync();
                                 if (_selectedList.Any() && _selectedList.First().ID != id)
                                 {
                                     int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(id);
                                     await AdminGrid.Grid.SelectRowAsync(_index);
                                 }

                                 if (id == 0)
                                 {
                                     Title = "Add";
                                     if (DocTypeRecordClone == null)
                                     {
                                         DocTypeRecordClone = new();
                                     }
                                     else
                                     {
                                         DocTypeRecordClone.Clear();
                                     }
                                 }
                                 else
                                 {
                                     Title = "Edit";
                                     DocTypeRecordClone = DocTypeRecord.Copy();
                                 }

                                 StateHasChanged();
                                 await DocumentTypeDialog.ShowDialog();
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
    private Task ExecuteMethod(Func<Task> task)
    {
        return General.ExecuteMethod(_semaphore, task, Logger);
    }

    /// <summary>
    ///     Applies the provided filter to the grid.
    /// </summary>
    /// <param name="source">The source of the change event, containing the new filter value.</param>
    /// <remarks>
    ///     This method sets the filter according to the provided value and refreshes the grid to reflect the changes.
    /// </remarks>
    private Task FilterGrid(ChangeEventArgs<string, KeyValues> source)
    {
        return ExecuteMethod(() =>
                             {
                                 FilterSet(source.Value);
                                 return AdminGrid.Grid.Refresh();
                             });
    }

    /// <summary>
    ///     Sets the filter value for the DocumentTypes grid.
    /// </summary>
    /// <param name="value">The value to be used as the filter.</param>
    /// <remarks>
    ///     This method is used to update the filter value for the DocumentTypes grid. The filter value is set to the passed
    ///     value,
    ///     which is then used to filter the grid items. The method uses the General.FilterSet method to ensure the filter
    ///     value is
    ///     properly formatted before being applied.
    /// </remarks>
    private static void FilterSet(string value) => Filter = General.FilterSet(Filter, value);

    /// <summary>
    ///     Initializes the component asynchronously.
    /// </summary>
    /// <remarks>
    ///     This method is invoked when the component is first initialized. It retrieves the user's login information from
    ///     local storage
    ///     and checks if the user has administrative rights. If the user is not an administrator, they are redirected to the
    ///     home page.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _initializationTaskSource = new();
        await ExecuteMethod(async () =>
                            {
                                LoginCookyUser = await NavManager.RedirectInner(LocalStorage);
                                RoleID = LoginCookyUser.RoleID;
                                string _result = await LocalStorage.GetItemAsStringAsync("autoDocumentType");
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
    ///     Refreshes the DocumentTypes grid.
    /// </summary>
    /// <remarks>
    ///     This method is used to refresh the data in the DocumentTypes grid. It is an asynchronous operation that calls the
    ///     Refresh method of the SfGrid component.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private Task RefreshGrid() => AdminGrid.Grid.Refresh();

    /// <summary>
    ///     Handles the event when a row is selected in the DocumentTypes grid.
    /// </summary>
    /// <param name="docType">The selected DocumentType object from the grid.</param>
    /// <remarks>
    ///     This method is triggered when a user selects a row in the DocumentTypes grid.
    ///     The selected DocumentType object is then assigned to the DocTypeRecord property for further processing.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<DocumentType> docType) => DocTypeRecord = docType.Data;

    /// <summary>
    ///     Asynchronously saves the document type.
    /// </summary>
    /// <remarks>
    ///     This method saves the document type by calling the General.SaveDocumentTypeAsync method with the DocTypeRecordClone
    ///     and DocTypeRecord as parameters.
    ///     After the document type is saved, the method updates the ID with the returned value and refreshes the grid.
    ///     It then gets the index of the saved document type in the grid by its primary key and selects the corresponding row.
    ///     Finally, it scrolls to the selected row in the grid.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private Task SaveDocumentType()
    {
        return ExecuteMethod(async () =>
                             {
                                 int _returnValue = await General.PostRest<int>("Admin/SaveDocType", null, DocTypeRecordClone);

                                 if (DocTypeRecord != null)
                                 {
                                     DocTypeRecord = DocTypeRecordClone.Copy();
                                 }

                                 ID = _returnValue;
                                 await AdminGrid.Grid.Refresh();

                                 int _index = await AdminGrid.Grid.GetRowIndexByPrimaryKeyAsync(ID);
                                 await AdminGrid.Grid.SelectRowAsync(_index);
                                 await JsRuntime.InvokeVoidAsync("scroll", _index);
                             });
    }

    /// <summary>
    ///     The AdminDocumentTypeAdaptor class is a custom data adaptor used in the DocumentTypes page.
    ///     It inherits from the DataAdaptor class and overrides the ReadAsync method.
    /// </summary>
    /// <remarks>
    ///     This class is used to fetch document types asynchronously. It uses the General.GetDocTypesAsync method
    ///     to retrieve the document types based on the provided filter and DataManagerRequest.
    ///     The _reading field is used to prevent multiple simultaneous reads.
    /// </remarks>
    public class AdminDocumentTypeAdaptor : DataAdaptor
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        /// <summary>
        ///     Asynchronously reads data for the DocumentTypes page.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object that contains the parameters for the read operation.</param>
        /// <param name="key">An optional key to identify a specific data item. Default is null.</param>
        /// <returns>
        ///     A Task that represents the asynchronous read operation. The value of the TResult parameter contains the data
        ///     read from the DocumentTypes page.
        /// </returns>
        /// <remarks>
        ///     This method uses the General.GetDocTypesAsync method to fetch document types based on the provided filter and
        ///     DataManagerRequest.
        ///     If the method is already reading data (_reading is true), it returns null to prevent multiple simultaneous reads.
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
                object _returnValue = await General.GetDocTypesAsync(Filter, dm);
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