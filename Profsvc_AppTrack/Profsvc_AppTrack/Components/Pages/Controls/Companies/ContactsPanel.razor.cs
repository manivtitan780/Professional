#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           ContactsPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-29-2023 16:25
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Companies;

/// <summary>
///     Represents a panel for managing company contacts.
/// </summary>
/// <remarks>
///     This class provides functionality for deleting and editing contacts, as well as handling user interactions.
///     It also allows for the customization of the panel's appearance, such as the row height.
/// </remarks>
public partial class ContactsPanel
{
    /// <summary>
    ///     An event that is triggered when a contact needs to be deleted.
    /// </summary>
    /// <remarks>
    ///     This event is invoked with the ID of the contact that should be deleted.
    ///     The actual deletion logic should be implemented by the event handler.
    /// </remarks>
    [Parameter]
    public EventCallback<int> DeleteContact
    {
        get;
        set;
    }

    /// <summary>
    ///     An event that is triggered when a contact needs to be edited.
    /// </summary>
    /// <remarks>
    ///     This event is invoked with the ID of the contact that should be edited.
    ///     The actual editing logic should be implemented by the event handler.
    /// </remarks>
    [Parameter]
    public EventCallback<int> EditContact
    {
        get;
        set;
    }

    /// <summary>
    ///     Represents a grid control for displaying and interacting with company contacts.
    /// </summary>
    /// <remarks>
    ///     This grid control is used to display a list of company contacts in a tabular format.
    ///     It provides functionality for selecting a row, which represents a contact, and performing operations like editing
    ///     and deleting.
    ///     The grid control is bound to a list of 'CompanyContact' objects.
    /// </remarks>
    private SfGrid<CompanyContact> GridContacts
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance.
    /// </summary>
    /// <remarks>
    ///     This instance of IJSRuntime is used to invoke JavaScript functions from C# code.
    ///     For example, it is used to display a confirmation dialog when a contact is being deleted.
    /// </remarks>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of company contacts.
    /// </summary>
    /// <remarks>
    ///     This property holds the list of contacts that are displayed in the contacts panel.
    ///     Each contact in the list is represented by an instance of the CompanyContact class.
    /// </remarks>
    [Parameter]
    public List<CompanyContact> Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the height of the rows in the contacts grid.
    /// </summary>
    /// <remarks>
    ///     This property controls the height of each row in the contacts grid.
    ///     A larger value will result in taller rows, while a smaller value will result in shorter rows.
    ///     The value is specified in pixels.
    /// </remarks>
    [Parameter]
    public double RowHeight
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the currently selected row in the contacts panel.
    /// </summary>
    /// <remarks>
    ///     This property holds the instance of the CompanyContact class that represents the contact currently selected in the
    ///     contacts panel.
    ///     It is updated whenever a new row is selected in the panel.
    /// </remarks>
    public CompanyContact SelectedRow
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the username of the current user interacting with the contacts panel.
    /// </summary>
    /// <remarks>
    ///     This property holds the username of the user currently interacting with the contacts panel.
    ///     It is used for tracking user actions and may be used for personalization purposes.
    /// </remarks>
    [Parameter]
    public string UserName
    {
        get;
        set;
    }

    /// <summary>
    ///     Deletes a contact from the contacts panel.
    /// </summary>
    /// <param name="id">The ID of the contact to be deleted.</param>
    /// <remarks>
    ///     This method first selects the contact in the grid by its ID. Then, it displays a confirmation dialog to the user.
    ///     If the user confirms the deletion, it invokes the DeleteContact event with the ID of the contact.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    private async Task DeleteContactMethod(int id)
    {
        int _index = await GridContacts.GetRowIndexByPrimaryKeyAsync(id);
        await GridContacts.SelectRowAsync(_index);
        if (await JsRuntime.Confirm($"Are you sure you want to delete this Contact?{Environment.NewLine}Note: This action cannot be reversed."))
        {
            await DeleteContact.InvokeAsync(id);
        }
    }

    /// <summary>
    ///     Initiates the process of editing a contact.
    /// </summary>
    /// <param name="id">The ID of the contact to be edited.</param>
    /// <remarks>
    ///     This method is invoked when the user clicks on the edit button in the contacts panel.
    ///     It selects the row in the grid that corresponds to the contact with the given ID and triggers the EditContact
    ///     event.
    ///     The actual editing logic should be implemented by the event handler.
    ///     This method is asynchronous and returns a Task.
    /// </remarks>
    private async Task EditContactDialog(int id)
    {
        int _index = await GridContacts.GetRowIndexByPrimaryKeyAsync(id);
        await GridContacts.SelectRowAsync(_index);
        await EditContact.InvokeAsync(id);
    }

    /// <summary>
    ///     Handles the event when a row is selected in the contacts panel.
    /// </summary>
    /// <param name="contact">The event arguments containing the selected contact.</param>
    /// <remarks>
    ///     This method updates the SelectedRow property with the data of the selected contact.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<CompanyContact> contact)
    {
        if (contact != null)
        {
            SelectedRow = contact.Data;
        }
    }
}