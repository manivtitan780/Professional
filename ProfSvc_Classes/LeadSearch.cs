#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           LeadSearch.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-21-2023 18:42
// Last Updated On:     10-26-2023 21:18
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a search operation for leads in the system.
/// </summary>
/// <remarks>
///     This class provides functionality to perform a search operation for leads based on various parameters such as name,
///     page number, item count, sort field, sort direction, and user.
///     It also provides functionality to reset the search parameters to their default values and to create a copy of the
///     current LeadSearch instance.
/// </remarks>
public class LeadSearch
{
	/// <summary>
	///     Initializes a new instance of the <see cref="LeadSearch" /> class with the specified parameters and resets its
	///     properties to their default values.
	/// </summary>
	public LeadSearch()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="LeadSearch" /> class with the specified parameters.
	/// </summary>
	/// <param name="name">The name for the LeadSearch.</param>
	/// <param name="page">The page number for the lead search.</param>
	/// <param name="itemCount">The total number of items that match the search criteria in a LeadSearch operation.</param>
	/// <param name="sortField">The field by which the lead search results are sorted.</param>
	/// <param name="sortDirection">
	///     The sort direction for the LeadSearch. A byte where 1 indicates ascending order and 0
	///     indicates descending order.
	/// </param>
	/// <param name="user">The user who is performing the lead search.</param>
	public LeadSearch(string name, int page, int itemCount, byte sortField, byte sortDirection, string user)
	{
		Name = name;
		Page = page;
		ItemCount = itemCount;
		SortField = sortField;
		SortDirection = sortDirection;
		User = user;
	}

	/// <summary>
	///     Gets or sets the total number of items that match the search criteria in a LeadSearch operation.
	/// </summary>
	/// <value>
	///     The total number of items that match the search criteria.
	/// </value>
	/// <remarks>
	///     This property is used in conjunction with the Page property to implement pagination in LeadSearch operations.
	/// </remarks>
	public int ItemCount
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name for the LeadSearch.
	/// </summary>
	/// <value>
	///     The name of the LeadSearch.
	/// </value>
	public string Name
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the page number for the lead search.
	/// </summary>
	/// <value>
	///     The page number.
	/// </value>
	/// <remarks>
	///     This property is used to specify the page number when retrieving leads.
	///     It is used in conjunction with the ItemCount property to determine the range of leads to retrieve.
	///     For example, if Page is set to 2 and ItemCount is set to 10, the method retrieving the leads would skip the first
	///     10 leads and return the next 10.
	/// </remarks>
	public int Page
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the sort direction for the LeadSearch.
	/// </summary>
	/// <value>
	///     The sort direction is represented as a byte where 1 indicates ascending order and 0 indicates descending order.
	/// </value>
	/// <remarks>
	///     This property is used to determine the order in which leads are displayed.
	///     It is used in conjunction with the SortField property to sort the leads based on a specific field in either
	///     ascending or descending order.
	/// </remarks>
	public byte SortDirection
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the field by which the lead search results are sorted.
	/// </summary>
	/// <value>
	///     A byte value that represents the field by which the lead search results are sorted.
	///     The value corresponds to the following fields: 1 - Company, 2 - Location, 3 - Industry, 4 - Status, 5 - Updated.
	/// </value>
	/// <remarks>
	///     This property is used in conjunction with the SortDirection property to determine the order of the lead search
	///     results.
	/// </remarks>
	public byte SortField
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the User property of the LeadSearch class.
	/// </summary>
	/// <value>
	///     A string representing the user who is performing the lead search.
	/// </value>
	/// <remarks>
	///     This property is used to store the UserID of the user who is performing the search.
	///     It can be used to track or restrict search operations based on user permissions.
	/// </remarks>
	public string User
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the LeadSearch instance to their default values.
	/// </summary>
	/// <remarks>
	///     This method is used to clear the search parameters in the LeadSearch instance.
	///     It sets the Name property to an empty string, the Page property to 1, the ItemCount property to 25,
	///     the SortField property to 1, the SortDirection property to 0, and the User property to "ADMIN".
	/// </remarks>
	public void Clear()
	{
		Name = "";
		Page = 1;
		ItemCount = 25;
		SortField = 1;
		SortDirection = 0;
		User = "ADMIN";
	}

	/// <summary>
	///     Creates a copy of the current LeadSearch instance.
	/// </summary>
	/// <returns>
	///     A new LeadSearch object that is a copy of the current instance.
	/// </returns>
	public LeadSearch Copy() => MemberwiseClone() as LeadSearch;
}