#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           Zips.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          09-17-2022 20:01
// Last Updated On:     09-17-2022 21:22
// *****************************************/

#endregion

namespace ProfSvc_WebAPI.Code;

/// <summary>
///     Class to store the Zip Codes.
/// </summary>
public class Zips
{
    #region Constructors

    /// <summary>
    ///     Initializes the Zips Class.
    /// </summary>
    /// <param name="zip">Zip Code of the city.</param>
    /// <param name="city">Name of the City.</param>
    public Zips(string zip, string city)
    {
        ZipCode = zip;
        City = city;
    }

    #endregion

    #region Properties

    /// <summary>
    ///     Name of the City.
    /// </summary>
    public string City
    {
        get;
    }

    /// <summary>
    ///     Zip Code of the city.
    /// </summary>
    public string ZipCode
    {
        get;
    }

    #endregion
}