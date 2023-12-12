#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           AdminGridFooter.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-06-2023 15:26
// Last Updated On:     09-01-2023 15:14
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents the footer section of an administrative grid in the application.
///     This component displays the total count of items in the grid.
/// </summary>
public partial class AdminGridFooter
{
	/// <summary>
	///     Gets or sets the total count of rows in the administrative grid.
	///     This value is used to display the total number of items in the grid footer.
	/// </summary>
	[Parameter]
	public int Count
	{
		get;
		set;
	}
}