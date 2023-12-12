#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           Cooky.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          09-17-2022 20:01
// Last Updated On:     09-17-2022 21:22
// *****************************************/

#endregion

namespace ProfSvc_WebAPI.Code;

[Serializable]
public class Cooky
{
    #region Login

    /// <summary>
    ///     User ID
    /// </summary>
    public string UserID
    {
        get;
        set;
    }

    /// <summary>
    ///     User Role ID
    /// </summary>
    public string RoleID
    {
        get;
        set;
    }

    /// <summary>
    ///     User Role Description
    /// </summary>
    public string Role
    {
        get;
        set;
    }

    /// <summary>
    ///     User Email Address
    /// </summary>
    public string Email
    {
        get;
        set;
    }

    /// <summary>
    ///     User First Name
    /// </summary>
    public string FirstName
    {
        get;
        set;
    }

    /// <summary>
    ///     User Last Name
    /// </summary>
    public string LastName
    {
        get;
        set;
    }

    /// <summary>
    ///     User Last Login Date and Time
    /// </summary>
    public DateTime Login
    {
        get;
        set;
    }

    /// <summary>
    ///     User Last Login IP
    /// </summary>
    public string LoginIp
    {
        get;
        set;
    }

    public static implicit operator ValueTask<object>(Cooky v) => throw new NotImplementedException();

    #endregion
}