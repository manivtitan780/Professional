#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           LeadDetails.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-22-2023 21:07
// Last Updated On:     10-26-2023 21:18
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents the details of a lead in the Professional Services system.
/// </summary>
/// <remarks>
///     This class is used to store and manage the details of a lead, including personal information, company details, and
///     lead status.
///     It includes properties for each detail and methods for initializing the class.
/// </remarks>
public class LeadDetails
{
	/// <summary>
	///     Initializes a new instance of the <see cref="LeadDetails" /> class and resets its properties to their default
	///     values.
	/// </summary>
	public LeadDetails()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="LeadDetails" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the LeadDetails instance.</param>
	/// <param name="company">The name of the company associated with the lead.</param>
	/// <param name="firstName">The first name of the lead.</param>
	/// <param name="lastName">The last name of the lead.</param>
	/// <param name="email">The email address of the lead.</param>
	/// <param name="phone">The phone number associated with the lead.</param>
	/// <param name="street">The street address of the lead.</param>
	/// <param name="city">The city of the lead.</param>
	/// <param name="state">The state identifier for the lead.</param>
	/// <param name="zipCode">The zip code of the lead.</param>
	/// <param name="noOfEmployees">The number of employees in the lead's company.</param>
	/// <param name="revenue">The revenue of the lead's company.</param>
	/// <param name="source">The source from which the lead was obtained.</param>
	/// <param name="industry">The industry of the lead's company.</param>
	/// <param name="description">A description of the lead.</param>
	/// <param name="website">The website of the lead's company.</param>
	/// <param name="currentStatus">The current status of the lead.</param>
	/// <param name="createdBy">The user who created the lead.</param>
	/// <param name="createdDate">The date when the lead was created.</param>
	/// <param name="updatedBy">The user who last updated the lead.</param>
	/// <param name="updatedDate">The date when the lead was last updated.</param>
	/// <param name="leadSource">The source from which the lead was obtained.</param>
	/// <param name="leadIndustry">The industry of the lead's company.</param>
	/// <param name="leadStatus">The current status of the lead.</param>
	/// <param name="stateName">The name of the state of the lead.</param>
	public LeadDetails(int id, string company, string firstName, string lastName, string email, string phone, string street, string city,
					   int state, string zipCode, int noOfEmployees, decimal revenue, byte source, byte industry, string description, string website,
					   byte currentStatus, string createdBy, DateTime createdDate, string updatedBy, DateTime updatedDate, string leadSource, string leadIndustry, string leadStatus,
					   string stateName)
	{
		ID = id;
		Company = company;
		FirstName = firstName;
		LastName = lastName;
		Email = email;
		Phone = phone;
		Street = street;
		City = city;
		State = state;
		ZipCode = zipCode;
		NoOfEmployees = noOfEmployees;
		Revenue = revenue;
		Source = source;
		Industry = industry;
		Description = description;
		Website = website;
		CurrentStatus = currentStatus;
		CreatedBy = createdBy;
		CreatedDate = createdDate;
		UpdatedBy = updatedBy;
		UpdatedDate = updatedDate;
		LeadSource = leadSource;
		LeadIndustry = leadIndustry;
		LeadStatus = leadStatus;
		StateName = stateName;
	}

