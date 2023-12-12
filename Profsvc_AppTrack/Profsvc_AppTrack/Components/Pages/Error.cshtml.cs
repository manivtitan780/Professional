#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Error.cshtml.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-03-2023 21:02
// *****************************************/

#endregion

#region Using

using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     Represents the error model for the application.
/// </summary>
/// <remarks>
///     This class is responsible for handling errors and exceptions that occur within the application.
///     It provides information about the error, such as the request ID.
/// </remarks>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true), IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    /// <summary>
    ///     Gets or sets the request ID associated with the current HTTP request.
    /// </summary>
    /// <value>
    ///     The request ID associated with the current HTTP request.
    /// </value>
    /// <remarks>
    ///     This property is used to track the HTTP request within the application. It is displayed on the Error page when an
    ///     error occurs.
    /// </remarks>
    public string RequestID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets a value indicating whether the request ID should be displayed.
    /// </summary>
    /// <value>
    ///     True if the request ID should be displayed; otherwise, false.
    /// </value>
    /// <remarks>
    ///     This property checks if the RequestID is not null or empty. If the RequestID has a value, it returns true,
    ///     indicating that the RequestID should be displayed.
    /// </remarks>
    public bool ShowRequestID => !string.IsNullOrEmpty(RequestID);

    /// <summary>
    ///     Handles the GET request for the Error page.
    /// </summary>
    /// <remarks>
    ///     This method is responsible for retrieving the request ID from the current activity or the HTTP context.
    /// </remarks>
    public void OnGet()
    {
        RequestID = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}
//public ErrorModel(ILogger<ErrorModel> logger) => _logger = logger;
//private readonly ILogger<ErrorModel> _logger;