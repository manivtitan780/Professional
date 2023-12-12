#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           StateCodeExistsValidationAttribute.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          09-17-2022 20:01
// Last Updated On:     09-17-2022 21:09
// *****************************************/

#endregion

#region Using

#endregion

namespace ProfSvc_AppTrack.Pages.Validation;

public class StateCodeExistsValidationAttribute : ValidationAttribute
{
    #region Methods

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };
        if (context.ObjectInstance is not State _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        if (_currentContext.Code.NullOrWhiteSpace() || _currentContext.Code.Length > 2)
        {
            return new("State Code is required and should be maximum 2 characters long.", _memberNames);
        }

        if (!AdminListDefault.IsAdd)
        {
            return ValidationResult.Success;
        }

        string _url =
            $"{Start.ApiHost}admin/CheckState?code={_currentContext.Code}";

        //if (AdminListDefault.ClientFactory == null)
        //{
        //    return new("Could not verify. Try again.", _memberNames);
        //}

        //HttpClient _client = AdminListDefault.ClientFactory.CreateClient("app");

        //Task<HttpResponseMessage> _response = _client.GetAsync(_url);

        //Task<string> _responseStream = _response.Result.Content.ReadAsStringAsync();

        //return _responseStream.Result == "false" ? ValidationResult.Success : new("State Code already exists. Try again.", _memberNames);

        return ValidationResult.Success;
    }

    #endregion
}