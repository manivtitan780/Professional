#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           VariableCommission.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:21
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a commission that varies based on certain parameters.
/// </summary>
/// <remarks>
///     This class is used to calculate the commission for employees based on variables such as the number of hours worked,
///     overhead cost, W2 tax loading rate, cost rate for 1099 employees, and the Full-Time Employment (FTE) rate offered.
/// </remarks>
public class VariableCommission
{
	/// <summary>
	///     Initializes a new instance of the <see cref="VariableCommission" /> class and resets its properties to their
	///     default values.
	/// </summary>
	public VariableCommission()
	{
		ClearData();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="VariableCommission" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the variable commission.</param>
	/// <param name="noOfHours">The number of hours for the variable commission.</param>
	/// <param name="overheadCost">The overhead cost for the variable commission.</param>
	/// <param name="w2TaxLoadingRate">The W2 tax loading rate for the variable commission.</param>
	/// <param name="costRate1099">The cost rate for 1099 employees in the variable commission.</param>
	/// <param name="fteRateOffered">The Full-Time Employment (FTE) rate offered in the variable commission.</param>
	public VariableCommission(int id, short noOfHours, byte overheadCost, byte w2TaxLoadingRate, byte costRate1099, byte fteRateOffered)
	{
		ID = id;
		NoOfHours = noOfHours;
		OverheadCost = overheadCost;
		W2TaxLoadingRate = w2TaxLoadingRate;
		CostRate1099 = costRate1099;
		FTERateOffered = fteRateOffered;
	}

	/// <summary>
	///     Gets or sets the cost rate for 1099 employees in the VariableCommission instance.
	/// </summary>
	/// <value>
	///     The cost rate for 1099 employees.
	/// </value>
	/// <remarks>
	///     This property is used in the calculation of variable commission.
	/// </remarks>
	public byte CostRate1099
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Full-Time Employment (FTE) rate offered.
	/// </summary>
	/// <value>
	///     The FTE rate offered, represented as a byte.
	/// </value>
	/// <remarks>
	///     This property is used in the calculation of variable commission.
	/// </remarks>
	public byte FTERateOffered
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID for the variable commission.
	/// </summary>
	/// <value>
	///     The ID represented as an integer.
	/// </value>
	/// <remarks>
	///     This property uniquely identifies the variable commission. It is used in various operations such as searching,
	///     updating, and deleting variable commissions.
	/// </remarks>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the number of hours for the variable commission.
	/// </summary>
	/// <value>
	///     The number of hours as a short integer.
	/// </value>
	public short NoOfHours
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the overhead cost for the variable commission.
	/// </summary>
	/// <value>
	///     The overhead cost represented as a byte.
	/// </value>
	/// <remarks>
	///     This property is used in the calculation of the variable commission. It is also used in the
	///     VariableCommissionDialog for data binding.
	/// </remarks>
	public byte OverheadCost
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the W2 tax loading rate for the variable commission.
	/// </summary>
	/// <value>
	///     The W2 tax loading rate represented as a byte.
	/// </value>
	/// <remarks>
	///     This property is used in the calculation of the variable commission. It is also used in the AdminController for
	///     saving data and in the VariableCommissionDialog for data binding.
	/// </remarks>
	public byte W2TaxLoadingRate
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all the properties of the VariableCommission instance to their default values.
	/// </summary>
	public void ClearData()
	{
		ID = 0;
		NoOfHours = 0;
		OverheadCost = 0;
		W2TaxLoadingRate = 0;
		CostRate1099 = 0;
		FTERateOffered = 0;
	}

	/// <summary>
	///     Creates a copy of the current VariableCommission instance.
	/// </summary>
	/// <returns>
	///     A new VariableCommission object that is a copy of the current instance.
	/// </returns>
	public VariableCommission Copy() => MemberwiseClone() as VariableCommission;
}