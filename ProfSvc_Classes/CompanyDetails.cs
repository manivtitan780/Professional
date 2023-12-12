#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CompanyDetails.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:16
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents the details of a company.
/// </summary>
/// <remarks>
///     This class is used to store and manage the details of a company, such as its name, address, contact information,
///     and other relevant details.
/// </remarks>
public class CompanyDetails
{
	/// <summary>
	///     Initializes a new instance of the <see cref="CompanyDetails" /> class and resets its properties to their default
	///     values.
	/// </summary>
	public CompanyDetails()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="CompanyDetails" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the company.</param>
	/// <param name="companyName">The name of the company.</param>
	/// <param name="address">The address of the company.</param>
	/// <param name="city">The city where the company is located.</param>
	/// <param name="stateID">The identifier of the state for the company.</param>
	/// <param name="zipCode">The zip code of the company.</param>
	/// <param name="emailAddress">The email address of the company.</param>
	/// <param name="website">The website URL of the company.</param>
	/// <param name="phone">The phone number of the company.</param>
	/// <param name="extension">The extension number associated with the company's phone number.</param>
	/// <param name="fax">The fax number of the company.</param>
	/// <param name="isHot">A boolean value indicating whether the company is marked as hot.</param>
	/// <param name="owner">The owner of the company.</param>
	/// <param name="statusCode">The status code of the company.</param>
	/// <param name="notes">Any notes associated with the company.</param>
	/// <param name="state">The state of the company.</param>
	/// <param name="status">The status of the company.</param>
	/// <param name="createdBy">The user who created the company record.</param>
	/// <param name="createdDate">The date when the company record was created.</param>
	/// <param name="updatedBy">The user who last updated the company record.</param>
	/// <param name="updatedDate">The date when the company record was last updated.</param>
	public CompanyDetails(int id, string companyName, string address, string city, int stateID, string zipCode, string emailAddress, string website,
						  string phone, string extension, string fax, bool isHot, string owner, string statusCode, string notes, string state,
						  string status, string createdBy, DateTime createdDate, string updatedBy, DateTime updatedDate)
	{
		ID = id;
		CompanyName = companyName;
		Address = address;
		City = city;
		StateID = stateID;
		ZipCode = zipCode;
		EmailAddress = emailAddress;
		Website = website;
		Phone = phone;
		Extension = extension;
		Fax = fax;
		IsHot = isHot;
		Owner = owner;
		StatusCode = statusCode;
		Notes = notes;
		State = state;
		Status = status;
		CreatedBy = createdBy;
		CreatedDate = createdDate;
		UpdatedBy = updatedBy;
		UpdatedDate = updatedDate;
	}

	/// <summary>
	///     Gets or sets the address of the company.
	/// </summary>
	/// <value>
	///     The address of the company.
	/// </value>
	/// <remarks>
	///     This property is used when saving company details in the 'SaveCompany' method of the 'Companies' class.
	///     It is also used in the 'EditContact' and 'SetupAddress' methods of the 'Companies' class.
	/// </remarks>
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
	/// <remarks>
	///     This property is used when saving company details in the 'SaveCompany' method of the 'CompanyController'.
	///     It is also used in the 'EditContact' and 'SetupAddress' methods of the 'Companies' class.
	/// </remarks>
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
	///     Gets or sets the identifier of the user who created the company details.
	/// </summary>
	/// <value>
	///     The identifier of the user who created the company details.
	/// </value>
	public string CreatedBy
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date and time when the company details were created.
	/// </summary>
	/// <value>
	///     The date and time of creation of the company.
	/// </value>
	public DateTime CreatedDate
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
	///     Gets or sets the extension number associated with the company's phone number.
	/// </summary>
	/// <value>
	///     The extension number as a string.
	/// </value>
	/// <remarks>
	///     This property is used when the company's phone number has an extension.
	/// </remarks>
	public string Extension
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the fax number of the company.
	/// </summary>
	/// <value>
	///     The fax number of the company.
	/// </value>
	/// <remarks>
	///     This property is used when saving company details in the 'SaveCompany' method of the 'CompanyController'.
	/// </remarks>
	public string Fax
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the company.
	/// </summary>
	/// <value>
	///     The unique identifier for the company.
	/// </value>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the company is marked as hot.
	/// </summary>
	/// <value>
	///     <c>true</c> if this company is hot; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     A hot company is one that is currently of high interest or priority. This could be due to various factors such as
	///     recent significant growth, strategic importance, or other business-specific criteria.
	/// </remarks>
	public bool IsHot
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the notes for the company.
	/// </summary>
	/// <value>
	///     The notes for the company.
	/// </value>
	public string Notes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the owner of the company.
	/// </summary>
	/// <value>
	///     A string representing the owner of the company.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the owner of the company. It is used in various methods to determine
	///     the ownership of the company.
	/// </remarks>
	public string Owner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number of the company.
	/// </summary>
	/// <value>
	///     The phone number represented as a string.
	/// </value>
	public string Phone
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the state of the company.
	/// </summary>
	/// <value>
	///     A string representing the state where the company is located.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the state of the company.
	///     It is used in various methods to determine the location of the company.
	/// </remarks>
	public string State
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier of the state for the company.
	/// </summary>
	/// <value>
	///     An integer representing the identifier of the state.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the identifier of the state for the company.
	///     It is used in various methods to determine the state of the company.
	/// </remarks>
	public int StateID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status of the company.
	/// </summary>
	/// <value>
	///     A string representing the status of the company.
	/// </value>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status code of the company.
	/// </summary>
	/// <value>
	///     The status code of the company.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the status code of the company. It is used in various methods to
	///     determine the current status of the company.
	/// </remarks>
	public string StatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier of the user who last updated the company details.
	/// </summary>
	public string UpdatedBy
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date and time when the company details were last updated.
	/// </summary>
	public DateTime UpdatedDate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the website of the company.
	/// </summary>
	/// <value>
	///     A string representing the website URL of the company.
	/// </value>
	/// <remarks>
	///     This property is used when saving company details in the 'SaveCompany' method of the 'CompanyController'.
	/// </remarks>
	public string Website
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
	/// <remarks>
	///     This property is used to store and retrieve the zip code of the company. It is used in the `SetupAddress` method to
	///     generate a formatted address string.
	/// </remarks>
	public string ZipCode
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the CompanyDetails instance to their default values.
	/// </summary>
	/// <remarks>
	///     This method is used to clear the current state of the CompanyDetails object. It sets all string properties to an
	///     empty string, all integer properties to zero or one, and all DateTime properties to the current date. The method is
	///     typically used when preparing a CompanyDetails object for reuse.
	/// </remarks>
	public void Clear()
	{
		ID = 0;
		CompanyName = "";
		Address = "";
		City = "";
		StateID = 1;
		ZipCode = "";
		EmailAddress = "";
		Website = "";
		Phone = "";
		Extension = "";
		Fax = "";
		IsHot = false;
		Owner = "";
		StatusCode = "ACT";
		Notes = "";
		State = "Pennsylvania";
		Status = "Active";
		CreatedBy = "ADMIN";
		CreatedDate = DateTime.Today;
		UpdatedBy = "ADMIN";
		UpdatedDate = DateTime.Today;
	}

	/// <summary>
	///     Creates a shallow copy of the current CompanyDetails object.
	/// </summary>
	/// <returns>
	///     A shallow copy of the current CompanyDetails object.
	/// </returns>
	public CompanyDetails Copy() => MemberwiseClone() as CompanyDetails;
}