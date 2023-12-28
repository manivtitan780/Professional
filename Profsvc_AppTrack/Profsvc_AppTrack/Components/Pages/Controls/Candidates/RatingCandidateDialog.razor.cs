#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           RatingCandidateDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-28-2023 16:32
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a dialog for rating a candidate in the application.
/// </summary>
/// <remarks>
///     The `RatingCandidateDialog` class is a Blazor component that provides a dialog interface for rating a candidate.
///     It includes parameters for handling events such as `CancelRating` and `Save`, and properties for managing the
///     rating grid, model, row height, and spinner.
///     The `ShowDialog` method is used to display the dialog, while the `CancelRatingDialog` and `SaveRatingDialog`
///     methods handle the cancellation and saving of the rating respectively.
/// </remarks>
public partial class RatingCandidateDialog
{
	/// <summary>
	///     Gets or sets the event callback that is invoked when the cancel action is triggered in the dialog.
	/// </summary>
	/// <remarks>
	///     This event callback is used to handle the cancellation of the editing operation in the dialog.
	///     It is invoked when the user clicks on the cancel button in the dialog.
	/// </remarks>
	[Parameter]
	public EventCallback<MouseEventArgs> Cancel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the Syncfusion Blazor Dialog component used in the RatingCandidateDialog.
	/// </summary>
	/// <remarks>
	///     This dialog is used to display and edit the experience details of a candidate.
	///     It is shown or hidden using the ShowDialog and CallCancelMethod methods respectively.
	/// </remarks>
	private SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the form used for editing a candidate's rating.
	/// </summary>
	/// <remarks>
	///     This form is used within the RatingCandidateDialog to capture the details of a candidate's rating.
	///     It includes fields for the employer, description, location, title, and start and end dates of the experience.
	/// </remarks>
	private EditForm EditRatingForm
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the footer dialog of the RatingCandidateDialog.
	/// </summary>
	/// <remarks>
	///     This property represents the footer dialog of the RatingCandidateDialog, which contains the Save and Cancel
	///     buttons.
	///     The footer dialog is a part of the DialogFooter class, which manages the Cancel and Save buttons in the dialog.
	/// </remarks>
	private DialogFooter FooterDialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the rating that is being edited in the dialog.
	/// </summary>
	/// <value>
	///     The rating.
	/// </value>
	/// <remarks>
	///     This property is bound to the form fields in the dialog, and changes to this property
	///     are reflected in the form and vice versa.
	/// </remarks>
	[Parameter]
	public CandidateRatingMPC Model
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the list of Rating records displayed in the grid.
	/// </summary>
	/// <value>
	///     The list of Rating records.
	/// </value>
	/// <remarks>
	///     This property is bound to the data source of the grid in the dialog. Changes to this property
	///     are reflected in the grid and vice versa. Each record in the list represents a row in the grid.
	/// </remarks>
	[Parameter]
	public List<CandidateRating> RatingGrid
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the height of the rows in the Rating grid.
	/// </summary>
	/// <value>
	///     The height of the rows in the Rating grid.
	/// </value>
	/// <remarks>
	///     This property is used to control the height of the rows in the Rating grid displayed in the dialog.
	///     The default value is 38.
	/// </remarks>
	[Parameter]
	public int RowHeight
	{
		get;
		set;
	} = 38;

	/// <summary>
	///     Gets or sets the event callback that is invoked when the save action is triggered in the dialog.
	/// </summary>
	/// <remarks>
	///     This event callback is used to handle the saving of the editing operation in the dialog.
	///     It is invoked when the user clicks on the save button in the dialog.
	/// </remarks>
	[Parameter]
	public EventCallback<EditContext> Save
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the Syncfusion spinner control used in the dialog.
	/// </summary>
	/// <remarks>
	///     This spinner control is displayed when the dialog is performing an operation such as saving or canceling.
	///     The visibility of the spinner is controlled programmatically based on the state of the operation.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Asynchronously cancels the operation of editing a candidate's rating.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the cancel action.</param>
	/// <remarks>
	///     This method is invoked when the user decides to cancel the operation of editing a candidate's rating.
	///     It calls the 'CallCancelMethod' of the 'General' class, passing the necessary parameters to hide the dialog
	///     and enable the dialog buttons.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task CancelRatingDialog(MouseEventArgs args) => await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);

	/// <summary>
	///     Validates the form context when the dialog is opened.
	/// </summary>
	/// <remarks>
	///     This method is invoked when the dialog is opened. It validates the form context
	///     associated with the editing of a candidate's rating. If the form context is not valid,
	///     the form will not be submitted.
	/// </remarks>
	private void OpenDialog(BeforeOpenEventArgs arg) => EditRatingForm.EditContext?.Validate();

	/// <summary>
	///     Asynchronously saves the changes made in the RatingCandidateDialog.
	/// </summary>
	/// <param name="editContext">The edit context associated with the save action.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method invokes the CallSaveMethod from the General class, passing in the edit context, spinner, footer dialog,
	///     dialog, and save event callback.
	///     It is responsible for executing the save operation when the user confirms the changes in the RatingCandidateDialog.
	/// </remarks>
	private async Task SaveRatingDialog(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Save);

	/// <summary>
	///     Asynchronously shows the dialog for editing a candidate's rating.
	/// </summary>
	/// <returns>
	///     A <see cref="Task" /> that represents the asynchronous operation.
	/// </returns>
	/// <remarks>
	///     This method is used to display the dialog for editing the experience details of a candidate.
	///     It is invoked when the system is prepared for the editing operation.
	/// </remarks>
	public async Task ShowDialog() => await Dialog.ShowAsync();
}