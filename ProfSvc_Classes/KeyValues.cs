#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           KeyValues.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:18
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a key-value pair in the Professional Services Classes.
/// </summary>
/// <remarks>
///     This class is used to store a key and its associated value. It provides methods to clear the key and value, and to
///     create a copy of the KeyValues instance.
/// </remarks>
public class KeyValues
{
	/// <summary>
	///     Initializes a new instance of the <see cref="KeyValues" /> class and resets its properties to their default values.
	/// </summary>
	public KeyValues()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="KeyValues" /> class.
	/// </summary>
	/// <param name="key">The key to be stored in the KeyValues instance.</param>
	/// <param name="value">The value to be associated with the key in the KeyValues instance.</param>
	public KeyValues(string key, string value)
	{
		Key = key;
		Value = value;
	}

	/// <summary>
	///     Gets or sets the Key in the KeyValues instance.
	/// </summary>
	/// <value>
	///     The Key of a key-value pair in the KeyValues instance.
	/// </value>
	/// <remarks>
	///     This property is used to store the key part of a key-value pair in the KeyValues instance.
	/// </remarks>
	public string Key
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the value associated with the Key in the KeyValues instance.
	/// </summary>
	/// <value>
	///     The value associated with the Key.
	/// </value>
	/// <remarks>
	///     This property is used to store the value part of a key-value pair in the KeyValues instance.
	/// </remarks>
	public string Value
	{
		get;
		set;
	}

	/// <summary>
	///     Clears the Key and Value properties by setting them to an empty string.
	/// </summary>
	public void Clear()
	{
		Key = "";
		Value = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current KeyValues instance.
	/// </summary>
	/// <returns>
	///     A new KeyValues object copied from the current instance.
	/// </returns>
	public KeyValues Copy() => MemberwiseClone() as KeyValues;
}