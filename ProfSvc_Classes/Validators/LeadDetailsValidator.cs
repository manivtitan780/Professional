#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           LeadDetailsValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-24-2023 16:16
// Last Updated On:     10-15-2023 19:07
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the <see cref="ProfSvc_Classes.LeadDetails" /> class.
/// </summary>
/// <remarks>
///     This class extends the <see cref="AbstractValidator{T}" /> class, where T is
///     <see cref="ProfSvc_Classes.LeadDetails" />.
///     It defines the rules for validating the properties of the <see cref="ProfSvc_Classes.LeadDetails" /> class.
/// </remarks>
public class LeadDetailsValidator : AbstractValidator<LeadDetails>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LeadDetailsValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the validation rules for the <see cref="LeadDetails" /> class.
    ///     It includes rules for validating the company name, first name, last name, phone number, email address, zip code,
    ///     city, website, and description.
    /// </remarks>
    public LeadDetailsValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Company).NotEmpty().WithMessage("Company Name cannot be empty.")
                               .Length(2, 100).WithMessage("Company Name should be between {MinLength} and {MaxLength} characters.");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name cannot be empty.")
                                 .Length(2, 50).WithMessage("First Name should be between {MinLength} and {MaxLength} characters.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name cannot be empty.")
                                .Length(2, 50).WithMessage("Last Name should be between {MinLength} and {MaxLength} characters.");

        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone Number cannot be empty.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email Address should not be empty")
                             .Length(1, 255).WithMessage("Email Address should be less than {MaxLength} characters.")
                             .Matches(@"^(([^<>()\[\]\\.,;:\s@\""]+(\.[^<>()\[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$")
                             .WithMessage("Please enter a valid e-mail address.");

        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Zip Code cannot be empty.")
                               .Length(5).WithMessage("Zip Code has to be exactly {MaxLength} characters.");

        RuleFor(x => x.City).NotEmpty().WithMessage("City cannot be empty.")
                            .Length(2, 50).WithMessage("City should be between {MinLength} and {MaxLength} characters.");

        When(x => !x.Website.NullOrWhiteSpace(), () =>
                                                 {
                                                     RuleFor(x => x.Website).MaximumLength(255).WithMessage("Website should be less than {MaxLength} characters.")
                                                                            .Matches(@"^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$").WithMessage("Enter a valid Website URL");
                                                 });

        RuleFor(x => x.Description).MaximumLength(8000).WithMessage("Description should be less than {MaxLength} characters.");
    }
}