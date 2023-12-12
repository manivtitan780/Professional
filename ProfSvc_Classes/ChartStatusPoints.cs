#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           ChartStatusPoints.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          05-22-2023 19:12
// Last Updated On:     10-26-2023 21:09
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents the status points for a chart in the Professional Services application.
/// </summary>
/// <remarks>
///     This class is used to store and manage the status points for a chart, including the status codes, statuses, and
///     colors for different statuses.
///     The statuses include Pen (Pending Submittal), Hir (Candidate Hired), Oex (Offer Extended), and Wdr (Candidate
///     Withdrawn).
/// </remarks>
public class ChartStatusPoints
{
	/// <summary>
	///     Initializes a new instance of the <see cref="ChartStatusPoints" /> class.
	/// </summary>
	public ChartStatusPoints()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="ChartStatusPoints" /> class.
	/// </summary>
	/// <param name="penStatusCode">The status code for the pen.</param>
	/// <param name="penStatus">The status for the pen.</param>
	/// <param name="penColor">The color for the pen.</param>
	/// <param name="hirStatusCode">The status code for the hir.</param>
	/// <param name="hirStatus">The status for the hir.</param>
	/// <param name="hirColor">The color for the hir.</param>
	/// <param name="oexStatusCode">The status code for the oex.</param>
	/// <param name="oexStatus">The status for the oex.</param>
	/// <param name="oexColor">The color for the oex.</param>
	/// <param name="wdrStatusCode">The status code for the wdr.</param>
	/// <param name="wdrStatus">The status for the wdr.</param>
	/// <param name="wdrColor">The color for the wdr.</param>
	public ChartStatusPoints(string penStatusCode, string penStatus, string penColor, string hirStatusCode, string hirStatus, string hirColor, string oexStatusCode, string oexStatus,
							 string oexColor, string wdrStatusCode, string wdrStatus, string wdrColor)
	{
		PenStatusCode = penStatusCode;
		PenStatus = penStatus;
		PenColor = penColor;
		HirStatusCode = hirStatusCode;
		HirStatus = hirStatus;
		HirColor = hirColor;
		OexStatusCode = oexStatusCode;
		OexStatus = oexStatus;
		OexColor = oexColor;
		WDRStatusCode = wdrStatusCode;
		WDRStatus = wdrStatus;
		WDRColor = wdrColor;
	}

	/// <summary>
	///     Gets or sets the color for the Hir (Candidate Hired) status.
	/// </summary>
	/// <value>
	///     The color for the Hir status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the color for the Hir status in the ChartStatusPoints object.
	///     It is typically set to a string representing a color, such as "#004300" for dark green.
	/// </remarks>
	public string HirColor
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Hir (Candidate Hired) status.
	/// </summary>
	/// <value>
	///     The status for the Hir (Candidate Hired) status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the Hir status in the ChartStatusPoints object.
	///     It is typically set to a string representing the status of a candidate, such as "Candidate Hired".
	/// </remarks>
	public string HirStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status code for the Hir (Candidate Hired) status.
	/// </summary>
	/// <value>
	///     The status code for the Hir status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the status code for the Hir status in the ChartStatusPoints object.
	///     It is typically set to "HIR".
	/// </remarks>
	public string HirStatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the color for the Oex (Offer Extended) status.
	/// </summary>
	/// <value>
	///     The color for the Oex status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the color for the Oex status in the ChartStatusPoints object.
	///     It is typically set to a string representing a color, such as "#00cc00" for green.
	/// </remarks>
	public string OexColor
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Oex (Offer Extended) status.
	/// </summary>
	/// <value>
	///     The status for the Oex (Offer Extended) status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the Oex status in the ChartStatusPoints object.
	///     It is typically set to a string representing the status of a candidate, such as "Offer Extended".
	/// </remarks>
	public string OexStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status code for the Oex (Offer Extended) status.
	/// </summary>
	/// <value>
	///     The status code for the Oex status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the status code for the Oex status in the ChartStatusPoints object.
	///     It is typically set to "OEX".
	/// </remarks>
	public string OexStatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the color for the Pen (Pending Submittal) status.
	/// </summary>
	/// <value>
	///     The color for the Pen status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the color for the Pen status in the ChartStatusPoints object.
	///     It is typically set to a string representing a color, such as "#ffa500" for orange.
	/// </remarks>
	public string PenColor
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Pen (Pending Submittal) status.
	/// </summary>
	/// <value>
	///     The status for the Pen (Pending Submittal) status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the Pen status in the ChartStatusPoints object.
	///     It is typically set to a string representing the status of a candidate, such as "Pending Submittal".
	/// </remarks>
	public string PenStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status code for the Pen (Pending Submittal) status.
	/// </summary>
	/// <value>
	///     The status code for the Pen status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the status code for the Pen status in the ChartStatusPoints object.
	///     It is typically set to "PEN".
	/// </remarks>
	public string PenStatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the color for the WDR (Candidate Withdrawn) status.
	/// </summary>
	/// <value>
	///     The color for the WDR status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the color for the WDR status in the ChartStatusPoints object.
	///     It is typically set to a string representing a color, such as "#808080" for gray.
	/// </remarks>
	public string WDRColor
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the WDR (Candidate Withdrawn) status.
	/// </summary>
	/// <value>
	///     The status for the WDR (Candidate Withdrawn) status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the WDR status in the ChartStatusPoints object.
	///     It is typically set to a string representing the status of a candidate, such as "Candidate Withdrawn".
	/// </remarks>
	public string WDRStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status code for the WDR (Candidate Withdrawn) status.
	/// </summary>
	/// <value>
	///     The status code for the WDR status.
	/// </value>
	/// <remarks>
	///     This property is used to represent the status code for the WDR status in the ChartStatusPoints object.
	///     It is typically set to "WDR".
	/// </remarks>
	public string WDRStatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the ChartStatusPoints object to their default values.
	/// </summary>
	/// <remarks>
	///     This method is used to initialize or reset the ChartStatusPoints object. It sets the status codes, statuses, and
	///     colors for Pen, Hir, Oex, and WDR to their default values.
	/// </remarks>
	public void Clear()
	{
		PenStatusCode = "PEN";
		PenStatus = "Pending Submittal";
		PenColor = "#ffa500";
		HirStatusCode = "HIR";
		HirStatus = "Candidate Hired";
		HirColor = "#004300";
		OexStatusCode = "Offer Extended";
		OexStatus = "OEX";
		OexColor = "#00cc00";
		WDRStatusCode = "WDR";
		WDRStatus = "Candidate Withdrawn";
		WDRColor = "#808080";
	}

	/// <summary>
	///     Creates a shallow copy of the current ChartStatusPoints object.
	/// </summary>
	/// <returns>
	///     A new ChartStatusPoints object with the same values as the current object.
	/// </returns>
	public ChartStatusPoints Copy() => MemberwiseClone() as ChartStatusPoints;
}