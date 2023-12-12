#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           StatusDrop.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:20
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a status drop class that encapsulates a status and its associated code.
/// </summary>
/// <remarks>
///     This class provides methods to set and get the status and code,
///     as well as to clear the status and code to their default values.
/// </remarks>
public class StatusDrop
{
	/// <summary>
	///     Initializes a new instance of the <see cref="StatusDrop" /> class.
	/// </summary>
	/// <remarks>
	///     The constructor initializes the instance by calling the Clear method, which resets the 'Code' and 'Status'
	///     properties to their default values.
	/// </remarks>
	public StatusDrop()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="StatusDrop" /> class.
	/// </summary>
	/// <param name="code">The code to associate with the StatusDrop instance.</param>
	/// <param name="status">The status to associate with the StatusDrop instance.</param>
	public StatusDrop(string code, string status)
	{
		Code = code;
		Status = status;
	}

	/// <summary>
	///     Gets or sets the code associated with the StatusDrop instance.
	/// </summary>
	/// <value>
	///     The code as a string.
	/// </value>
	public string Code
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status associated with the StatusDrop instance.
	/// </summary>
	/// <value>
	///     The status as a string.
	/// </value>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the 'Code' and 'Status' properties to their default values.
	/// </summary>
	public void Clear()
	{
		Code = "";
		Status = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current StatusDrop instance.
	/// </summary>
	/// <returns>
	///     A new StatusDrop object that is a copy of the current instance.
	/// </returns>
	public StatusDrop Copy() => MemberwiseClone() as StatusDrop;
}