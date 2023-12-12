#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CandidateNotesValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-27-2023 15:03
// Last Updated On:     10-14-2023 20:20
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the CandidateNotes class.
/// </summary>
/// <remarks>
///     The CandidateNotesValidator class extends the AbstractValidator class and defines rules for validating instances of
///     the CandidateNotes class.
///     The validation rules ensure that the Notes property of the CandidateNotes class is not empty and its length is
///     between 5 and 1000 characters.
/// </remarks>
public class CandidateNotesValidator : AbstractValidator<CandidateNotes>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CandidateNotesValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets the RuleLevelCascadeMode to Stop and defines validation rules for the Notes property of the
    ///     CandidateNotes class.
    ///     The Notes property must not be empty and its length should be between 5 and 1000 characters.
    /// </remarks>
    public CandidateNotesValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Notes).NotEmpty().WithMessage("Notes cannot be empty")
                             .Length(5, 1000).WithMessage("Notes should be between {MinLength} and {MaxLength} characters.");
    }
}