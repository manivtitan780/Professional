#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           UploadResume.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          04-12-2023 19:24
// Last Updated On:     10-26-2023 21:20
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a class for uploading resumes in the Professional Services application.
/// </summary>
/// <remarks>
///     This class provides functionality for managing the upload of resumes,
///     including the ability to clear and copy resume data. It also provides
///     properties for accessing and modifying the ID and associated files of the resume.
/// </remarks>
public class UploadResume
{
	/// <summary>
	///     Initializes a new instance of the <see cref="UploadResume" /> class and resets its properties to their default
	///     values.
	/// </summary>
	public UploadResume()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="UploadResume" /> class with the specified ID.
	/// </summary>
	/// <param name="id">The unique identifier to set for the UploadResume instance.</param>
	/// <remarks>
	///     This constructor sets the ID of the UploadResume instance to the provided value and initializes the Files list.
	///     If the Files list is null, a new list is created.
	/// </remarks>
	public UploadResume(int id)
	{
		ID = id;
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
	///     Gets or sets the list of file names associated with the UploadResume instance.
	/// </summary>
	/// <value>
	///     The list of file names associated with the UploadResume instance.
	/// </value>
	/// <remarks>
	///     This property is used to store the names of the files that are uploaded as part of the resume.
	///     Each string in the list represents the name of a file.
	/// </remarks>
	public List<string> Files
	{
		get;
		set;
	} = new();

	/// <summary>
	///     Gets or sets the unique identifier for the UploadResume instance.
	/// </summary>
	/// <value>
	///     The unique identifier for the UploadResume instance.
	/// </value>
	/// <remarks>
	///     This property is used as the primary key for the UploadResume instance.
	///     It is also used to link the UploadResume instance with a specific candidate in the Candidate context.
	/// </remarks>
	public int ID
	{
		get;
		set;
	}

	/// <summary>
	///     Clears the ID and Files of the current UploadResume instance.
	/// </summary>
	/// <remarks>
	///     This method sets the ID to 0 and reinitializes the Files list.
	///     If the Files list is null, a new list is created.
	/// </remarks>
	public void Clear()
	{
		ID = 0;
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
	///     Creates a shallow copy of the current <see cref="UploadResume" /> object.
	/// </summary>
	/// <returns>
	///     A shallow copy of the current <see cref="UploadResume" /> object.
	/// </returns>
	public UploadResume Copy() => MemberwiseClone() as UploadResume;
}