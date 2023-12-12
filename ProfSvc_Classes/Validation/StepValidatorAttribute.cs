#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           StepValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          11-04-2022 20:17
// Last Updated On:     11-04-2022 20:26
// *****************************************/

#endregion

#region Using

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.IdentityModel.Protocols;

using ConfigurationManager = System.Configuration.ConfigurationManager;

#endregion

namespace ProfSvc_Classes.Validation;

internal class StepValidatorAttribute : ValidationAttribute
{
    #region Methods

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        
        IConfigurationBuilder _builder = new ConfigurationBuilder();
        _builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
        IConfigurationRoot _root = _builder.AddJsonFile("appsettings.json").Build();
        string _settings = _root["APIHost"];

        //new Microsoft.Extensions.Configuration.Json.JsonConfigurationSource().
        //string _host = ConfigurationManager.AppSettings["APIHost"];
        return new("Check the error" + _settings);
    }

    #endregion
}