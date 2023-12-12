#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           JobOptionDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-02-2022 19:12
// Last Updated On:     09-02-2023 18:53
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing job options in the admin controls of the application.
///     This dialog provides an interface for creating and editing job options.
/// </summary>
/// <remarks>
///     The dialog includes fields for the job option code, option, description, rate text, percentage text, tax terms,
///     cost percent, and several switches for various requirements.
///     The dialog also includes a save and cancel button for committing or discarding changes.
/// </remarks>
public partial class JobOptionDialog
{
	/// <summary>
	///     Gets or sets the event callback for the cancel action in the job option dialog.
	/// </summary>
	/// <remarks>
	///     This event is triggered when the cancel button is clicked in the job option dialog.
	///     It is used to discard any changes made in the dialog and close it.
	/// </remarks>
	[Parameter]
	public EventCallback<MouseEventArgs> Cancel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfDialog component used in the job option dialog.
	/// </summary>
	/// <remarks>
	///     This component is used to display the dialog for managing job options. It includes various fields for job option
	///     details and buttons for saving or cancelling changes.
	///     The visibility of the dialog is controlled by the ShowAsync and HideAsync methods.
	/// </remarks>
	public SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogFooter instance associated with the JobOptionDialog.
	/// </summary>
	/// <remarks>
	///     The DialogFooter instance represents the footer of the dialog, which contains the Cancel and Save buttons.
	///     This property is used to manage the state and behavior of the dialog's footer.
	/// </remarks>
	private DialogFooter DialogFooter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the form used for editing a job option in the JobOptionDialog.
	/// </summary>
	/// <value>
	///     The form used for editing a job option.
	/// </value>
	private EditForm EditJobOptionForm
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the header string for the JobOptionDialog.
	/// </summary>
	/// <value>
	///     The header string displayed at the top of the dialog.
	/// </value>
	/// <remarks>
	///     This property is bound to the __Header parameter in the dialog's render tree.
	/// </remarks>
	[Parameter]
	public string HeaderString
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the model of the job option for the dialog.
	/// </summary>
	/// <value>
	///     The model of the job option.
	/// </value>
	/// <remarks>
	///     The model is of type <see cref="JobOption" />, which is used to store the Candidate / Requisition Job Option.
	/// </remarks>
	[Parameter]
	public JobOption Model
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback for the save action in the job option dialog.
	/// </summary>
	/// <remarks>
	///     This event is triggered when the save button is clicked in the job option dialog.
	///     It is used to commit any changes made in the dialog and close it.
	///     The event receives an <see cref="EditContext" /> parameter, which provides context for the form being edited.
	/// </remarks>
	[Parameter]
	public EventCallback<EditContext> Save
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfSpinner component used in the job option dialog.
	/// </summary>
	/// <remarks>
	///     This component is used to display a spinner animation while the dialog is processing an action, such as saving or
	///     cancelling.
	///     The visibility of the spinner is controlled by the ShowAsync and HideAsync methods.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the list of tax terms associated with the job option.
	/// </summary>
	/// <value>
	///     The list of tax terms is represented as a list of KeyValues instances.
	/// </value>
	[Parameter]
	public List<KeyValues> TaxTerm
	{
		get;
		set;
	}

	/// <summary>
	///     Asynchronously cancels the current job option operation.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the cancel action.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method invokes the Cancel event, hides the spinner and the dialog.
	/// </remarks>
	private async Task CancelJobOption(MouseEventArgs args)
	{
		await General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);
	}

	/// <summary>
	///     Asynchronously opens the job option dialog.
	/// </summary>
	/// <param name="arg">
	///     An instance of <see cref="BeforeOpenEventArgs" /> which contains event data.
	/// </param>
	/// <remarks>
	///     This method is triggered before the dialog is opened. It yields control back to the caller before
	///     validating the form context of the `EditJobOptionForm`.
	/// </remarks>
	private async Task OpenDialog(BeforeOpenEventArgs arg)
	{
		await Task.Yield();
		EditJobOptionForm.EditContext?.Validate();
	}

	/// <summary>
	///     Asynchronously saves the job option changes made in the dialog.
	/// </summary>
	/// <param name="editContext">
	///     The context for the form being edited, which includes the model and the validation status.
	/// </param>
	/// <remarks>
	///     This method is triggered when the save button is clicked in the job option dialog.
	///     It first checks if the dialog footer buttons are not disabled, then shows a spinner, disables the buttons, invokes
	///     the save event, and finally hides the spinner and the dialog.
	///     This method is designed to be used with Blazor's EditForm component and its EditContext.
	/// </remarks>
	private async Task SaveJobOption(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);
}