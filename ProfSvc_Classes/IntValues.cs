#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           IntValues.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     10-26-2023 21:17
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a key-value pair where the key is an integer and the value is a string.
/// </summary>
/// <remarks>
///     This class is used in various contexts throughout the application, such as representing location keys, eligibility
///     IDs, and state IDs with their corresponding names, eligibility statuses, and state names.
/// </remarks>
public class IntValues
{
	/// <summary>
	///     Initializes a new instance of the <see cref="IntValues" /> class with the specified key and resets its properties
	///     to their default values.
	/// </summary>
	public IntValues()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="IntValues" /> class.
	/// </summary>
	/// <param name="key">The integer key.</param>
	/// <param name="value">The string value associated with the key.</param>
	public IntValues(int key, string value)
	{
		Key = key;
		Value = value;
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="IntValues" /> class with the specified key.
	/// </summary>
	/// <param name="key">The integer key to initialize the <see cref="IntValues" /> object with.</param>
	/// <remarks>
	///     The value of the <see cref="IntValues" /> object is set to the string representation of the key.
	/// </remarks>
	public IntValues(int key)
	{
		Key = key;
		Value = key.ToString();
	}

	/// <summary>
	///     Gets or sets the Key of the IntValues object.
	/// </summary>
	/// <value>
	///     The integer Key.
	/// </value>
	/// <remarks>
	///     This property is used as an identifier in various contexts throughout the application, such as representing
	///     location keys, eligibility IDs, and state IDs.
	/// </remarks>
	public int Key
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the value associated with the Key in the IntValues object.
	/// </summary>
	/// <value>
	///     The string value associated with the Key.
	/// </value>
	/// <remarks>
	///     This property is used in various contexts throughout the application, such as representing location names,
	///     eligibility statuses, and state names.
	/// </remarks>
	public string Value
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the Key and Value properties of the current IntValues object.
	/// </summary>
	public void Clear()
	{
		Key = 0;
		Value = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current IntValues object.
	/// </summary>
	/// <returns>
	///     A new IntValues object with the same value of the Key and Value properties as the current IntValues object.
	/// </returns>
	public IntValues Copy() => MemberwiseClone() as IntValues;
}