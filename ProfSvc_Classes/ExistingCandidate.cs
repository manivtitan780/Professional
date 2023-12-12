#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           ExistingCandidate.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          04-29-2023 19:26
// Last Updated On:     10-26-2023 21:17
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents an existing candidate in the Professional Services application.
/// </summary>
/// <remarks>
///     The ExistingCandidate class includes properties such as ID, Name, Email, and Phone.
///     It also includes methods for creating a new instance of the ExistingCandidate class and copying an existing
///     instance.
/// </remarks>
public class ExistingCandidate
{
	/// <summary>
	///     Initializes a new instance of the <see cref="ExistingCandidate" /> class with the specified ID, name, email, and
	///     phone.
	/// </summary>
	public ExistingCandidate()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ExistingCandidate" /> class with the specified ID, name, email, and
	///     phone.
	/// </summary>
	/// <param name="id">The unique identifier of the existing candidate.</param>
	/// <param name="name">The name of the existing candidate.</param>
	/// <param name="email">The email address of the existing candidate.</param>
	/// <param name="phone">The phone number of the existing candidate.</param>
	public ExistingCandidate(int id, string name, string email, string phone)
	{
		ID = id;
		Name = name;
		Email = email;
		Phone = phone;
	}

	/// <summary>
	///     Gets or sets the email address of the existing candidate.
	/// </summary>
	public string Email
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the existing candidate.
	/// </summary>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the existing candidate.
	/// </summary>
	public string Name
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone number of the existing candidate.
	/// </summary>
	public string Phone
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the ExistingCandidate instance to their default values.
	/// </summary>
	private void Clear()
	{
		ID = 0;
		Name = "";
		Email = "";
		Phone = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current ExistingCandidate object.
	/// </summary>
	/// <returns>
	///     A new ExistingCandidate object with the same values as the current object.
	/// </returns>
	public ExistingCandidate Copy() => MemberwiseClone() as ExistingCandidate;
}