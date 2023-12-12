﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           Candidates.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:09
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a candidate in the professional services context.
/// </summary>
/// <remarks>
///     This class is used to manage and manipulate candidate data, including their personal information, status, and
///     resumes.
/// </remarks>
public class Candidates
{
	/// <summary>
	///     Initializes a new instance of the <see cref="Candidates" /> class.
	/// </summary>
	public Candidates()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="Candidates" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the candidate.</param>
	/// <param name="name">The name of the candidate.</param>
	/// <param name="phone">The phone number of the candidate.</param>
	/// <param name="email">The email address of the candidate.</param>
	/// <param name="location">The location of the candidate.</param>
	/// <param name="updated">The date and time when the candidate's information was last updated.</param>
	/// <param name="status">The status of the candidate.</param>
	/// <param name="mpc">A value indicating whether the candidate is an MPC (Most Placeable Candidate).</param>
	/// <param name="rating">The rating of the candidate.</param>
	/// <param name="originalResume">A value indicating whether the original resume of the candidate exists.</param>
	/// <param name="formattedResume">A value indicating whether the formatted resume of the candidate exists.</param>
	public Candidates(int id, string name, string phone, string email, string location, string updated, string status, bool mpc,
					  byte rating, bool originalResume, bool formattedResume)
	{
		ID = id;
		Name = name;
		Phone = phone;
		Email = email;
		Location = location;
		Updated = updated;
		Status = status;
		Mpc = mpc;
		Rating = rating;
		OriginalResume = originalResume;
		FormattedResume = formattedResume;
		//Notes = notes;
	}

	/// <summary>
	///     Gets or sets the email address of the candidate.
	/// </summary>
	/// <value>
	///     The email address of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used in the Candidate.razor page to display and sort the candidates by their email addresses.
	/// </remarks>
	public string Email
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the formatted resume of the candidate exists.
	/// </summary>
	/// <value>
	///     A boolean value. If true, the formatted resume of the candidate exists. Otherwise, false.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the information about the existence of the formatted resume of the
	///     candidate.
	/// </remarks>
	public bool FormattedResume
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the candidate.
	/// </summary>
	/// <value>
	///     The unique identifier for the candidate.
	/// </value>
	/// <remarks>
	///     This property is used as the primary key in the database and is hidden in the user interface.
	/// </remarks>
	public int ID
	{
		set;
		get;
	}

	/// <summary>
	///     Gets or sets the location of the candidate.
	/// </summary>
	/// <value>
	///     The location of the candidate.
	/// </value>
	public string Location
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the candidate is an MPC (Most Placeable Candidate).
	/// </summary>
	/// <value>
	///     A boolean value. If true, the candidate is an MPC. Otherwise, false.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the information about whether the candidate is considered as an MPC.
	/// </remarks>
	public bool Mpc
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the candidate.
	/// </summary>
	/// <value>
	///     The name of the candidate.
	/// </value>
	public string Name
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the original resume of the candidate exists.
	/// </summary>
	/// <value>
	///     A boolean value. If true, the original resume of the candidate exists. Otherwise, false.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the information about the existence of the original resume of the
	///     candidate.
	/// </remarks>
	public bool OriginalResume
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number of the candidate.
	/// </summary>
	public string Phone
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the rating of the candidate.
	/// </summary>
	/// <value>
	///     A byte representing the rating of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the rating of the candidate. It is a byte value.
	/// </remarks>
	public byte Rating
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the state of the candidate.
	/// </summary>
	/// <value>
	///     A string representing the state of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the state of the candidate. It is a string value.
	/// </remarks>
	public string State
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status of the candidate.
	/// </summary>
	/// <value>
	///     A string representing the status of the candidate.
	/// </value>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date and time when the candidate's information was last updated.
	/// </summary>
	public string Updated
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the zip code of the candidate.
	/// </summary>
	/// <value>
	///     The zip code of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the zip code of the candidate. It is a string value.
	/// </remarks>
	public string ZipCode
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the Candidates object to their default values.
	/// </summary>
	/// <remarks>
	///     This method is used to clear the current state of the Candidates object, setting all properties to their default
	///     values.
	///     For string properties, the default value is an empty string. For the integer and byte properties, the default value
	///     is zero.
	///     For boolean properties, the default value is false. The Rating property is set to 3 by default.
	/// </remarks>
	public void Clear()
	{
		ID = 0;
		Name = "";
		Phone = "";
		Email = "";
		Location = "";
		Updated = "";
		Status = "";
		Mpc = false;
		Rating = 3;
		OriginalResume = false;
		FormattedResume = false;
	}

	/// <summary>
	///     Creates a deep copy of the current Candidates object.
	/// </summary>
	/// <returns>A new Candidates object that is a deep copy of this instance.</returns>
	public Candidates Copy() => MemberwiseClone() as Candidates;
}