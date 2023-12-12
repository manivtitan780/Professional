#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           ChartUsersStatus.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          05-22-2023 15:55
// Last Updated On:     10-26-2023 21:10
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents the status of users in a chart format.
/// </summary>
public class ChartUsersStatus
{
	/// <summary>
	///     Initializes a new instance of the <see cref="ChartUsersStatus" /> class and resets its properties to their default
	///     values.
	/// </summary>
	public ChartUsersStatus()
	{
		Clear();
	}

	/// <param name="userID">The unique identifier for the user.</param>
	/// <param name="name">The name associated with the user status.</param>
	/// <param name="pending">The number of users whose status is pending.</param>
	/// <param name="hired">The number of users who have been hired.</param>
	/// <param name="offerExtended">The number of users to whom an offer has been extended.</param>
	/// <param name="withdrawn">The number of users who have withdrawn their status.</param>
	public ChartUsersStatus(string userID, string name, int pending, int hired, int offerExtended, int withdrawn)
	{
		UserID = userID;
		Name = name;
		Pending = pending;
		Hired = hired;
		OfferExtended = offerExtended;
		Withdrawn = withdrawn;
	}

	/// <summary>
	///     Gets or sets the number of users who have been hired.
	/// </summary>
	public int Hired
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name associated with the user status.
	/// </summary>
	public string Name
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the number of users to whom an offer has been extended.
	/// </summary>
	public int OfferExtended
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the number of users whose status is pending.
	/// </summary>
	public int Pending
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the user.
	/// </summary>
	public string UserID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the number of users who have withdrawn their status.
	/// </summary>
	public int Withdrawn
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the ChartUsersStatus object to their default values.
	/// </summary>
	public void Clear()
	{
		UserID = "";
		Pending = 0;
		Hired = 0;
		OfferExtended = 0;
		Withdrawn = 0;
	}

	/// <summary>
	///     Creates a shallow copy of the current ChartUsersStatus object.
	/// </summary>
	/// <returns>A new ChartUsersStatus object that is a shallow copy of this instance.</returns>
	public ChartUsersStatus Copy() => MemberwiseClone() as ChartUsersStatus;
}