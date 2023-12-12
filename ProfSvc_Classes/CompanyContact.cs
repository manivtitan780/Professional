#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CompanyContact.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:13
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a contact for a company.
/// </summary>
/// <remarks>
///     This class is used to store and manage information about a company's contact.
///     It includes details such as the contact's name, address, phone number, and associated company details.
/// </remarks>
public class CompanyContact
{
	/// <summary>
	///     Initializes a new instance of the <see cref="CompanyContact" /> class and resets its properties to their default
	///     values.
	/// </summary>
	public CompanyContact()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="CompanyContact" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the CompanyContact.</param>
	/// <param name="clientID">The ID of the client associated with the Company Contact.</param>
	/// <param name="contactName">The name of the contact associated with the company.</param>
	/// <param name="city">The city of the company contact.</param>
	/// <param name="stateID">The identifier for the state associated with the company contact.</param>
	/// <param name="zipCode">The Zip Code of the company contact.</param>
	/// <param name="companyName">The name of the company associated with the contact.</param>
	public CompanyContact(int id, int clientID, string contactName, string city, int stateID, string zipCode, string companyName)
	{
		ID = id;
		ClientID = clientID;
		ContactName = contactName;
		City = city;
		StateID = stateID;
		ZipCode = zipCode;
		CompanyName = companyName;
		FirstName = "";
		LastName = "";
		MiddleName = "";
		EmailAddress = "";
		Extension = "";
		Fax = "";
		Phone = "";
		CellPhone = "";
		TitleID = 0;
		Title = "";
		Department = "";
		Address = "";
		Owner = "";
		StatusCode = "";
		IsPrimary = false;
		State = "";
		Status = "";
		CreatedBy = "";
		CreatedDate = DateTime.MinValue;
		UpdatedBy = "";
		UpdatedDate = DateTime.MinValue;
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="CompanyContact" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the company contact.</param>
	/// <param name="clientID">The identifier for the client associated with the company contact.</param>
	/// <param name="contactName">The name of the contact associated with the company.</param>
	/// <param name="city">The city of the company contact.</param>
	/// <param name="stateID">The identifier for the state associated with the company contact.</param>
	/// <param name="zipCode">The postal code for the location of the company contact.</param>
	/// <param name="companyName">The name of the company.</param>
	/// <param name="firstName">The first name of the contact.</param>
	/// <param name="lastName">The last name of the contact.</param>
	/// <param name="middleName">The middle name of the contact.</param>
	/// <param name="emailAddress">The email address of the contact.</param>
	/// <param name="phone">The phone number of the contact.</param>
	/// <param name="extension">The extension number of the contact's phone.</param>
	/// <param name="fax">The fax number of the contact.</param>
	/// <param name="cellPhone">The cell phone number of the contact.</param>
	/// <param name="titleID">The identifier for the title of the contact.</param>
	/// <param name="title">The title of the contact.</param>
	/// <param name="department">The department of the contact.</param>
	/// <param name="address">The address of the contact.</param>
	/// <param name="owner">The owner of the contact information.</param>
	/// <param name="statusCode">The status code of the contact.</param>
	/// <param name="isPrimary">A value indicating whether the contact is the primary contact for the company.</param>
	/// <param name="state">The state of the contact.</param>
	/// <param name="status">The status of the contact.</param>
	/// <param name="createdBy">The identifier of the user who created the contact.</param>
	/// <param name="createdDate">The date and time when the contact was created.</param>
	/// <param name="updatedBy">The identifier of the user who last updated the contact.</param>
	/// <param name="updatedDate">The date and time when the contact was last updated.</param>
	public CompanyContact(int id, int clientID, string contactName, string city, int stateID, string zipCode, string companyName, string firstName,
						  string lastName, string middleName, string emailAddress, string phone, string extension, string fax, string cellPhone, int titleID,
						  string title, string department, string address, string owner, string statusCode, bool isPrimary, string state, string status,
						  string createdBy, DateTime createdDate, string updatedBy, DateTime updatedDate)
	{
		ID = id;
		ClientID = clientID;
		ContactName = contactName;
		City = city;
		StateID = stateID;
		ZipCode = zipCode;
		CompanyName = companyName;
		FirstName = firstName;
		LastName = lastName;
		MiddleName = middleName;
		EmailAddress = emailAddress;
		Phone = phone;
		Extension = extension;
		Fax = fax;
		CellPhone = cellPhone;
		TitleID = titleID;
		Title = title;
		Department = department;
		Address = address;
		Owner = owner;
		StatusCode = statusCode;
		IsPrimary = isPrimary;
		State = state;
		Status = status;
		CreatedBy = createdBy;
		CreatedDate = createdDate;
		UpdatedBy = updatedBy;
		UpdatedDate = updatedDate;
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="CompanyContact" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the CompanyContact.</param>
	/// <param name="clientID">The ID of the client associated with the Company Contact.</param>
	/// <param name="contactName">The name of the contact associated with the company.</param>
	public CompanyContact(int id, int clientID, string contactName)
	{
		ID = id;
		ClientID = clientID;
		ContactName = contactName;
		City = "";
		CompanyName = "";
		StateID = 0;
		ZipCode = "";
		FirstName = "";
		LastName = "";
		MiddleName = "";
		EmailAddress = "";
		Extension = "";
		Fax = "";
		Phone = "";
		CellPhone = "";
		TitleID = 0;
		Title = "";
		Department = "";
		Address = "";
		Owner = "";
		StatusCode = "";
		IsPrimary = false;
		State = "";
		Status = "";
		CreatedBy = "";
		CreatedDate = DateTime.MinValue;
		UpdatedBy = "";
		UpdatedDate = DateTime.MinValue;
	}

	/// <summary>
	///     Gets or sets the address of the company contact.
	/// </summary>
	/// <value>
	///     The address of the company contact.
	/// </value>
	public string Address
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the cell phone number of the company contact.
	/// </summary>
	/// <value>
	///     The cell phone number of the company contact.
	/// </value>
	public string CellPhone
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the city of the company contact.
	/// </summary>
	/// <value>
	///     The city of the company contact.
	/// </value>
	public string City
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Client ID associated with the Company Contact.
	/// </summary>
	/// <value>
	///     The ID of the client.
	/// </value>
	public int ClientID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the company associated with the contact.
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
	///     Gets or sets the name of the contact associated with the company.
	/// </summary>
	/// <value>
	///     The name of the contact.
	/// </value>
	public string ContactName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the username of the user who created the company contact.
	/// </summary>
	/// <value>
	///     The username of the creator.
	/// </value>
	public string CreatedBy
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date and time when the company contact was created.
	/// </summary>
	/// <value>
	///     The date and time of creation.
	/// </value>
	public DateTime CreatedDate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the department of the company contact.
	/// </summary>
	/// <value>
	///     The department of the company contact.
	/// </value>
	public string Department
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the email address of the company contact.
	/// </summary>
	/// <value>
	///     The email address of the company contact.
	/// </value>
	public string EmailAddress
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the extension number associated with the company contact's phone number.
	/// </summary>
	/// <value>
	///     The extension number as a string.
	/// </value>
	public string Extension
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the fax number for the company contact.
	/// </summary>
	/// <value>
	///     The fax number as a string.
	/// </value>
	public string Fax
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the first name of the company contact.
	/// </summary>
	/// <value>
	///     The first name of the company contact.
	/// </value>
	public string FirstName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the CompanyContact.
	/// </summary>
	/// <value>
	///     The unique identifier for the CompanyContact.
	/// </value>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether this contact is the primary contact for the company.
	/// </summary>
	/// <value>
	///     <c>true</c> if this contact is the primary contact for the company; otherwise, <c>false</c>.
	/// </value>
	public bool IsPrimary
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the last name of the company contact.
	/// </summary>
	/// <value>
	///     The last name of the company contact.
	/// </value>
	public string LastName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the middle name of the company contact.
	/// </summary>
	/// <value>
	///     The middle name of the company contact.
	/// </value>
	public string MiddleName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the owner of the company contact.
	/// </summary>
	/// <value>
	///     The owner of the company contact.
	/// </value>
	public string Owner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number of the company contact.
	/// </summary>
	/// <value>
	///     The phone number of the company contact.
	/// </value>
	public string Phone
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the state of the company contact.
	/// </summary>
	/// <value>
	///     The state of the company contact.
	/// </value>
	public string State
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier for the state associated with the company contact.
	/// </summary>
	/// <value>
	///     The identifier for the state.
	/// </value>
	/// <remarks>
	///     This property is used to link the company contact to a specific state.
	///     It is used in various parts of the application, such as the Companies page and the EditContactDialog.
	/// </remarks>
	public int StateID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status of the company contact.
	/// </summary>
	/// <value>
	///     A string representing the status of the company contact. This could represent various states of the contact, such
	///     as 'Active', 'Inactive', etc.
	/// </value>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status code for the company contact.
	/// </summary>
	/// <value>
	///     A string representing the status code. The status code indicates the current status of the company contact.
	///     For example, "ACT" means the contact is active, while "INA" means the contact is inactive.
	/// </value>
	public string StatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the title of the company contact.
	/// </summary>
	/// <value>
	///     The title of the company contact.
	/// </value>
	public string Title
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier for the title of the company contact.
	/// </summary>
	/// <value>
	///     The identifier for the title.
	/// </value>
	public int TitleID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier of the user who last updated the company contact.
	/// </summary>
	/// <value>
	///     The identifier of the user who last updated the company contact.
	/// </value>
	public string UpdatedBy
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date and time when the CompanyContact was last updated.
	/// </summary>
	public DateTime UpdatedDate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Zip Code of the company contact.
	///     This property is used to store the postal code for the location of the company contact.
	/// </summary>
	public string ZipCode
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the CompanyContact instance to their default values.
	/// </summary>
	public void Clear()
	{
		ID = 0;
		ClientID = 0;
		ContactName = "";
		City = "";
		StateID = 0;
		ZipCode = "";
		CompanyName = "";
		FirstName = "";
		LastName = "";
		MiddleName = "";
		EmailAddress = "";
		Extension = "";
		Fax = "";
		Phone = "";
		CellPhone = "";
		TitleID = 0;
		Title = "";
		Department = "";
		Address = "";
		Owner = "";
		StatusCode = "";
		IsPrimary = false;
		State = "";
		Status = "";
		CreatedBy = "";
		CreatedDate = DateTime.MinValue;
		UpdatedBy = "";
		UpdatedDate = DateTime.MinValue;
	}

	/// <summary>
	///     Creates a shallow copy of the current CompanyContact object.
	/// </summary>
	/// <returns>
	///     A shallow copy of the current CompanyContact object.
	/// </returns>
	public CompanyContact Copy() => MemberwiseClone() as CompanyContact;
}