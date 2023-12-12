#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           General.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-08-2023 14:44
// Last Updated On:     10-10-2023 20:00
// *****************************************/

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Provides general utility methods and properties for the application.
/// </summary>
/// <remarks>
///     This class is static and cannot be instantiated. It includes properties and methods that are used across the
///     application.
/// </remarks>
internal static class GeneralClass
{
    /// <summary>
    ///     Initializes the General class by loading the API host value from the configuration.
    /// </summary>
    /// <remarks>
    ///     This constructor is thread-safe and uses a lock to ensure that the initialization is done only once.
    /// </remarks>
    static GeneralClass()
    {
        lock (Lock)
        {
            IConfigurationBuilder _builder =
                new ConfigurationBuilder().AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

            IConfigurationRoot _configuration = _builder.Build();

            ApiHost = _configuration.GetSection("APIHost").Value;
        }
    }

    /// <summary>
    ///     The object used for locking to ensure thread safety when initializing the General class.
    /// </summary>
    private static readonly object Lock = new();

    /// <summary>
    ///     Gets the API host value from the configuration.
    /// </summary>
    /// <value>
    ///     The API host as a string.
    /// </value>
    /// <remarks>
    ///     This property is used to configure the base address for REST client instances in various parts of the application.
    /// </remarks>
    public static string ApiHost
    {
        get;
    }
}