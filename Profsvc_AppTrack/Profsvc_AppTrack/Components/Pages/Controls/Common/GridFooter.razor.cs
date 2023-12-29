#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           GridFooter.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-28-2023 21:6
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Common;

/// <summary>
///     The GridFooter class is a part of the ProfSvc_AppTrack.Pages.Controls.Common namespace.
///     It is a partial class that is used to create a footer for a grid in the application.
///     This class contains several parameters such as AllAlphabet, AlphabetMethod, ClearFilter, ClearLinkClicked, Count,
///     EndRecord, IsRequisition, LinkClicked, Name, StartRecord, and StatusLinks.
///     These parameters are used to control various aspects of the grid footer such as the alphabet method, clear filter,
///     clear link clicked, count, end record, whether it is a requisition, link clicked, name, start record, and status
///     links.
/// </summary>
public partial class GridFooter
{
	/// <summary>
	///     Gets or sets the event callback that is triggered when the "All" button in the grid footer is clicked.
	///     This event callback is of type <see cref="EventCallback{MouseEventArgs}" />, which means it will provide event data
	///     about the mouse event.
	/// </summary>
	[Parameter]
	public EventCallback<MouseEventArgs> AllAlphabet
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when an alphabet button in the grid footer is clicked.
	///     This event callback is of type <see cref="EventCallback{Char}" />, which means it will provide the clicked alphabet
	///     character as event data.
	/// </summary>
	[Parameter]
	public EventCallback<char> AlphabetMethod
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when the "Clear" button in the grid footer is clicked.
	///     This event callback is of type <see cref="EventCallback{MouseEventArgs}" />, which means it will provide event data
	///     about the mouse event.
	/// </summary>
	[Parameter]
	public EventCallback<MouseEventArgs> ClearFilter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when the "Clear Statuses" button in the grid footer is clicked.
	///     This event callback is of type <see cref="EventCallback" />, which means it will not provide any event data.
	/// </summary>
	[Parameter]
	public EventCallback ClearLinkClicked
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the total count of rows in the grid. This property is of type decimal.
	/// </summary>
	[Parameter]
	public decimal Count
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the end index of the current page in the grid. This property is of type int.
	///     The value of this property is displayed in the grid footer to indicate the range of items being displayed on the
	///     current page.
	///     For example, if the grid is displaying items 10 to 20 out of 100, this property will have the value 20.
	/// </summary>
	[Parameter]
	public int EndRecord
	{
		get;
		set;
	}

    /// <summary>
    ///     Gets or sets a value indicating whether the grid footer is for a requisition.
    ///     This property is of type bool.
    ///     If true, the grid footer will display status links and a "Clear Statuses" button.
    ///     If false, the grid footer will display alphabet buttons, an "All" button, and a "Clear" button.
    /// </summary>
    [Parameter]
	public bool IsRequisition
	{
		get;
		set;
	}

    /// <summary>
    ///     Gets or sets the event callback that is triggered when a status link in the grid footer is clicked.
    ///     This event callback is of type <see cref="EventCallback{String}" />, which means it will provide the key of the
    ///     clicked status link as event data.
    /// </summary>
    [Parameter]
	public EventCallback<string> LinkClicked
	{
		get;
		set;
	}

    /// <summary>
    ///     Gets or sets the name of the currently selected alphabet button in the grid footer.
    ///     This property is of type string.
    ///     The value of this property is used to determine the additional CSS class for the alphabet button.
    ///     If the alphabet button's character matches this property's value, the button will have the "selectedAlphabet" CSS
    ///     class.
    /// </summary>
    [Parameter]
	public string Name
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the start index of the current page in the grid. This property is of type int.
	///     The value of this property is displayed in the grid footer to indicate the range of items being displayed on the
	///     current page.
	///     For example, if the grid is displaying items 10 to 20 out of 100, this property will have the value 10.
	/// </summary>
	[Parameter]
	public int StartRecord
	{
		get;
		set;
	}

    /// <summary>
    ///     Gets or sets the list of status links to be displayed in the grid footer.
    ///     Each status link is represented by an instance of the <see cref="ProfSvc_Classes.KeyValues" /> class.
    ///     The grid footer will display a button for each status link in the list.
    ///     When a status link button is clicked, the <see cref="LinkClicked" /> event callback is triggered with the key of
    ///     the clicked status link as event data.
    /// </summary>
    [Parameter]
	public List<KeyValues> StatusLinks
	{
		get;
		set;
	} = [];
}