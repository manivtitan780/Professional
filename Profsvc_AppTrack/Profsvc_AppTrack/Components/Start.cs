#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           Start.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 20:1
// Last Updated On:     12-19-2023 15:56
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components;

/// <summary>
///     The 'Start' class is part of the 'ProfSvc_AppTrack' namespace. This class contains static properties and methods
///     that are used throughout the application.
/// </summary>
/// <remarks>
///     This class includes properties for the API host, connection string, JSON file path, memory cache, and uploads path.
///     These properties are used to configure and manage various aspects of the application.
///     The 'SetCache' method is used to asynchronously set the cache with various application data. This method retrieves
///     data from the "Admin/GetCache" endpoint and deserializes it into various data types. The data is then stored in the
///     memory cache with a sliding expiration of 1440 minutes.
/// </remarks>
public class Start
{
    /// <summary>
    ///     Gets or sets the API host URL.
    /// </summary>
    /// <value>
    ///     The API host URL as a string.
    /// </value>
    /// <remarks>
    ///     This property is used to configure the base URL for API requests throughout the application.
    /// </remarks>
    public static string ApiHost
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the database connection string.
    /// </summary>
    /// <value>
    ///     The database connection string as a string.
    /// </value>
    /// <remarks>
    ///     This property is used to configure the connection to the database throughout the application.
    /// </remarks>
    public static string ConnectionString
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the path to the JSON file used for data storage and retrieval in the application.
    /// </summary>
    /// <value>
    ///     The path to the JSON file.
    /// </value>
    /// <remarks>
    ///     This property is used in various parts of the application where data needs to be stored or retrieved from a JSON
    ///     file.
    ///     For example, it is used as a query parameter in the 'SaveCandidate' and 'SaveCandidateActivity' methods in the
    ///     'Candidate' class.
    /// </remarks>
    public static string JsonFilePath
    {
        get;
        set;
    }

    ///// <summary>
    /////     Gets or sets the application's memory cache.
    ///// </summary>
    ///// <value>
    /////     The memory cache instance used for caching data throughout the application.
    ///// </value>
    ///// <remarks>
    /////     This property is used to store and retrieve data that is expensive to generate.
    /////     Data stored in the memory cache can improve the performance of the application by reducing the need to regenerate
    /////     complex data or perform costly database queries.
    ///// </remarks>
    //public static IMemoryCache MemCache
    //{
    //    get;
    //    set;
    //}

    /// <summary>
    ///     Gets or sets the path for uploading files in the application.
    /// </summary>
    /// <value>
    ///     The path where the application's uploaded files are stored.
    /// </value>
    /// <remarks>
    ///     This property is used in various parts of the application where file upload is required, such as in the 'Candidate'
    ///     page for parsing resumes.
    /// </remarks>
    public static string UploadsPath
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously sets the cache with various application data.
    /// </summary>
    /// <remarks>
    ///     This method retrieves data from the "Admin/GetCache" endpoint and deserializes it into various data types.
    ///     The data is then stored in the memory cache with a sliding expiration of 1440 minutes.
    /// </remarks>
    public static async void SetCache()
    {
        RestClient _restClient = new($"{ApiHost}");
        RestRequest _request = new("Admin/GetCache");
        await _restClient.GetAsync<Dictionary<string, object>>(_request);
    }
}