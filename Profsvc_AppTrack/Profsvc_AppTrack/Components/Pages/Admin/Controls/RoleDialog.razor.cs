#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           RoleDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-06-2022 18:52
// Last Updated On:     09-02-2023 19:02
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing roles in the Admin section of the application.
///     This dialog provides functionality for creating, editing, and saving roles.
/// </summary>
/// <remarks>
///     The dialog includes parameters for handling events such as Cancel and Save,
///     and properties for managing the dialog's state such as the HeaderString and the Role Model.
/// </remarks>
public partial class RoleDialog
{
	/// <summary>
	///     Gets or sets the event to be triggered when the Cancel action is invoked in the RoleDialog.
	/// </summary>
	/// <value>
	///     The event callback that takes MouseEventArgs as a parameter.
	/// </value>
	[Parameter]
	public EventCallback<MouseEventArgs> Cancel
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfDialog component used in the RoleDialog.
	/// </summary>
	/// <value>
	///     The SfDialog component that provides the user interface for managing roles.
	/// </value>
	public SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the DialogFooter component used in the RoleDialog.
	/// </summary>
	/// <value>
	///     The DialogFooter component that provides the user interface for the footer of the dialog,
	///     including the Cancel and Save buttons.
	/// </value>
	private DialogFooter DialogFooter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the form used for editing a role in the RoleDialog.
	/// </summary>
	/// <value>
	///     The form used for editing a role.
	/// </value>
	private EditForm EditRoleForm
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the header string for the RoleDialog.
	///     This property is used to set the title displayed on the dialog.
	/// </summary>
	[Parameter]
	public string HeaderString
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Role model for the RoleDialog.
	/// </summary>
	/// <value>
	///     The Role model represents the role that is currently being created or edited in the dialog.
	/// </value>
	[Parameter]
	public Role Model
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event to be triggered when the Save action is invoked in the RoleDialog.
	/// </summary>
	/// <value>
	///     The event callback that takes EditContext as a parameter. The EditContext represents the context of the form being
	///     edited.
	/// </value>
	[Parameter]
	public EventCallback<EditContext> Save
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfSpinner component used in the RoleDialog.
	/// </summary>
	/// <value>
	///     The SfSpinner component that provides a visual indication of processing when the user performs actions such as
	///     saving or canceling.
	/// </value>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Asynchronously executes the cancellation routine for the RoleDialog.
	/// </summary>
	/// <param name="args">The mouse event arguments associated with the cancel action.</param>
	/// <returns>A task that represents the asynchronous operation of cancelling the role dialog.</returns>
	/// <remarks>
	///     This method calls the General.CallCancelMethod, passing in the necessary components and the Cancel event callback.
	///     The cancellation routine hides the spinner and dialog, and enables the dialog buttons.
	/// </remarks>
	private async Task CancelRole(MouseEventArgs args) => await General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);

	/// <summary>
	///     Opens the dialog for editing a role.
	/// </summary>
	/// <param name="arg">The arguments for the BeforeOpen event.</param>
	/// <remarks>
	///     This method is invoked before the dialog is opened. It ensures that the form is validated before the dialog is
	///     displayed.
	/// </remarks>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private async Task OpenDialog(BeforeOpenEventArgs arg)
	{
		await Task.Yield();
		EditRoleForm.EditContext?.Validate();
	}

	/// <summary>
	///     Asynchronously saves the role using the provided edit context.
	/// </summary>
	/// <param name="editContext">The edit context associated with the save action.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task SaveRole(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);

	/// <summary>
	///     Handles the opening of a tooltip in the RoleDialog.
	/// </summary>
	/// <param name="args">The arguments associated with the tooltip event.</param>
	/// <remarks>
	///     This method cancels the opening of the tooltip if it does not contain any text.
	/// </remarks>
	private static void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}