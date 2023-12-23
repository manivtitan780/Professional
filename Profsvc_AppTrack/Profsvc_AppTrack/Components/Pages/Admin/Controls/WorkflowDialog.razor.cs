#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           WorkflowDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-23-2023 16:9
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing workflows in the application.
/// </summary>
/// <remarks>
///     The WorkflowDialog class provides a user interface for creating, editing, and managing workflows.
///     It includes parameters for handling events such as saving and cancelling changes, as well as properties for
///     managing the dialog's appearance and behavior.
///     The dialog uses an instance of the AppWorkflow class as a model to bind the workflow data for editing.
///     It also provides methods for programmatically showing the dialog, handling the opening of tooltips, and executing
///     the cancellation and save processes for the workflow.
/// </remarks>
public partial class WorkflowDialog
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
    ///     Gets or sets the SfButton control that represents the cancel button in the dialog.
    /// </summary>
    /// <value>
    ///     The SfButton control for the cancel action.
    /// </value>
    /// <remarks>
    ///     This control is typically used to trigger the cancellation of the operation in the dialog.
    ///     It is linked to the Cancel event callback, and clicking it will invoke the associated event handler.
    /// </remarks>
    private SfButton CancelButton
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfDialog instance used in the workflow dialog.
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
    ///     Gets or sets the DialogFooter instance associated with the WorkflowDialog.
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
    ///     Gets or sets the form used for editing a workflow.
    /// </summary>
    /// <value>
    ///     The form used for editing a workflow.
    /// </value>
    private EditForm EditWorkflowForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the header string for the workflow dialog.
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
    ///     Gets or sets the model representing the workflow for the dialog.
    /// </summary>
    /// <value>
    ///     The model representing the workflow.
    /// </value>
    /// <remarks>
    ///     This model is used to bind the workflow data in the dialog for editing purposes.
    /// </remarks>
    [Parameter]
    public AppWorkflow Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the original username associated with the workflow.
    /// </summary>
    /// <value>
    ///     The original username as a string.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve the username of the user who originally initiated or is associated with
    ///     the workflow.
    /// </remarks>
    [Parameter]
    public string OriginalUserName
    {
        get;
        set;
    } = "";

    /// <summary>
    ///     Gets or sets the list of roles that can be assigned to the workflow.
    /// </summary>
    /// <value>
    ///     The list of roles.
    /// </value>
    /// <remarks>
    ///     This list is used to populate the roles dropdown in the workflow dialog for selection.
    ///     Each role is represented by a KeyValues object, where the key is the role ID and the value is the role name.
    /// </remarks>
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
    ///     Gets or sets the SfButton control that represents the save button in the dialog.
    /// </summary>
    /// <value>
    ///     The SfButton control for the save action.
    /// </value>
    /// <remarks>
    ///     This control is typically used to trigger the saving of the operation in the dialog.
    ///     It is linked to the Save event callback, and clicking it will invoke the associated event handler.
    /// </remarks>
    private SfButton SaveButton
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the spinner control used in the workflow dialog.
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
    ///     Gets or sets the list of key-value pairs representing the steps in the workflow.
    /// </summary>
    /// <value>
    ///     The list of key-value pairs for the steps.
    /// </value>
    /// <remarks>
    ///     This list is used as a data source for the MultiSelectControl in the dialog, allowing the user to select one or
    ///     more steps in the workflow.
    /// </remarks>
    [Parameter]
    public List<KeyValues> Steps
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously executes the cancellation process for the workflow dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <remarks>
    ///     This method calls the general cancellation method, which hides the spinner and dialog, and enables the dialog
    ///     buttons.
    ///     It is typically invoked when the user triggers the cancel action in the workflow dialog.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CancelWorkflow(MouseEventArgs args)
    {
        await Cancel.InvokeAsync(args);
        await Spinner.HideAsync();
        await Dialog.HideAsync();
    }

    /// <summary>
    ///     Asynchronously prepares the workflow dialog for opening.
    /// </summary>
    /// <param name="args">The arguments associated with the dialog opening event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is invoked before the dialog is opened. It initializes the edit context for the workflow form and
    ///     validates it.
    /// </remarks>
    private void OpenDialog(BeforeOpenEventArgs args) => EditWorkflowForm.EditContext?.Validate();

    /// <summary>
    ///     Asynchronously saves the workflow changes made in the dialog.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <remarks>
    ///     This method calls the general save method, passing in the edit context, spinner, dialog footer, dialog, and save
    ///     event callback.
    ///     It is typically triggered when the user confirms the save operation in the dialog.
    /// </remarks>
    private async Task SaveWorkflow(EditContext editContext)
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

    /// <summary>
    ///     Asynchronously displays the workflow dialog.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is used to programmatically display the dialog for editing a workflow.
    ///     It invokes the ShowAsync method of the SfDialog instance associated with the dialog.
    /// </remarks>
    internal Task ShowDialog() => Dialog.ShowAsync();
}