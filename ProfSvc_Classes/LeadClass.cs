#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           LeadClass.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-21-2023 16:28
// Last Updated On:     10-26-2023 21:18
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a lead in the Professional Services system.
/// </summary>
/// <remarks>
///     A lead is a potential sales contact, an individual or organization that expresses an interest in your goods or
///     services.
///     Leads are typically obtained through the referral of an existing customer, or through a direct response to
///     advertising or publicity.
/// </remarks>
[Serializable]
public class LeadClass
{
	/// <summary>
	///     Initializes a new instance of the <see cref="LeadClass" /> class and resets its properties to their default values.
	/// </summary>
	public LeadClass()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="LeadClass" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the lead.</param>
	/// <param name="company">The name of the company that the lead belongs to.</param>
	/// <param name="contact">The name of the person or entity that is the point of contact for the lead.</param>
	/// <param name="phone">The phone number of the lead.</param>
	/// <param name="location">The location of the lead, typically in the format of "City, State".</param>
	/// <param name="industry">
	///     The industry of the lead, used to categorize the lead into a specific sector or type of
	///     business.
	/// </param>
	/// <param name="status">The status of the lead, used to track the progress of the lead in the sales pipeline.</param>
	/// <param name="lastUpdated">The last updated date and user of the lead, in the format of "Date [User]".</param>
	/// <param name="owner">The owner of the lead, the person or entity responsible for managing the lead.</param>
	public LeadClass(int id, string company, string contact, string phone, string location, string industry, string status, string lastUpdated,
					 string owner)
	{
		ID = id;
		Company = company;
		Contact = contact;
		Phone = phone;
		Location = location;
		Industry = industry;
		Status = status;
		LastUpdated = lastUpdated;
		Owner = owner;
	}

	/// <summary>
	///     Gets or sets the company of the lead.
	/// </summary>
	/// <value>
	///     A string representing the company of the lead.
	/// </value>
	/// <remarks>
	///     The Company property is used to store the name of the company that the lead belongs to.
	/// </remarks>
	public string Company
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the contact name of the lead.
	/// </summary>
	/// <value>
	///     A string representing the contact name of the lead.
	/// </value>
	/// <remarks>
	///     The Contact property is used to store the name of the person or entity that is the point of contact for the lead.
	/// </remarks>
	public string Contact
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID of the Lead.
	/// </summary>
	/// <value>
	///     The unique identifier for the Lead.
	/// </value>
	/// <remarks>
	///     This property is used as the primary key in the Leads table and is hidden from the user interface.
	///     It is used internally for operations such as Lead conversion, note deletion, and data binding.
	/// </remarks>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the industry of the lead.
	/// </summary>
	/// <value>
	///     A string representing the industry of the lead.
	/// </value>
	/// <remarks>
	///     The Industry property is used to categorize the lead into a specific sector or type of business.
	/// </remarks>
	public string Industry
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the last updated date and user of the lead.
	/// </summary>
	/// <value>
	///     A string representing the last updated date and user of the lead.
	/// </value>
	/// <remarks>
	///     The LastUpdated property is in the format of "Date [User]".
	/// </remarks>
	public string LastUpdated
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the location of the lead.
	/// </summary>
	/// <value>
	///     A string representing the location of the lead.
	/// </value>
	/// <remarks>
	///     The location is typically in the format of "City, State".
	/// </remarks>
	public string Location
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the owner of the lead.
	/// </summary>
	/// <value>
	///     A string representing the owner of the lead.
	/// </value>
	/// <remarks>
	///     The owner of a lead is the person or entity responsible for managing the lead.
	/// </remarks>
	public string Owner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number of the lead.
	/// </summary>
	/// <value>
	///     A string representing the phone number of the lead.
	/// </value>
	/// <remarks>
	///     The phone number is used for contact purposes.
	/// </remarks>
	public string Phone
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
	///     The status of a lead is used to track the progress of the lead in the sales pipeline.
	/// </remarks>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the LeadClass instance to their default values.
	/// </summary>
	public void Clear()
	{
		ID = 0;
		Contact = "";
		Phone = "";
		Location = "";
		LastUpdated = "";
		Status = "New";
	}

	/// <summary>
	///     Creates a shallow copy of the current LeadClass object.
	/// </summary>
	/// <returns>
	///     A new LeadClass object with the same value as the current LeadClass object.
	/// </returns>
	public LeadClass Copy() => MemberwiseClone() as LeadClass;
}