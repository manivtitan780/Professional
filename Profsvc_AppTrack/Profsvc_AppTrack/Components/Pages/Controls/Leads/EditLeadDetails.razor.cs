#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           EditLeadDetails.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-31-2023 20:28
// Last Updated On:     09-30-2023 16:31
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Leads;

/// <summary>
///     Represents a component for editing lead details in the application.
/// </summary>
/// <remarks>
///     This class contains parameters for various properties of a lead, such as industry, source, state, and status,
///     which are represented by lists of `ByteValues` and `IntValues`. It also includes event callbacks for saving
///     the changes or cancelling the edit operation.
///     The `ShowDialog` method can be used to display the dialog for editing the lead details.
/// </remarks>
public partial class EditLeadDetails
{
    /// <summary>
    ///     Gets or sets the event callback that is invoked when the edit operation is cancelled.
    /// </summary>
    /// <value>
    ///     The event callback which takes a <see cref="MouseEventArgs" /> as a parameter.
    /// </value>
    /// <remarks>
    ///     This callback is typically used to perform additional actions after the edit operation has been cancelled,
    ///     such as updating the UI or logging the operation.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the dialog component used for editing lead details.
    /// </summary>
    /// <value>
    ///     The dialog component of type `SfDialog`.
    /// </value>
    /// <remarks>
    ///     This dialog is shown when the `ShowDialog` method is called and hidden when the `CancelDialog` method is called.
    ///     The dialog contains form fields for editing the properties of a lead. The changes are saved when the
    ///     `SaveLeadDialog` method is called.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the form used for editing lead details.
    /// </summary>
    /// <remarks>
    ///     This property represents the form that contains the input fields for editing the details of a lead.
    ///     It is used in the `BuildRenderTree` method to bind the form fields to the corresponding properties of the lead.
    /// </remarks>
    private EditForm EditLeadForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter component used in the EditLeadDetails component.
    /// </summary>
    /// <value>
    ///     The DialogFooter component which provides the footer for the dialog used to edit lead details.
    ///     It contains properties and methods for managing the Cancel and Save buttons in the dialog.
    /// </value>
    private DialogFooter FooterDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the header string for the dialog.
    /// </summary>
    /// <value>
    ///     The header string displayed at the top of the dialog when editing lead details.
    /// </value>
    [Parameter]
    public string HeaderString
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of industries associated with the lead.
    /// </summary>
    /// <value>
    ///     The list of industries is represented by instances of the `ByteValues` class.
    /// </value>
    [Parameter]
    public List<ByteValues> Industries
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model representing the details of a lead.
    /// </summary>
    /// <value>
    ///     The model is of type `LeadDetails` which holds the various properties of a lead.
    /// </value>
    /// <remarks>
    ///     This model is used in the `EditLeadDetails` component for binding the lead details in the form fields.
    ///     The form fields are then used to edit the details of the lead.
    /// </remarks>
    [Parameter]
    public LeadDetails Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the lead details are saved.
    /// </summary>
    /// <value>
    ///     The event callback which takes an <see cref="EditContext" /> as a parameter.
    /// </value>
    /// <remarks>
    ///     This callback is typically used to perform additional actions after the lead details have been saved,
    ///     such as updating the UI or logging the operation.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of sources associated with the lead.
    /// </summary>
    /// <value>
    ///     The list of sources is represented by instances of the `ByteValues` class.
    /// </value>
    [Parameter]
    public List<ByteValues> Sources
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfSpinner control used in the EditLeadDetails component.
    /// </summary>
    /// <value>
    ///     The SfSpinner control, which is used to indicate a loading state while the lead details are being saved or
    ///     cancelled.
    /// </value>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of states associated with the lead.
    /// </summary>
    /// <value>
    ///     The list of states is represented as a list of `IntValues`.
    /// </value>
    [Parameter]
    public List<IntValues> States
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of status values for the lead.
    /// </summary>
    /// <value>
    ///     The list of status values represented as instances of <see cref="ByteValues" />.
    /// </value>
    [Parameter]
    public List<ByteValues> Status
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously cancels the dialog for editing lead details.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method calls the general cancel method, passing in the mouse event arguments, spinner, footer dialog, dialog,
    ///     and cancel parameters.
    /// </remarks>
    private async Task CancelDialog(MouseEventArgs args)
    {
        await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);
    }

    /// <summary>
    ///     Handles the change event for the Industry dropdown.
    /// </summary>
    /// <param name="args">The event arguments containing the selected industry value.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method updates the 'LeadIndustry' property of the model with the selected industry value from the dropdown.
    /// </remarks>
    private async Task IndustryDropValueChange(ChangeEventArgs<byte, ByteValues> args)
    {
        await Task.Yield();
        Model.LeadIndustry = args.ItemData?.Value;
    }

    /// <summary>
    ///     Validates the current edit context of the lead form.
    /// </summary>
    /// <remarks>
    ///     This method is called when the dialog for editing lead details is opened.
    ///     It ensures that the form data in the `EditLeadForm` is valid according to the defined validation rules.
    /// </remarks>
    private void OpenDialog()
    {
        EditLeadForm.EditContext?.Validate();
    }

    /// <summary>
    ///     Asynchronously saves the changes made to the lead details.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <remarks>
    ///     This method calls the `General.CallSaveMethod` to save the changes made to the lead details.
    ///     It passes the `editContext`, `Spinner`, `FooterDialog`, `Dialog`, and `Save` as parameters to the `CallSaveMethod`.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SaveLeadDialog(EditContext editContext)
    {
        await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Save);
    }

    /// <summary>
    ///     Asynchronously displays the dialog for editing lead details.
    /// </summary>
    /// <remarks>
    ///     This method is used to show the dialog that allows the user to edit the details of a lead.
    ///     It is typically called when the user initiates an edit operation, such as by clicking an "Edit" button.
    /// </remarks>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    internal async Task ShowDialog()
    {
        await Dialog.ShowAsync();
    }

    /// <summary>
    ///     Handles the event when the value of the source dropdown changes.
    /// </summary>
    /// <param name="args">The arguments of the change event, containing the new selected item data.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sets the `LeadSource` property of the model to the value of the newly selected item.
    /// </remarks>
    private async Task SourceDropValueChange(ChangeEventArgs<byte, ByteValues> args)
    {
        await Task.Yield();
        Model.LeadSource = args.ItemData?.Value;
    }

    /// <summary>
    ///     Handles the event when the selected value in the State dropdown changes.
    /// </summary>
    /// <param name="args">The arguments of the change event, containing the new selected item of type `IntValues`.</param>
    /// <returns>A `Task` representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sets the `StateName` property of the `Model` to the `Value` of the newly selected item.
    /// </remarks>
    private async Task StateDropValueChange(ChangeEventArgs<int, IntValues> args)
    {
        await Task.Yield();
        Model.StateName = args.ItemData?.Value;
    }

    /// <summary>
    ///     Handles the change event for the Status dropdown.
    /// </summary>
    /// <param name="args">The event arguments containing the new selected value.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is invoked when the Status dropdown value changes. It updates the `LeadStatus` property of the `Model`
    ///     with the new selected value from the dropdown.
    /// </remarks>
    private async Task StatusDropValueChange(ChangeEventArgs<byte, ByteValues> args)
    {
        await Task.Yield();
        Model.LeadStatus = args.ItemData?.Value;
    }
}