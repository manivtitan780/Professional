#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           DocumentTypeDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          10-30-2022 16:29
// Last Updated On:     09-01-2023 19:45
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     The DocumentTypeDialog class is a part of the ProfSvc_AppTrack.Pages.Admin.Controls namespace.
///     This class is used to create a dialog for managing document types in the application.
///     It contains parameters for handling events such as Cancel and Save, and properties for setting the HeaderString,
///     Model, and Placeholder.
///     It also contains methods for handling dialog actions such as CancelDocType, SaveDocType, ShowDialog, and
///     HideDialog.
/// </summary>
public partial class DocumentTypeDialog
{
	/// <summary>
	///     Gets or sets the event callback that is invoked when the Cancel action is triggered.
	///     This event callback is of type <see cref="EventCallback{MouseEventArgs}" />, which means it carries the mouse event
	///     arguments associated with the Cancel action.
	/// </summary>
	[Parameter]
	public EventCallback<MouseEventArgs> Cancel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the Syncfusion Blazor Buttons SfButton that represents the Cancel button in the
	///     dialog.
	///     This button, when clicked, triggers the cancellation of any ongoing document type editing operation and closes the
	///     dialog.
	/// </summary>
	private SfButton CancelButton
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the SfDialog component used in the DocumentTypeDialog class.
	///     This property is used to control the visibility and behavior of the dialog.
	/// </summary>
	private SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogFooter instance associated with the DocumentTypeDialog.
	///     The DialogFooter is a component that represents the footer of the dialog, containing the Cancel and Save buttons.
	///     This property is used to manage the state and behavior of the dialog's footer.
	/// </summary>
	private DialogFooter DialogFooter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the EditForm instance for the dialog. This instance is used to manage the form editing process.
	///     It provides a context for data binding, event handling, and validation of the form associated with editing a
	///     DocumentType.
	/// </summary>
	private EditForm EditDocType
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the header string for the dialog. This string is displayed as the title of the dialog.
	/// </summary>
	[Parameter]
	public string HeaderString
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the IJSRuntime interface. This interface provides methods for interacting with
	///     JavaScript from .NET code.
	/// </summary>
	/// <remarks>
	///     The IJSRuntime instance is used to invoke JavaScript functions from .NET code. In the DocumentTypeDialog class, it
	///     is used
	///     to open a new browser tab for preventing entry of special characters in the
	///     `DocumentTypeDialog.NoSpecialCharacters()` method.
	/// </remarks>
	[Inject]
	private IJSRuntime JsRuntime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Model of the dialog, which is an instance of the DocumentType class.
	///     This property is used to bind the dialog's form to a DocumentType object, allowing the user to edit the properties
	///     of the DocumentType.
	/// </summary>
	[Parameter]
	public DocumentType Model
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the placeholder text for the TextBoxControl in the dialog.
	///     This text is displayed in the TextBoxControl when it is empty, before the user enters a value.
	/// </summary>
	[Parameter]
	public string Placeholder
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is invoked when the Save action is triggered.
	///     This event callback is of type <see cref="EventCallback{EditContext}" />, which means it carries the EditContext
	///     associated with the Save action.
	///     The EditContext provides a mechanism for tracking changes and validation state of the Model being edited.
	/// </summary>
	[Parameter]
	public EventCallback<EditContext> Save
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the SfButton control used as the Save button in the dialog.
	///     This button triggers the Save action which invokes the Save event callback.
	/// </summary>
	private SfButton SaveButton
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Spinner control of the dialog. The Spinner control is an instance of the SfSpinner class.
	///     This control is used to display a loading spinner animation while the dialog is performing an operation, such as
	///     saving or canceling.
	/// </summary>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Asynchronously cancels the document type operation.
	///     This method invokes the general cancellation routine, which hides the spinner and dialog, and enables the dialog
	///     buttons.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the cancel action.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async Task CancelDocType(MouseEventArgs args)
	{
		await Task.Yield();
		await General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);
		//await Cancel.InvokeAsync(args);
		//await Spinner.HideAsync();
		//await Dialog.HideAsync();
	}

	/// <summary>
	///     Hides the dialog of the DocumentTypeDialog component.
	///     This method is used to close the dialog when an operation is completed or cancelled.
	/// </summary>
	public void HideDialog()
	{
		Dialog.HideAsync();
	}

	/// <summary>
	///     Prevents the entry of special characters in the TextBoxControl of the DocumentTypeDialog component.
	///     This method is invoked when the TextBoxControl is created.
	/// </summary>
	/// <param name="args">The event arguments associated with the TextBoxControl creation.</param>
	/// <returns>A task that represents the asynchronous operation of invoking the JavaScript function "onCreateNoSpecial".</returns>
	private async Task NoSpecialCharacters(object args)
	{
		await JsRuntime.InvokeVoidAsync("onCreateNoSpecial", "textText");
	}

	/// <summary>
	///     Asynchronously opens the dialog of the DocumentTypeDialog component.
	///     This method is invoked before the dialog is opened and it validates the EditForm's EditContext.
	/// </summary>
	/// <param name="arg">The arguments associated with the BeforeOpen event.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task OpenDialog(BeforeOpenEventArgs arg)
	{
		await Task.Yield();
		EditDocType.EditContext?.Validate();
	}

	/// <summary>
	///     Asynchronously saves the document type information entered in the dialog.
	/// </summary>
	/// <param name="editContext">
	///     The edit context associated with the save action. This context provides a mechanism for
	///     tracking changes and validation state of the Model being edited.
	/// </param>
	/// <returns>A task that represents the asynchronous save operation.</returns>
	/// <remarks>
	///     This method calls the General.CallSaveMethod to handle the save operation, which includes showing a spinner,
	///     disabling dialog buttons during the save operation, and hiding the spinner and enabling the buttons after the
	///     operation.
	/// </remarks>
	public async Task SaveDocType(EditContext editContext)
	{
		await Task.Yield();
		await General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);
	}

	/// <summary>
	///     Shows the dialog of the DocumentTypeDialog component.
	///     This method is used to show the dialog when an add or edit operation in initiated.
	/// </summary>
	internal async Task ShowDialog()
	{
		await Dialog.ShowAsync();
	}

	/// <summary>
	///     Handles the opening event of the tooltip.
	/// </summary>
	/// <param name="args">The arguments for the tooltip event.</param>
	/// <remarks>
	///     This method is triggered when the tooltip is about to open. If the tooltip does not contain any text,
	///     the opening of the tooltip is cancelled.
	/// </remarks>
	public void ToolTipOpen(TooltipEventArgs args)
	{
		args.Cancel = !args.HasText;
	}
}