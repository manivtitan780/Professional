#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           ByteValues.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-31-2023 20:51
// Last Updated On:     10-26-2023 19:21
// *****************************************/

#endregion

#region Using

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a class that associates a byte key with a string value.
/// </summary>
/// <remarks>
///     This class provides a way to store byte keys and their associated string values.
///     It includes methods for creating new instances of the class, getting and setting the key and value,
///     resetting the key and value, and creating a shallow copy of the object.
/// </remarks>
public class ByteValues
{
	/// <summary>
	///     Initializes a new instance of the ByteValues class.
	/// </summary>
	/// <remarks>
	///     This constructor initializes the Key property to 0 and the Value property to an empty string.
	/// </remarks>
	public ByteValues()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the ByteValues class with the specified key and value.
	/// </summary>
	/// <param name="key">The byte value to be used as a key.</param>
	/// <param name="value">The string value to be associated with the key.</param>
	/// <remarks>
	///     This constructor initializes the Key property with the provided byte value and sets the Value property to the
	///     provided string value.
	/// </remarks>
	public ByteValues(byte key, string value)
	{
		Key = key;
		Value = value;
	}

	/// <summary>
	///     Initializes a new instance of the ByteValues class with the specified key.
	/// </summary>
	/// <param name="key">The byte value to be used as a key.</param>
	/// <remarks>
	///     This constructor initializes the Key property with the provided byte value and sets the Value property to the
	///     string representation of the byte value.
	/// </remarks>
	public ByteValues(byte key)
	{
		Key = key;
		Value = key.ToString();
	}

	/// <summary>
	///     Gets or sets the Key for the ByteValues object.
	/// </summary>
	/// <value>
	///     The byte value used as a key.
	/// </value>
	/// <remarks>
	///     This property is used to store the byte key for the ByteValues object.
	///     It can be used in various scenarios such as indexing the ByteValues in a collection or using it as a unique
	///     identifier.
	/// </remarks>
	public byte Key
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the value associated with the ByteValues object.
	/// </summary>
	/// <value>
	///     The string representation of the byte value.
	/// </value>
	/// <remarks>
	///     This property is used to store the string representation of the byte key.
	///     It can be used in various scenarios such as displaying the value in a user interface or processing it in business
	///     logic.
	/// </remarks>
	public string Value
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the Key and Value properties of the ByteValues object.
	/// </summary>
	public void Clear()
	{
		Key = 0;
		Value = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current ByteValues object.
	/// </summary>
	/// <returns>A new ByteValues object with the same Key and Value as the current object.</returns>
	public ByteValues Copy() => MemberwiseClone() as ByteValues;
}