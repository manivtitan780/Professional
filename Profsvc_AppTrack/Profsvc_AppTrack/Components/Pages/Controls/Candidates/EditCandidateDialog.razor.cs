﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           EditCandidateDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-27-2023 21:19
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a dialog for editing candidate details in the application.
/// </summary>
/// <remarks>
///     This class provides properties for various parameters required for editing candidate details, such as
///     communication, eligibility, experience, job options, tax terms, and more.
///     It also provides events for handling actions like saving changes or cancelling the edit operation.
///     The dialog is displayed when the `ShowDialog` method is called.
/// </remarks>
public partial class EditCandidateDialog
{
	/// <summary>
	///     A list of toolbar items for the rich text editor in the candidate edit dialog.
	/// </summary>
	/// <remarks>
	///     This list includes commands for text formatting such as bold, italic, underline, strikethrough, lower case, upper
	///     case, superscript, subscript, and clear format. It also includes commands for undo and redo actions.
	/// </remarks>
	private readonly List<ToolbarItemModel> _tools =
	[
		new() {Command = ToolbarCommand.Bold},
		new() {Command = ToolbarCommand.Italic},
		new() {Command = ToolbarCommand.Underline},
		new() {Command = ToolbarCommand.StrikeThrough},
		new() {Command = ToolbarCommand.LowerCase},
		new() {Command = ToolbarCommand.UpperCase},
		new() {Command = ToolbarCommand.SuperScript},
		new() {Command = ToolbarCommand.SubScript},
		new() {Command = ToolbarCommand.Separator},
		new() {Command = ToolbarCommand.ClearFormat},
		new() {Command = ToolbarCommand.Separator},
		new() {Command = ToolbarCommand.Undo},
		new() {Command = ToolbarCommand.Redo}
	];

