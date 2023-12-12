#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           Company.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:12
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a company with properties such as ID, name, address, city, state, zip code, email address, phone number,
///     and owner.
/// </summary>
/// <remarks>
///     This class is used to manage and manipulate company data. It provides constructors for initializing a company with
///     different sets of data,
///     properties for getting and setting company data, and methods for clearing data and creating a copy of a company
///     instance.
/// </remarks>
public class Company
{
	/// <summary>
	///     Initializes a new instance of the <see cref="Company" /> class with the specified values and resets its properties
	///     to their default values.
	/// </summary>
	public Company()
	{
		ClearData();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="Company" /> class with the specified values.
	/// </summary>
	/// <param name="id">The unique identifier for the company.</param>
	/// <param name="companyName">The name of the company.</param>
	/// <param name="address">The address of the company.</param>
	/// <param name="city">The city where the company is located.</param>
	/// <param name="stateId">The identifier of the state where the company is located.</param>
	/// <param name="zipCode">The zip code of the company.</param>
	/// <param name="emailAddress">The email address of the company. Defaults to an empty string if not provided.</param>
	/// <param name="state">The state where the company is located. Defaults to an empty string if not provided.</param>
	/// <param name="phone">The phone number of the company. Defaults to an empty string if not provided.</param>
	/// <param name="owner">The owner of the company. Defaults to an empty string if not provided.</param>
	public Company(int id, string companyName, string address, string city, int stateId, string zipCode, string emailAddress = "", string state = "",
				   string phone = "", string owner = "")
	{
		ID = id;
		CompanyName = companyName;
		Address = address;
		City = city;
		StateID = stateId;
		ZipCode = zipCode;
		EmailAddress = emailAddress;
		State = state;
		Phone = phone;
		Owner = owner;
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="Company" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the company.</param>
	/// <param name="companyName">The name of the company.</param>
	/// <remarks>
	///     This constructor sets the ID and CompanyName properties to the provided values, and initializes other properties to
	///     their default values.
	/// </remarks>
	public Company(int id, string companyName)
	{
		ID = id;
		CompanyName = companyName;
		Address = "";
		City = "";
		StateID = 0;
		ZipCode = "";
		EmailAddress = "";
		State = "";
	}

	/// <summary>
	///     Gets or sets the address of the company.
	/// </summary>
	/// <value>
	///     The address of the company.
	/// </value>
	public string Address
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the city where the company is located.
	/// </summary>
	/// <value>
	///     The city of the company.
	/// </value>
	public string City
	{
		get;
		set;
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
	///     Gets or sets the email address of the company.
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
	///     Gets or sets the unique identifier for the Company.
	/// </summary>
	/// <value>
	///     The unique identifier for the Company.
	/// </value>
	/// <remarks>
	///     This property is used as the primary key in the database and is hidden in the grid view.
	/// </remarks>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the owner of the company.
	/// </summary>
	/// <value>
	///     The owner of the company.
	/// </value>
	public string Owner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number of the company.
	/// </summary>
	/// <value>
	///     The phone number of the company.
	/// </value>
	public string Phone
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the state where the company is located.
	/// </summary>
	/// <value>
	///     The state of the company.
	/// </value>
	public string State
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the state identifier for the company.
	/// </summary>
	/// <value>
	///     The identifier of the state where the company is located.
	/// </value>
	public int StateID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the zip code of the company.
	/// </summary>
	/// <value>
	///     The zip code of the company.
	/// </value>
	public string ZipCode
	{
		get;
		set;
	}

	/// <summary>
	///     Clears all data of the Company instance.
	/// </summary>
	/// <remarks>
	///     This method sets all properties of the Company instance to their default values.
	/// </remarks>
	public void ClearData()
	{
		ID = 0;
		CompanyName = "";
		Address = "";
		City = "";
		StateID = 0;
		ZipCode = "";
		EmailAddress = "";
		State = "";
	}

	/// <summary>
	///     Creates a copy of the current Company instance.
	/// </summary>
	/// <returns>
	///     A new Company object that is a copy of the current instance.
	/// </returns>
	public Company Copy() => MemberwiseClone() as Company;
}