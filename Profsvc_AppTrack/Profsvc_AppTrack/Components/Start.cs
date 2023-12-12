#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Start.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-04-2023 19:59
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

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

    /// <summary>
    ///     Gets or sets the application's memory cache.
    /// </summary>
    /// <value>
    ///     The memory cache instance used for caching data throughout the application.
    /// </value>
    /// <remarks>
    ///     This property is used to store and retrieve data that is expensive to generate.
    ///     Data stored in the memory cache can improve the performance of the application by reducing the need to regenerate
    ///     complex data or perform costly database queries.
    /// </remarks>
    public static IMemoryCache MemCache
    {
        get;
        set;
    }

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
        if (MemCache == null)
        {
            return;
        }

        RestClient _restClient = new($"{ApiHost}");
        RestRequest _request = new("Admin/GetCache");
        Dictionary<string, object> _restResponse = await _restClient.GetAsync<Dictionary<string, object>>(_request);
        MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1440));

        if (_restResponse == null)
        {
            return;
        }

        List<IntValues> _states = General.DeserializeObject<List<IntValues>>(_restResponse["States"]);
        List<IntValues> _eligibility = General.DeserializeObject<List<IntValues>>(_restResponse["Eligibility"]);
        List<KeyValues> _jobOptions = General.DeserializeObject<List<KeyValues>>(_restResponse["JobOptions"]);
        List<KeyValues> _taxTerms = General.DeserializeObject<List<KeyValues>>(_restResponse["TaxTerms"]);
        List<IntValues> _skills = General.DeserializeObject<List<IntValues>>(_restResponse["Skills"]);
        List<IntValues> _experience = General.DeserializeObject<List<IntValues>>(_restResponse["Experience"]);
        List<Template> _templates = General.DeserializeObject<List<Template>>(_restResponse["Templates"]);
        List<User> _users = General.DeserializeObject<List<User>>(_restResponse["Users"]);
        List<StatusCode> _statusCodes = General.DeserializeObject<List<StatusCode>>(_restResponse["StatusCodes"]);
        List<Zip> _zips = General.DeserializeObject<List<Zip>>(_restResponse["Zips"]);
        List<IntValues> _education = General.DeserializeObject<List<IntValues>>(_restResponse["Education"]);
        List<Company> _companies = General.DeserializeObject<List<Company>>(_restResponse["Companies"]);
        List<CompanyContact> _companyContacts = General.DeserializeObject<List<CompanyContact>>(_restResponse["CompanyContacts"]);
        List<Role> _roles = General.DeserializeObject<List<Role>>(_restResponse["Roles"]);
        List<IntValues> _titles = General.DeserializeObject<List<IntValues>>(_restResponse["Titles"]);
        List<ByteValues> _leadSources = General.DeserializeObject<List<ByteValues>>(_restResponse["LeadSources"]);
        List<ByteValues> _leadIndustries = General.DeserializeObject<List<ByteValues>>(_restResponse["LeadIndustries"]);
        List<ByteValues> _leadStatus = General.DeserializeObject<List<ByteValues>>(_restResponse["LeadStatus"]);
        List<CommissionConfigurator> _commissionConfigurators = General.DeserializeObject<List<CommissionConfigurator>>(_restResponse["CommissionConfigurators"]);
        List<VariableCommission> _variableCommissions = General.DeserializeObject<List<VariableCommission>>(_restResponse["VariableCommissions"]);
        List<AppWorkflow> _workflows = General.DeserializeObject<List<AppWorkflow>>(_restResponse["Workflow"]);
        List<KeyValues> _communications = new();
        List<IntValues> _documentTypes = General.DeserializeObject<List<IntValues>>(_restResponse["DocumentTypes"]);
        Preferences _preferences =
            JsonConvert.DeserializeObject<Preferences>((_restResponse["Preferences"].NullOrWhiteSpace() ? string.Empty : _restResponse["Preferences"].ToString()) ?? string.Empty);
        _communications.AddRange(new[]
                                 {
                                     new KeyValues("A", "Average"), new KeyValues("X", "Excellent"), new KeyValues("F", "Fair"),
                                     new KeyValues("G", "Good")
                                 });

        MemCache.Set("States", _states, _cacheOptions);
        MemCache.Set("Eligibility", _eligibility, _cacheOptions);
        MemCache.Set("JobOptions", _jobOptions, _cacheOptions);
        MemCache.Set("TaxTerms", _taxTerms, _cacheOptions);
        MemCache.Set("Skills", _skills, _cacheOptions);
        MemCache.Set("Experience", _experience, _cacheOptions);
        MemCache.Set("Templates", _templates, _cacheOptions);
        MemCache.Set("Users", _users, _cacheOptions);
        MemCache.Set("StatusCodes", _statusCodes, _cacheOptions);
        MemCache.Set("Zips", _zips, _cacheOptions);
        MemCache.Set("Education", _education, _cacheOptions);
        MemCache.Set("Companies", _companies, _cacheOptions);
        MemCache.Set("CompanyContacts", _companyContacts, _cacheOptions);
        MemCache.Set("Roles", _roles, _cacheOptions);
        MemCache.Set("Titles", _titles, _cacheOptions);
        MemCache.Set("LeadSources", _leadSources, _cacheOptions);
        MemCache.Set("LeadIndustries", _leadIndustries, _cacheOptions);
        MemCache.Set("LeadStatus", _leadStatus, _cacheOptions);
        MemCache.Set("CommissionConfigurators", _commissionConfigurators, _cacheOptions);
        MemCache.Set("VariableCommissions", _variableCommissions, _cacheOptions);
        MemCache.Set("Communication", _communications, _cacheOptions);
        MemCache.Set("Workflow", _workflows, _cacheOptions);
        MemCache.Set("DocumentTypes", _documentTypes, _cacheOptions);
        MemCache.Set("Preferences", _preferences, _cacheOptions);
    }
}