	/// <summary>
	///     Gets or sets the city of the lead.
	/// </summary>
	/// <value>
	///     The city of the lead.
	/// </value>
	/// <remarks>
	///     This property is used in the ProfSvc_AppTrack.Pages.Controls.Leads.BasicLeadsPanel.ShowAddress method to generate a
	///     formatted address string.
	///     It is also bound to a TextBoxControl in the ProfSvc_AppTrack.Pages.Controls.Leads.EditLeadDetails component for
	///     editing.
	/// </remarks>
	public string City
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the company associated with the lead.
	/// </summary>
	/// <value>
	///     The name of the company.
	/// </value>
	public string Company
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the user who created the lead.
	/// </summary>
	/// <value>
	///     The name of the user who created the lead.
	/// </value>
	public string CreatedBy
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date and time when the lead was created.
	/// </summary>
	public DateTime CreatedDate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the current status of the lead.
	/// </summary>
	/// <value>
	///     The current status of the lead.
	/// </value>
	/// <remarks>
	///     This property is used to track the current status of the lead. The value is a byte, where each value represents a
	///     different status.
	/// </remarks>
	public byte CurrentStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the description of the lead.
	/// </summary>
	/// <value>
	///     The description of the lead.
	/// </value>
	public string Description
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the email address of the lead.
	/// </summary>
	/// <value>
	///     The email address of the lead.
	/// </value>
	public string Email
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the first name of the lead.
	/// </summary>
	/// <value>
	///     The first name of the lead.
	/// </value>
	/// <remarks>
	///     This property is validated to be not empty and its length should be between 2 and 50 characters.
	/// </remarks>
	public string FirstName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the LeadDetails instance.
	/// </summary>
	/// <value>
	///     The unique identifier for the LeadDetails instance.
	/// </value>
	/// <remarks>
	///     This property is used to uniquely identify a LeadDetails instance. It is typically set by the database when a new
	///     lead is created.
	/// </remarks>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the industry code of the lead.
	/// </summary>
	/// <value>
	///     The industry code of the lead.
	/// </value>
	/// <remarks>
	///     This property represents the industry sector to which the lead belongs, encoded as a byte value.
	/// </remarks>
	public byte Industry
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the lead details are being added.
	/// </summary>
	/// <value>
	///     <c>true</c> if this instance represents adding new lead details; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     This property is used to distinguish between adding new lead details and editing existing ones.
	/// </remarks>
	public bool IsAdd
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the last name of the lead.
	/// </summary>
	/// <value>
	///     The last name of the lead.
	/// </value>
	/// <remarks>
	///     This property is validated to be not empty and its length should be between 2 and 50 characters.
	/// </remarks>
	public string LastName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the industry of the lead.
	/// </summary>
	/// <value>
	///     The industry of the lead.
	/// </value>
	/// <remarks>
	///     This property represents the industry sector to which the lead belongs.
	/// </remarks>
	public string LeadIndustry
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the source of the lead.
	/// </summary>
	/// <value>
	///     The source of the lead.
	/// </value>
	/// <remarks>
	///     This property is used to track where the lead originated from,
	///     such as a marketing campaign, a referral, or other sources.
	/// </remarks>
	public string LeadSource
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status of the lead.
	/// </summary>
	/// <value>
	///     A string representing the status of the lead.
	/// </value>
	/// <remarks>
	///     This property is used to track the current status of a lead in the system.
	///     It can be used to filter and sort leads based on their status.
	/// </remarks>
	public string LeadStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the number of employees in the lead's company.
	/// </summary>
	/// <value>
	///     The number of employees.
	/// </value>
	public int NoOfEmployees
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number associated with the lead.
	/// </summary>
	/// <value>
	///     The phone number as a string.
	/// </value>
	public string Phone
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the revenue associated with the lead.
	/// </summary>
	/// <value>
	///     The revenue in decimal format.
	/// </value>
	public decimal Revenue
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the source identifier for the lead.
	/// </summary>
	/// <value>
	///     The source identifier for the lead.
	/// </value>
	/// <remarks>
	///     This property is used to store the identifier of the source associated with the lead.
	///     It is used in various parts of the application, such as saving the lead's source in the database
	///     and setting the source value when editing lead details.
	/// </remarks>
	public byte Source
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the state identifier for the lead.
	/// </summary>
	/// <value>
	///     The state identifier for the lead.
	/// </value>
	/// <remarks>
	///     This property is used to store the identifier of the state associated with the lead.
	///     It is used in various parts of the application, such as saving the lead's state in the database
	///     and setting the state value when editing lead details.
	/// </remarks>
	public int State
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the state for the lead.
	/// </summary>
	/// <value>
	///     The name of the state for the lead.
	/// </value>
	/// <remarks>
	///     This property is used in various parts of the application, such as displaying the lead's address in the user
	///     interface,
	///     and setting the state value when editing lead details.
	/// </remarks>
	public string StateName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the street address of the lead.
	/// </summary>
	/// <value>
	///     The street address of the lead.
	/// </value>
	/// <remarks>
	///     This property is used in various parts of the application, including saving lead details to the database and
	///     displaying the lead's address in the user interface.
	/// </remarks>
	public string Street
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier of the user who last updated the lead details.
	/// </summary>
	/// <value>
	///     The identifier of the user who last updated the lead details.
	/// </value>
	public string UpdatedBy
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date and time when the lead details were last updated.
	/// </summary>
	public DateTime UpdatedDate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the website associated with the lead.
	/// </summary>
	/// <value>
	///     The website URL of the lead.
	/// </value>
	/// <remarks>
	///     This property is validated to ensure it is a valid URL and does not exceed 255 characters in length.
	/// </remarks>
	public string Website
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the zip code for the lead.
	/// </summary>
	/// <value>
	///     The zip code as a string.
	/// </value>
	/// <remarks>
	///     This property is used in the ProfSvc_AppTrack.Pages.Controls.Leads.BasicLeadsPanel.ShowAddress method
	///     to generate a formatted address string, and in the ProfSvc_AppTrack.Pages.Controls.Leads.EditLeadDetails component
	///     for data binding.
	/// </remarks>
	public string ZipCode
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the LeadDetails instance to their default values.
	/// </summary>
	/// <remarks>
	///     This method is used to clear the current state of the LeadDetails object. It sets all properties to their default
	///     values, such as 0 for integers, empty string for strings, and 'ADMIN' for CreatedBy and UpdatedBy properties. The
	///     CurrentStatus is set to 1, and the CreatedDate and UpdatedDate are set to the current date.
	/// </remarks>
	public void Clear()
	{
		ID = 0;
		Company = "";
		FirstName = "";
		LastName = "";
		Email = "";
		Phone = "";
		Street = "";
		City = "";
		State = 0;
		ZipCode = "";
		NoOfEmployees = 0;
		Revenue = 0M;
		Source = 0;
		Industry = 0;
		Description = "";
		Website = "";
		CurrentStatus = 1;
		CreatedBy = "ADMIN";
		CreatedDate = DateTime.Today;
		UpdatedBy = "ADMIN";
		UpdatedDate = DateTime.Today;
		LeadSource = "";
		LeadIndustry = "";
		LeadStatus = "";
		StateName = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current LeadDetails object.
	/// </summary>
	/// <returns>
	///     A new LeadDetails object that is a shallow copy of this instance.
	/// </returns>
	public LeadDetails Copy() => MemberwiseClone() as LeadDetails;
}