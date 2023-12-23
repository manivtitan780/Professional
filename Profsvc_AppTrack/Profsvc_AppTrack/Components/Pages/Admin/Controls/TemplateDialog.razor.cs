#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           TemplateDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-23-2023 16:4
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing templates in the admin controls section of the application.
/// </summary>
/// <remarks>
///     This dialog provides functionality for handling template-related operations such as saving and cancelling.
///     It also provides an interface for setting and getting properties like HeaderString, Model, OriginalTemplateName,
///     and event callbacks like BlurSubject, Cancel, and Save.
/// </remarks>
public partial class TemplateDialog
{
    private bool _isFocusedSubject = false, _isFocusedContent = true;

    /// <summary>
    ///     Represents a collection of toolbar items for the Rich Text Editor in the Template Dialog.
    /// </summary>
    /// <remarks>
    ///     This collection includes commands for text formatting such as bold, italic, underline, StrikeThrough,
    ///     lower case, upper case, superscript, subscript, clear format, undo, and redo.
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
    ///     Gets a list of actions represented by byte values and their corresponding descriptions.
    /// </summary>
    /// <value>
    ///     The list includes actions like "Candidate Created", "Candidate Updated", "Candidate Submitted", etc., each
    ///     associated with a specific byte value.
    /// </value>
    private List<ByteValues> Action
    {
        get;
    } =
    [
        new(1, "Candidate Created"), new(2, "Candidate Updated"), new(3, "Candidate Submitted"), new(4, "Candidate Deleted"), new(5, "Candidate Status Changed"), new(6, "Requisition Created"),
        new(7, "Requisition Updated"), new(8, "Requisition Status Changed"), new(9, "Candidate Submission Updated"), new(10, "No Action")
    ];

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the cancel action is triggered in the dialog.
    /// </summary>
    /// <value>
    ///     The event callback for the cancel action.
    /// </value>
    /// <remarks>
    ///     This event callback is typically used to perform cleanup tasks when the user cancels the operation in the dialog.
    ///     The event handler receives the mouse event arguments associated with the cancel action.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfDialog instance used in the template dialog.
    /// </summary>
    /// <value>
    ///     The SfDialog instance.
    /// </value>
    /// <remarks>
    ///     This instance is used to control the visibility and behavior of the dialog.
    ///     It is bound to the dialog component in the Razor markup and can be used to programmatically show or hide the
    ///     dialog.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter instance associated with the TemplateDialog.
    /// </summary>
    /// <value>
    ///     The DialogFooter instance.
    /// </value>
    /// <remarks>
    ///     The DialogFooter instance is used to manage the footer section of the dialog,
    ///     which includes the Cancel and Save buttons.
    /// </remarks>
    private DialogFooter DialogFooter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the form used for editing a template.
    /// </summary>
    /// <value>
    ///     The form used for editing a template.
    /// </value>
    private EditForm EditTemplateForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the header string for the template dialog.
    /// </summary>
    /// <value>
    ///     The header string displayed at the top of the dialog.
    /// </value>
    [Parameter]
    public string HeaderString
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the JavaScript runtime instance for the dialog.
    /// </summary>
    /// <value>
    ///     The JavaScript runtime instance.
    /// </value>
    /// <remarks>
    ///     This property is used to invoke JavaScript methods from the dialog.
    ///     For example, it is used in the KeywordChanged method to insert text at the cursor position in the SubjectTextBox.
    /// </remarks>
    [Inject]
    private IJSRuntime JsRuntime
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets a list of key-value pairs representing template keywords.
    /// </summary>
    /// <remarks>
    ///     Each keyword is represented by a KeyValues object, where the key is the keyword name and the value is its
    ///     corresponding placeholder.
    ///     These keywords are used in the template management system for various template-related operations.
    /// </remarks>
    private List<KeyValues> Keyword
    {
        get;
    } =
    [
        new("Current Date", "$TODAY$"), new("Candidate Full Name", "$FULL_NAME$"), new("Candidate First Name", "$FIRST_NAME$"), new("Candidate Last Name", "$LAST_NAME$"),
        new("Candidate Location", "$CAND_LOCATION$"), new("Candidate Phone Number", "$CAND_PHONE_PRIMARY$"), new("Candidate Summary", "$CAND_SUMMARY$"), new("Requisition Code", "$REQ_ID$"),
        new("Requisition Title", "$REQ_TITLE$"), new("Requisition Company", "$COMPANY$"), new("Requisition Location", "$LOCATION$"), new("Requisition Description", "$DESCRIPTION$"),
        new("Submission Notes", "$SUBMISSION_NOTES$"), new("Logged-in User Name", "$LOGGED_USER$")
    ];

    /// <summary>
    ///     Gets or sets the model representing the template for the dialog.
    /// </summary>
    /// <value>
    ///     The model representing the template.
    /// </value>
    /// <remarks>
    ///     This model is used to bind the template data in the dialog for editing purposes.
    /// </remarks>
    [Parameter]
    public Template Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the original name of the template.
    /// </summary>
    /// <value>
    ///     The original name of the template.
    /// </value>
    /// <remarks>
    ///     This property is used to store the original name of the template before any modifications are made in the dialog.
    ///     It can be used to compare the original and the modified template names.
    /// </remarks>
    [Parameter]
    public string OriginalTemplateName
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the save action is triggered in the dialog.
    /// </summary>
    /// <value>
    ///     The event callback for the save action.
    /// </value>
    /// <remarks>
    ///     This event callback is typically used to perform save tasks when the user confirms the operation in the dialog.
    ///     The event handler receives the EditContext associated with the save action.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets a list of key-value pairs representing the different roles to which a template can be sent.
    /// </summary>
    /// <value>
    ///     The list includes roles such as "Administrator", "Sales Manager", "Recruiter", "Full Desk", "Requisition Owner",
    ///     "Candidate Owner", "Requisition Assigned", "Everyone", and "Others".
    /// </value>
    private List<KeyValues> SendTo
    {
        get;
    } =
    [
        new("Administrator", "Administrator"), new("Sales Manager", "Sales Manager"), new("Recruiter", "Recruiter"), new("Full Desk", "Full Desk"),
        new("Requisition Owner", "Requisition Owner"), new("Candidate Owner", "Candidate Owner"), new("Requisition Assigned", "Requisition Assigned"), new("Everyone", "Everyone"),
        new("Others", "Others")
    ];

