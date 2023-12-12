#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           JobOptionsCandidate.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:18
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a candidate for job options in the professional services domain.
/// </summary>
/// <remarks>
///     This class provides properties for storing job option details such as code, description, job options, and the date
///     of the last update.
///     It also provides methods for initializing and managing the state of the object.
/// </remarks>
public class JobOptionsCandidate
{
	/// <summary>
	///     Initializes a new instance of the <see cref="JobOptionsCandidate" /> class with the specified code, job options,
	///     description, and updated date.
	/// </summary>
	/// <param name="code">The code for the JobOptionsCandidate.</param>
	/// <param name="jobOptions">The job options for the JobOptionsCandidate.</param>
	/// <param name="description">The description for the JobOptionsCandidate. An empty string by default.</param>
	/// <param name="updatedDate">The date when the JobOptionsCandidate was last updated. An empty string by default.</param>
	public JobOptionsCandidate(string code, string jobOptions, string description = "", string updatedDate = "")
	{
		Code = code;
		JobOptions = jobOptions;
		Description = description;
		UpdatedDate = updatedDate;
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="JobOptionsCandidate" /> class.
	/// </summary>
	/// <remarks>
	///     This constructor will initialize the object and set all properties to their default values.
	/// </remarks>
	public JobOptionsCandidate()
	{
		Clear();
	}

	/// <summary>
	///     Gets or sets the code for the JobOptionsCandidate.
	/// </summary>
	/// <value>
	///     A string representing the code.
	/// </value>
	public string Code
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the description for the JobOptionsCandidate.
	/// </summary>
	/// <value>
	///     A string representing the description.
	/// </value>
	public string Description
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the job options for the JobOptionsCandidate.
	/// </summary>
	/// <value>
	///     A string representing the job options.
	/// </value>
	public string JobOptions
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the date when the JobOptionsCandidate was last updated.
	/// </summary>
	/// <value>
	///     A string representing the updated date in the format "yyyy-MM-dd".
	/// </value>
	public string UpdatedDate
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the JobOptionsCandidate object to their default values.
	/// </summary>
	public void Clear()
	{
		Code = "";
		JobOptions = "";
		Description = "";
		UpdatedDate = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current JobOptionsCandidate object.
	/// </summary>
	/// <returns>
	///     A shallow copy of the current JobOptionsCandidate object.
	/// </returns>
	public JobOptionsCandidate Copy() => MemberwiseClone() as JobOptionsCandidate;
}