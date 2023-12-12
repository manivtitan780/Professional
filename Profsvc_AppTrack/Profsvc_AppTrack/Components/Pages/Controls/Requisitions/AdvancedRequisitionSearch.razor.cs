#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           AdvancedRequisitionSearch.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     10-02-2023 16:32
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Controls.Requisitions;

/// <summary>
///     Represents a component for advanced requisition search functionality.
/// </summary>
/// <remarks>
///     This component provides a user interface for advanced search of requisitions. It includes various parameters for
///     search criteria such as autocomplete data for city and zip code fields, list of companies, eligibility options, job
///     options, and status options. It also provides event callbacks for cancel and search actions.
/// </remarks>
public partial class AdvancedRequisitionSearch
{
    /// <summary>
    ///     Gets or sets the autocomplete data for city and zip code fields in the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The autocomplete data, represented as an object.
    /// </value>
    [Parameter]
    public object AutoCompleteCityZip
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the cancel action is performed in the Advanced Requisition
    ///     Search.
    /// </summary>
    /// <value>
    ///     The event callback that takes a MouseEventArgs as a parameter and returns a Task.
    /// </value>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of companies for advanced requisition search.
    /// </summary>
    /// <value>
    ///     The list of companies, represented as instances of <see cref="KeyValues" />.
    /// </value>
    [Parameter]
    public List<KeyValues> Companies
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the maximum creation date for the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The maximum creation date, represented as an instance of the <see cref="DateControl" /> class.
    /// </value>
    private DateControl CreatedMax
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the dialog for the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The dialog, represented as an instance of the <see cref="SfDialog" /> class.
    /// </value>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the maximum due date control for the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The maximum due date, represented as an instance of the
    ///     <see cref="LabelComponents.Areas.MyFeature.Pages.DateControl" /> class.
    /// </value>
    private DateControl DueMax
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the EditForm for the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The EditForm used to manage the inputs for the Advanced Requisition Search.
    /// </value>
    private EditForm EditSearchForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of eligibility options for the advanced requisition search.
    /// </summary>
    /// <value>
    ///     The list of eligibility options, represented as instances of the <see cref="IntValues" /> class.
    /// </value>
    [Parameter]
    public List<IntValues> EligibilityDropDown
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the dialog footer for the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The dialog footer, represented as an instance of the <see cref="DialogFooter" /> class.
    /// </value>
    internal DialogFooter FooterDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of job options for the advanced requisition search.
    /// </summary>
    /// <value>
    ///     The list of job options, represented as instances of the <see cref="KeyValues" /> class.
    /// </value>
    [Parameter]
    public List<KeyValues> JobOption
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of job options for the dropdown in the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The list of key-value pairs, where the key is the job option identifier and the value is the job option name.
    /// </value>
    [Parameter]
    public List<KeyValues> JobOptionsDropDown
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the model for the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The model is of type <see cref="RequisitionSearch" />, which represents the search criteria for requisitions.
    /// </value>
    [Parameter]
    public RequisitionSearch Model
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets the list of integer values representing the proximity units for the advanced requisition search.
    /// </summary>
    /// <value>
    ///     The list of integer values for proximity units.
    /// </value>
    private List<IntValues> ProximityUnit
    {
        get;
        //set;
    } = new();

    /// <summary>
    ///     Gets the list of integer values for the proximity search in the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The list of integer values representing proximity.
    /// </value>
    private List<IntValues> ProximityValue
    {
        get;
        //set;
    } = new();

    /// <summary>
    ///     Gets or sets the list of key-value pairs for the Relocate dropdown in the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The list of key-value pairs.
    /// </value>
    private List<KeyValues> RelocateDropDown
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the event callback for the search operation in the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The event callback that takes an EditContext as a parameter and returns a Task.
    /// </value>
    [Parameter]
    public EventCallback<EditContext> Search
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of key-value pairs for the security clearance dropdown in the advanced requisition search.
    /// </summary>
    /// <value>
    ///     The list of key-value pairs.
    /// </value>
    private List<KeyValues> SecurityClearanceDropDown
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets a list of KeyValues instances representing the requisitions to be displayed.
    /// </summary>
    private List<KeyValues> ShowRequisitions
    {
        get;
    } = new();

    /// <summary>
    ///     Gets or sets the spinner control for the Advanced Requisition Search.
    /// </summary>
    /// <value>
    ///     The spinner control.
    /// </value>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of integer values for the state dropdown in the advanced requisition search.
    /// </summary>
    /// <value>
    ///     The list of integer values.
    /// </value>
    [Parameter]
    public List<IntValues> StateDropDown
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the list of key-value pairs for the status dropdown in the advanced requisition search.
    /// </summary>
    /// <value>
    ///     The list of key-value pairs.
    /// </value>
    [Parameter]
    public List<KeyValues> StatusDropDown
    {
        get;
        set;
    }

    private DateTime DueEndMax
    {
        get;
        set;
    }

    private DateTime DueEndMin
    {
        get;
        set;
    }

    private DateTime CreatedEndMax
    {
        get;
        set;
    }

    private DateTime CreatedEndMin
    {
        get;
        set;
    }

    /// <summary>
    ///     Cancels the search dialog asynchronously.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task CancelSearchDialog(MouseEventArgs args)
    {
        await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);
    }

    /// <summary>
    ///     Handles the selection of a creation date.
    /// </summary>
    /// <param name="args">The event arguments containing the selected date.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sets the minimum and maximum creation dates based on the selected date.
    ///     The maximum date is set to 6 months after the selected date.
    /// </remarks>
    private async Task CreatedOnSelect(ChangedEventArgs<DateTime> args)
    {
        await Task.Yield();
        CreatedEndMin = args.Value;
        CreatedEndMax = args.Value.AddMonths(6);
    }

    /// <summary>
    ///     Handles the selection of a due date.
    /// </summary>
    /// <param name="args">The event arguments containing the selected date.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task DueOnSelect(ChangedEventArgs<DateTime> args)
    {
        await Task.Yield();
        DueEndMin = args.Value;
        DueEndMax = args.Value.AddMonths(6);
    }

    /// <summary>
    ///     Initializes the component asynchronously. This method is called when the component is first initialized.
    /// </summary>
    /// <remarks>
    ///     This method populates the 'ShowRequisitions' list with predefined options for requisition filtering.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        await Task.Yield();
        ShowRequisitions.Add(new("My Requisitions Only", "M"));
        ShowRequisitions.Add(new("My Requisitions and Assigned Requisitions", "A"));
        ShowRequisitions.Add(new("All Assigned Requisitions", "R"));
        ShowRequisitions.Add(new("All Requisitions", "%"));
    }

    /// <summary>
    ///     Opens the dialog for advanced requisition search.
    ///     This method validates the EditForm's EditContext upon opening the dialog.
    /// </summary>
    private void OpenDialog()
    {
        EditSearchForm.EditContext?.Validate();
    }

    /// <summary>
    ///     Asynchronously executes the search for candidates based on the provided context.
    /// </summary>
    /// <param name="editContext">The edit context associated with the search action.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SearchCandidateDialog(EditContext editContext)
    {
        await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Search);
    }

    /// <summary>
    ///     Asynchronously shows the dialog for advanced requisition search.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task ShowDialog()
    {
        await Dialog.ShowAsync();
    }
}