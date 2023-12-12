#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           LeadNotesPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-30-2023 21:07
// Last Updated On:     09-30-2023 16:32
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Pages.Controls.Common;

namespace Profsvc_AppTrack.Components.Pages.Controls.Leads;

/// <summary>
/// The LeadNotesPanel class is a Razor component that provides functionality for displaying and interacting with lead notes.
/// </summary>
/// <remarks>
/// This component includes several properties and methods that allow for the management of lead notes, including the ability to delete and edit notes.
/// The LeadNotesPanel is used in the Leads page, where it is instantiated and managed through the `PanelNotes` property of the `Leads` class.
/// The LeadNotesPanel includes an instance of the `SfGrid` control for displaying notes in a tabular format, and properties for managing the selected note, the user's edit rights, and the height of each row in the grid.
/// </remarks>
public partial class LeadNotesPanel
{
    private int _selectedID;

 /// <summary>
/// Gets or sets an event callback that is invoked when a note is deleted. The callback receives the ID of the deleted note as a parameter.
/// This property is used to handle the note deletion functionality in the LeadNotesPanel.
/// </summary>
    [Parameter]
   public EventCallback<int> DeleteNotes
    {
        get;
        set;
    }

  /// <summary>
/// Gets or sets the ConfirmDialog component used in the LeadNotesPanel.
/// </summary>
/// <remarks>
/// The ConfirmDialog component is used to display a confirmation dialog to the user for various actions such as deletion of notes.
/// The dialog's appearance and behavior can be customized according to the requirements.
/// </remarks>
  private ConfirmDialog DialogConfirm
    {
        get;
        set;
    }

 /// <summary>
/// Gets or sets an event callback that is invoked when a note is edited. The callback receives the ID of the edited note as a parameter.
/// This property is used to handle the note editing functionality in the LeadNotesPanel.
/// </summary>
   [Parameter]
    public EventCallback<int> EditNotes
    {
        get;
        set;
    }

 /// <summary>
/// Gets or sets a value indicating whether the user has the rights to edit the notes.
/// This property is used to control the visibility of the edit functionality in the LeadNotesPanel.
/// If set to true, the user can edit the notes. If set to false, the edit functionality is disabled.
/// By default, this property is set to true.
/// </summary>
   [Parameter]
    public bool EditRights
    {
        get;
        set;
    } = true;

  /// <summary>
/// Gets or sets the SfGrid control for displaying notes related to a lead. Each note is represented by an instance of the <see cref="ProfSvc_Classes.CandidateNotes"/> class.
/// The grid is used to present the notes in a tabular format, allowing users to view, edit, and delete notes.
/// The grid's data source is set to the `Model` property, which is a list of `CandidateNotes`.
/// The grid's row selection feature is used in conjunction with the `DeleteNotesMethod` and `EditNotesDialog` methods to select a note for deletion or editing.
/// </summary>
  public SfGrid<CandidateNotes> GridNotes
    {
        get;
        set;
    }

 /// <summary>
/// Gets or sets the list of notes associated with a lead. Each note is represented by an instance of the <see cref="ProfSvc_Classes.CandidateNotes"/> class.
/// This list is used as the data source for the notes grid in the LeadNotesPanel, as seen in the LeadNotesPanel.razor file.
/// Each note in the list contains information such as the note's content, the user who last updated the note, and the date of the last update.
/// </summary>
   [Parameter]
    public List<CandidateNotes> Model
    {
        get;
        set;
    }

 /// <summary>
/// Gets or sets the height of each row in the notes grid of the LeadNotesPanel.
/// This property is used to control the visual presentation of the notes grid.
/// It is set to 38 by default, which means each row in the grid will be 38 pixels tall.
/// </summary>
   [Parameter]
    public int RowHeight
    {
        get;
        set;
    } = 38;

 /// <summary>
/// Gets or sets the selected row in the notes grid of the LeadNotesPanel.
/// This property is of type <see cref="ProfSvc_Classes.CandidateNotes"/>, which encapsulates the notes related to a candidate.
/// It is updated whenever a row is selected in the grid, as seen in the `LeadNotesPanel.RowSelected()` method.
/// It is also used in the `Leads.EditNotes()` method to obtain the selected note for editing.
/// </summary>
   public CandidateNotes SelectedRow
    {
        get;
        set;
    }

/// <summary>
/// Gets or sets the username of the user who has updated the notes. This property is used to check if the current user is the one who updated the note, and if so, display the edit and delete buttons for that note.
/// </summary>
    [Parameter]
    public string User
    {
        get;
        set;
    } = "";

 /// <summary>
/// Deletes a note with the specified ID.
/// </summary>
/// <param name="id">The ID of the note to delete.</param>
/// <returns>A task that represents the asynchronous delete operation.</returns>
/// <remarks>
/// This method first sets the selected ID to the provided ID. Then, it retrieves the index of the row in the grid that corresponds to the note with the provided ID.
/// After selecting the row in the grid, it shows a confirmation dialog to the user. If the user confirms the deletion, the note is deleted.
/// </remarks>
   private async Task DeleteNotesMethod(int id)
    {
        _selectedID = id;
        int _index = await GridNotes.GetRowIndexByPrimaryKeyAsync(id);
        await GridNotes.SelectRowAsync(_index);
        await DialogConfirm.ShowDialog();
    }

 /// <summary>
/// Asynchronously opens the dialog for editing notes.
/// </summary>
/// <param name="id">The ID of the note to be edited.</param>
/// <returns>A Task representing the asynchronous operation.</returns>
   private async Task EditNotesDialog(int id)
    {
        int _index = await GridNotes.GetRowIndexByPrimaryKeyAsync(id);
        await GridNotes.SelectRowAsync(_index);
        await EditNotes.InvokeAsync(id);
    }

  /// <summary>
/// Handles the row selection event of the grid.
/// </summary>
/// <param name="note">The selected row data of type <see cref="CandidateNotes"/>.</param>
  private void RowSelected(RowSelectEventArgs<CandidateNotes> note)
    {
        if (note != null)
        {
            SelectedRow = note.Data;
        }
    }
}