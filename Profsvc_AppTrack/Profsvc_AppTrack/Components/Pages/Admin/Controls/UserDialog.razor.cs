#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           UserDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-23-2023 16:5
// *****************************************/

#endregion

// ReSharper disable MemberCanBePrivate.Global
namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog control for managing user information in the administration section.
/// </summary>
/// <remarks>
///     This dialog control provides the following functionalities:
///     - Cancel: An event that is triggered when the dialog is cancelled.
///     - Dialog: Represents the dialog control itself.
///     - HeaderString: The header text displayed in the dialog.
///     - Model: The user data model that is being edited in the dialog.
///     - RolesList: A list of roles that can be assigned to the user.
///     - Save: An event that is triggered when the user data is saved.
///     - Spinner: Represents a spinner control used during data loading and processing.
/// </remarks>
public partial class UserDialog
{
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
    ///     Gets or sets the SfButton instance representing the cancel button in the dialog.
    /// </summary>
    /// <value>
    ///     The SfButton instance representing the cancel button.
    /// </value>
    /// <remarks>
    ///     This instance is used to control the behavior of the cancel button in the dialog.
    ///     It is bound to the cancel button in the Razor markup and can be used to programmatically control the button's
    ///     properties and events.
    /// </remarks>
    private SfButton CancelButton
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfDialog instance used in the user dialog.
    /// </summary>
    /// <value>
    ///     The SfDialog instance.
    /// </value>
    /// <remarks>
    ///     This instance is used to control the visibility and behavior of the dialog.
    ///     It is bound to the dialog component in the Razor markup and can be used to programmatically show or hide the
    ///     dialog.
    /// </remarks>
    public SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter instance associated with the UserDialog.
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
    ///     Gets or sets the form used for editing a user.
    /// </summary>
    /// <value>
    ///     The form used for editing a user.
    /// </value>
    private EditForm EditUserForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the header string for the user dialog.
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
    ///     Gets or sets the model representing the user for the dialog.
    /// </summary>
    /// <value>
    ///     The model representing the user.
    /// </value>
    /// <remarks>
    ///     This model is used to bind the user data in the dialog for editing purposes.
    /// </remarks>
    [Parameter]
    public User Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of roles that can be assigned to the user.
    /// </summary>
    /// <value>
    ///     The list of roles, represented as a collection of <see cref="KeyValues" /> instances.
    /// </value>
    [Parameter]
    public List<KeyValues> RolesList
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
    ///     Gets or sets the SfButton instance used as the save button in the user dialog.
    /// </summary>
    /// <value>
    ///     The SfButton instance.
    /// </value>
    /// <remarks>
    ///     This instance is used to trigger the save action in the dialog.
    ///     It is bound to the save button component in the Razor markup and can be used to programmatically control the
    ///     behavior of the save button.
    /// </remarks>
    private SfButton SaveButton
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the spinner control used in the user dialog.
    /// </summary>
    /// <value>
    ///     The spinner control.
    /// </value>
    /// <remarks>
    ///     This control is used to display a loading indicator while the dialog is performing operations such as saving or
    ///     canceling.
    /// </remarks>
    public SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously executes the cancellation process for the user dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <remarks>
    ///     This method calls the general cancellation method, which hides the spinner and dialog, and enables the dialog
    ///     buttons.
    ///     It is typically invoked when the user triggers the cancel action in the user dialog.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CancelUserList(MouseEventArgs args)
    {
        await Task.Yield();
        await Cancel.InvokeAsync(args);
        await Spinner.HideAsync();
        await Dialog.HideAsync();
    }

    /// <summary>
    ///     Asynchronously prepares the user dialog for opening.
    /// </summary>
    /// <param name="args">The arguments associated with the dialog opening event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is invoked before the dialog is opened. It initializes the edit context for the user form and
    ///     validates it.
    /// </remarks>
    private void OpenDialog(BeforeOpenEventArgs args) => EditUserForm.EditContext?.Validate();

    /// <summary>
    ///     Asynchronously saves the list of users.
    /// </summary>
    /// <param name="editContext">The context for the form being edited.</param>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Yields control to allow the UI to update.
    ///     - Checks if the dialog footer buttons are not disabled.
    ///     - If the buttons are not disabled, it does the following:
    ///     - Shows the spinner to indicate processing.
    ///     - Disables the dialog footer buttons to prevent further actions.
    ///     - Invokes the Save event with the provided edit context.
    ///     - Yields control to allow the UI to update.
    ///     - Enables the dialog footer buttons.
    ///     - Hides the spinner to indicate the end of processing.
    ///     - Hides the dialog.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SaveUserList(EditContext editContext)
    {
        if (!DialogFooter.ButtonsDisabled())
        {
            await Spinner.ShowAsync();
            DialogFooter.DisableButtons();
            await Save.InvokeAsync(editContext);

            DialogFooter.EnableButtons();
            await Spinner.HideAsync();
            await Dialog.HideAsync();
        }
    }
}