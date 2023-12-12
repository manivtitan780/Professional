#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           AdminListTextExistsValidationAttribute.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          11-15-2022 21:08
// Last Updated On:     11-29-2022 21:47
// *****************************************/

#endregion

#region Using

using System.Configuration;

using RestSharp;

#endregion

namespace ProfSvc_Classes.Validation;

public class AdminListTextExistsValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not AdminList _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        string _name = ConfigurationManager.AppSettings["APIHost"];

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        string _entity = _currentContext.Entity;

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckText")
        {
            RequestFormat = DataFormat.Json
        };
        _request.AddQueryParameter("id", _currentContext.ID);
        _request.AddQueryParameter("text", value.ToString());
        _request.AddQueryParameter("entity", _entity);
        _request.AddQueryParameter("code", _currentContext.Code.NullOrWhiteSpace() ? "" : _currentContext.Code);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new($"Entered {_entity} already exists. Please enter another {_entity}.", _memberNames) : ValidationResult.Success;
    }
}