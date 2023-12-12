#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CompanyContactValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-05-2023 21:15
// Last Updated On:     10-14-2023 20:23
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the <see cref="ProfSvc_Classes.CompanyContact" /> class.
/// </summary>
/// <remarks>
///     This class is responsible for validating the properties of a <see cref="ProfSvc_Classes.CompanyContact" />
///     instance.
///     It checks for conditions such as non-empty fields, maximum length of strings, and the format of the email address.
/// </remarks>
public class CompanyContactValidator : AbstractValidator<CompanyContact>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CompanyContactValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the validation rules for the <see cref="CompanyContact" /> class.
    ///     It validates the properties of the <see cref="CompanyContact" /> class such as FirstName, LastName, EmailAddress,
    ///     Phone, ZipCode, and City.
    /// </remarks>
    public CompanyContactValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be empty.")
                                 .MaximumLength(50).WithMessage("First Name should not be more than {MaxLength} characters long.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name cannot be empty.")
                                .MaximumLength(50).WithMessage("Last Name should not be more than {MaxLength} characters long.");

        RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email Address cannot be empty.")
                                    .Matches(@"^(([^<>()\[\]\\.,;:\s@\""]+(\.[^<>()\[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$")
                                    .WithMessage("Please enter a valid e-mail address.");

        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone number cannot be empty");

        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Zip Code cannot be empty.")
                               .Length(5).WithMessage("Zip Code has to be exactly {MaxLength} characters.");

        RuleFor(x => x.City).NotEmpty().WithMessage("City cannot be empty.")
                            .Length(2, 50).WithMessage("City should be between {MinLength} and {MaxLength} characters.");
    }
}