#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           NotesPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-28-2023 16:30
// *****************************************/

#endregion

#region Using

using Profsvc_AppTrack.Components.Pages.Controls.Common;

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     The NotesPanel class is a partial class that represents a panel for displaying and managing notes related to
///     candidates.
/// </summary>
/// <remarks>
///     The NotesPanel class includes properties for managing the display and interaction with notes, such as deletion and
///     editing of notes,
///     user rights for editing, and the display grid for the notes. It also includes methods for handling the deletion and
///     editing of notes,
///     and the selection of a row in the grid.
/// </remarks>
public partial class NotesPanel
{
	private int _selectedID;

	/// <summary>
	///     Gets or sets the event callback that is triggered when a note entry is to be deleted.
	/// </summary>
	/// <value>
	///     The event callback that takes an integer parameter representing the ID of the note entry to be deleted.
	/// </value>
	[Parameter]
	public EventCallback<int> DeleteNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ConfirmDialog component used to display a confirmation dialog to the user.
	/// </summary>
	/// <value>
	///     The ConfirmDialog component.
	/// </value>
	/// <remarks>
	///     The ConfirmDialog component is used to display a confirmation dialog to the user when they attempt to delete a
	///     note entry.
	///     The dialog provides the user with the option to confirm or cancel the deletion operation.
	/// </remarks>
	private ConfirmDialog DialogConfirm
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when a note entry is to be edited.
	/// </summary>
	/// <value>
	///     The event callback that takes an integer parameter representing the ID of the note entry to be edited.
	/// </value>
	[Parameter]
	public EventCallback<int> EditNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the current user has rights to edit the note entries.
	/// </summary>
	/// <value>
	///     true if the current user has edit rights; otherwise, false. The default is true.
	/// </value>
	/// <remarks>
	///     This property is a boolean flag that determines if the current user has the rights to edit the note entries.
	///     If EditRights is true, the user can edit the entries; if EditRights is false, the user cannot. The default value is
	///     true, meaning that, by default, users have the rights to edit the entries.
	/// </remarks>
	[Parameter]
	public bool EditRights
	{
		get;
		set;
	} = true;

	/// <summary>
	///     Gets or sets the Syncfusion Blazor Grid component that displays the list of a candidate's notes.
	/// </summary>
	/// <value>
	///     The Syncfusion Blazor Grid component that displays the candidate's notes.
	/// </value>
	/// <remarks>
	///     The GridExperience property is a Syncfusion Blazor Grid component that displays a list of a candidate's
	///     notes.
	///     Each row in the grid represents a note entry of the candidate.
	///     The grid provides functionalities such as sorting, filtering, and paging.
	///     The grid is bound to the Model property, which provides the data source for the grid.
	/// </remarks>
	private SfGrid<CandidateNotes> GridNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the current context is a requisition.
	/// </summary>
	/// <value>
	///     true if the current context is a requisition; otherwise, false. The default is false.
	/// </value>
	/// <remarks>
	///     This property is a boolean flag that determines if the current context is a requisition. If IsRequisition is true,
	///     the context is a requisition; if IsRequisition is false, the context is not a requisition. The default value is
	///     false, meaning that, by default, the context is not a requisition.
	/// </remarks>
	[Parameter]
	public bool IsRequisition
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the model for the note entries.
	/// </summary>
	/// <value>
	///     A list of CandidateNotes objects representing the note entries of a candidate.
	/// </value>
	/// <remarks>
	///     The Model property is a list of CandidateNotes objects that represent the note entries of a candidate.
	///     These entries are displayed in a grid in the NotesPanel.
	///     Each entry includes details about a particular note of the candidate.
	/// </remarks>
	[Parameter]
	public List<CandidateNotes> Model
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the height of each row in the grid displaying the note entries.
	/// </summary>
	/// <value>
	///     An integer representing the height of each row in pixels. The default is 38.
	/// </value>
	/// <remarks>
	///     The RowHeight property determines the height of each row in the grid that displays the note entries of a
	///     candidate.
	///     This property allows for customization of the grid's appearance and can be adjusted to accommodate the amount of
	///     information in each row.
	/// </remarks>
	[Parameter]
	public int RowHeight
	{
		get;
		set;
	} = 38;

	/// <summary>
	///     Gets or sets the currently selected row in the grid.
	/// </summary>
	/// <value>
	///     A CandidateNotes object representing the currently selected note entry in the grid.
	/// </value>
	/// <remarks>
	///     The SelectedRow property is used to keep track of the currently selected row in the grid that displays the
	///     note entries of a candidate.
	///     This property is updated whenever a row is selected in the grid, and it is used to perform operations on the
	///     selected note entry, such as editing or deleting the entry.
	/// </remarks>
	public CandidateNotes SelectedRow
	{
		get;
		private set;
	}

	/// <summary>
	///     Gets or sets the logged-in Username.
	/// </summary>
	/// <value>
	///     The currently logged-in Username.
	/// </value>
	[Parameter]
	public string UserName
	{
		get;
		set;
	}

	/// <summary>
	///     Asynchronously deletes the note detail of a candidate.
	/// </summary>
	/// <param name="id">The ID of the note detail to be deleted.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sets the selected ID to the provided ID, gets the index of the row in the grid corresponding to the ID,
	///     selects the row in the grid, and shows a confirmation dialog.
	/// </remarks>
	private async Task DeleteNotesMethod(int id)
	{
		_selectedID = id;
		int _index = await GridNotes.GetRowIndexByPrimaryKeyAsync(id);
		await GridNotes.SelectRowAsync(_index);
		await DialogConfirm.ShowDialog();
	}

	/// <summary>
	///     Asynchronously opens the dialog for editing the note details of a candidate.
	/// </summary>
	/// <param name="id">The ID of the note detail to be edited.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	/// <remarks>
	///     This method sets the selected ID to the provided ID, gets the index of the row in the grid corresponding to the
	///     provided ID, selects the row in the grid, and invokes the EditExperience event callback.
	/// </remarks>
	private async Task EditNotesDialog(int id)
	{
		int _index = await GridNotes.GetRowIndexByPrimaryKeyAsync(id);
		await GridNotes.SelectRowAsync(_index);
		await EditNotes.InvokeAsync(id);
	}

	/// <summary>
	///     Handles the row selection event in the note details grid.
	/// </summary>
	/// <param name="note">
	///     The event arguments containing the selected row data of type
	///     <see cref="CandidateNotes" />.
	/// </param>
	/// <remarks>
	///     This method is triggered when a row is selected in the note details grid.
	///     It sets the SelectedRow property to the data of the selected row.
	/// </remarks>
	private void RowSelected(RowSelectEventArgs<CandidateNotes> note)
	{
		if (note != null)
		{
			SelectedRow = note.Data;
		}
	}
}