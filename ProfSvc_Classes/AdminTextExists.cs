#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           AdminTextExists.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          09-20-2022 21:05
// Last Updated On:     11-17-2022 20:51
// *****************************************/

#endregion

namespace ProfSvc_Classes;

public class AdminTextExists : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };
        return context.ObjectInstance is not AdminList _currentContext ? new("Could not verify. Try again.", _memberNames) : ValidationResult.Success;
    }
}