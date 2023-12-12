#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             GMailService
// File Name:           GMailUsers.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          04-07-2023 15:35
// Last Updated On:     09-28-2023 19:13
// *****************************************/

#endregion

#region Using

using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;

#endregion

namespace GMailService;

public class GMailUsers
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GMailUsers" /> class.
    /// </summary>
    /// <param name="jsonPath">The path to the JSON file containing the service account credentials.</param>
    /// <param name="serviceAccountUser">The user for the service account.</param>
    public GMailUsers(string jsonPath, string serviceAccountUser)
    {
        JsonPath = jsonPath;
        ServiceAccountUser = serviceAccountUser;
    }

    /// <summary>
    ///     Gets or sets the path to the JSON file containing the service account credentials.
    /// </summary>
    /// <value>
    ///     The path to the JSON file.
    /// </value>
    public string JsonPath
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the service account user.
    /// </summary>
    /// <value>
    ///     The service account user.
    /// </value>
    public string ServiceAccountUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Retrieves a list of active user email addresses from the GMail service.
    /// </summary>
    /// <returns>
    ///     A list of strings, where each string is the primary email address of an active user.
    /// </returns>
    /// <remarks>
    ///     This method uses the Google Directory API to fetch the list of users.
    ///     It initializes the service using the service account credentials and user specified in the constructor.
    ///     It then sends a request to the API with a query to only return users who are not suspended.
    ///     The primary email addresses of the users are then extracted from the API response and returned as a list.
    /// </remarks>
    public List<string> GetUsers()
    {
        GoogleCredential _credential = GoogleCredential.FromFile(JsonPath)
                                                       .CreateScoped(DirectoryService.Scope.AdminDirectoryUserReadonly)
                                                       .CreateWithUser(ServiceAccountUser);

        DirectoryService _service = new(new()
                                        {
                                            HttpClientInitializer = _credential
                                        });

        UsersResource.ListRequest _request = _service.Users.List();
        _request.Customer = "my_customer";
        //_request.Domain = "titan-techs.com";
        _request.Query = "isSuspended=false";
        _request.Fields = "users(primaryEmail)";
        Users _results = _request.Execute();

        // Extract the list of user email addresses from the API response
        IList<User> _users = _results.UsersValue;

        return _users.Select(user => user.PrimaryEmail).ToList();
    }
}