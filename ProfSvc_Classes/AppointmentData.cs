#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           AppointmentData.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          05-15-2023 15:41
// Last Updated On:     10-24-2023 21:22
// *****************************************/

#endregion

#region Using

using System.Text.Json;

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents an appointment with properties such as start time, end time, description, subject, location, and
///     recurrence details.
/// </summary>
public class AppointmentData
{
	/// <summary>
	///     Initializes a new instance of the <see cref="AppointmentData" /> class.
	/// </summary>
	public AppointmentData()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="AppointmentData" /> class.
	/// </summary>
	/// <param name="id">The identifier for the appointment.</param>
	/// <param name="startTime">The start time of the appointment.</param>
	/// <param name="endTime">The end time of the appointment.</param>
	/// <param name="description">The description of the appointment.</param>
	/// <param name="subject">The subject of the appointment.</param>
	/// <param name="isAllDay">Indicates whether the appointment is an all-day event.</param>
	/// <param name="location">The location of the appointment.</param>
	/// <param name="recurrenceException">The exception dates for the recurrence pattern of the appointment.</param>
	/// <param name="recurrenceID">The identifier for the recurrence pattern of the appointment.</param>
	/// <param name="recurrenceRule">The recurrence rule for the appointment.</param>
	/// <param name="color">The color of the appointment.</param>
	public AppointmentData(int id, DateTime startTime, DateTime endTime, string description, string subject, bool isAllDay, string location, string recurrenceException,
						   int? recurrenceID, string recurrenceRule, string color)
	{
		ID = id;
		StartTime = startTime;
		EndTime = endTime;
		Description = description;
		Subject = subject;
		IsAllDay = isAllDay;
		Location = location;
		RecurrenceException = recurrenceException;
		RecurrenceID = recurrenceID;
		RecurrenceRule = recurrenceRule;
		Color = color;
	}

	/// <summary>
	///     Gets or sets the color of the appointment.
	/// </summary>
	/// <value>
	///     The color of the appointment as a string. This color is used for the background of the appointment when it is
	///     rendered on the Dashboard.
	/// </value>
	public string Color
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the description of the appointment.
	/// </summary>
	/// <value>
	///     The description of the appointment as a string.
	/// </value>
	public string Description
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the end time of the appointment.
	/// </summary>
	/// <value>
	///     The end time of the appointment.
	/// </value>
	public DateTime EndTime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier for the appointment.
	/// </summary>
	/// <value>
	///     The identifier of the appointment as an integer.
	/// </value>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the appointment is an all-day event.
	/// </summary>
	/// <value>
	///     true if the appointment is an all-day event; otherwise, false.
	/// </value>
	public bool IsAllDay
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the location of the appointment.
	/// </summary>
	/// <value>
	///     The location of the appointment as a string.
	/// </value>
	public string Location
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the exception dates for the recurrence pattern of the appointment.
	/// </summary>
	/// <value>
	///     The exception dates as a string. If the appointment does not have any exception dates, this value is an empty
	///     string.
	/// </value>
	public string RecurrenceException
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier for the recurrence pattern of the appointment.
	/// </summary>
	/// <value>
	///     The identifier for the recurrence pattern. If the appointment does not have a recurrence pattern, this value is
	///     null.
	/// </value>
	public int? RecurrenceID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the recurrence rule for the appointment.
	/// </summary>
	/// <value>
	///     The recurrence rule as a string. This rule defines the pattern of recurrence for the appointment.
	/// </value>
	public string RecurrenceRule
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the start time of the appointment.
	/// </summary>
	/// <value>
	///     The start time of the appointment.
	/// </value>
	public DateTime StartTime
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the subject of the appointment.
	/// </summary>
	/// <value>
	///     The subject of the appointment.
	/// </value>
	public string Subject
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the AppointmentData instance to their default values.
	/// </summary>
	public void Clear()
	{
		ID = 0;
		StartTime = DateTime.MinValue;
		EndTime = DateTime.MinValue;
		Description = "";
		Subject = "";
		IsAllDay = false;
		Location = "";
		RecurrenceException = "";
		RecurrenceID = null;
		RecurrenceRule = "";
		Color = "#0000bb";
	}

	/// <summary>
	///     Creates a deep copy of the current AppointmentData instance.
	/// </summary>
	/// <returns>
	///     A new AppointmentData object that is a deep copy of this instance.
	/// </returns>
	public AppointmentData Copy()
	{
		string json = JsonSerializer.Serialize(this);
		return JsonSerializer.Deserialize<AppointmentData>(json);
	}
}