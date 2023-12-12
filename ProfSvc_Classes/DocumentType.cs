﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           DocumentType.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-01-2022 18:50
// Last Updated On:     10-26-2023 21:17
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a type of document in the system.
/// </summary>
/// <remarks>
///     The DocumentType class is used to categorize and manage different types of documents in the system.
///     Each instance of DocumentType has a unique identifier (ID) and a string representation (DocType).
///     The class provides constructors for creating new instances with specific IDs and document types,
///     as well as methods for resetting the properties to their default values and creating shallow copies of instances.
/// </remarks>
public class DocumentType
{
	/// <summary>
	///     Initializes a new instance of the <see cref="DocumentType" /> class and resets its properties to their default
	///     values.
	/// </summary>
	public DocumentType()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="DocumentType" /> class using the specified ID and document type.
	/// </summary>
	/// <param name="id">The unique identifier for the DocumentType.</param>
	/// <param name="docType">The type of the document.</param>
	/// <remarks>
	///     This constructor sets the ID property to the provided id and the DocType property to the provided docType.
	///     It is typically used when creating a new DocumentType instance with a specific id and document type.
	/// </remarks>
	public DocumentType(int id, string docType)
	{
		ID = id;
		DocType = docType;
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="DocumentType" /> class using the specified ID.
	/// </summary>
	/// <param name="id">The unique identifier for the DocumentType.</param>
	/// <remarks>
	///     This constructor sets the ID property to the provided id and the DocType property to the string representation of
	///     the id.
	///     It is typically used when creating a new DocumentType instance with a specific id and the document type is not yet
	///     known or specified.
	/// </remarks>
	public DocumentType(int id)
	{
		ID = id;
		DocType = id.ToString();
	}

	/// <summary>
	///     Gets or sets the document type.
	/// </summary>
	/// <value>
	///     The document type.
	/// </value>
	/// <remarks>
	///     This property is used to specify the type of the document. It is used throughout the system to categorize and
	///     manage documents.
	/// </remarks>
	public string DocType
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the DocumentType.
	/// </summary>
	/// <value>
	///     The unique identifier for the DocumentType.
	/// </value>
	/// <remarks>
	///     This property is used as the primary key in the database and is also used to link the DocumentType to other related
	///     entities in the system.
	/// </remarks>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the DocumentType instance to their default values.
	/// </summary>
	/// <remarks>
	///     This method sets the ID property to 0 and the DocType property to an empty string.
	///     It is typically used when preparing a DocumentType instance for reuse, such as when creating a new document type.
	/// </remarks>
	public void Clear()
	{
		ID = 0;
		DocType = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current DocumentType instance.
	/// </summary>
	/// <returns>
	///     A new DocumentType object that is a copy of this instance.
	/// </returns>
	public DocumentType Copy() => MemberwiseClone() as DocumentType;
}