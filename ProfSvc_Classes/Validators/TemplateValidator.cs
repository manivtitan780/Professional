#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           TemplateValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-08-2023 14:42
// Last Updated On:     10-15-2023 19:09
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the <see cref="ProfSvc_Classes.Template" /> class.
/// </summary>
/// <remarks>
///     This class extends the <see cref="AbstractValidator{T}" /> class and defines specific validation rules for the
///     <see cref="ProfSvc_Classes.Template" /> class.
///     The rules include:
///     - CC should be less than 2000 characters.
///     - Notes should not be empty and should be between 5 and 500 characters.
///     - Subject should not be empty and should be between 5 and 255 characters.
///     - Template Content should not be empty and should be between 20 and 32768 characters and can contain HTML tags.
///     - Template Name should not be empty, should be between 2 and 50 characters, and should not already exist in the
///     system.
/// </remarks>
public class TemplateValidator : AbstractValidator<Template>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the validation rules for a <see cref="Template" /> object.
    ///     The rules include:
    ///     - CC should be less than 2000 characters.
    ///     - Notes should not be empty and should be between 5 and 500 characters.
    ///     - Subject should not be empty and should be between 5 and 255 characters.
    ///     - Template Content should not be empty and should be between 20 and 32768 characters and can contain HTML tags.
    ///     - Template Name should not be empty, should be between 2 and 50 characters, and should not already exist in the
    ///     system.
    /// </remarks>
    public TemplateValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CC).MaximumLength(2000).WithMessage("CC should be less than {MaxLength} characters.");

        RuleFor(x => x.Notes).NotEmpty().WithMessage("Notes should not be empty.")
                             .Length(5, 500).WithMessage("Notes should be between {MinLength} and {MaxLength} characters.");

        RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject should not be empty.")
                               .Length(5, 255).WithMessage("Subject should be between {MinLength} and {MaxLength} characters.");

        RuleFor(x => x.TemplateContent).NotEmpty().WithMessage("Template Content should not be empty.")
                                       .Length(20, 32768).WithMessage("Template Content should be between {MinLength} and {MaxLength} characters and can contain HTML tags.");

        RuleFor(x => x.TemplateName).NotEmpty().WithMessage("Template Name should not be empty.")
                                    .Length(2, 50).WithMessage("Template Name should be between {MinLength} and {MaxLength} characters.")
                                    .Must((obj, templateName) => CheckTemplateNameExists(obj.ID, templateName)).WithMessage("Template Name already exists. Enter another Template Name.");
    }

    /// <summary>
    ///     Checks if a template name already exists in the system.
    /// </summary>
    /// <param name="id">The identifier of the template.</param>
    /// <param name="templateName">The name of the template.</param>
    /// <returns>Returns 'true' if the template name does not exist, 'false' otherwise.</returns>
    /// <remarks>
    ///     This method sends a GET request to the 'Admin/CheckTemplateName' endpoint with the template id and name as query
    ///     parameters.
    ///     The endpoint is expected to return a boolean indicating whether the template name exists or not.
    /// </remarks>
    private static bool CheckTemplateNameExists(int id, string templateName)
    {
        RestClient _restClient = new(GeneralClass.ApiHost ?? string.Empty);
        RestRequest _request = new("Admin/CheckTemplateName")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", id);
        _request.AddQueryParameter("templateName", templateName);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return !_response;
    }
}