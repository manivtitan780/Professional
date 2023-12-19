﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           RequisitionDetailsPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-02-2023 21:16
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Controls.Requisitions;

/// <summary>
///     Represents a panel for displaying and managing requisition details in the application.
/// </summary>
/// <remarks>
///     This class is a part of the ProfSvc_AppTrack.Pages.Controls.Requisitions namespace and is used to manage various
///     aspects of a requisition,
///     including associated companies, contacts, education details, eligibility criteria, experience requirements, job
///     options, recruiters, skills, and states.
///     It also provides event callbacks for handling actions such as Cancel, Save, File Upload, and StateID changes.
/// </remarks>
public partial class RequisitionDetailsPanel
{
    private bool _actionProgress = false;

    private EditContext _editContext;

    /// <summary>
    ///     Gets or sets the filtered list of company contacts associated with the requisition.
    /// </summary>
    /// <value>
    ///     The filtered list of company contacts.
    /// </value>
    /// <remarks>
    ///     This field is used to store the filtered list of company contacts based on certain criteria.
    ///     It is used in the RequisitionDetailsPanel for displaying only the relevant contacts to the user.
    /// </remarks>
    private List<CompanyContact> _filteredCompanyContacts = new();

    /// <summary>
    ///     Represents a collection of toolbar items used in the RequisitionDetailsPanel.
    /// </summary>
    /// <remarks>
    ///     Each toolbar item in this collection corresponds to a specific command, such as Bold, Italic, Underline, etc.
    ///     These commands are used to manipulate the text in the RequisitionDetailsPanel.
    /// </remarks>
    private readonly List<ToolbarItemModel> _tools1 = new()
                                                      {
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.Bold
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.Italic
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.Underline
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.StrikeThrough
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.LowerCase
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.UpperCase
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.SuperScript
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.SubScript
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.Separator
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.ClearFormat
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.Separator
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.Undo
                                                          },
                                                          new()
                                                          {
                                                              Command = ToolbarCommand.Redo
                                                          }
                                                      };

    private bool _zipChanging;

    /// <summary>
    ///     Gets or sets the AutoCompleteButton control used in the RequisitionDetailsPanel.
    /// </summary>
    /// <value>
    ///     The AutoCompleteButton control.
    /// </value>
    /// <remarks>
    ///     This control provides a user interface for input with autocomplete suggestions.
    ///     It includes various parameters to customize the behavior and appearance of the autocomplete feature.
    ///     The parameters include options to control the event callbacks, tooltip creation, state persistence,
    ///     input length restrictions, placeholder text, and positioning of the autocomplete component.
    /// </remarks>
    private AutoCompleteButton AutoCompleteControl
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the Cancel action is performed.
    /// </summary>
    /// <value>
    ///     The event callback containing the MouseEventArgs associated with the Cancel action.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the Cancel action in the RequisitionDetailsPanel.
    ///     The MouseEventArgs provides detailed information and functionality for the Cancel action.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of companies associated with the requisition.
    /// </summary>
    /// <value>
    ///     The list of companies.
    /// </value>
    [Parameter]
    public List<Company> Companies
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of company contacts associated with the requisition.
    /// </summary>
    /// <value>
    ///     The list of company contacts.
    /// </value>
    [Parameter]
    public List<CompanyContact> CompanyContacts
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the query for contact details in the requisition.
    /// </summary>
    /// <value>
    ///     The query for contact details.
    /// </value>
    private Query ContactQuery
    {
        get;
        set;
    } = null;

    /// <summary>
    ///     Gets or sets the CSS class name for the description field in the requisition details panel.
    /// </summary>
    /// <value>
    ///     The CSS class name. The default value is "success". It changes to "error" if there are any validation errors in the
    ///     description field.
    /// </value>
    /// <remarks>
    ///     This property is used to change the visual representation of the description field based on the validation state.
    /// </remarks>
    private string CssClassName
    {
        get;
        set;
    } = "success";

    /// <summary>
    ///     Gets or sets the Dialog property, which represents a dialog in the RequisitionDetailsPanel.
    /// </summary>
    /// <value>
    ///     The Dialog property gets/sets the value of a SfDialog object.
    /// </value>
    /// <remarks>
    ///     This property is used for managing the dialog display in the RequisitionDetailsPanel.
    ///     The SfDialog object provides detailed control over the dialog's behavior and appearance.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets a list of duration codes. Each code is represented as a <see cref="ProfSvc_Classes.KeyValues" /> object,
    ///     where the key is the code and the value is the description of the duration (e.g., "M" for months, "Y" for years).
    /// </summary>
    private List<KeyValues> DurationCode
    {
        get;
    } = new() {new("M", "Months"), new("Y", "Years")};

    /// <summary>
    ///     Gets or sets the EditForm for the RequisitionDetailsPanel.
    ///     This form is used to edit the details of a requisition.
    /// </summary>
    private EditForm EditRequisitionForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the education details for the requisition.
    /// </summary>
    /// <value>
    ///     The education details represented as a list of <see cref="IntValues" />.
    /// </value>
    [Parameter]
    public List<IntValues> Education
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the eligibility criteria as a list of integer values.
    /// </summary>
    /// <value>
    ///     The eligibility criteria.
    /// </value>
    [Parameter]
    public List<IntValues> Eligibility
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the experience requirements for the requisition.
    /// </summary>
    /// <value>
    ///     The experience requirements are represented as a list of <see cref="IntValues" />.
    /// </value>
    [Parameter]
    public List<IntValues> Experience
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the FooterDialog property, which represents a dialog footer in the admin controls of the application.
    /// </summary>
    /// <value>
    ///     The FooterDialog property gets/sets the value of a DialogFooter object.
    /// </value>
    /// <remarks>
    ///     This property is used for managing the Cancel and Save buttons in the dialog.
    /// </remarks>
    public DialogFooter FooterDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the job options for the requisition details panel.
    /// </summary>
    /// <value>
    ///     The job options are represented as a list of <see cref="ProfSvc_Classes.KeyValues" /> instances.
    /// </value>
    [Parameter]
    public List<KeyValues> JobOptions
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance.
    /// </summary>
    /// <value>
    ///     The JavaScript runtime instance used for invoking JavaScript functions from .NET code.
    /// </value>
    /// <remarks>
    ///     This property is injected and used to perform operations that require JavaScript interop.
    /// </remarks>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model for the Requisition Details Panel.
    /// </summary>
    /// <value>
    ///     The model representing the details of a requisition.
    /// </value>
    [Parameter]
    public RequisitionDetails Model
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the event callback that is triggered when a file is uploaded.
    /// </summary>
    /// <value>
    ///     The event callback containing the UploadChangeEventArgs associated with the file upload action.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the file upload action in the RequisitionDetailsPanel.
    ///     The UploadChangeEventArgs provides detailed information and functionality for the file upload action.
    /// </remarks>
    [Parameter]
    public EventCallback<UploadChangeEventArgs> OnFileUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets the list of priority levels for the requisition.
    /// </summary>
    /// <value>
    ///     The list of priority levels, represented as <see cref="IntValues" />, where the integer key represents the priority
    ///     level and the string value represents the priority description.
    /// </value>
    private List<IntValues> Priority
    {
        get;
    } = new() {new(0, "Low"), new(1, "Medium"), new(2, "High")};

    /// <summary>
    ///     Gets or sets the list of recruiters associated with the requisition.
    /// </summary>
    /// <value>
    ///     The list of recruiters, represented as instances of the KeyValues class.
    /// </value>
    [Parameter]
    public List<KeyValues> Recruiters
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the Save action is performed.
    /// </summary>
    /// <value>
    ///     The event callback containing the EditContext associated with the Save action.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the Save action in the RequisitionDetailsPanel.
    ///     The EditContext provides detailed information and functionality for the Save action.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of skills associated with the requisition.
    /// </summary>
    /// <value>
    ///     The list of skills, each represented as an instance of the <see cref="IntValues" /> class.
    /// </value>
    [Parameter]
    public List<IntValues> Skills
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfSpinner control of the RequisitionDetailsPanel.
    /// </summary>
    /// <value>
    ///     The SfSpinner control.
    /// </value>
    /// <remarks>
    ///     This property is used to display a spinner animation during asynchronous operations.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the StateID changes.
    /// </summary>
    /// <value>
    ///     The event callback containing the change event arguments, which includes the old and new StateID values.
    /// </value>
    [Parameter]
    public EventCallback<ChangeEventArgs<int, IntValues>> StateIDChanged
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of states for the RequisitionDetailsPanel.
    /// </summary>
    /// <value>
    ///     The list of states, represented as instances of the IntValues class.
    /// </value>
    [Parameter]
    public List<IntValues> States
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title of the RequisitionDetailsPanel.
    /// </summary>
    /// <value>
    ///     The title of the RequisitionDetailsPanel.
    /// </value>
    /// <remarks>
    ///     This property is used to set the header of the RequisitionDetailsPanel dialog.
    /// </remarks>
    [Parameter]
    public string Title
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of toolbar items for the RequisitionDetailsPanel.
    /// </summary>
    /// <value>
    ///     The list of toolbar items.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the toolbar items in the RequisitionDetailsPanel. Each item in the list represents
    ///     a specific tool in the toolbar.
    /// </remarks>
    [Parameter]
    public List<ToolbarItemModel> Tools
    {
        get;
        set;
    }

    /// <summary>
    ///     Handles the cancellation of the dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancellation event.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private Task CancelDialog(MouseEventArgs args)
    {
        return General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);
    }

    /// <summary>
    ///     Gets or sets the Redis service used for caching data.
    /// </summary>
    /// <value>
    ///     The Redis service.
    /// </value>
    /// <remarks>
    ///     This property is injected and used for operations like retrieving or storing data in Redis cache.
    /// </remarks>
    [Inject]
    private RedisService Redis
    {
        get;
        set;
    }

    /// <summary>
    ///     Handles the event when the selected company changes in the Requisition Details Panel.
    /// </summary>
    /// <param name="company">
    ///     The ChangeEventArgs object containing the new selected company's ID and the company object
    ///     itself.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method updates the ContactQuery based on the new company's ID. If the company data is not null, it also
    ///     updates the Model's company-related properties.
    ///     It uses the memory cache to retrieve the company list and find the new company's details.
    /// </remarks>
    private async Task CompanyChanged(ChangeEventArgs<int, Company> company)
    {
        await Task.Yield();
        if (!company.Value.NullOrWhiteSpace())
        {
            ContactQuery = new Query().Where(new WhereFilter {Field = "ClientID", Operator = "equal", value = company.Value});
            if (company.ItemData != null)
            {
                Model.CompanyName = company.ItemData.CompanyName;
                //IMemoryCache _memoryCache = Start.MemCache;
                List<Company> _companyList = await Redis.GetOrCreateAsync<List<Company>>("Companies");
                //_memoryCache.TryGetValue("Companies", out List<Company> _companyList);
                Company _company = _companyList.First(x => x.ID == company.ItemData.ID);
                Model.CompanyCity = _company.City;
                Model.CompanyState = _company.State;
                Model.CompanyZip = _company.ZipCode;
            }
        }
    }

    /// <summary>
    ///     Handles the event when the contact changes.
    /// </summary>
    /// <param name="contact">The change event arguments containing the old and new contact.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    private async Task ContactChanged(ChangeEventArgs<int, CompanyContact> contact)
    {
        await Task.Yield();
        if (contact is {ItemData: not null})
        {
            Model.ContactName = contact.ItemData.ContactName;
        }
    }

    /// <summary>
    ///     Handles the change of value in a dropdown.
    /// </summary>
    /// <param name="option">The option that has been changed, encapsulating the new value.</param>
    /// <param name="type">
    ///     The type of the dropdown that has been changed. 0 for Priority, 1 for Eligibility, 2 for Experience,
    ///     and 3 for Education.
    /// </param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private async Task DropValueChanged(ChangeEventArgs<int, IntValues> option, byte type)
    {
        await Task.Yield();
        switch (type)
        {
            case 0:
                Model.Priority = option.ItemData.Value;
                break;
            case 1:
                Model.Eligibility = option.ItemData.Value;
                break;
            case 2:
                Model.Experience = option.ItemData.Value;
                break;
            case 3:
                Model.Education = option.ItemData.Value;
                break;
        }
    }

    /// <summary>
    ///     Handles the event when the due date value changes.
    /// </summary>
    /// <param name="args">The arguments containing the new due date.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     If the expected start date is not the minimum value and is greater than or equal to the new due date,
    ///     the expected start date is set to be 7 days before the new due date.
    ///     After the due date change, it notifies that the DueDate field has changed and requests a UI update.
    /// </remarks>
    private async Task DueDateValueChange(ChangedEventArgs<DateTime> args)
    {
        await Task.Yield();
        if (Model.ExpectedStart != DateTime.MinValue && Model.ExpectedStart >= args.Value)
        {
            Model.ExpectedStart = args.Value.AddDays(-7);
        }

        _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.DueDate)));
        StateHasChanged();
    }

    /// <summary>
    ///     Handles the field changed event of the EditContext.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An instance of <see cref="FieldChangedEventArgs" /> containing event data.</param>
    /// <remarks>
    ///     This method is triggered when a field in the EditContext changes. If the field that changed is not 'Description',
    ///     the method returns immediately.
    ///     If the 'Description' field is the one that changed, the method validates the field and updates the 'CssClassName'
    ///     property based on whether any validation errors exist.
    /// </remarks>
    private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
    {
        if (e.FieldIdentifier.FieldName != "Description")
        {
            return;
        }

        FieldIdentifier _fieldIdentifier = new(Model, nameof(Model.Description));
        IEnumerable<string> _errorMessages = _editContext.GetValidationMessages(_fieldIdentifier);
        CssClassName = _errorMessages.Any() ? "error" : "success";
        StateHasChanged();
    }

    /// <summary>
    ///     Handles the change of the expected start date.
    /// </summary>
    /// <param name="args">The arguments of the change event, which contain the new date.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     If the due date is not the minimum value for <see cref="DateTime" /> and is less than or equal to the new start
    ///     date,
    ///     the due date is set to be one week after the new start date. After the due date is potentially changed,
    ///     the method notifies that the due date field has changed and requests a UI update.
    /// </remarks>
    private async Task ExpectedStartValueChange(ChangedEventArgs<DateTime> args)
    {
        await Task.Yield();
        if (Model.DueDate != DateTime.MinValue && Model.DueDate <= args.Value)
        {
            Model.DueDate = args.Value.AddDays(7);
        }

        _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.DueDate)));
        StateHasChanged();
    }

    /// <summary>
    ///     Asynchronously hides the dialog.
    /// </summary>
    /// <remarks>
    ///     This method is used to hide the dialog of the RequisitionDetailsPanel component.
    /// </remarks>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    public Task HideDialog()
    {
        return Dialog.HideAsync();
    }

    /// <summary>
    ///     Handles the change of job options in the requisition details panel.
    /// </summary>
    /// <param name="jobOption">The new job option selected by the user. It contains the key-value pair of the job option.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    private async Task JobOptionChange(ChangeEventArgs<string, KeyValues> jobOption)
    {
        await Task.Yield();
        if (jobOption is {ItemData: not null})
        {
            Model.JobOptions = jobOption.ItemData.Value;
        }
    }

    /// <summary>
    ///     Ensures that only numeric input is allowed in the specified field.
    /// </summary>
    /// <param name="args">The event arguments associated with the input field.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    private async Task NumbersOnly(object args)
    {
        await JsRuntime.InvokeVoidAsync("onCreate", "textDuration", true);
    }

    /// <summary>
    ///     Executes after the component has been rendered.
    /// </summary>
    /// <param name="firstRender">
    ///     A boolean value that indicates whether this is the first time the component is being
    ///     rendered.
    /// </param>
    /// <remarks>
    ///     If this is not the first render and the Model's CompanyID is greater than 0,
    ///     it sets the ContactQuery to a new Query where the ClientID is equal to the Model's CompanyID.
    /// </remarks>
    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender && Model is {CompanyID: > 0})
        {
            ContactQuery = new Query().Where(new WhereFilter {Field = "ClientID", Operator = "equal", value = Model.CompanyID});
        }

        base.OnAfterRender(firstRender);
    }

    /// <summary>
    ///     Handles the creation event.
    /// </summary>
    /// <param name="arg">The argument passed to the creation event.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private async Task OnCreate(object arg)
    {
        await JsRuntime.InvokeVoidAsync("onCreate", "autoLocZip", true);
    }

    /// <summary>
    ///     Asynchronously opens the dialog of the requisition details panel.
    /// </summary>
    /// <param name="args">The arguments for the BeforeOpen event of the dialog.</param>
    /// <remarks>
    ///     This method performs several actions:
    ///     - Yields the current task, allowing other tasks to execute.
    ///     - If no action is in progress, it sets the action progress flag and updates the ContactQuery based on the CompanyID
    ///     of the Model.
    ///     - It validates the edit context of the RequisitionForm and subscribes to the OnFieldChanged event.
    ///     - It checks for validation messages for the Description field of the Model and sets the CssClassName accordingly.
    ///     - Finally, it signals the component to re-render and resets the action progress flag.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    private async Task OpenDialog(BeforeOpenEventArgs args)
    {
        await Task.Yield();
        if (!_actionProgress)
        {
            _actionProgress = true;
            ContactQuery = new Query().Where(new WhereFilter {Field = "ClientID", Operator = "equal", value = Model.CompanyID});
            _editContext = EditRequisitionForm.EditContext;
            _editContext?.Validate();
            if (_editContext != null)
            {
                _editContext.OnFieldChanged += EditContext_OnFieldChanged;
            }

            FieldIdentifier _fieldIdentifier = new(Model, nameof(Model.Description));
            IEnumerable<string> _errorMessages = _editContext?.GetValidationMessages(_fieldIdentifier);
            CssClassName = _errorMessages != null && _errorMessages.Any() ? "error" : "success";
            StateHasChanged();
            _actionProgress = false;
        }
    }

    /// <summary>
    ///     Asynchronously saves the changes made in the requisition details panel.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    private Task SaveDialog(EditContext editContext)
    {
        return General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Save);
    }

    /// <summary>
    ///     Asynchronously shows the dialog of the requisition details panel.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    public Task ShowDialog()
    {
        return Dialog.ShowAsync();
    }

    /// <summary>
    ///     Handles the opening of a tooltip.
    /// </summary>
    /// <param name="args">The arguments for the tooltip event.</param>
    /// <remarks>
    ///     This method cancels the opening of the tooltip if it does not contain any text.
    /// </remarks>
    private void ToolTipOpen(TooltipEventArgs args)
    {
        args.Cancel = !args.HasText;
    }

    /// <summary>
    ///     Handles the change of the Zip code in the RequisitionDetailsPanel.
    /// </summary>
    /// <param name="arg">The ChangeEventArgs containing the new Zip code and associated KeyValues.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method updates the City and StateID fields of the Model based on the new Zip code.
    ///     It uses a memory cache to retrieve a list of Zip codes. If the new Zip code is found in the list,
    ///     the City and StateID fields are updated accordingly. The method is designed to handle concurrent changes to the Zip
    ///     code.
    /// </remarks>
    private async Task ZipChange(ChangeEventArgs<string, KeyValues> arg)
    {
        if (!_zipChanging)
        {
            await Task.Yield();
            _zipChanging = true;
            IMemoryCache _memoryCache = Start.MemCache;
            List<Zip> _zips = null;
            while (_zips == null)
            {
                _memoryCache.TryGetValue("Zips", out _zips);
            }

            if (_zips.Count > 0)
            {
                foreach (Zip _zip in _zips.Where(zip => zip.ZipCode == arg.Value))
                {
                    Model.City = _zip.City;
                    Model.StateID = _zip.StateID;
                    _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.City)));
                }
            }

            _zipChanging = false;
        }
    }

    /// <summary>
    ///     The ZipDropDownAdaptor class is a DataAdaptor used for handling ZIP code data in the RequisitionDetailsPanel.
    /// </summary>
    /// <remarks>
    ///     This class is responsible for asynchronously reading data using the provided DataManagerRequest.
    ///     It utilizes the General.GetAutocompleteZipAsync method to fetch data.
    ///     If the method is already reading data (_reading is true), it returns null.
    /// </remarks>
    public class ZipDropDownAdaptor : DataAdaptor
    {
        private bool _reading;

        /// <summary>
        ///     Asynchronously reads data using the provided DataManagerRequest.
        /// </summary>
        /// <param name="dm">The DataManagerRequest to use for reading data.</param>
        /// <param name="key">An optional key to use for reading data. Default is null.</param>
        /// <returns>
        ///     A task that represents the asynchronous read operation. The task result contains the data read.
        /// </returns>
        /// <remarks>
        ///     This method utilizes the General.GetAutocompleteZipAsync method to fetch data.
        ///     If the method is already reading data (_reading is true), it returns null.
        /// </remarks>
        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            if (_reading)
            {
                return null;
            }

            await Task.Yield();
            _reading = true;
            object _returnValue = await General.GetAutocompleteZipAsync(dm);
            _reading = false;
            return _returnValue;
        }
    }
}