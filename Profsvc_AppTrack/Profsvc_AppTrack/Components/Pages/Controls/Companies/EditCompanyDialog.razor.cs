#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           EditCompanyDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-29-2023 20:09
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Controls.Companies;

/// <summary>
///     Represents a dialog for editing company details in the application.
/// </summary>
/// <remarks>
///     This class provides properties and methods for managing the editing of company details.
///     It includes properties for handling events such as Cancel, Save, and StateIDChanged.
///     It also includes a method for showing the dialog.
/// </remarks>
public partial class EditCompanyDialog
{
    private EditContext _editContext;

    /// <summary>
    ///     Gets or sets the AutoCompleteButton control used in the EditCompanyDialog.
    /// </summary>
    /// <remarks>
    ///     This control provides autocomplete functionality for user input within the EditCompanyDialog.
    /// </remarks>
    private AutoCompleteButton AutoCompleteControl
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the Cancel action is triggered in the dialog.
    /// </summary>
    /// <value>
    ///     The event callback for the Cancel action.
    /// </value>
    /// <remarks>
    ///     This event callback is used to handle the cancellation of the editing process in the dialog.
    ///     It is triggered by a mouse event, such as clicking the Cancel button.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfDialog control used in the EditCompanyDialog.
    /// </summary>
    /// <value>
    ///     The SfDialog control.
    /// </value>
    /// <remarks>
    ///     This control is used to display the dialog for editing company details.
    ///     It is shown and hidden using the ShowDialog and CancelDialog methods respectively.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the form for editing a company.
    /// </summary>
    /// <value>
    ///     The form for editing a company.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the form that allows users to edit company details.
    ///     It is used in the BuildRenderTree method to bind the form to the dialog.
    /// </remarks>
    private EditForm EditCompanyForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the footer dialog of the EditCompanyDialog.
    /// </summary>
    /// <value>
    ///     The footer dialog which contains the Save and Cancel buttons.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the footer section of the dialog, which includes the Save and Cancel buttons.
    ///     It is an instance of the DialogFooter class, which provides properties and methods for managing the buttons in the
    ///     dialog footer.
    /// </remarks>
    public DialogFooter FooterDialog
    {
        get;
        set;
    }

    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance for invoking JavaScript functions from .NET code.
    /// </summary>
    /// <value>
    ///     The JavaScript runtime instance.
    /// </value>
    /// <remarks>
    ///     This property is used to invoke JavaScript functions from .NET code in the context of the EditCompanyDialog.
    ///     It is used, for example, in the OnCreate method to call the JavaScript function "onCreate".
    /// </remarks>
    [Parameter]
    public CompanyDetails Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the Save action is triggered in the dialog.
    /// </summary>
    /// <value>
    ///     The event callback for the Save action.
    /// </value>
    /// <remarks>
    ///     This event callback is used to handle the saving of the edited company details in the dialog.
    ///     It is triggered by a mouse event, such as clicking the Save button.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfSpinner control used in the EditCompanyDialog.
    /// </summary>
    /// <value>
    ///     The SfSpinner control.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the spinner control in the dialog, which is used to indicate a loading state while
    ///     the dialog is processing an action, such as saving the company details.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the StateID value changes.
    /// </summary>
    /// <value>
    ///     The event callback for the StateID change event.
    /// </value>
    /// <remarks>
    ///     This event callback provides a way for the parent component to react to changes in the StateID value.
    ///     The event callback receives a ChangeEventArgs object that contains the old and new StateID values.
    /// </remarks>
    [Parameter]
    public EventCallback<ChangeEventArgs<int, IntValues>> StateIDChanged
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of states for the company.
    /// </summary>
    /// <value>
    ///     The list of states, represented as instances of the IntValues class.
    /// </value>
    /// <remarks>
    ///     This property is used to populate the dropdown list for selecting a state in the edit company dialog.
    /// </remarks>
    [Parameter]
    public List<IntValues> States
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title of the EditCompanyDialog.
    /// </summary>
    /// <value>
    ///     The title of the dialog.
    /// </value>
    /// <remarks>
    ///     This property is used to set the header of the dialog. It is displayed at the top of the dialog when it is opened.
    ///     The value of this property is concatenated with the string " Company" to form the complete header of the dialog.
    /// </remarks>
    [Parameter]
    public string Title
    {
        get;
        set;
    }