	/// <summary>
	///     Gets or sets the event to be triggered when the cancel action is performed in the dialog.
	/// </summary>
	/// <remarks>
	///     This event is triggered when the user clicks the cancel button in the dialog. The associated method will hide the
	///     dialog, hide the spinner, and enable the dialog buttons.
	/// </remarks>
	[Parameter]
	public EventCallback<MouseEventArgs> Cancel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the collection of communication details for the candidate.
	/// </summary>
	/// <value>
	///     The collection of `KeyValues` that represents the communication details for the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to populate the communication details in the edit dialog. Each `KeyValues` object in the
	///     collection represents a specific communication detail.
	/// </remarks>
	[Parameter]
	public IEnumerable<KeyValues> Communication
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the Syncfusion Blazor Dialog component used in the EditCandidateDialog.
	/// </summary>
	/// <remarks>
	///     This instance of the SfDialog component is used to display the dialog for editing candidate details.
	///     It is shown when the `ShowDialog` method is called and hidden when the `CancelDialog` or `SaveCandidateDialog`
	///     methods are called.
	/// </remarks>
	private SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the form used for editing candidate details.
	/// </summary>
	/// <value>
	///     The form used for editing candidate details.
	/// </value>
	/// <remarks>
	///     This property is used to manage the data and actions related to the form for editing candidate details.
	/// </remarks>
	private EditForm EditCandidateForm
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the eligibility of the candidate.
	/// </summary>
	/// <value>
	///     The eligibility of the candidate, represented as a collection of <see cref="ProfSvc_Classes.IntValues" />.
	/// </value>
	[Parameter]
	public IEnumerable<IntValues> Eligibility
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the collection of experience values for the candidate.
	/// </summary>
	/// <value>
	///     The collection of experience values represented as instances of the IntValues class.
	/// </value>
	/// <remarks>
	///     This property is used to populate the experience field in the edit candidate dialog.
	/// </remarks>
	[Parameter]
	public IEnumerable<IntValues> Experience
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogFooter instance associated with the EditCandidateDialog.
	/// </summary>
	/// <remarks>
	///     The DialogFooter instance is used to manage the footer section of the EditCandidateDialog,
	///     which includes the Cancel and Save buttons. The FooterDialog property is bound to the DialogFooter component in the
	///     Razor markup.
	/// </remarks>
	private DialogFooter FooterDialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the job options for the candidate.
	/// </summary>
	/// <value>
	///     The job options are represented as a collection of `KeyValues` instances.
	///     Each `KeyValues` instance represents a single job option.
	/// </value>
	[Parameter]
	public IEnumerable<KeyValues> JobOptions
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the model representing the details of a candidate.
	/// </summary>
	/// <value>
	///     The model is of type <see cref="CandidateDetails" />, which holds various details about a candidate.
	/// </value>
	/// <remarks>
	///     This model is used to bind the data in the dialog for editing candidate details.
	///     It is populated when the dialog is opened and updated when changes are saved.
	/// </remarks>
	[Parameter]
	public CandidateDetails Model
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event to be triggered when the save action is performed in the dialog.
	/// </summary>
	/// <remarks>
	///     This event is triggered when the user clicks the save button in the dialog. The associated method will validate and
	///     save the changes made to the candidate details, hide the dialog, hide the spinner, and enable the dialog buttons.
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
	/// <value>
	///     The instance of the Syncfusion spinner control.
	/// </value>
	/// <remarks>
	///     The spinner is used to indicate a loading state while the dialog is performing operations like saving changes or
	///     cancelling the edit operation.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when the State ID changes.
	/// </summary>
	/// <value>
	///     The event callback of type <see cref="EventCallback{ChangeEventArgs}" />.
	/// </value>
	/// <remarks>
	///     This event callback is used to handle the changes in the State ID during the candidate editing process.
	/// </remarks>
	[Parameter]
	public EventCallback<ChangeEventArgs<int, IntValues>> StateIDChanged
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the list of states associated with the candidate.
	/// </summary>
	/// <value>
	///     The list of states is represented as a collection of `IntValues` instances.
	/// </value>
	/// <remarks>
	///     This property is used to populate the state selection in the edit dialog. Each `IntValues` instance represents a
	///     state.
	/// </remarks>
	[Parameter]
	public List<IntValues> States
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the collection of tax terms associated with the candidate.
	/// </summary>
	/// <value>
	///     The collection of tax terms, represented as instances of <see cref="KeyValues" />.
	/// </value>
	/// <remarks>
	///     This property is used to populate the tax terms section in the candidate editing dialog.
	/// </remarks>
	[Parameter]
	public IEnumerable<KeyValues> TaxTerms
	{
		get;
		set;
	}

	/// <summary>
	///     Cancels the candidate editing operation and closes the dialog.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the cancel action.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method is triggered when the user clicks on the cancel button in the dialog.
	///     It calls the `General.CallCancelMethod` to handle the cancellation and closing of the dialog.
	/// </remarks>
	private async Task CancelDialog(MouseEventArgs args) => await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);

	/// <summary>
	///     Opens the dialog for editing candidate details.
	/// </summary>
	/// <remarks>
	///     This method is called when the dialog is opened. It validates the form context of the dialog.
	/// </remarks>
	private void DialogOpen() => EditCandidateForm.EditContext?.Validate();

	/// <summary>
	///     Asynchronously saves the changes made in the candidate dialog.
	/// </summary>
	/// <param name="editContext">
	///     The edit context associated with the save action. This context contains the candidate details that are being
	///     edited.
	/// </param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method calls the `General.CallSaveMethod` utility method, passing in the edit context, spinner, dialog footer,
	///     dialog, and save method.
	///     The spinner is shown and the dialog buttons are disabled when the save operation starts, and hidden and enabled
	///     respectively when the save operation completes.
	/// </remarks>
	private async Task SaveCandidateDialog(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Save);

	/// <summary>
	///     Displays the dialog for editing candidate details.
	/// </summary>
	/// <remarks>
	///     This method is responsible for displaying the dialog that allows users to edit candidate details.
	///     It does this by calling the `ShowAsync` method on the `Dialog` instance of the `SfDialog` component.
	/// </remarks>
	public async Task ShowDialog() => await Dialog.ShowAsync();
}