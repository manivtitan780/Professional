#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           ShowAddChoice.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-28-2023 16:35
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a dialog for adding a new candidate in the application.
/// </summary>
/// <remarks>
///     This dialog provides two options for creating a new candidate:
///     - Manual: The user can manually enter the details of the candidate.
///     - Parse Resume: The user can upload a resume which will be parsed to extract the details of the candidate.
/// </remarks>
public partial class ShowAddChoice
{
	/// <summary>
	///     Gets or sets the event callback that is invoked when the 'Manual' button is clicked.
	/// </summary>
	/// <value>
	///     The event callback for the 'Manual' button click event.
	/// </value>
	/// <remarks>
	///     This event is used to trigger the process of manually adding a new candidate's details.
	/// </remarks>
	[Parameter]
	public EventCallback<MouseEventArgs> ClickManual
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is invoked when the 'Parse Resume' button is clicked.
	/// </summary>
	/// <value>
	///     The event callback for the 'Parse Resume' button click event.
	/// </value>
	/// <remarks>
	///     This event is used to trigger the process of parsing a resume to add a new candidate's details.
	/// </remarks>
	[Parameter]
	public EventCallback<MouseEventArgs> ClickParse
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the dialog component used for adding a new candidate.
	/// </summary>
	/// <value>
	///     The dialog component.
	/// </value>
	/// <remarks>
	///     This dialog is shown when the user chooses to add a new candidate. It provides options for manual entry or parsing
	///     a resume.
	/// </remarks>
	private SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the 'Manual' button component used in the dialog for adding a new candidate.
	/// </summary>
	/// <value>
	///     The 'Manual' button component.
	/// </value>
	/// <remarks>
	///     This button, when clicked, triggers the process of manually adding a new candidate's details.
	/// </remarks>
	private SfButton ManualButton
	{
		get;
		set;
	}

	private bool ManualDisabled
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the 'Parse Resume' button component used in the dialog for adding a new candidate.
	/// </summary>
	/// <value>
	///     The 'Parse Resume' button component.
	/// </value>
	/// <remarks>
	///     This button, when clicked, triggers the process of parsing a resume to add a new candidate's details.
	/// </remarks>
	private SfButton ParseButton
	{
		get;
		set;
	}

	private bool ParseDisabled
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Spinner control used in the dialog.
	/// </summary>
	/// <value>
	///     The Spinner control.
	/// </value>
	/// <remarks>
	///     This control is used to display a loading spinner while the dialog is processing an action.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Handles the click event of the 'Manual' button in the dialog for adding a new candidate.
	/// </summary>
	/// <param name="args">The arguments of the mouse event.</param>
	/// <remarks>
	///     This method is invoked when the 'Manual' button is clicked. It disables the 'Manual' and 'Parse Resume' buttons,
	///     invokes the event callback for the 'Manual' button click event, re-enables the buttons, and hides the dialog.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task Manual(MouseEventArgs args)
	{
		if (!ManualButton.Disabled)
		{
			ManualDisabled = true;
			ParseDisabled = true;
			await ClickManual.InvokeAsync(args);
			ManualDisabled = false;
			ParseDisabled = false;
			await Dialog.HideAsync();
		}
	}

	/// <summary>
	///     Handles the click event on the modal overlay.
	/// </summary>
	/// <param name="arg">The arguments associated with the overlay modal click event.</param>
	/// <remarks>
	///     This method is invoked when the user clicks outside the dialog on the overlay. It hides the dialog.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task ModalClick(OverlayModalClickEventArgs arg) => await Dialog.HideAsync();

	/// <summary>
	///     Handles the click event of the 'Parse Resume' button.
	/// </summary>
	/// <param name="args">The arguments of the mouse event.</param>
	/// <remarks>
	///     This method is invoked when the 'Parse Resume' button is clicked. It disables the 'Manual' and 'Parse Resume'
	///     buttons,
	///     invokes the 'Parse Resume' click event, and then re-enables the buttons. If the 'Manual' button is not disabled,
	///     it hides the dialog after the operation.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task Parse(MouseEventArgs args)
	{
		if (!ManualButton.Disabled)
		{
			await Task.Yield();
			ManualDisabled = true;
			ParseDisabled = true;
			await ClickParse.InvokeAsync(args);
			ManualDisabled = false;
			ParseDisabled = false;
			await Dialog.HideAsync();
		}
	}

	/// <summary>
	///     Displays the dialog for adding a new candidate.
	/// </summary>
	/// <remarks>
	///     This method is used to asynchronously show the dialog which provides options for adding a new candidate either
	///     manually or by parsing a resume.
	/// </remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async Task ShowDialog() => await Dialog.ShowAsync();
}