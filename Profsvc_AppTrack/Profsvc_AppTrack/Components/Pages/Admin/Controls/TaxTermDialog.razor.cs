#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           TaxTermDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-20-2023 21:18
// Last Updated On:     09-02-2023 21:03
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing Tax Terms in the Admin section of the application.
///     This dialog provides functionality for creating, editing, and saving Tax Terms.
///     It includes parameters for handling events such as Cancel and Save, and properties for setting the HeaderString and
///     the Model.
/// </summary>
public partial class TaxTermDialog
{
	/// <summary>
	///     Gets or sets the Cancel event handler. This event is triggered when the user clicks the Cancel button in the
	///     TaxTermDialog.
	///     The associated method should contain the logic to handle the cancellation of the operation, such as closing the
	///     dialog and discarding any unsaved changes.
	/// </summary>
	[Parameter]
	public EventCallback<MouseEventArgs> Cancel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the SfDialog component used in the TaxTermDialog.
	///     This dialog is used to create, edit, and save Tax Terms in the Admin section of the application.
	/// </summary>
	private SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogFooter instance associated with the TaxTermDialog.
	///     The DialogFooter represents the footer of the dialog, containing the Cancel and Save buttons.
	/// </summary>
	private DialogFooter DialogFooter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the EditForm for the TaxTermDialog. This form is used to create or edit Tax Terms.
	///     The form includes fields for the Tax Term Code, Tax Term Text, and a switch for the Tax Term Status.
	///     The form also includes validation for these fields.
	/// </summary>
	private EditForm EditTaxTermForm
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the HeaderString property. This property is used to set the header of the TaxTermDialog.
	///     The value of this property is displayed as the title of the dialog when it is opened.
	/// </summary>
	[Parameter]
	public string HeaderString
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Model of the TaxTermDialog. The Model is of type AdminList and it represents the data being edited
	///     in the dialog.
	///     The Model includes properties such as Code, Text, and IsEnabled which correspond to the Tax Term Code, Tax Term
	///     Text, and the status of the Tax Term respectively.
	/// </summary>
	[Parameter]
	public AdminList Model
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Save event handler. This event is triggered when the user clicks the Save button in the
	///     TaxTermDialog.
	///     The associated method should contain the logic to handle the saving of the operation, such as validating the data
	///     and persisting the changes.
	/// </summary>
	[Parameter]
	public EventCallback<EditContext> Save
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the spinner control used in the tax term dialog.
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
	///     Asynchronously executes the cancellation process for the tax term dialog.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the cancel action.</param>
	/// <remarks>
	///     This method calls the general cancellation method, which hides the spinner and dialog, and enables the dialog
	///     buttons.
	///     It is typically invoked when the user triggers the cancel action in the tax term dialog.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task CancelTaxTerm(MouseEventArgs args) => await General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);

	/// <summary>
	///     Asynchronously prepares the tax term dialog for opening.
	/// </summary>
	/// <param name="args">The arguments associated with the dialog opening event.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method is invoked before the dialog is opened. It initializes the edit context for the tax term form and
	///     validates it.
	/// </remarks>
	private void OpenDialog(BeforeOpenEventArgs args) => EditTaxTermForm.EditContext?.Validate();

	/// <summary>
	///     Asynchronously saves the tax term changes made in the dialog.
	/// </summary>
	/// <param name="editContext">The edit context associated with the save action.</param>
	/// <remarks>
	///     This method calls the general save method, passing in the edit context, spinner, dialog footer, dialog, and save
	///     event callback.
	///     It is typically triggered when the user confirms the save operation in the dialog.
	/// </remarks>
	private async Task SaveTaxTerm(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);

	/// <summary>
	///     Asynchronously displays the tax term dialog.
	/// </summary>
	/// <returns>
	///     A task that represents the asynchronous operation.
	/// </returns>
	/// <remarks>
	///     This method is used to programmatically display the dialog for editing a tax term.
	///     It invokes the ShowAsync method of the SfDialog instance associated with the dialog.
	/// </remarks>
	public async Task ShowDialog() => await Dialog.ShowAsync();
}