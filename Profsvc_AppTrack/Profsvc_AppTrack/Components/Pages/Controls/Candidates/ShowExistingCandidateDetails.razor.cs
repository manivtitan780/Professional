#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           ShowExistingCandidateDetails.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-28-2023 21:1
// *****************************************/

#endregion

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a dialog for displaying and managing existing candidates.
/// </summary>
/// <remarks>
///     This class is a Blazor component that displays a dialog with a list of existing candidates.
///     The dialog includes a list box that displays the name, email, and phone number of each candidate.
///     It also includes a button for continuing the parsing process.
///     The dialog can be shown or hidden using the ShowDialog and HideDialog methods, respectively.
///     The EnableButton and DisableButton methods can be used to control the state of the continue parsing button.
/// </remarks>
public partial class ShowExistingCandidateDetails
{
	/// <summary>
	///     Gets or sets the event callback that is invoked when the parsing process is continued.
	/// </summary>
	/// <remarks>
	///     This event callback is triggered in the Parse method, when the SaveButton is not disabled.
	///     It allows for custom logic to be executed when the parsing process is continued.
	/// </remarks>
	[Parameter]
	public EventCallback<MouseEventArgs> ContinueParsing
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfDialog component used for displaying the existing candidates.
	/// </summary>
	/// <remarks>
	///     This SfDialog component is used to display a dialog box containing a list of existing candidates.
	///     The dialog box can be shown or hidden using the ShowDialog and HideDialog methods, respectively.
	/// </remarks>
	private SfDialog Dialog
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a list of existing candidates to be displayed in the dialog.
	/// </summary>
	/// <remarks>
	///     This property is a list of ExistingCandidate objects. Each ExistingCandidate object represents a candidate with
	///     their name, email, and phone number.
	///     The list is used as a data source for the ListBox in the dialog, displaying the details of each candidate.
	///     The list can be populated externally, allowing for dynamic updating of the displayed candidates.
	/// </remarks>
	[Parameter]
	public List<ExistingCandidate> ExistingCandidates
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the ListBox control used to display the list of existing candidates.
	/// </summary>
	/// <remarks>
	///     This property is a reference to the ListBox control in the dialog, which displays the details of each candidate.
	///     The ListBox is populated using the ExistingCandidates property as its data source.
	///     Each item in the ListBox represents an ExistingCandidate object, displaying the candidate's name, email, and phone
	///     number.
	/// </remarks>
	private SfListBox<int, ExistingCandidate> ListBox
	{
		get;
		set;
	}

	private bool SaveDisabled
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfSpinner component used for indicating a loading state during the parsing process.
	/// </summary>
	/// <remarks>
	///     This SfSpinner component is used to display a loading spinner when the parsing process is ongoing.
	///     The spinner is shown when the Parse method is called and hidden when the parsing process is completed.
	/// </remarks>
	private SfSpinner Spinner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier of the selected candidate.
	/// </summary>
	/// <remarks>
	///     This property represents the identifier of the candidate selected in the dialog.
	///     It is used as a parameter in a POST request to the "Candidates/SaveParsedData" endpoint
	///     to continue the parsing process for the specific candidate.
	/// </remarks>
	[Parameter]
	public int Value
	{
		get;
		set;
	}

	/// <summary>
	///     Disables the SaveButton component.
	/// </summary>
	/// <remarks>
	///     This method sets the Disabled property of the SaveButton component to true,
	///     preventing the user from triggering the parsing process by clicking the button.
	/// </remarks>
	public void DisableButton()
	{
		SaveDisabled = true;
	}

	/// <summary>
	///     Enables the SaveButton component.
	/// </summary>
	/// <remarks>
	///     This method sets the Disabled property of the SaveButton component to false,
	///     allowing the user to trigger the parsing process by clicking the button.
	/// </remarks>
	public void EnableButton() => SaveDisabled = false;

	/// <summary>
	///     Hides the dialog for displaying and managing existing candidates.
	/// </summary>
	/// <remarks>
	///     This method invokes the HideAsync method of the SfDialog component,
	///     effectively hiding the dialog from the user.
	///     It is an asynchronous method, meaning it returns a Task that represents the asynchronous operation.
	/// </remarks>
	public Task HideDialog() => Dialog.HideAsync();

	/// <summary>
	///     Handles the modal click event in the dialog.
	/// </summary>
	/// <param name="arg">The arguments associated with the modal click event.</param>
	/// <remarks>
	///     This method is invoked when the modal overlay is clicked. It hides the dialog asynchronously.
	/// </remarks>
	private Task ModalClick(OverlayModalClickEventArgs arg) => HideDialog();

	/// <summary>
	///     Initiates the parsing process for the selected candidate.
	/// </summary>
	/// <remarks>
	///     This method is invoked when the SaveButton component is clicked, and it's not disabled.
	///     It shows a spinner, invokes the ContinueParsing event callback, hides the spinner, and finally hides the dialog.
	///     This method is asynchronous, meaning it returns a Task that represents the asynchronous operation.
	/// </remarks>
	/// <param name="args">The Mouse event arguments associated with the SaveButton click event.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	private async Task Parse(MouseEventArgs args)
	{
		if (!SaveDisabled)
		{
			await Spinner.ShowAsync();
			await ContinueParsing.InvokeAsync(args);
			await Spinner.HideAsync();
			await Dialog.HideAsync();
		}
	}

	/// <summary>
	///     Shows the dialog for displaying and managing existing candidates.
	/// </summary>
	/// <remarks>
	///     This method invokes the ShowAsync method of the SfDialog component,
	///     effectively displaying the dialog to the user.
	///     It is an asynchronous method, meaning it returns a Task that represents the asynchronous operation.
	/// </remarks>
	public void ShowDialog() => Dialog.ShowAsync();
}