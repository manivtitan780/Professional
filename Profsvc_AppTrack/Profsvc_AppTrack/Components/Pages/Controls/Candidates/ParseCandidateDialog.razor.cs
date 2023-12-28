#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           ParseCandidateDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-28-2023 16:31
// *****************************************/

#endregion

// ReSharper disable UnusedMember.Global
namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     The ParseCandidateDialog class represents a dialog component for parsing new candidate information.
/// </summary>
/// <remarks>
///     This dialog contains an uploader component that allows the user to upload a resume file for parsing.
///     The dialog provides two events: CancelCandidate and OnFileUpload.
///     The CancelCandidate event is triggered when the dialog is closed,
///     and the OnFileUpload event is triggered when a new file is uploaded.
///     The dialog also provides methods to show and hide the dialog programmatically.
/// </remarks>
public partial class ParseCandidateDialog
{
	//private bool _value;

	/// <summary>
	///     Gets or sets the event callback that is triggered when the ParseCandidateDialog is closed.
	/// </summary>
	/// <value>
	///     The event callback for the CancelCandidate event.
	/// </value>
	/// <remarks>
	///     This event callback is used to handle any actions that need to be performed when the dialog is closed, such as
	///     cleaning up resources or updating the UI.
	/// </remarks>
	[Parameter]
	public EventCallback<CloseEventArgs> CancelCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfDialog component used for the ParseCandidateDialog.
	/// </summary>
	/// <value>
	///     The SfDialog component.
	/// </value>
	/// <remarks>
	///     This component is used to display the dialog for parsing new candidate information.
	///     It contains an uploader for resume files and triggers events for file upload and dialog closure.
	/// </remarks>
	private SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when a new file is uploaded.
	/// </summary>
	/// <value>
	///     The event callback for the OnFileUpload event.
	/// </value>
	/// <remarks>
	///     This event callback is used to handle any actions that need to be performed when a new file is uploaded, such as
	///     parsing the uploaded file or updating the UI.
	/// </remarks>
	[Parameter]
	public EventCallback<UploadChangeEventArgs> OnFileUpload
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfSpinner component used in the ParseCandidateDialog.
	/// </summary>
	/// <value>
	///     The SfSpinner component.
	/// </value>
	/// <remarks>
	///     This component is used to provide a visual indication when a file is being uploaded and processed.
	///     It is shown when the file upload begins and hidden when the upload is completed.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Handles the file upload process asynchronously.
	/// </summary>
	/// <param name="args">The arguments associated with the file upload event.</param>
	/// <remarks>
	///     This method is triggered when a new file is uploaded. It shows a spinner during the upload process,
	///     invokes the OnFileUpload event with the provided arguments, hides the spinner after the upload is completed,
	///     and then hides the dialog.
	/// </remarks>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private async Task FileUpload(UploadChangeEventArgs args)
	{
		await Spinner.ShowAsync();
		await OnFileUpload.InvokeAsync(args);
		await Spinner.HideAsync();
		await Dialog.HideAsync();
	}

	/// <summary>
	///     Hides the ParseCandidateDialog asynchronously.
	/// </summary>
	/// <remarks>
	///     This method is used to programmatically hide the dialog for parsing new candidate information.
	///     It is typically called when a user action, such as a button click, triggers the need to hide the dialog.
	/// </remarks>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	public async Task HideDialog() => await Dialog.HideAsync();

	/// <summary>
	///     Displays the ParseCandidateDialog asynchronously.
	/// </summary>
	/// <remarks>
	///     This method is used to programmatically display the dialog for parsing new candidate information.
	///     It is typically called when a user action, such as a button click, triggers the need to display the dialog.
	/// </remarks>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	public async Task ShowDialog() => await Dialog.ShowAsync();
}