#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           EditContactDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-30-2023 14:55
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Companies;

/// <summary>
///     Represents a dialog for editing company contact information.
/// </summary>
/// <remarks>
///     This dialog provides the following functionalities:
///     - Canceling the edit operation, which is represented by the <see cref="Cancel" /> event.
///     - Saving the edited company contact, which is represented by the <see cref="Save" /> event.
///     - Changing the state ID, which is represented by the <see cref="StateIDChanged" /> event.
///     - Displaying a list of states and title types, which are represented by the <see cref="States" /> and
///     <see cref="TitleTypes" /> properties respectively.
///     - Displaying a spinner during asynchronous operations, which is represented by the <see cref="Spinner" /> property.
///     - Showing the dialog, which is represented by the <see cref="ShowDialog" /> method.
/// </remarks>
public partial class EditContactDialog
{
    private EditContext _editContext;

    /// <summary>
    ///     Gets or sets the AutoCompleteButton control used in the EditContactDialog.
    /// </summary>
    /// <value>
    ///     The AutoCompleteButton control.
    /// </value>
    /// <remarks>
    ///     This control provides a user interface for input with autocomplete suggestions within the EditContactDialog.
    /// </remarks>
    private AutoCompleteButton AutoCompleteControl
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the Cancel action is performed.
    /// </summary>
    /// <value>
    ///     The event callback that takes a <see cref="MouseEventArgs" /> parameter.
    /// </value>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the Syncfusion Blazor Dialog component used in the EditContactDialog.
    ///     This dialog is used to display the form for editing a company contact.
    /// </summary>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the form for editing a contact in the EditContactDialog component.
    /// </summary>
    /// <value>
    ///     The form for editing a contact.
    /// </value>
    private EditForm EditContactForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the footer dialog of the EditContactDialog.
    ///     The footer dialog is a component that contains the Cancel and Save buttons for the dialog.
    /// </summary>
    private DialogFooter FooterDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the JavaScript runtime (IJSRuntime) for the EditContactDialog.
    ///     This instance is used to invoke JavaScript functions from the C# code.
    /// </summary>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model of the company contact for the EditContactDialog.
    ///     This model is used to bind the data of the company contact in the dialog.
    /// </summary>
    [Parameter]
    public CompanyContact Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the save operation is performed.
    /// </summary>
    /// <value>
    ///     The event callback that takes an <see cref="EditContext" /> parameter.
    /// </value>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the Syncfusion Blazor Spinner component used in the EditContactDialog.
    ///     This spinner is displayed during asynchronous operations to indicate that the process is running and the user
    ///     should wait.
    /// </summary>
    internal SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the state ID changes.
    /// </summary>
    /// <value>
    ///     The event callback that takes a <see cref="ChangeEventArgs{Int, IntValues}" /> parameter.
    /// </value>
    [Parameter]
    public EventCallback<ChangeEventArgs<int, IntValues>> StateIDChanged
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of states for the EditContactDialog.
    ///     Each state is represented as an instance of the IntValues class.
    /// </summary>
    [Parameter]
    public List<IntValues> States
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the title of the EditContactDialog.
    ///     This title is displayed in the dialog header.
    /// </summary>
    [Parameter]
    public string Title
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of title types for the EditContactDialog.
    ///     Each title type is represented by an instance of the IntValues class.
    /// </summary>
    [Parameter]
    public List<IntValues> TitleTypes
    {
        get;
        set;
    }

    /// <summary>
    ///     Cancels the dialog operation asynchronously.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task CancelDialog(MouseEventArgs args)
    {
        await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);
    }

    /// <summary>
    ///     Initializes the dialog for editing a company contact.
    ///     This method sets the EditContext of the EditContactForm and validates it.
    /// </summary>
    private void DialogOpen()
    {
        _editContext = EditContactForm.EditContext;
        EditContactForm.EditContext?.Validate();
    }

    /// <summary>
    ///     Executes an asynchronous operation that invokes a JavaScript function named "onCreate" with specified parameters.
    ///     This method is designed to ensure that only numeric input is allowed in a certain context.
    /// </summary>
    /// <param name="args">The arguments to pass to the JavaScript function.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private async Task NumbersOnly(object args)
    {
        await JsRuntime.InvokeVoidAsync("onCreate", "textExtension", true);
    }

    /// <summary>
    ///     Asynchronously invokes the JavaScript function 'onCreate' with specified parameters.
    /// </summary>
    /// <param name="arg">The argument passed to the method, currently not used within the method body.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private async Task OnCreate(object arg)
    {
        await JsRuntime.InvokeVoidAsync("onCreate", "autoZip", true);
    }

    /// <summary>
    ///     Asynchronously executes the save operation for the dialog. This method is called when the user submits the form in
    ///     the dialog.
    ///     It uses the General.CallSaveMethod to perform the save operation, which includes showing a spinner, disabling
    ///     dialog buttons during the save operation, and hiding the spinner and dialog after the save operation is complete.
    /// </summary>
    /// <param name="editContext">
    ///     The EditContext associated with the form in the dialog. This context includes information
    ///     about the form and its fields, including their current values and validation state.
    /// </param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task SaveDialog(EditContext editContext)
    {
        await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Save);
    }

    /// <summary>
    ///     Asynchronously displays the dialog for editing a company contact.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation of showing the dialog.</returns>
    public async Task ShowDialog()
    {
        await Dialog.ShowAsync();
    }

    /// <summary>
    ///     Handles the event when the title value changes in the EditContactDialog.
    /// </summary>
    /// <param name="args">The arguments of the change event, containing the new title value.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    private async Task ValueChangeTitle(ChangeEventArgs<int, IntValues> args)
    {
        await Task.Yield();
        Model.Title = args.ItemData.Value;
    }

    /// <summary>
    ///     Handles the change of the Zip code in the EditContactDialog.
    /// </summary>
    /// <param name="arg">The event arguments containing the new Zip code and associated key values.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method retrieves a list of Zip codes from the memory cache. If the new Zip code exists in the list,
    ///     it updates the City and StateID fields of the Model with the corresponding values from the Zip code entry.
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
    ///     Represents a data adaptor for a dropdown that displays zip codes. This class is a part of the EditContactDialog
    ///     component.
    ///     It extends the DataAdaptor class and overrides the ReadAsync method to fetch zip codes asynchronously.
    /// </summary>
    public class ZipDropDownAdaptor : DataAdaptor
    {
        /// <summary>
        ///     Asynchronously reads data for the zip code dropdown based on the provided DataManagerRequest.
        ///     This method leverages the General.GetAutocompleteZipAsync method to fetch the required data.
        ///     The DataManagerRequest should contain the filtering conditions for the zip codes.
        /// </summary>
        /// <param name="dm">The DataManagerRequest containing the filtering conditions for the zip codes.</param>
        /// <param name="key">An optional key parameter. This parameter is not used in the current implementation.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains a list of zip codes and their
        ///     count if required by the DataManagerRequest.
        /// </returns>
        public override Task<object> ReadAsync(DataManagerRequest dm, string key = null) => General.GetAutocompleteZipAsync(dm);
    }
}