    /// <summary>
    ///     Gets or sets the spinner control used in the template dialog.
    /// </summary>
    /// <value>
    ///     The spinner control.
    /// </value>
    /// <remarks>
    ///     This control is used to display a loading indicator while the dialog is performing operations such as saving or
    ///     canceling.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the TextBoxControl component used for accepting user input in the dialog.
    /// </summary>
    /// <value>
    ///     The TextBoxControl component.
    /// </value>
    /// <remarks>
    ///     This component is part of the user interface and is used for accepting user input in a text format.
    ///     It provides various customization options such as setting the CSS class, height, ID, maximum and minimum length of
    ///     the input,
    ///     whether it's multiline or not, placeholder text, whether it's read-only or not, the number of rows,
    ///     the type of the input, tooltip, and whether to validate on input.
    ///     It also provides options for handling events such as when the component is created, when it loses focus, and when
    ///     it gains focus.
    /// </remarks>
    private TextBoxControl SubjectTextBox
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the rich text editor for the template content in the dialog.
    /// </summary>
    /// <value>
    ///     The rich text editor for the template content.
    /// </value>
    /// <remarks>
    ///     This property is used to manipulate the template content in the dialog. It provides functionalities like executing
    ///     commands for inserting HTML content.
    /// </remarks>
    private SfRichTextEditor TemplateContent
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously executes the cancellation process for the template dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <remarks>
    ///     This method calls the general cancellation method, which hides the spinner and dialog, and enables the dialog
    ///     buttons.
    ///     It is typically invoked when the user triggers the cancel action in the template dialog.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task CancelTemplate(MouseEventArgs args) => General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);

    /// <summary>
    ///     Handles the focus event for the Rich Text Box (RTB) in the dialog.
    /// </summary>
    /// <param name="obj">The focus event arguments.</param>
    /// <remarks>
    ///     This method is invoked when the RTB in the dialog receives focus.
    ///     It sets the internal state to indicate that the RTB is focused and the subject is not.
    /// </remarks>
    private void FocusRTB(FocusEventArgs obj)
    {
        _isFocusedContent = true;
        _isFocusedSubject = false;
    }

    /// <summary>
    ///     Handles the focus event for the Subject TextBox.
    /// </summary>
    /// <param name="obj">The event arguments associated with the focus event.</param>
    /// <remarks>
    ///     When this method is invoked, it sets the '_isFocusedSubject' field to true indicating that the Subject TextBox is
    ///     in focus,
    ///     and '_isFocusedContent' field to false indicating that the Content TextBox is not in focus.
    /// </remarks>
    private void FocusSubjectTextBox(FocusInEventArgs obj)
    {
        _isFocusedSubject = true;
        _isFocusedContent = false;
    }

    /// <summary>
    ///     Handles the event when a keyword is changed.
    /// </summary>
    /// <param name="args">The arguments containing the changed keyword and associated key values.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     If the content is focused, it executes the InsertHTML command with the changed keyword.
    ///     If the subject is focused, it inserts the changed keyword at the cursor position in the SubjectTextBox.
    /// </remarks>
    private async Task KeywordChanged(ChangeEventArgs<string, KeyValues> args)
    {
        await Task.Yield();
        if (_isFocusedContent)
        {
            await TemplateContent.ExecuteCommandAsync(CommandName.InsertHTML, args.Value);
        }
        else if (_isFocusedSubject)
        {
            await JsRuntime.InvokeVoidAsync("insertTextAtCursor", SubjectTextBox.ID, args.Value);
        }
    }

    /// <summary>
    ///     Asynchronously prepares the template dialog for opening.
    /// </summary>
    /// <param name="args">The arguments associated with the dialog opening event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is invoked before the dialog is opened. It initializes the edit context for the template form and
    ///     validates it.
    /// </remarks>
    private void OpenDialog(BeforeOpenEventArgs args) => EditTemplateForm.EditContext?.Validate();

    /// <summary>
    ///     Asynchronously saves the template changes made in the dialog.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <remarks>
    ///     This method calls the general save method, passing in the edit context, spinner, dialog footer, dialog, and save
    ///     event callback.
    ///     It is typically triggered when the user confirms the save operation in the dialog.
    /// </remarks>
    private Task SaveTemplate(EditContext editContext) => General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);

    /// <summary>
    ///     Asynchronously displays the template dialog.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method is used to programmatically display the dialog for editing a template.
    ///     It invokes the ShowAsync method of the SfDialog instance associated with the dialog.
    /// </remarks>
    public Task ShowDialog() => Dialog.ShowAsync();

    /// <summary>
    ///     Handles the opening of a tooltip.
    /// </summary>
    /// <param name="args">The arguments associated with the tooltip event.</param>
    /// <remarks>
    ///     This method cancels the opening of the tooltip if it does not contain any text.
    /// </remarks>
    private void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}