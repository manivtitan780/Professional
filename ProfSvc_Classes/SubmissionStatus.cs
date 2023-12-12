#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           SubmissionStatus.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          05-16-2023 20:07
// Last Updated On:     10-26-2023 21:20
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents the status of a submission in the Professional Services application.
/// </summary>
/// <remarks>
///     This class provides properties for storing the status code, status description, count of submissions, and color
///     associated with a submission status.
///     It also provides methods for clearing the status data and creating a copy of the status instance.
/// </remarks>
public class SubmissionStatus
{
	/// <summary>
	///     Initializes a new instance of the <see cref="SubmissionStatus" /> class and resets its properties to their default
	///     values.
	/// </summary>
	public SubmissionStatus()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="SubmissionStatus" /> class.
	/// </summary>
	/// <param name="statusCode">The status code of the submission.</param>
	/// <param name="status">The status of the submission.</param>
	/// <param name="count">The count of submissions associated with the status.</param>
	/// <param name="color">The color associated with the submission status. Default is "#0000bb".</param>
	/// <remarks>
	///     This constructor allows to create a new instance of the <see cref="SubmissionStatus" /> class with specified status
	///     code, status, count, and color.
	/// </remarks>
	public SubmissionStatus(string statusCode, string status, int count, string color = "#0000bb")
	{
		StatusCode = statusCode;
		Status = status;
		Count = count;
		Color = color;
	}

	/// <summary>
	///     Gets or sets the color associated with the submission status.
	/// </summary>
	/// <value>
	///     The color represented as a string.
	/// </value>
	/// <remarks>
	///     The color is typically used for visual representation of the submission status in a user interface. The color
	///     should be specified in the hexadecimal format, e.g., "#0000bb".
	/// </remarks>
	public string Color
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the count of submissions.
	/// </summary>
	/// <value>
	///     The count represented as an integer.
	/// </value>
	/// <remarks>
	///     The count indicates the number of submissions associated with a particular status.
	/// </remarks>
	public int Count
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status of the submission.
	/// </summary>
	/// <value>
	///     The status represented as a string.
	/// </value>
	/// <remarks>
	///     The status provides a human-readable description of the submission's current state.
	/// </remarks>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status code of the submission.
	/// </summary>
	/// <value>
	///     The status code represented as a string.
	/// </value>
	/// <remarks>
	///     The status code provides a unique identifier for the status of the submission.
	/// </remarks>
	public string StatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Clears the data of the current SubmissionStatus instance.
	/// </summary>
	/// <remarks>
	///     This method resets the StatusCode and Status to an empty string, Count to 0, and Color to "#0000bb".
	/// </remarks>
	public void Clear()
	{
		StatusCode = "";
		Status = "";
		Count = 0;
		Color = "#0000bb";
	}

	/// <summary>
	///     Creates a copy of the current SubmissionStatus instance.
	/// </summary>
	/// <returns>
	///     A new SubmissionStatus object that is a copy of the current instance.
	/// </returns>
	public SubmissionStatus Copy() => MemberwiseClone() as SubmissionStatus;
}