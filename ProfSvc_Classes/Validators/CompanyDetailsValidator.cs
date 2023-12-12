#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CompanyDetailsValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          10-10-2023 15:00
// Last Updated On:     10-14-2023 20:23
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Provides validation for the <see cref="ProfSvc_Classes.CompanyDetails" /> class.
/// </summary>
/// <remarks>
///     This class extends the <see cref="AbstractValidator{T}" /> class and sets up validation rules for the properties of
///     the <see cref="ProfSvc_Classes.CompanyDetails" /> class.
///     The rules are set up in such a way that if any rule fails, the rest of the rules for that property will not be
///     checked (CascadeMode.Stop).
/// </remarks>
public class CompanyDetailsValidator : AbstractValidator<CompanyDetails>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CompanyDetailsValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the validation rules for the <see cref="CompanyDetails" /> class.
    ///     It includes rules for the CompanyName, City, ZipCode, and Phone properties.
    ///     The rules are set up in such a way that if any rule fails, the rest of the rules for that property will not be
    ///     checked (CascadeMode.Stop).
    /// </remarks>
    public CompanyDetailsValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        When(x => x.ID == 0, () =>
                             {
                                 RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Company Name is required.")
                                                            .MaximumLength(200).WithMessage("Company Name should not be more than 200 characters long.");
                             });
        RuleFor(x => x.City).NotEmpty().WithMessage("City Name is required.")
                            .MaximumLength(200).WithMessage("City Name should not be more than 50 characters long.");
        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Zip Code is required.")
                               .Length(5, 5).WithMessage("Zip Code should be 5 characters long.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone Number is required.");
    }
}