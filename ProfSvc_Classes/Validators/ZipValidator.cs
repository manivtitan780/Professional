#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           ZipValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-17-2023 15:42
// Last Updated On:     10-15-2023 19:04
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the <see cref="ProfSvc_Classes.Zip" /> class.
/// </summary>
/// <remarks>
///     This class is used to validate instances of the <see cref="ProfSvc_Classes.Zip" /> class.
///     It extends the <see cref="AbstractValidator{T}" /> class and defines specific validation rules for the
///     <see cref="ProfSvc_Classes.Zip" /> class.
/// </remarks>
public class ZipValidator : AbstractValidator<Zip>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ZipValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the validation rules for the <see cref="Zip" /> class.
    ///     The rules include:
    ///     - ZipCode cannot be empty and should be exactly 5 characters.
    ///     - StateID cannot be empty.
    ///     - City cannot be empty and should be between 2 and 50 characters.
    /// </remarks>
    public ZipValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Zip Code cannot be empty.")
                               .Length(5).WithMessage("Zip Code should be exactly {MaxLength} characters.");

        RuleFor(x => x.StateID).NotEmpty().WithMessage("State cannot be empty.");

        RuleFor(x => x.City).NotEmpty().WithMessage("City cannot be empty.")
                            .Length(2, 50).WithMessage("City should be between {MinLength} and {MaxLength} characters.");
    }
}