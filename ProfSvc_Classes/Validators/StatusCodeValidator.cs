#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           StatusCodeValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-07-2023 20:24
// Last Updated On:     10-15-2023 19:29
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the <see cref="ProfSvc_Classes.StatusCode" /> class.
/// </summary>
/// <remarks>
///     This class is responsible for validating the properties of a <see cref="ProfSvc_Classes.StatusCode" /> object.
///     It checks for the following:
///     - If the status code is being added and the 'AppliesToCode' property is not null or white space, it validates that
///     the 'Code' property is not empty, has exactly 3 characters, and does not already exist.
///     - The 'Description' property should be less than 100 characters.
///     - The 'AppliesToCode' property should not be empty.
///     - If the 'AppliesToCode' property is not null or white space, it validates that the 'Status' property is not empty,
///     has between 3 and 50 characters, and does not already exist.
/// </remarks>
public class StatusCodeValidator : AbstractValidator<StatusCode>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StatusCodeValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the validation rules for a <see cref="StatusCode" /> object.
    ///     It checks for the following:
    ///     - If the status code is being added and the 'AppliesToCode' property is not null or white space, it validates that
    ///     the 'Code' property is not empty, has exactly 3 characters, and does not already exist.
    ///     - The 'Description' property should be less than 100 characters.
    ///     - The 'AppliesToCode' property should not be empty.
    ///     - If the 'AppliesToCode' property is not null or white space, it validates that the 'Status' property is not empty,
    ///     has between 3 and 50 characters, and does not already exist.
    /// </remarks>
    public StatusCodeValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        When(x => x.IsAdd && !x.AppliesToCode.NullOrWhiteSpace(), () =>
                                                                  {
                                                                      RuleFor(x => x.Code).NotEmpty().WithMessage("Status Code should not be empty.")
                                                                                          .Length(3).WithMessage("Status Code should be exactly {MaxLength} characters.")
                                                                                          .Must((obj, code) => CheckStatusCodeExists(code, obj.AppliesToCode))
                                                                                          .WithMessage("Status Code already exists. Enter another Status Code.");
                                                                  });

        RuleFor(x => x.Description).Length(0, 100).WithMessage("Status Code Description should be less than {MaxLength} characters.");

        RuleFor(x => x.AppliesToCode).NotEmpty().WithMessage("Applies To should not be empty.");

        When(x => !x.AppliesToCode.NullOrWhiteSpace(), () =>
                                                       {
                                                           RuleFor(x => x.Status).NotEmpty().WithMessage("Status Code Text should not be empty.")
                                                                                 .Length(3, 50).WithMessage("Status Code Text should be between {MinLength} and {MaxLength} characters.")
                                                                                 .Must((obj, text) => CheckStatusExists(obj.Code, text, obj.AppliesToCode))
                                                                                 .WithMessage("Status Code Text already exists. Enter another Status Code Text.");
                                                       });
    }

    /// <summary>
    ///     Checks if the provided status code already exists in the system.
    /// </summary>
    /// <param name="statusCode">The status code to check.</param>
    /// <param name="appliesTo">The code that the status code applies to.</param>
    /// <returns>Returns <c>true</c> if the status code does not exist; otherwise, returns <c>false</c>.</returns>
    /// <remarks>
    ///     This method sends an asynchronous GET request to the "Admin/CheckStatusCode" endpoint of the API
    ///     with the provided status code and appliesTo code as query parameters.
    ///     The response is a boolean indicating whether the status code exists in the system.
    /// </remarks>
    private static bool CheckStatusCodeExists(string statusCode, string appliesTo)
    {
        RestClient _restClient = new(GeneralClass.ApiHost ?? string.Empty);
        RestRequest _request = new("Admin/CheckStatusCode")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", statusCode);
        _request.AddQueryParameter("appliesTo", appliesTo);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return !_response;
    }

    /// <summary>
    ///     Checks if a status exists based on the provided status code, status text, and applies-to code.
    /// </summary>
    /// <param name="statusCode">The status code to check.</param>
    /// <param name="status">The status text to check.</param>
    /// <param name="appliesTo">The applies-to code to check.</param>
    /// <returns>Returns false if the status exists, otherwise true.</returns>
    /// <remarks>
    ///     This method makes a REST API call to the "Admin/CheckStatus" endpoint with the provided parameters.
    ///     The API is expected to return a boolean indicating whether the status exists or not.
    /// </remarks>
    private static bool CheckStatusExists(string statusCode, string status, string appliesTo)
    {
        RestClient _restClient = new(GeneralClass.ApiHost ?? string.Empty);
        RestRequest _request = new("Admin/CheckStatus")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", statusCode);
        _request.AddQueryParameter("text", status);
        _request.AddQueryParameter("appliesTo", appliesTo);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return !_response;
    }
}