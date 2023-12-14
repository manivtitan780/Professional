#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           StatusCodeDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-14-2023 16:6
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing status codes in the application.
/// </summary>
/// <remarks>
///     This dialog provides functionality for editing a status code. It includes parameters for handling the cancel event,
///     setting the header string, managing the status code model, and handling the save event.
/// </remarks>
public partial class StatusCodeDialog
{
    private EditContext _editContext = null;

    /// <summary>
    ///     A list of key-value pairs representing status codes and their corresponding descriptions.
    /// </summary>
    /// <remarks>
    ///     This list is used to populate the dropdown menu in the status code dialog. Each key-value pair consists of a status
    ///     code (key) and its description (value).
    ///     The following status codes are included: "CLI" for Client, "CND" for Candidate, "REQ" for Requisition, "SCN" for
    ///     Candidate Submission, "SUB" for Submission, "USR" for User, and "VND" for Vendor.
    /// </remarks>
    private readonly List<KeyValues> _statusDropItems = new()
                                                        {
                                                            new("CLI", "Client"), new("CND", "Candidate"), new("REQ", "Requisition"),
                                                            new("SCN", "Candidate Submission"), new("SUB", "Submission"), new("USR", "User"),
                                                            new("VND", "Vendor")
                                                        };

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the cancel action is triggered in the dialog.
    /// </summary>
    /// <value>
    ///     The event callback for the cancel action.
    /// </value>
    /// <remarks>
    ///     This event callback is typically used to perform cleanup tasks when the user cancels the operation in the dialog.
    ///     The event handler receives the mouse event arguments associated with the cancel action.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfDialog instance used in the status code dialog.
    /// </summary>
    /// <value>
    ///     The SfDialog instance.
    /// </value>
    /// <remarks>
    ///     This instance is used to control the visibility and behavior of the dialog.
    ///     It is bound to the dialog component in the Razor markup and can be used to programmatically show or hide the
    ///     dialog.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter instance associated with the StatusCodeDialog.
    /// </summary>
    /// <value>
    ///     The DialogFooter instance.
    /// </value>
    /// <remarks>
    ///     The DialogFooter instance is used to manage the footer section of the dialog,
    ///     which includes the Cancel and Save buttons.
    /// </remarks>
    private DialogFooter DialogFooter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the form used for editing a status code.
    /// </summary>
    /// <value>
    ///     The form used for editing a status code.
    /// </value>
    private EditForm EditStatusCodeForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the header string for the status code dialog.
    /// </summary>
    /// <value>
    ///     The header string displayed at the top of the dialog.
    /// </value>
    [Parameter]
    public string HeaderString
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model representing the status code for the dialog.
    /// </summary>
    /// <value>
    ///     The model representing the status code.
    /// </value>
    /// <remarks>
    ///     This model is used to bind the status code data in the dialog for editing purposes.
    /// </remarks>
    [Parameter]
    public StatusCode Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the save action is triggered in the dialog.
    /// </summary>
    /// <value>
    ///     The event callback for the save action.
    /// </value>
    /// <remarks>
    ///     This event callback is typically used to perform save tasks when the user confirms the operation in the dialog.
    ///     The event handler receives the EditContext associated with the save action.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the spinner control used in the status code dialog.
    /// </summary>
    /// <value>
    ///     The spinner control.
    /// </value>
    /// <remarks>
    ///     This control is used to display a loading indicator while the dialog is performing operations such as saving or
    ///     canceling.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously executes the cancellation process for the status code dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <remarks>
    ///     This method calls the general cancellation method, which hides the spinner and dialog, and enables the dialog
    ///     buttons.
    ///     It is typically invoked when the user triggers the cancel action in the status code dialog.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task CancelStatusCode(MouseEventArgs args) => General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);

    /// <summary>
    ///     Asynchronously prepares the status code dialog for opening.
    /// </summary>
    /// <param name="args">The arguments associated with the dialog opening event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is invoked before the dialog is opened. It initializes the edit context for the status code form and
    ///     validates it.
    /// </remarks>
    private async Task OpenDialog(BeforeOpenEventArgs args)
    {
        await Task.Yield();
        _editContext = EditStatusCodeForm.EditContext;
        _editContext?.Validate();
    }

    /// <summary>
    ///     Asynchronously saves the status code changes made in the dialog.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <remarks>
    ///     This method calls the general save method, passing in the edit context, spinner, dialog footer, dialog, and save
    ///     event callback.
    ///     It is typically triggered when the user confirms the save operation in the dialog.
    /// </remarks>
    private Task SaveStatusCode(EditContext editContext) => General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);

    /// <summary>
    ///     Asynchronously sets the validators for the 'Code' and 'Status' fields of the model.
    /// </summary>
    /// <param name="arg">The change event arguments containing the new values for the 'Code' and 'Status' fields.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method triggers a field change notification for the 'Code' and 'Status' fields of the model,
    ///     which in turn triggers the validation process for these fields.
    /// </remarks>
    private async Task SetValidators(ChangeEventArgs<string, KeyValues> arg)
    {
        await Task.Yield();
        _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.Code)));
        _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.Status)));
    }

    /// <summary>
    ///     Asynchronously displays the status code dialog.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is used to programmatically display the dialog for editing a status code.
    ///     It invokes the ShowAsync method of the SfDialog instance associated with the dialog.
    /// </remarks>
    public Task ShowDialog() => Dialog.ShowAsync();

    /// <summary>
    ///     Handles the opening of a tooltip.
    /// </summary>
    /// <param name="args">The arguments associated with the tooltip event.</param>
    /// <remarks>
    ///     This method cancels the opening of the tooltip if it does not contain any text.
    /// </remarks>
    private static void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}