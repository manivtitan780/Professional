#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CommissionConfigurator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:12
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a configurator for commissions in the Professional Services system.
/// </summary>
public class CommissionConfigurator
{
	/// <summary>
	///     Initializes a new instance of the <see cref="CommissionConfigurator" /> class and resets its properties to their
	///     default values.
	/// </summary>
	public CommissionConfigurator()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="CommissionConfigurator" /> class with the specified parameters.
	/// </summary>
	/// <param name="id">The unique identifier.</param>
	/// <param name="minSpread">The minimum spread value.</param>
	/// <param name="maxSpread">The maximum spread value.</param>
	/// <param name="commission">The commission value.</param>
	/// <param name="points">The points value.</param>
	public CommissionConfigurator(int id, short minSpread, short maxSpread, byte commission, byte points)
	{
		ID = id;
		MinSpread = minSpread;
		MaxSpread = maxSpread;
		Commission = commission;
		Points = points;
	}

	/// <summary>
	///     Gets or sets the commission value for the configurator.
	/// </summary>
	/// <value>
	///     The commission as a byte.
	/// </value>
	public byte Commission
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the CommissionConfigurator instance.
	/// </summary>
	/// <value>
	///     The identifier as an integer.
	/// </value>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the maximum spread value for the CommissionConfigurator instance.
	/// </summary>
	/// <value>
	///     The maximum spread as a short.
	/// </value>
	public short MaxSpread
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the minimum spread value for the CommissionConfigurator instance.
	/// </summary>
	/// <value>
	///     The minimum spread as a short.
	/// </value>
	public short MinSpread
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the points value for the CommissionConfigurator instance.
	/// </summary>
	/// <value>
	///     The points as a byte.
	/// </value>
	public byte Points
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the current <see cref="CommissionConfigurator" /> instance to their default values.
	/// </summary>
	public void Clear()
	{
		ID = 0;
		MinSpread = 0;
		MaxSpread = 0;
		Commission = 0;
		Points = 0;
	}

	/// <summary>
	///     Creates a shallow copy of the current <see cref="CommissionConfigurator" /> instance.
	/// </summary>
	/// <returns>
	///     A new <see cref="CommissionConfigurator" /> instance with the same property values as the current instance.
	/// </returns>
	public CommissionConfigurator Copy() => MemberwiseClone() as CommissionConfigurator;
}