#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           JobCodeValidationAttribute.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          09-17-2022 20:01
// Last Updated On:     09-17-2022 21:09
// *****************************************/

#endregion

#region Using

using System.Data;

#endregion

namespace ProfSvc_AppTrack.Pages.Validation;

public class JobCodeValidationAttribute : ValidationAttribute
{
    #region Methods

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };
        if (context.ObjectInstance is not JobOption _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        if (!_currentContext.IsAdd)
        {
            return ValidationResult.Success;
        }

        string _connectionString = Start.ConnectionString; //_currentContext.ConnectionString;
        using SqlConnection _con = new(_connectionString);
        _con.Open();
        try
        {
            using SqlCommand _command = new("Admin_CheckJobOptionCode", _con)
                                        {
                                            CommandType = CommandType.StoredProcedure
                                        };
            _command.Char("@Code", 1, _currentContext.Code);

            bool _exists = _command.ExecuteScalar().ToBoolean();

            return _exists ? new("Job Code already exists. Enter another Code.", _memberNames) : ValidationResult.Success;
        }
        catch
        {
            return new("Could not verify. Try again.", _memberNames);
        }
    }

    #endregion
}