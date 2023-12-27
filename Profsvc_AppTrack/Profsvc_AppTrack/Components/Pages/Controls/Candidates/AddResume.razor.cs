#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           AddResume.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-27-2023 21:0
// *****************************************/

#endregion

// ReSharper disable UnusedMember.Global
namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a dialog for adding a resume in the application.
/// </summary>
/// <remarks>
///     This class is a Blazor component, which is used to manage the process of uploading a resume.
///     It provides several parameters for handling events such as uploading, saving, and canceling.
///     It also provides methods to show the dialog, and enable or disable buttons within the dialog.
/// </remarks>
public partial class AddResume
{
    private EditContext _editContext;

    /// <summary>
    ///     Gets or sets the event callback that is invoked after a resume upload is completed.
    /// </summary>
    /// <value>
    ///     The event callback for the action completed event.
    /// </value>
    /// <remarks>
    ///     This event callback is triggered when the upload of a resume for the candidate is completed.
    ///     It uses the Syncfusion Blazor's ActionCompleteEventArgs to provide details about the completed action.
    /// </remarks>
    [Parameter]
    public EventCallback<ActionCompleteEventArgs> AfterUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked before a resume upload starts.
    /// </summary>
    /// <value>
    ///     The event callback for the before upload event.
    /// </value>
    /// <remarks>
    ///     This event callback is triggered before the upload of a resume for the candidate starts.
    ///     It uses the Syncfusion Blazor's BeforeUploadEventArgs to provide details about the upload that is about to start.
    /// </remarks>
    [Parameter]
    public EventCallback<BeforeUploadEventArgs> BeforeUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the resume upload is cancelled.
    /// </summary>
    /// <value>
    ///     The event callback for the cancel event.
    /// </value>
    /// <remarks>
    ///     This event callback is triggered when the user cancels the upload of a resume for the candidate.
    ///     It uses the MouseEventArgs to provide details about the mouse event that triggered the cancellation.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the Syncfusion Blazor Dialog component used in the AddResume.
    /// </summary>
    /// <value>
    ///     The Syncfusion Blazor Dialog component.
    /// </value>
    /// <remarks>
    ///     This property is used to control the visibility and behavior of the dialog.
    ///     It provides methods to show or hide the dialog programmatically.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter instance used in the AddResume.
    /// </summary>
    /// <value>
    ///     The DialogFooter instance.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the Cancel and Save buttons in the AddResume.
    ///     It provides methods to enable or disable these buttons.
    /// </remarks>
    private DialogFooter DialogFooter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the EditForm instance for the resume upload dialog.
    /// </summary>
    /// <value>
    ///     The EditForm instance.
    /// </value>
    /// <remarks>
    ///     This EditForm instance is used to manage the form state and validation for the resume upload dialog.
    ///     It is referenced in the OpenDialog method to validate the form before the dialog is opened.
    /// </remarks>
    private EditForm EditResumeForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model representing a candidate resume for the dialog.
    /// </summary>
    /// <value>
    ///     The model is of type <see cref="UploadResume" /> which represents the resume details of a candidate.
    /// </value>
    /// <remarks>
    ///     This model is used to bind the form fields in the dialog, enabling the user to input or edit the resume details
    ///     of a candidate.
    /// </remarks>
    [Parameter]
    public UploadResume Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when a file is selected for upload.
    /// </summary>
    /// <value>
    ///     The event callback for the file upload event.
    /// </value>
    /// <remarks>
    ///     This event callback is triggered when the user selects a file to upload in the dialog.
    ///     It uses the Syncfusion Blazor's UploadChangeEventArgs to provide details about the selected file.
    /// </remarks>
    [Parameter]
    public EventCallback<UploadChangeEventArgs> OnFileUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the type of the resume.
    /// </summary>
    /// <value>
    ///     The type of the resume as a string.
    /// </value>
    /// <remarks>
    ///     This property is used to specify the type of the resume that is being uploaded.
    ///     The type of the resume can be used to categorize the resumes or to provide specific handling based on the type.
    /// </remarks>
    [Parameter]
    public string ResumeType
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the resume details are saved.
    /// </summary>
    /// <value>
    ///     The event callback for the save event.
    /// </value>
    /// <remarks>
    ///     This event callback is triggered when the user saves the resume details in the dialog.
    ///     It uses the EditContext to provide the context of the form that is being edited.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the resume uploads should be sequential.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the resume uploads should be sequential; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     When this property is set to <c>true</c>, the resume will be uploaded one after the other in the order they were
    ///     added.
    ///     When this property is set to <c>false</c>, the resume will be uploaded concurrently.
    /// </remarks>
    [Parameter]
    public bool SequentialUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the Syncfusion Blazor Spinner component used in the dialog.
    /// </summary>
    /// <value>
    ///     The instance of the Syncfusion Blazor Spinner component.
    /// </value>
    /// <remarks>
    ///     The Spinner component is used to indicate the processing state when a user interacts with the dialog, such as when
    ///     uploading a document or saving changes.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously executes the cancellation process for the resume dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <remarks>
    ///     This method calls the general cancellation method, which hides the spinner and dialog, and enables the dialog
    ///     buttons.
    ///     It is typically invoked when the user triggers the cancel action in the workflow dialog.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CancelResumeDialog(MouseEventArgs args) => await General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);

