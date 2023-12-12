﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           TaxTerm.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          02-04-2022 18:47
// Last Updated On:     02-04-2022 19:14
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Class for storing the Candidate Tax Terms.
/// </summary>
public class TaxTerm
{
    /// <summary>
    ///     Initializes the Tax Terms class.
    /// </summary>
    public TaxTerm()
    {
        ClearData();
    }

    /// <summary>
    ///     Initializes the Tax Terms class.
    /// </summary>
    /// <param name="code">Code of the Tax Term.</param>
    /// <param name="taxTermItem">Name of the Tax Term.</param>
    /// <param name="description">Description of the Tax Term.</param>
    /// <param name="updatedDate">Last Updated Date.</param>
    /// <param name="status">String Status for the Tax Term.</param>
    /// <param name="isEnabled">Is the Tax Term Enabled or Disabled.</param>
    public TaxTerm(string code, string taxTermItem, string description, string updatedDate, string status, bool isEnabled)
    {
        Code = code;
        TaxTermItem = taxTermItem;
        Description = description;
        UpdatedDate = updatedDate;
        Status = status;
        IsEnabled = isEnabled;
    }

    /// <summary>
    ///     Code of the Tax Term.
    /// </summary>
    [Required(ErrorMessage = "Tax Term Code is required."), StringLength(1, MinimumLength = 1, ErrorMessage = "Code should be exactly 1 character.")]
    public string Code
    {
        get;
        set;
    }

    /// <summary>
    ///     Description of the Tax Term.
    /// </summary>
    [Required(ErrorMessage = "Tax Term Description is required."),
     StringLength(500, MinimumLength = 10, ErrorMessage = "Tax Term Description should be between 10 to 500 characters.")]
    public string Description
    {
        get;
        set;
    } = "";

    /// <summary>
    ///     Is the Tax Term Enabled or Disabled.
    /// </summary>
    public bool IsEnabled
    {
        get;
        set;
    }

    /// <summary>
    ///     String Status for the Tax Term.
    /// </summary>
    public string Status
    {
        get;
        set;
    } = "";

    /// <summary>
    ///     Name of the Tax Term.
    /// </summary>
    [Required(ErrorMessage = "Tax Term is required."), StringLength(50, MinimumLength = 2, ErrorMessage = "Tax Term should be between 2 to 500 character.")]
    public string TaxTermItem
    {
        get;
        set;
    } = "";

    /// <summary>
    ///     Last Updated Date.
    /// </summary>
    public string UpdatedDate
    {
        get;
        set;
    } = DateTime.Today.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// </summary>
    public void ClearData()
    {
        Code = "";
        TaxTermItem = "";
        Description = "";
        UpdatedDate = "";
        Status = "";
        IsEnabled = false;
    }

    /// <summary>
    ///     Creates a shallow copy of the current object.
    /// </summary>
    /// <returns></returns>
    public TaxTerm Copy() => MemberwiseClone() as TaxTerm;
}