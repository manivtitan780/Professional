﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           User.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     10-26-2023 21:20
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents a user in the system.
/// </summary>
/// <remarks>
///     This class contains properties that represent the user's information such as username, full name, role, status,
///     email address, first name, last name, role ID, status code, and password.
///     It also provides methods to initialize a new instance of the user, reset all properties to their default values,
///     and create a shallow copy of the current user object.
/// </remarks>
public class User
{
	/// <summary>
	///     Initializes a new instance of the <see cref="User" /> class and resets its properties to their default values.
	/// </summary>
	public User()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="User" /> class with the specified parameters.
	/// </summary>
	/// <param name="userName">The username of the user.</param>
	/// <param name="fullName">The full name of the user.</param>
	/// <param name="role">The role of the user.</param>
	/// <param name="status">The status of the user.</param>
	/// <param name="emailAddress">The email address of the user.</param>
	/// <param name="firstName">The first name of the user.</param>
	/// <param name="lastName">The last name of the user.</param>
	/// <param name="roleID">The role ID of the user.</param>
	/// <param name="statusCode">The status code of the user.</param>
	/// <param name="password">The password of the user.</param>
	/// <remarks>
	///     This constructor initializes the user with the provided parameters.
	///     The 'StatusEnabled' property is initialized based on the 'statusCode' parameter (true if 'statusCode' is "ACT").
	/// </remarks>
	public User(string userName, string fullName, string role, string status, string emailAddress, string firstName, string lastName, string roleID,
				string statusCode, string password)
	{
		UserName = userName;
		FullName = fullName;
		Role = role;
		Status = status;
		EmailAddress = emailAddress;
		FirstName = firstName;
		LastName = lastName;
		RoleID = roleID;
		StatusCode = statusCode;
		StatusEnabled = statusCode == "ACT";
		Password = password;
	}

	/// <summary>
	///     Initializes a new instance of the <see cref="User" /> class.
	/// </summary>
	/// <param name="userName">The username of the user.</param>
	/// <param name="fullName">The full name of the user.</param>
	/// <param name="role">The role of the user.</param>
	/// <param name="status">The status of the user.</param>
	/// <remarks>
	///     This constructor initializes the user with the provided username, full name, role, and status.
	///     Other properties are initialized with default values.
	/// </remarks>
	public User(string userName, string fullName, string role, string status)
	{
		UserName = userName;
		FullName = fullName;
		Role = role;
		Status = status;
		EmailAddress = "";
		FirstName = "";
		LastName = "";
		RoleID = "RC";
		StatusCode = status;
		StatusEnabled = status == "ACT";
		Password = "";
	}

	/// <summary>
	///     Gets or sets the email address of the user.
	/// </summary>
	/// <value>
	///     The email address of the user.
	/// </value>
	public string EmailAddress
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the first name of the user.
	/// </summary>
	/// <value>
	///     The first name of the user.
	/// </value>
	public string FirstName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the full name of the user.
	/// </summary>
	/// <value>
	///     The full name of the user.
	/// </value>
	public string FullName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the user is being added.
	/// </summary>
	/// <value>
	///     <c>true</c> if this instance represents a user being added; otherwise, <c>false</c>.
	/// </value>
	public bool IsAdd
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the last name of the user.
	/// </summary>
	/// <value>
	///     The last name of the user.
	/// </value>
	public string LastName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Password of the User.
	/// </summary>
	/// <value>
	///     The Password is a string that represents the user's password. It should be between 6 and 16 characters and contain
	///     at least 1 uppercase, lowercase character and 1 of either a numeric or special character.
	/// </value>
	public string Password
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Role of the User.
	/// </summary>
	/// <value>
	///     The Role is a string that represents the role assigned to a User in the system.
	/// </value>
	public string Role
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the RoleID of the User.
	/// </summary>
	/// <value>
	///     The RoleID is a string that represents the unique identifier for a User's role.
	/// </value>
	public string RoleID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Status of the User.
	/// </summary>
	/// <value>
	///     The Status is a string that represents the current status of a User.
	/// </value>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the StatusCode of the User.
	/// </summary>
	/// <value>
	///     The StatusCode is a string that represents the current status code of a User.
	/// </value>
	public string StatusCode
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the User's status is enabled.
	/// </summary>
	/// <value>
	///     true if the User's status is enabled; otherwise, false. Default value is determined by the 'StatusCode' property
	///     during object initialization.
	/// </value>
	public bool StatusEnabled
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the UserName of the User.
	/// </summary>
	/// <value>
	///     The UserName is a string that represents the unique identifier for a User.
	/// </value>
	public string UserName
	{
		get;
		set;
	}

	/// <summary>
	///     Resets all properties of the User object to their default values.
	/// </summary>
	public void Clear()
	{
		UserName = "";
		FullName = "";
		Role = "";
		Status = "";
		EmailAddress = "";
		FirstName = "";
		LastName = "";
		RoleID = "RC";
		StatusCode = "ACT";
		StatusEnabled = true;
		Password = "";
	}

	/// <summary>
	///     Creates a shallow copy of the current User object.
	/// </summary>
	/// <returns>A new User object copied from the current instance.</returns>
	public User Copy()
    {
        return MemberwiseClone() as User;
    }
}