    /// <summary>
    ///     Disables the buttons in the AddResume.
    /// </summary>
    /// <remarks>
    ///     This method is used to disable the Cancel and Save buttons in the AddResume,
    ///     preventing any further user interaction with these buttons until they are re-enabled.
    ///     It is typically called before a resume upload starts to prevent any unintended user actions during the upload
    ///     process.
    /// </remarks>
    internal void DisableButtons() => DialogFooter.DisableButtons();

    /// <summary>
    ///     Enables the buttons in the AddResume.
    /// </summary>
    /// <remarks>
    ///     This method is used to enable the Cancel and Save buttons in the AddResume,
    ///     allowing any further user interaction with these buttons until they are disabled.
    ///     It is typically called after a resume upload ends during the upload process.
    /// </remarks>
    internal void EnableButtons() => DialogFooter.EnableButtons();

    /// <summary>
    ///     Handles the event when a file is removed from the upload queue.
    /// </summary>
    /// <param name="arg">
    ///     The arguments associated with the file removal event.
    /// </param>
    /// <remarks>
    ///     This method is invoked when a file is removed from the upload queue in the resume upload dialog.
    ///     It clears the list of files in the model and notifies the edit context about the change.
    /// </remarks>
    private async Task OnFileRemoved(RemovingEventArgs arg)
    {
        await Task.Yield();
        Model.Files = null;
        _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.Files)));
    }

    /// <summary>
    ///     Handles the file selection event in the resume upload process.
    /// </summary>
    /// <param name="file">
    ///     The selected file event arguments containing the details of the selected file.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is invoked when a file is selected in the document upload dialog.
    ///     It updates the model's file list with the name of the selected file and notifies the edit context of the change.
    /// </remarks>
    private async Task OnFileSelected(SelectedEventArgs file)
    {
        await Task.Yield();
        if (Model.Files == null)
        {
            Model.Files = [file.FilesData[0].Name];
        }
        else
        {
            Model.Files.Clear();
            Model.Files.Add(file.FilesData[0].Name);
        }

        _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.Files)));
    }

    /// <summary>
    ///     Asynchronously prepares the resume dialog for opening.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is invoked before the dialog is opened. It initializes the edit context for the workflow form and
    ///     validates it.
    /// </remarks>
    private void OpenDialog()
    {
        _editContext = EditResumeForm.EditContext;
        _editContext?.Validate();
    }

    /// <summary>
    ///     Asynchronously saves the resume changes made in the dialog.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <remarks>
    ///     This method calls the general save method, passing in the edit context, spinner, dialog footer, dialog, and save
    ///     event callback.
    ///     It is typically triggered when the user confirms the save operation in the dialog.
    /// </remarks>
    private async Task SaveResumeDialog(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);

    /// <summary>
    ///     Asynchronously displays the AddResume.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is used to programmatically display the dialog for adding a resume for a candidate.
    ///     It uses the ShowAsync method of the Syncfusion Blazor Dialog component.
    /// </remarks>
    public async Task ShowDialog() => await Dialog.ShowAsync();
}