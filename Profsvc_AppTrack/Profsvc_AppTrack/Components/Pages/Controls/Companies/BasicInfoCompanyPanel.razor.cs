#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           BasicInfoCompanyPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-28-2023 16:27
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Pages.Controls.Common;

namespace Profsvc_AppTrack.Components.Pages.Controls.Companies;

/// <summary>
///     Represents a component in the application that displays and manages the basic information of a company.&lt;br /&gt;
///     This component is used within the Companies page and includes parameters for the company's address,
///     model details, and associated contacts. It also provides event callbacks for editing and deleting contacts.
/// </summary>
public partial class BasicInfoCompanyPanel
{
    private int _selectedID;

    /// <summary>
    ///     Gets or sets the address of the company. This is a markup string, which allows for HTML content.
    /// </summary>
    [Parameter]
    public MarkupString Address
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the EventCallback for deleting a contact associated with the company.<br />
    ///     This callback is invoked when the user confirms the deletion of a contact in the UI.<br />
    ///     The integer parameter of the callback represents the ID of the contact to be deleted.
    /// </summary>
    [Parameter]
    public EventCallback<int> DeleteContact
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ConfirmDialog component used for confirming user actions within the BasicInfoCompanyPanel.
    /// </summary>
    /// <remarks>
    ///     This property is used to manage the ConfirmDialog component which provides a dialog interface for confirming
    ///     various user actions.<br />
    ///     The dialog can be used to confirm actions such as deletion, cancellation, and activity toggling.
    /// </remarks>
    private ConfirmDialog DialogConfirm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the EventCallback for editing a contact associated with the company.<br />
    ///     This callback is invoked when the user initiates the editing of a contact in the UI.<br />
    ///     The integer parameter of the callback represents the ID of the contact to be edited.
    /// </summary>
    [Parameter]
    public EventCallback<int> EditContact
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfGrid component that displays the list of company contacts.<br />
    ///     This grid is used to manage the contacts associated with the company,
    ///     including operations like selecting a contact for editing or deletion.
    /// </summary>
    private SfGrid<CompanyContact> GridContacts
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model representing the details of a company. This model is used to populate the fields in the
    ///     BasicInfoCompanyPanel.
    /// </summary>
    [Parameter]
    public CompanyDetails Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of contacts associated with the company. <br />
    ///     Each contact is represented by an instance of the CompanyContact class.
    /// </summary>
    [Parameter]
    public List<CompanyContact> ModelContact
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the height of a row in the BasicInfoCompanyPanel. <br />
    ///     The default value is 38.
    /// </summary>
    [Parameter]
    public double RowHeight
    {
        get;
        set;
    } = 38;

    /// <summary>
    ///     Gets or sets the selected company contact in the BasicInfoCompanyPanel.<br />
    ///     This property is populated when a row is selected in the UI, and it represents the contact details of the selected
    ///     company.
    /// </summary>
    public CompanyContact SelectedRow
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the username of the current user interacting with the BasicInfoCompanyPanel.<br />
    ///     This property is used to determine if the current user has the rights to edit or delete contacts.
    /// </summary>
    [Parameter]
    public string UserName
    {
        get;
        set;
    }

    /// <summary>
    ///     Initiates the process of deleting a contact associated with the company.
    /// </summary>
    /// <param name="id">The ID of the contact to be deleted.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sets the selected contact ID, finds the index of the contact in the grid, selects the row in the grid,
    ///     and then shows a confirmation dialog to the user.<br />
    ///     The actual deletion of the contact is not performed in this method, but is instead handled by the DeleteContact
    ///     EventCallback, which should be invoked when the user confirms the deletion in the UI.
    /// </remarks>
    private async Task DeleteContactMethod(int id)
    {
        _selectedID = id;
        int _index = await GridContacts.GetRowIndexByPrimaryKeyAsync(id);
        await GridContacts.SelectRowAsync(_index);
        await DialogConfirm.ShowDialog();
    }

    /// <summary>
    ///     Asynchronously opens the dialog for editing a contact associated with the company.
    /// </summary>
    /// <param name="id">The ID of the contact to be edited.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task EditContactDialog(int id)
    {
        int _index = await GridContacts.GetRowIndexByPrimaryKeyAsync(id);
        await GridContacts.SelectRowAsync(_index);
        await EditContact.InvokeAsync(id);
    }

    /// <summary>
    ///     This method is called when a row in the BasicInfoCompanyPanel is bound with data.
    ///     It checks if the status code of the company contact is "INA", and if so, it adds a "disabledCell" class to the row.
    ///     This could be used to visually indicate that the contact is inactive.
    /// </summary>
    /// <param name="args">The event arguments containing the data bound to the row.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private async Task RowDataBound(RowDataBoundEventArgs<CompanyContact> args)
    {
        await Task.Yield();
        if (args.Data.StatusCode == "INA")
        {
            args.Row.AddClass(new[] {"disabledCell"});
        }
    }

    /// <summary>
    ///     Handles the row selection event in the BasicInfoCompanyPanel.<br />
    ///     This method is invoked when a row is selected in the UI.<br />
    ///     The selected row's data is assigned to the SelectedRow property.
    /// </summary>
    /// <param name="contact">The event arguments containing the data of the selected company contact.</param>
    private void RowSelected(RowSelectEventArgs<CompanyContact> contact)
    {
        if (contact != null)
        {
            SelectedRow = contact.Data;
        }
    }
}