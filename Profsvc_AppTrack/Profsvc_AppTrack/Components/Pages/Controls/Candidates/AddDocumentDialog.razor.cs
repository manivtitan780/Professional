﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           AddDocumentDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-04-2023 15:56
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a dialog for adding a document to a candidate.
/// </summary>
/// <remarks>
///     This class is a Blazor component that provides a user interface for uploading a document to a candidate.
///     It includes parameters for handling various events such as upload completion, upload initiation, cancellation, and
///     saving.
///     It also provides methods to show the dialog and to enable or disable buttons within the dialog.
/// </remarks>
public partial class AddDocumentDialog
{
	private EditContext _editContext;

	/// <summary>
	///     Gets or sets the event callback that is invoked after a document upload is completed.
	/// </summary>
	/// <value>
	///     The event callback for the action completed event.
	/// </value>
	/// <remarks>
	///     This event callback is triggered when the upload of a document to the candidate is completed.
	///     It uses the Syncfusion Blazor's ActionCompleteEventArgs to provide details about the completed action.
	/// </remarks>
	[Parameter]
	public EventCallback<ActionCompleteEventArgs> AfterUpload
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is invoked before a document upload starts.
	/// </summary>
	/// <value>
	///     The event callback for the before upload event.
	/// </value>
	/// <remarks>
	///     This event callback is triggered before the upload of a document to the candidate starts.
	///     It uses the Syncfusion Blazor's BeforeUploadEventArgs to provide details about the upload that is about to start.
	/// </remarks>
	[Parameter]
	public EventCallback<BeforeUploadEventArgs> BeforeUpload
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is invoked when the document upload is cancelled.
	/// </summary>
	/// <value>
	///     The event callback for the cancel event.
	/// </value>
	/// <remarks>
	///     This event callback is triggered when the user cancels the upload of a document to the candidate.
	///     It uses the MouseEventArgs to provide details about the mouse event that triggered the cancellation.
	/// </remarks>
	[Parameter]
	public EventCallback<MouseEventArgs> Cancel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Syncfusion Blazor Dialog component used in the AddDocumentDialog.
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
	///     Gets or sets the list of document types available for upload.
	/// </summary>
	/// <value>
	///     The list of document types represented as IntValues.
	/// </value>
	/// <remarks>
	///     This list is used to populate the dropdown control for selecting the document type during the upload process.
	///     Each IntValues item represents a document type with a unique identifier.
	/// </remarks>
	[Parameter]
	public List<IntValues> DocumentTypes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the EditForm instance for the document upload dialog.
	/// </summary>
	/// <value>
	///     The EditForm instance.
	/// </value>
	/// <remarks>
	///     This EditForm instance is used to manage the form state and validation for the document upload dialog.
	///     It is referenced in the OpenDialog method to validate the form before the dialog is opened.
	/// </remarks>
	private EditForm EditDocumentForm
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogFooter instance used in the AddDocumentDialog.
	/// </summary>
	/// <value>
	///     The DialogFooter instance.
	/// </value>
	/// <remarks>
	///     This property is used to manage the Cancel and Save buttons in the AddDocumentDialog.
	///     It provides methods to enable or disable these buttons.
	/// </remarks>
	private DialogFooter DialogFooter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the model representing a candidate document for the dialog.
	/// </summary>
	/// <value>
	///     The model is of type <see cref="CandidateDocument" /> which represents the document details of a candidate.
	/// </value>
	/// <remarks>
	///     This model is used to bind the form fields in the dialog, enabling the user to input or edit the document details
	///     of a candidate.
	/// </remarks>
	[Parameter]
	public CandidateDocument Model
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
	///     Gets or sets the event callback that is invoked when the document details are saved.
	/// </summary>
	/// <value>
	///     The event callback for the save event.
	/// </value>
	/// <remarks>
	///     This event callback is triggered when the user saves the document details in the dialog.
	///     It uses the EditContext to provide the context of the form that is being edited.
	/// </remarks>
	[Parameter]
	public EventCallback<EditContext> Save
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the document uploads should be sequential.
	/// </summary>
	/// <value>
	///     <c>true</c> if the document uploads should be sequential; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, the documents will be uploaded one after the other in the order they were
	///     added.
	///     When this property is set to <c>false</c>, the documents will be uploaded concurrently.
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
	///     Asynchronously executes the cancellation process for the document dialog.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the cancel action.</param>
	/// <remarks>
	///     This method calls the general cancellation method, which hides the spinner and dialog, and enables the dialog
	///     buttons.
	///     It is typically invoked when the user triggers the cancel action in the workflow dialog.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task CancelDocumentDialog(MouseEventArgs args)
	{
		await General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);
	}

	/// <summary>
	///     Disables the buttons in the AddDocumentDialog.
	/// </summary>
	/// <remarks>
	///     This method is used to disable the Cancel and Save buttons in the AddDocumentDialog,
	///     preventing any further user interaction with these buttons until they are re-enabled.
	///     It is typically called before a document upload starts to prevent any unintended user actions during the upload
	///     process.
	/// </remarks>
	internal void DisableButtons()
	{
		DialogFooter.DisableButtons();
	}

	/// <summary>
	///     Enables the buttons in the AddDocumentDialog.
	/// </summary>
	/// <remarks>
	///     This method is used to enable the Cancel and Save buttons in the AddDocumentDialog,
	///     allowing any further user interaction with these buttons until they are disabled.
	///     It is typically called after a document upload ends during the upload process.
	/// </remarks>
	internal void EnableButtons()
	{
		DialogFooter.EnableButtons();
	}

	/// <summary>
	///     Handles the event when a file is removed from the upload queue.
	/// </summary>
	/// <param name="arg">
	///     The arguments associated with the file removal event.
	/// </param>
	/// <remarks>
	///     This method is invoked when a file is removed from the upload queue in the document upload dialog.
	///     It clears the list of files in the model and notifies the edit context about the change.
	/// </remarks>
	private async Task OnFileRemoved(RemovingEventArgs arg)
	{
		await Task.Yield();
		Model.Files = null;
		_editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.Files)));
	}

	/// <summary>
	///     Handles the file selection event in the document upload process.
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
			Model.Files = new()
						  {
							  file.FilesData[0].Name
						  };
		}
		else
		{
			Model.Files.Clear();
			Model.Files.Add(file.FilesData[0].Name);
		}

		_editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.Files)));
	}

	/// <summary>
	///     Asynchronously prepares the document dialog for opening.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method is invoked before the dialog is opened. It initializes the edit context for the workflow form and
	///     validates it.
	/// </remarks>
	private void OpenDialog()
	{
		_editContext = EditDocumentForm.EditContext;
		_editContext?.Validate();
	}

	/// <summary>
	///     Asynchronously saves the document changes made in the dialog.
	/// </summary>
	/// <param name="editContext">The edit context associated with the save action.</param>
	/// <remarks>
	///     This method calls the general save method, passing in the edit context, spinner, dialog footer, dialog, and save
	///     event callback.
	///     It is typically triggered when the user confirms the save operation in the dialog.
	/// </remarks>
	private async Task SaveDocumentDialog(EditContext editContext)
	{
		await General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);
	}

	/// <summary>
	///     Asynchronously displays the AddDocumentDialog.
	/// </summary>
	/// <returns>
	///     A Task that represents the asynchronous operation.
	/// </returns>
	/// <remarks>
	///     This method is used to programmatically display the dialog for adding a document to a candidate.
	///     It uses the ShowAsync method of the Syncfusion Blazor Dialog component.
	/// </remarks>
	public async Task ShowDialog()
	{
		await Dialog.ShowAsync();
	}
}