#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           StateDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-14-2023 16:5
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing State entities in the Admin area.
/// </summary>
/// <remarks>
///     This dialog provides functionality for creating, editing, and saving State entities.
///     It exposes parameters for handling events such as Save and Cancel, and for setting the dialog header and the State
///     model.
/// </remarks>
public partial class StateDialog
{
    /// <summary>
    ///     Gets or sets the event callback that is invoked when the Cancel event is triggered.
    /// </summary>
    /// <value>
    ///     The event callback for the Cancel event.
    /// </value>
    /// <remarks>
    ///     This event callback is used to handle the cancellation of the state management process in the dialog.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfDialog instance used in the state dialog.
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
    ///     Gets or sets the DialogFooter instance associated with the StateDialog.
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
    ///     Gets or sets the EditForm for managing State entities in the dialog.
    /// </summary>
    /// <value>
    ///     The EditForm that represents the form for creating or editing a State entity.
    /// </value>
    /// <remarks>
    ///     This property is used to bind the form for managing State entities to the dialog.
    ///     It is used in the dialog for creating or editing a state.
    /// </remarks>
    private EditForm EditStateForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the header string for the StateDialog.
    /// </summary>
    /// <value>
    ///     The string that represents the header of the StateDialog.
    /// </value>
    /// <remarks>
    ///     This property is used to set the header of the StateDialog. It is displayed at the top of the dialog.
    /// </remarks>
    [Parameter]
    public string HeaderString
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the State model for the StateDialog.
    /// </summary>
    /// <value>
    ///     The State model that represents the state entity being managed in the dialog.
    /// </value>
    /// <remarks>
    ///     This property is used to bind the state entity data to the dialog. It is used in the form for creating or editing a
    ///     state.
    /// </remarks>
    [Parameter]
    public State Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the Save event is triggered.
    /// </summary>
    /// <value>
    ///     The event callback for the Save event.
    /// </value>
    /// <remarks>
    ///     This event callback is used to handle the saving of the state entity data in the dialog.
    ///     It is triggered when the user clicks the Save button in the dialog.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfSpinner control for the StateDialog.
    /// </summary>
    /// <value>
    ///     The SfSpinner control that represents the loading spinner in the dialog.
    /// </value>
    /// <remarks>
    ///     This property is used to control the loading spinner in the dialog.
    ///     It is displayed when the dialog is performing an operation that requires waiting, such as saving or cancelling.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously cancels the state management process in the dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <remarks>
    ///     This method is used to handle the cancellation of the state management process in the dialog.
    ///     It invokes the General.CallCancelMethod, passing in the necessary controls and the Cancel event callback.
    /// </remarks>
    private Task CancelState(MouseEventArgs args) => General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);

    /// <summary>
    ///     Asynchronously opens the dialog and validates the form context.
    /// </summary>
    /// <param name="arg">
    ///     The event arguments for the BeforeOpen event.
    /// </param>
    /// <remarks>
    ///     This method is invoked when the dialog is about to open. It ensures that the form context is validated before the
    ///     dialog is displayed.
    /// </remarks>
    private async Task OpenDialog(BeforeOpenEventArgs arg)
    {
        await Task.Yield();
        EditStateForm.EditContext?.Validate();
    }

    /// <summary>
    ///     Asynchronously saves the state entity data from the dialog.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is invoked when the user submits the form in the dialog.
    ///     It calls the general save method, which shows the spinner, disables the dialog buttons,
    ///     executes the save operation, and then hides the spinner and dialog, and enables the dialog buttons.
    /// </remarks>
    private Task SaveState(EditContext editContext) => General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);

    /// <summary>
    ///     Asynchronously displays the StateDialog.
    /// </summary>
    /// <remarks>
    ///     This method is used to open the dialog for managing State entities.
    ///     It is typically invoked when a user wants to create or edit a State entity.
    /// </remarks>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    public Task ShowDialog() => Dialog.ShowAsync();
}