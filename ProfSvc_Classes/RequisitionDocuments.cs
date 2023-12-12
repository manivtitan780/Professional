﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           RequisitionDocuments.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-26-2023 21:19
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a requisition document in the Professional Services application.
/// </summary>
/// <remarks>
///     A requisition document is a formal request or proposal for something needed for the services.
///     This class provides properties for storing details about the document such as its name, file name,
///     associated requisition ID, and other related information. It also provides methods for initializing
///     and managing the state of a requisition document.
/// </remarks>
public class RequisitionDocuments
{
	/// <summary>
	///     Initializes a new instance of the <see cref="RequisitionDocuments" /> class and resets its properties to their
	///     default values.
	/// </summary>
	public RequisitionDocuments()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="RequisitionDocuments" /> class.
	/// </summary>
	/// <param name="id">The unique identifier for the Requisition Document.</param>
	/// <param name="requisitionID">The ID of the requisition associated with the document.</param>
	/// <param name="documentName">The name of the document.</param>
	/// <param name="documentFileName">The file name of the document associated with a requisition.</param>
	/// <param name="documentNotes">The notes for the document in the requisition.</param>
	/// <param name="updateBy">The username of the user who last updated the requisition document.</param>
	/// <param name="originalFileName">The original file name of the requisition document.</param>
	/// <param name="requisitionOwner">The owner of the requisition.</param>
	/// <remarks>
	///     This constructor initializes the properties of the RequisitionDocuments instance with the provided parameters.
	///     It also ensures that the Files list is initialized and empty.
	/// </remarks>
	public RequisitionDocuments(int id, int requisitionID, string documentName, string documentFileName, string documentNotes, string updateBy, string originalFileName, string requisitionOwner)
	{
		ID = id;
		RequisitionID = requisitionID;
		DocumentName = documentName;
		DocumentFileName = documentFileName;
		DocumentNotes = documentNotes;
		UpdateBy = updateBy;
		OriginalFileName = originalFileName;
		RequisitionOwner = requisitionOwner;
		if (Files != null)
		{
			Files.Clear();
		}
		else
		{
			Files = new();
		}
	}

	/// <summary>
	///     Gets or sets the file name of the document associated with a requisition.
	/// </summary>
	/// <value>
	///     The file name of the document.
	/// </value>
	/// <remarks>
	///     This property is used to construct a query string for downloading the document.
	///     The query string is Base64 encoded and appended to the base URI to form the download URL.
	/// </remarks>
	public string DocumentFileName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the document.
	/// </summary>
	/// <value>
	///     The name of the document.
	/// </value>
	public string DocumentName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the notes for the document in the requisition.
	/// </summary>
	/// <value>
	///     The notes for the document.
	/// </value>
	/// <remarks>
	///     This property is validated to be not empty and its length should be between 10 and 2000 characters.
	/// </remarks>
	public string DocumentNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the list of file names associated with the requisition document.
	/// </summary>
	/// <value>
	///     The list of file names associated with the requisition document.
	/// </value>
	/// <remarks>
	///     This property is used to store the names of files that are uploaded as part of the requisition document and to
	///     check the validation that a file is being uploaded.
	/// </remarks>
	public List<string> Files
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the unique identifier for the Requisition Document.
	/// </summary>
	/// <value>
	///     The unique identifier for the Requisition Document.
	/// </value>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the original file name of the requisition document.
	/// </summary>
	/// <value>
	///     The original file name of the requisition document.
	/// </value>
	/// <remarks>
	///     This property is used when downloading the document to provide the user with the original file name.
	/// </remarks>
	public string OriginalFileName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID of the requisition associated with the document.
	/// </summary>
	/// <value>
	///     The ID of the requisition associated with the document.
	/// </value>
	public int RequisitionID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the owner of the requisition.
	/// </summary>
	/// <value>
	///     The owner of the requisition.
	/// </value>
	/// <remarks>
	///     This property is used to determine the user who owns the requisition.
	///     It is used in the context of permissions, allowing only the owner of the requisition to edit it.
	/// </remarks>
	public string RequisitionOwner
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the username of the user who last updated the requisition document.
	/// </summary>
	/// <value>
	///     The username of the user who last updated the requisition document.
	/// </value>
	public string UpdateBy
	{
		get;
		set;
	}

	/// <summary>
	///     Resets the properties of the RequisitionDocuments instance to their default values.
	/// </summary>
	/// <remarks>
	///     This method is used to clear the data of a RequisitionDocuments object. It sets the ID and RequisitionID to 0,
	///     DocumentName, DocumentFileName, DocumentNotes, UpdateBy, OriginalFileName, and RequisitionOwner to an empty string.
	///     If the Files list is not null, it clears the list. Otherwise, it initializes a new list.
	/// </remarks>
	public void Clear()
	{
		ID = 0;
		RequisitionID = 0;
		DocumentName = "";
		DocumentFileName = "";
		DocumentNotes = "";
		UpdateBy = "ADMIN";
		OriginalFileName = "";
		RequisitionOwner = "";
		if (Files != null)
		{
			Files.Clear();
		}
		else
		{
			Files = new();
		}
	}

	/// <summary>
	///     Creates a copy of the current RequisitionDocuments instance.
	/// </summary>
	/// <returns>
	///     A new RequisitionDocuments object that is a copy of the current instance.
	/// </returns>
	public RequisitionDocuments Copy() => MemberwiseClone() as RequisitionDocuments;
}