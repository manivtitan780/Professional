#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           UserRights.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          08-28-2023 16:14
// Last Updated On:     10-26-2023 21:20
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents the access rights of a user in the system.
/// </summary>
/// <remarks>
///     The 'UserRights' class encapsulates the various permissions a user has in the system.
///     These permissions include viewing, editing, and changing the status of candidates, requisitions, and companies.
///     Additional permissions include sending emails to candidates, forwarding resumes, downloading resumes, and
///     submitting candidates.
/// </remarks>
public class UserRights
{
	/// <summary>
	///     Initializes a new instance of the <see cref="UserRights" /> class.
	/// </summary>
	/// <remarks>
	///     This constructor initializes the instance with default rights, which are all set to false.
	/// </remarks>
	public UserRights()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="UserRights" /> class with specified rights.
	/// </summary>
	/// <param name="viewCandidate">Permission to view candidates.</param>
	/// <param name="viewRequisition">Permission to view requisitions.</param>
	/// <param name="viewCompany">Permission to view companies.</param>
	/// <param name="editCandidate">Permission to edit candidates.</param>
	/// <param name="editRequisition">Permission to edit requisitions.</param>
	/// <param name="editCompany">Permission to edit companies.</param>
	/// <param name="changeCandidateStatus">Permission to change the status of candidates.</param>
	/// <param name="changeRequisitionStatus">Permission to change the status of requisitions.</param>
	/// <param name="sendEmailCandidate">Permission to send emails to candidates.</param>
	/// <param name="forwardResume">Permission to forward resumes.</param>
	/// <param name="downloadResume">Permission to download resumes.</param>
	/// <param name="submitCandidate">Permission to submit candidates.</param>
	/// <remarks>
	///     This constructor initializes the instance with the specified rights.
	/// </remarks>
	public UserRights(bool viewCandidate, bool viewRequisition, bool viewCompany, bool editCandidate, bool editRequisition, bool editCompany, bool changeCandidateStatus,
					  bool changeRequisitionStatus, bool sendEmailCandidate, bool forwardResume, bool downloadResume, bool submitCandidate)
	{
		ViewCandidate = viewCandidate;
		ViewRequisition = viewRequisition;
		ViewCompany = viewCompany;
		EditCandidate = editCandidate;
		EditRequisition = editRequisition;
		EditCompany = editCompany;
		ChangeCandidateStatus = changeCandidateStatus;
		ChangeRequisitionStatus = changeRequisitionStatus;
		SendEmailCandidate = sendEmailCandidate;
		ForwardResume = forwardResume;
		DownloadResume = downloadResume;
		SubmitCandidate = submitCandidate;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to change the status of candidates.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can change the status of candidates; otherwise, <c>false</c>.
	/// </value>
	public bool ChangeCandidateStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to change the status of requisitions.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can change the status of requisitions; otherwise, <c>false</c>.
	/// </value>
	public bool ChangeRequisitionStatus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to download a candidate resume.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can download a candidate resume; otherwise, <c>false</c>.
	/// </value>
	public bool DownloadResume
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to edit a candidate record.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can edit a candidate record; otherwise, <c>false</c>.
	/// </value>
	public bool EditCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to edit a company record.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can edit a company record; otherwise, <c>false</c>.
	/// </value>
	public bool EditCompany
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to edit a requisition record.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can edit a requisition record; otherwise, <c>false</c>.
	/// </value>
	public bool EditRequisition
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to forward a candidate resume.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can forward a candidate resume; otherwise, <c>false</c>.
	/// </value>
	public bool ForwardResume
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to send an email to a candidate.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can send an email to a candidate; otherwise, <c>false</c>.
	/// </value>
	public bool SendEmailCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to submit a candidate to a requisition.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can submit a candidate to a requisition; otherwise, <c>false</c>.
	/// </value>
	public bool SubmitCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to view a candidate record.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can view a candidate record; otherwise, <c>false</c>.
	/// </value>
	public bool ViewCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to view a company record.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can view a company record; otherwise, <c>false</c>.
	/// </value>
	public bool ViewCompany
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user has permission to view a requisition record.
	/// </summary>
	/// <value>
	///     <c>true</c> if the user can view a requisition record; otherwise, <c>false</c>.
	/// </value>
	public bool ViewRequisition
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all user permissions to their default state (false).
	/// </summary>
	/// <remarks>
	///     This method is used to clear all the user's permissions. After calling this method, the user will not have any
	///     permissions until they are explicitly granted.
	/// </remarks>
	public void Clear()
	{
		ViewCandidate = false;
		ViewRequisition = false;
		ViewCompany = false;
		EditCandidate = false;
		EditRequisition = false;
		EditCompany = false;
		ChangeCandidateStatus = false;
		ChangeRequisitionStatus = false;
		SendEmailCandidate = false;
		ForwardResume = false;
		DownloadResume = false;
		SubmitCandidate = false;
	}

	/// <summary>
	///     Creates a shallow copy of the current UserRights instance.
	/// </summary>
	/// <returns>
	///     A new UserRights instance with the same values as the current instance.
	/// </returns>
	public UserRights Copy() => MemberwiseClone() as UserRights;
}