#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           UploadResumeValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          08-07-2023 21:13
// Last Updated On:     10-15-2023 19:07
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validators;

/// <summary>
///     Represents a validator for the <see cref="UploadResume" /> class.
/// </summary>
/// <remarks>
///     This validator ensures that the ID and Files properties of an <see cref="UploadResume" /> instance are properly
///     set.
///     The ID property cannot be empty and the Files property must contain at least one file.
/// </remarks>
public class UploadResumeValidator : AbstractValidator<UploadResume>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UploadResumeValidator" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets the RuleLevelCascadeMode to Stop and defines validation rules for the ID and Files properties
    ///     of the UploadResume instance.
    ///     The ID property cannot be empty and the Files property must contain at least one file.
    /// </remarks>
    public UploadResumeValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ID).NotEmpty().WithMessage("ID cannot be zero.");

        RuleFor(x => x.Files).NotEmpty().WithMessage("Select a file to upload.");
    }
}