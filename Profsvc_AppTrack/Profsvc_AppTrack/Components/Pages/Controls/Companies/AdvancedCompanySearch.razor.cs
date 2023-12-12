#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           AdvancedCompanySearch.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-28-2023 15:11
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Companies;

/// <summary>
///     Represents a component that provides advanced search functionality for companies.<br />
///     This class contains properties and methods for managing the search criteria and triggering the search operation.
///     <br />
///     It includes features such as a spinner for loading indication, a dialog footer for action buttons,
///     and dropdown lists for state and status selection.
/// </summary>
public partial class AdvancedCompanySearch
{
    /// <summary>
    ///     Gets or sets the event callback that is invoked when the cancel action is triggered.
    /// </summary>
    /// <value>
    ///     The event callback for the cancel action.
    /// </value>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the dialog component used in the advanced company search.<br />
    ///     This dialog is used to display the search interface to the user and handle user interactions.<br />
    ///     It is controlled programmatically through methods such as 'ShowDialog', 'CancelSearchDialog', and
    ///     'SearchCompanyDialog'.
    /// </summary>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter instance associated with the AdvancedCompanySearch component.<br />
    ///     The DialogFooter is a part of the user interface that contains action buttons for the advanced search
    ///     functionality.<br />
    ///     It includes a Cancel button to stop the search operation and a Save button to trigger the search operation based on
    ///     the specified criteria.
    /// </summary>
    /// <value>
    ///     The DialogFooter instance.
    /// </value>
    public DialogFooter FooterDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the IJSRuntime instance used for invoking JavaScript functions from .NET code.<br />
    ///     This property is used for interacting with the JavaScript runtime in the context of the AdvancedCompanySearch
    ///     component.<br />
    ///     For example, it is used in the NumbersOnly method to invoke the JavaScript function "onCreate".
    /// </summary>
    /// <value>
    ///     The IJSRuntime instance.
    /// </value>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the CompanySearch instance associated with the AdvancedCompanySearch component.<br />
    ///     The CompanySearch instance represents the search criteria for the advanced search functionality.<br />
    ///     It includes properties such as CompanyName, Phone, EmailAddress, Status, State, and MyCompanies, which are used to
    ///     filter the search results.
    /// </summary>
    /// <value>
    ///     The CompanySearch instance.
    /// </value>
    [Parameter]
    public CompanySearch Model
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the search operation is triggered.<br />
    ///     This event callback is used to perform the search operation based on the criteria specified in the CompanySearch
    ///     instance.
    /// </summary>
    /// <value>
    ///     The event callback for the search operation.
    /// </value>
    [Parameter]
    public EventCallback<EditContext> Search
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfSpinner instance associated with the AdvancedCompanySearch component.<br />
    ///     The SfSpinner instance represents a loading indicator that is displayed when the search operation is in progress.
    /// </summary>
    /// <value>
    ///     The SfSpinner instance.
    /// </value>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of integer values used for the state dropdown in the advanced company search.<br />
    ///     This list is used to populate the state selection dropdown in the user interface, allowing the user to filter the
    ///     search results by state.
    /// </summary>
    /// <value>
    ///     The list of integer values for the state dropdown.
    /// </value>
    [Parameter]
    public List<IntValues> StateDropDown
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of KeyValues instances for the status dropdown in the AdvancedCompanySearch component.<br />
    ///     Each KeyValues instance represents a status option in the dropdown list.<br />
    ///     This list is used to populate the status selection in the advanced search functionality.
    /// </summary>
    /// <value>
    ///     The list of KeyValues instances for the status dropdown.
    /// </value>
    [Parameter]
    public List<KeyValues> StatusDropDown
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously cancels the search dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    private async Task CancelSearchDialog(MouseEventArgs args)
    {
        await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);
        //await Task.Yield();
        //      await Cancel.InvokeAsync(args);
        //      await Spinner.HideAsync();
        //      await Dialog.HideAsync();
    }

    /// <summary>
    ///     An asynchronous method that restricts the input to numbers only for a specific text field.<br />
    ///     This method is used in the context of the AdvancedCompanySearch component, specifically for the phone number input
    ///     field.<br />
    ///     It invokes the JavaScript function "onCreate" with specific parameters to enforce the numbers-only restriction.
    /// </summary>
    /// <param name="args">
    ///     The arguments passed to the method. In the current context, this parameter is not used.
    /// </param>
    /// <returns>
    ///     A Task representing the asynchronous operation.
    /// </returns>
    private async Task NumbersOnly(object args) => await JsRuntime.InvokeVoidAsync("onCreate", "textPhone", true);

    /// <summary>
    ///     Executes the advanced company search operation.<br />
    ///     This method is responsible for invoking the 'CallSaveMethod' from the 'General' utility class, which manages the
    ///     search operation, including showing and hiding the loading spinner, disabling and enabling the dialog buttons, and
    ///     hiding the dialog upon completion.
    /// </summary>
    /// <param name="editContext">The edit context associated with the search operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SearchCompanyDialog(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Search);

    /// <summary>
    ///     Initiates the display of the advanced company search dialog.<br />
    ///     This method is used to programmatically open the dialog component of the AdvancedCompanySearch.<br />
    ///     The dialog provides an interface for the user to specify search criteria for companies.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ShowDialog() => await Dialog.ShowAsync();
}