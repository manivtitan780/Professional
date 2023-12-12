#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           RequisitionDocumentsValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          08-03-2023 16:33
// Last Updated On:     10-15-2023 20:06
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the <see cref="ProfSvc_Classes.RequisitionDocuments" /> class.
/// </summary>
/// <remarks>
///     This class extends the <see cref="AbstractValidator{T}" /> class from FluentValidation library,
///     and defines the validation rules for the properties of the <see cref="ProfSvc_Classes.RequisitionDocuments" />
///     class.
/// </remarks>
public class RequisitionDocumentsValidator : AbstractValidator<RequisitionDocuments>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RequisitionDocumentsValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up the validation rules for the <see cref="RequisitionDocuments" /> class.
    ///     It ensures that the DocumentName, DocumentNotes, and Files properties are not empty and have valid lengths.
    /// </remarks>
    public RequisitionDocumentsValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.DocumentName).NotEmpty().WithMessage("Document Name should not be empty.")
                                    .Length(3, 255).WithMessage("Document Name should be between {MinLength} and {MaxLength} characters.");

        RuleFor(x => x.DocumentNotes).NotEmpty().WithMessage("Notes should not be empty")
                                     .Length(10, 2000).WithMessage("Notes should be between {MinLength} and {MaxLength} characters.");

        RuleFor(x => x.Files).NotEmpty().WithMessage("Select a file to upload.");
    }
}