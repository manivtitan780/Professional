#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CompanySearch.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:16
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a search operation for companies.
/// </summary>
/// <remarks>
///     This class provides functionality to search for companies based on various parameters such as company name, phone
///     number, email address, state, and status.
///     It also supports pagination and sorting of the search results.
/// </remarks>
public class CompanySearch
{
	/// <summary>
	///     Initializes a new instance of the <see cref="CompanySearch" /> class with the specified parameters and resets its
	///     properties to their default values.
	/// </summary>
	public CompanySearch()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="CompanySearch" /> class with the specified parameters.
	/// </summary>
	/// <param name="companyName">The name of the company to search for.</param>
	/// <param name="phone">The phone number of the company to search for.</param>
	/// <param name="emailAddress">The email address of the company to search for.</param>
	/// <param name="state">The state of the company to search for.</param>
	/// <param name="myCompanies">A boolean value indicating whether to limit the search to the user's companies.</param>
	/// <param name="status">The status of the company to search for.</param>
	/// <param name="page">The page number for the search results.</param>
	/// <param name="itemCount">The number of items per page in the search results.</param>
	/// <param name="sortField">The field to sort the search results by.</param>
	/// <param name="sortDirection">The direction to sort the search results in.</param>
	/// <param name="user">The user associated with the company search.</param>
	public CompanySearch(string companyName, string phone, string emailAddress, string state, bool myCompanies, string status, int page, int itemCount,
						 byte sortField, byte sortDirection, string user)
	{
		CompanyName = companyName;
		Phone = phone;
		EmailAddress = emailAddress;
		State = state;
		MyCompanies = myCompanies;
		Status = status;
		Page = page;
		ItemCount = itemCount;
		SortField = sortField;
		SortDirection = sortDirection;
		User = user;
	}

	/// <summary>
	///     Gets or sets the name of the company.
	/// </summary>
	/// <value>
	///     The name of the company.
	/// </value>
	public string CompanyName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the email address used for the company search.
	/// </summary>
	/// <value>
	///     The email address of the company.
	/// </value>
	public string EmailAddress
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the count of items in the company search.
	/// </summary>
	/// <value>
	///     The count of items.
	/// </value>
	public int ItemCount
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the company search is limited to the user's companies.
	/// </summary>
	/// <value>
	///     <c>true</c> if the search is limited to the user's companies; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     This property is used in the `GetGridCompanies` method in the `CompanyController` class to filter the companies
	///     based on the user's companies. It is also used in the `AdvancedCompanySearch` component for binding the switch
	///     control.
	/// </remarks>
	public bool MyCompanies
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the current page number in the company search results.
	/// </summary>
	/// <value>
	///     The current page number.
	/// </value>
	/// <remarks>
	///     This property is used for pagination in the company search results. It is used in various methods in the
	///     `Companies` class to manage the state of the Companies grid, such as `AllAlphabet()`, `ChangeItemCount()`,
	///     `ClearFilter()`, `FilterGrid()`, `FirstClick()`, `LastClick()`, `NextClick()`, `OnInitializedAsync()`, and
	///     `PageNumberChanged()`.
	/// </remarks>
	public int Page
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number used in the company search.
	/// </summary>
	/// <value>
	///     A string that represents the phone number of the company.
	/// </value>
	/// <remarks>
	///     This property is used to filter the company search results based on the phone number.
	/// </remarks>
	public string Phone
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the sort direction for the company search results.
	/// </summary>
	/// <value>
	///     A byte that represents the sort direction. A value of 1 indicates ascending order, and a value of 0 indicates
	///     descending order.
	/// </value>
	/// <remarks>
	///     This property is used in conjunction with the SortField property to determine the order of the company search
	///     results.
	/// </remarks>
	public byte SortDirection
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the field by which the company search results are sorted.
	/// </summary>
	/// <value>
	///     A byte value that represents the field to sort by. The mapping of byte values to fields is as follows:
	///     1 - Updated, 2 - CompanyName, 3 - Location, 4 - Phone.
	/// </value>
	/// <remarks>
	///     This property is used in conjunction with the SortDirection property to determine the order of the search results.
	/// </remarks>
	public byte SortField
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the state of the company in the search criteria.
	/// </summary>
	/// <value>
	///     The state of the company.
	/// </value>
	/// <remarks>
	///     This property is used to filter the companies based on their state in the search operation.
	/// </remarks>
	public string State
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status of the company search.
	/// </summary>
	/// <value>
	///     A string representing the status of the company search. This can be used to filter the companies based on their
	///     status.
	/// </value>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the user associated with the company search.
	/// </summary>
	/// <value>
	///     The user is represented as a string. This property is used to filter the companies based on the user.
	/// </value>
	public string User
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the CompanySearch instance to their default values.
	/// </summary>
	/// <remarks>
	///     This method is typically used to clear the search criteria in the CompanySearch instance.
	/// </remarks>
	public void Clear()
	{
		CompanyName = "";
		Phone = "";
		EmailAddress = "";
		State = "";
		MyCompanies = false;
		Status = "";
		Page = 1;
		ItemCount = 50;
		SortField = 2;
		SortDirection = 1;
		User = "%";
	}

	/// <summary>
	///     Creates a shallow copy of the current instance of the CompanySearch class.
	/// </summary>
	/// <returns>
	///     A new instance of the CompanySearch class with the same values as the current instance.
	/// </returns>
	public CompanySearch Copy() => MemberwiseClone() as CompanySearch;
}