    /// <summary>
    ///     Cancels the dialog for editing company details.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method calls the general cancel method, passing in the mouse event arguments,
    ///     the spinner, footer dialog, dialog, and cancel objects. It is used to handle the
    ///     cancellation of the editing process.
    /// </remarks>
    private async Task CancelDialog(MouseEventArgs args)
    {
        await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);
    }

    /// <summary>
    ///     Prepares the dialog for opening.
    /// </summary>
    /// <param name="arg">
    ///     The arguments associated with the dialog opening event.
    /// </param>
    /// <remarks>
    ///     This method is invoked before the dialog is opened. It initializes the edit context and validates it.
    /// </remarks>
    private void DialogOpen(BeforeOpenEventArgs arg)
    {
        _editContext = EditCompanyForm.EditContext;
        _editContext?.Validate();
    }

    /// <summary>
    ///     Handles the 'Create' event asynchronously.
    /// </summary>
    /// <param name="arg">The event argument.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method invokes the JavaScript function 'onCreate' with parameters 'autoZip' and 'true'.
    /// </remarks>
    private async Task OnCreate(object arg)
    {
        await JsRuntime.InvokeVoidAsync("onCreate", "autoZip", true);
    }

    /// <summary>
    ///     Asynchronously saves the company details entered in the dialog.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <remarks>
    ///     This method calls the General.CallSaveMethod to handle the save operation.
    ///     It passes the edit context, spinner, footer dialog, main dialog, and save event callback to the CallSaveMethod.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SaveCompanyDialog(EditContext editContext)
    {
        await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Save);
    }

    /// <summary>
    ///     Asynchronously displays the EditCompanyDialog.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation of showing the dialog.
    /// </returns>
    /// <remarks>
    ///     This method is used to display the dialog for editing company details.
    ///     It is typically invoked when the user chooses to edit a company's details from the Companies page.
    /// </remarks>
    public async Task ShowDialog()
    {
        await Dialog.ShowAsync();
    }

    /// <summary>
    ///     Handles the change of the Zip code in the EditCompanyDialog.
    /// </summary>
    /// <param name="arg">The event arguments containing the new Zip code and associated key values.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is triggered when the Zip code is changed in the EditCompanyDialog. It updates the City and StateID
    ///     fields
    ///     in the Model based on the new Zip code. If the Zip code matches one in the memory cache, the City and StateID are
    ///     updated
    ///     accordingly and a field changed notification is sent for the City field.
    /// </remarks>
    private async Task ZipChange(ChangeEventArgs<string, KeyValues> arg)
    {
        await Task.Yield();
        IMemoryCache _memoryCache = Start.MemCache;
        List<Zip> _zips = null;
        while (_zips == null)
        {
            _memoryCache.TryGetValue("Zips", out _zips);
        }

        if (_zips.Count > 0)
        {
            foreach (Zip _zip in _zips.Where(zip => zip.ZipCode == arg.Value))
            {
                Model.City = _zip.City;
                Model.StateID = _zip.StateID;
                _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.City)));
            }
        }
    }

    /// <summary>
    ///     Provides a data adaptor for the zip code dropdown in the EditCompanyDialog.
    /// </summary>
    /// <remarks>
    ///     This class extends the DataAdaptor class and overrides the ReadAsync method to provide
    ///     data for the zip code dropdown. The data is fetched asynchronously using the
    ///     General.GetAutocompleteZipAsync method.
    /// </remarks>
    public class ZipDropDownAdaptor : DataAdaptor
    {
        /// <summary>
        ///     Asynchronously reads data for the zip code dropdown.
        /// </summary>
        /// <param name="dm">The DataManagerRequest containing the filtering conditions for the zip codes.</param>
        /// <param name="key">An optional key to further filter the data.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains a list of zip codes and their
        ///     count if required.
        /// </returns>
        /// <remarks>
        ///     This method uses the General.GetAutocompleteZipAsync method to retrieve the data.
        /// </remarks>
        public override Task<object> ReadAsync(DataManagerRequest dm, string key = null) => General.GetAutocompleteZipAsync(dm);
    }
}