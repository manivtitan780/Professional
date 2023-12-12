#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           _Host.cshtml.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-03-2023 19:44
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     Represents a model for the Host page in the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     This class is responsible for handling HTTP requests and responses for the Host page.
///     It provides properties for the database connection string, the IP address of the remote client,
///     and the User-Agent header from the HTTP request. It also includes a method to handle GET requests.
/// </remarks>
public class HostModel : PageModel
{
    public HostModel(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccssor = httpContextAccessor;
        _configuration = configuration;
    }

    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccssor;

    /// <summary>
    ///     Gets or sets the database connection string.
    /// </summary>
    /// <value>
    ///     The database connection string.
    /// </value>
    public string ConnectionString
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the IP address of the remote client.
    /// </summary>
    /// <value>
    ///     The IP address of the remote client.
    /// </value>
    public string IPAddress
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the User-Agent header from the HTTP request.
    /// </summary>
    /// <value>
    ///     The User-Agent header value.
    /// </value>
    public string UserAgent
    {
        get;
        set;
    }

    // The following links may be useful for getting the IP Adress:
    // https://stackoverflow.com/questions/35441521/remoteipaddress-is-always-null
    // https://stackoverflow.com/questions/28664686/how-do-i-get-client-ip-address-in-asp-net-core

    /// <summary>
    ///     Handles the GET request for the HostModel page.
    /// </summary>
    /// <remarks>
    ///     This method retrieves the User-Agent header from the request,
    ///     the IP address of the remote client, and the connection string from the configuration.
    ///     It also sets a test string to "ffff".
    /// </remarks>
    public void OnGet()
    {
        UserAgent = Request.Headers["User-Agent"].ToString();
        // Note that the RemoteIpAddress property returns an IPAddress object 
        // which you can query to get required information. Here, however, we pass 
        // its string representation
        if (Request.HttpContext.Connection.RemoteIpAddress != null)
        {
            IPAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        ConnectionString = _configuration.GetConnectionString("DBConnect");
    }
}