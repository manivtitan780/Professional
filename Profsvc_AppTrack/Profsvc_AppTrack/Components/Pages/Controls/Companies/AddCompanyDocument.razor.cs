#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           AddCompanyDocument.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-27-2023 19:48
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Controls.Companies;

/// <summary>
///     The AddCompanyDocument class is a Blazor component used to handle the addition of documents to a company.
///     It provides a dialog interface with form fields for document details and an upload control for the document file.
///     The class exposes several events that can be subscribed to, such as BeforeUpload, AfterUpload, and OnFileUpload,
///     which are triggered during the document upload process.
///     It also provides methods to enable or disable the dialog buttons.
/// </summary>
public partial class AddCompanyDocument
{
    private EditContext _editContext;

    /// <summary>
    ///     Gets or sets the event to be triggered after a document upload operation has completed.
    ///     This event receives an argument of type ActionCompleteEventArgs, which contains details about the completed upload
    ///     operation.
    /// </summary>
    [Parameter]
    public EventCallback<ActionCompleteEventArgs> AfterUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered before a document upload operation starts.
    ///     This event receives an argument of type BeforeUploadEventArgs, which contains details about the upload operation
    ///     that is about to start.
    /// </summary>
    [Parameter]
    public EventCallback<BeforeUploadEventArgs> BeforeUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered when the cancel action is invoked.
    ///     This event receives an argument of type MouseEventArgs, which contains details about the mouse event that triggered
    ///     the cancel action.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfDialog instance used to display the dialog interface for adding a company document.
    ///     This dialog contains form fields for document details and an upload control for the document file.
    ///     The dialog's visibility is controlled by the ShowAsync and HideAsync methods.
    /// </summary>
    public SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the EditForm instance used for editing the document details in the AddCompanyDocument component.
    ///     This form contains fields for document details and an upload control for the document file.
    ///     The form's state and validation status can be accessed and manipulated through this property.
    /// </summary>
    private EditForm EditDocumentForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered when a document upload operation fails.
    ///     This event receives an argument of type FailureEventArgs, which contains details about the failed upload operation.
    /// </summary>
    [Parameter]
    public EventCallback<FailureEventArgs> Failure
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered when a file is selected in the upload control.
    ///     This event receives an argument of type SelectedEventArgs, which contains details about the selected file.
    /// </summary>
    [Parameter]
    public EventCallback<SelectedEventArgs> FileSelect
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter instance used in the AddCompanyDocument component.
    ///     This footer contains the Cancel and Save buttons for the document upload dialog.
    ///     The state of these buttons can be managed through the methods of the DialogFooter class.
    /// </summary>
    private DialogFooter FooterDialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model of type RequisitionDocuments for the AddCompanyDocument component.
    ///     This model is used to bind the form fields in the dialog interface of the component.
    ///     It contains the data for the document that is to be added to the company.
    /// </summary>
    [Parameter]
    public RequisitionDocuments Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered when the file upload changes.
    ///     This event receives an argument of type UploadChangeEventArgs, which contains details about the upload operation
    ///     that has changed.
    /// </summary>
    [Parameter]
    public EventCallback<UploadChangeEventArgs> OnFileUpload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered when the save action is invoked.
    ///     This event receives an argument of type EditContext, which contains the current state of the form that is being
    ///     saved.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfSpinner instance used in the AddCompanyDocument component.
    ///     This spinner is displayed during the execution of asynchronous operations, such as document upload, to indicate
    ///     progress.
    /// </summary>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered when a document upload operation is successful.
    ///     This event receives an argument of type SuccessEventArgs, which contains details about the successful upload
    ///     operation.
    /// </summary>
    [Parameter]
    public EventCallback<SuccessEventArgs> Success
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event to be triggered when a document upload operation starts.
    ///     This event receives an argument of type UploadingEventArgs, which contains details about the upload operation
    ///     that is starting.
    /// </summary>
    [Parameter]
    public EventCallback<UploadingEventArgs> UploadStart
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously cancels the document dialog. This method is triggered when the cancel button is clicked in the
    ///     dialog.
    ///     It calls the general cancel method with the provided mouse event arguments and the references to the spinner,
    ///     footer dialog, main dialog, and cancel button.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel button click event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CancelDocumentDialog(MouseEventArgs args) => await General.CallCancelMethod(args, Spinner, FooterDialog, Dialog, Cancel);

    /// <summary>
    ///     This method is invoked when the dialog for adding a company document is opened.
    ///     It initializes the EditContext with the context of the EditDocumentForm and triggers validation on it.
    /// </summary>
    private void DialogOpen()
    {
        _editContext = EditDocumentForm.EditContext;
        _editContext?.Validate();
    }

    /// <summary>
    ///     Disables the Cancel and Save buttons in the dialog footer of the AddCompanyDocument component.
    /// </summary>
    /// <remarks>
    ///     This method calls the DisableButtons method of the DialogFooter instance used in the AddCompanyDocument component.
    ///     It is used to prevent any further user interaction with these buttons until they are re-enabled, typically during
    ///     the document upload process.
    /// </remarks>
    public void DisableButtons() => FooterDialog.DisableButtons();

    /// <summary>
    ///     Enables the Cancel and Save buttons in the dialog footer of the AddCompanyDocument component.
    /// </summary>
    /// <remarks>
    ///     This method is typically invoked after a document upload operation has completed, allowing further user
    ///     interaction with the Cancel and Save buttons in the dialog footer.
    /// </remarks>
    public void EnableButtons() => FooterDialog.EnableButtons();

    /// <summary>
    ///     Handles the event when a file is removed from the upload control in the AddCompanyDocument component.
    ///     This method is triggered when the OnRemove event of the UploaderControl is fired.
    ///     It sets the Files property of the Model to null and notifies the EditContext of the field change.
    /// </summary>
    /// <param name="arg">The arguments associated with the file removing event.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    private async Task OnFileRemoved(RemovingEventArgs arg)
    {
        await Task.Yield();
        Model.Files = null;
        _editContext?.NotifyFieldChanged(_editContext.Field(nameof(Model.Files)));
    }

    /// <summary>
    ///     Handles the event when a file is selected in the file upload control.
    ///     If the Model.Files list is null, a new list is created and the selected file's name is added to it.
    ///     If the Model.Files list is not null, it is cleared and the selected file's name is added.
    ///     After the file's name is added to the list, a field change notification is sent for the Model.Files field.
    /// </summary>
    /// <param name="file">The event arguments containing the selected file's data.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
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
    ///     Asynchronously saves the document details entered in the dialog form.
    ///     This method is invoked when the form is submitted. It calls the general save method, passing in the current edit
    ///     context,
    ///     the spinner, the dialog footer, the dialog, and the save event callback.
    ///     The general save method handles the process of executing the save operation, managing the spinner and dialog
    ///     states,
    ///     and invoking the save event callback.
    /// </summary>
    /// <param name="editContext">The edit context associated with the form that is being saved.</param>
    /// <returns>
    ///     A task that represents the asynchronous save operation.
    /// </returns>
    private async Task SaveDocumentDialog(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, FooterDialog, Dialog, Save);
}