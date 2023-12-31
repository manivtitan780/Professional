#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           Validations.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-06-2022 18:52
// Last Updated On:     10-17-2023 15:43
// *****************************************/

#endregion

namespace ProfSvc_Classes.Validation;

/// <summary>
///     Provides a set of static methods for validating various types of data.
/// </summary>
/// <remarks>
///     This class contains methods for validating candidate details, administrative list items, dates, document types,
///     hourly rates, job codes, job options, passwords, roles, role IDs, state codes, states, status codes, statuses,
///     template names, and user names.
///     Each method returns a ValidationResult object that indicates whether the validation was successful or not.
/// </remarks>
public class Validations
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Validations" /> class.
    /// </summary>
    /// <remarks>
    ///     The constructor builds the configuration for the application using the appsettings.json file and the environment
    ///     variable "ASPNETCORE_ENVIRONMENT".
    /// </remarks>
    static Validations()
    {
        IConfigurationBuilder _builder = new ConfigurationBuilder().AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

        Configuration = _builder.Build();
    }

    private static readonly IConfiguration Configuration;

    /// <summary>
    ///     Checks if a candidate with the given first name exists in the context.
    /// </summary>
    /// <param name="firstName">The first name of the candidate to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the candidate exists or not.</returns>
    /// <remarks>
    ///     This method checks if a candidate with the given first name exists in the context.
    ///     If the context object is not a CandidateDetails object with IsAdd property set to true,
    ///     or if the first name, last name, or email of the candidate is null or white space,
    ///     the method returns a successful ValidationResult.
    ///     Otherwise, it returns a successful ValidationResult indicating that the candidate exists.
    /// </remarks>
    public static ValidationResult CheckCandidateExists(string firstName, ValidationContext context)
    {
        if (context.ObjectInstance is not CandidateDetails {IsAdd: true} _currentContext)
        {
            return ValidationResult.Success;
        }

        if (firstName.NullOrWhiteSpace() || _currentContext.LastName.NullOrWhiteSpace() || _currentContext.Email.NullOrWhiteSpace())
        {
            return ValidationResult.Success;
        }

        return ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a code already exists in the context.
    /// </summary>
    /// <param name="docType">The document type to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the code exists or not.</returns>
    /// <remarks>
    ///     This method checks if a code already exists in the context.
    ///     If the context object is not an AdminList object with IsAdd property set to true,
    ///     or if the code is null or white space,
    ///     the method returns a ValidationResult indicating an error or success respectively.
    ///     Otherwise, it makes a request to the API to check if the code exists and returns a ValidationResult accordingly.
    /// </remarks>
    public static ValidationResult CheckCodeExists(string docType, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not AdminList _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        if (!_currentContext.IsAdd)
        {
            return ValidationResult.Success;
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        const string entity = "Tax Term";

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckTaxTermCode")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", _currentContext.Code);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new($"Entered {entity} already exists. Please enter another {entity}.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if the provided date is not greater than today's date.
    /// </summary>
    /// <param name="date">The date string to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the date is valid or not.</returns>
    /// <remarks>
    ///     This method converts the provided date string to a DateTime object and checks if it is greater than today's date.
    ///     If the date is greater than today's date, it returns a ValidationResult with an error message.
    ///     Otherwise, it returns a ValidationResult indicating success.
    /// </remarks>
    public static ValidationResult CheckDate(string date, ValidationContext context)
    {
        DateTime _date = date.ToDateTime();
        return _date > DateTime.Today ? new("Updated Date cannot be greater than today.") : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if the provided document type already exists.
    /// </summary>
    /// <param name="docType">The document type to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the document type exists or not.</returns>
    /// <remarks>
    ///     This method checks if a document type already exists in the context.
    ///     If the context object is not a DocumentType object,
    ///     or if the API host configuration is null or white space,
    ///     the method returns a ValidationResult indicating an error.
    ///     Otherwise, it makes a request to the API to check if the document type exists and returns a ValidationResult
    ///     accordingly.
    /// </remarks>
    public static ValidationResult CheckDocumentTypeExists(string docType, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not DocumentType _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        const string entity = "Document Type";

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckText")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", _currentContext.ID);
        _request.AddQueryParameter("text", docType);
        _request.AddQueryParameter("entity", entity);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new($"Entered {entity} already exists. Please enter another {entity}.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if the provided hourly rate is less than or equal to the maximum hourly rate.
    /// </summary>
    /// <param name="hourlyRate">The hourly rate to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the hourly rate is valid or not.</returns>
    /// <remarks>
    ///     This method checks if the provided hourly rate is less than or equal to the maximum hourly rate in the context.
    ///     If the context object is not a CandidateDetails object,
    ///     or if the hourly rate is zero or less than or equal to the maximum hourly rate,
    ///     the method returns a ValidationResult indicating success.
    ///     Otherwise, it returns a ValidationResult with an error message.
    /// </remarks>
    public static ValidationResult CheckHourlyRate(decimal hourlyRate, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not CandidateDetails _currentContext)
        {
            return ValidationResult.Success;
        }

        return _currentContext.HourlyRate == 0 || _currentContext.HourlyRateHigh >= _currentContext.HourlyRate ? ValidationResult.Success
                   : new("Hourly Rate Maximum should be equal to greater than Hourly Rate", _memberNames);
    }

    /// <summary>
    ///     Checks if the provided job code already exists.
    /// </summary>
    /// <param name="jobCode">The job code to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the job code exists or not.</returns>
    /// <remarks>
    ///     This method checks if a job code already exists in the context.
    ///     If the context object is not a JobOption object with IsAdd property set to true,
    ///     it returns a ValidationResult indicating success.
    ///     Otherwise, it makes a request to the API to check if the job code exists and returns a ValidationResult
    ///     accordingly.
    /// </remarks>
    public static ValidationResult CheckJobCodeExists(string jobCode, ValidationContext context)
    {
        string[] _memberNames = {context.MemberName};
        if (context.ObjectInstance is not JobOption {IsAdd: true})
        {
            return ValidationResult.Success;
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckJobCode")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", jobCode);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered Job Code already exists. Please enter another Job Code.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a job option already exists in the context.
    /// </summary>
    /// <param name="jobOption">The job option to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the job option exists or not.</returns>
    /// <remarks>
    ///     This method checks if a job option already exists in the context.
    ///     If the context object is not a JobOption object, it returns a ValidationResult with an error message.
    ///     Otherwise, it makes a request to the API to check if the job option exists and returns a ValidationResult
    ///     accordingly.
    /// </remarks>
    public static ValidationResult CheckJobOptionExists(string jobOption, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not JobOption _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckJobOption")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", _currentContext.Code);
        _request.AddQueryParameter("text", jobOption);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered Job Option already exists. Please enter another Job Option.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if the provided password meets the required criteria.
    /// </summary>
    /// <param name="password">The password to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the password is valid or not.</returns>
    /// <remarks>
    ///     This method checks if the password is not null, not empty, and not consisting only of white-space characters,
    ///     and if its length does not exceed 16 characters.
    ///     If the context object is not a User object with IsAdd property set to true,
    ///     or if the password does not meet the required criteria,
    ///     the method returns a ValidationResult indicating an error.
    ///     Otherwise, it returns a ValidationResult indicating success.
    /// </remarks>
    public static ValidationResult CheckPassword(string password, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not User {IsAdd: true})
        {
            return ValidationResult.Success;
        }

        if (password.NullOrWhiteSpace())
        {
            return new("Password cannot be empty.");
        }

        return password.Length > 16 ? new("Password cannot be greater than 16 characters long.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a role already exists in the context.
    /// </summary>
    /// <param name="role">The role to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the role exists or not.</returns>
    /// <remarks>
    ///     This method checks if a role already exists in the context.
    ///     If the context object is not a Role object,
    ///     the method returns a ValidationResult with an error message.
    ///     Otherwise, it makes a request to the API to check if the role exists and returns a ValidationResult accordingly.
    /// </remarks>
    public static ValidationResult CheckRoleExists(string role, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not Role _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckRole")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", _currentContext.ID);
        _request.AddQueryParameter("text", role);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered Role already exists. Please enter another Role.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a role with the given role ID already exists.
    /// </summary>
    /// <param name="roleID">The ID of the role to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the role ID exists or not.</returns>
    /// <remarks>
    ///     This method checks if a role with the given role ID already exists.
    ///     If the context object is not a Role object with IsAdd property set to true,
    ///     or if the role ID is null or white space,
    ///     the method returns a successful ValidationResult.
    ///     Otherwise, it makes a request to the API to check if the role ID exists and returns a ValidationResult accordingly.
    /// </remarks>
    public static ValidationResult CheckRoleIDExists(string roleID, ValidationContext context)
    {
        string[] _memberNames = {context.MemberName};
        if (context.ObjectInstance is not Role {IsAdd: true})
        {
            return ValidationResult.Success;
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckRoleID")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", roleID);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered RoleID already exists. Please enter another Role ID.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if the provided state code already exists.
    /// </summary>
    /// <param name="stateCode">The state code to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the state code exists or not.</returns>
    /// <remarks>
    ///     This method checks if a state code already exists.
    ///     If the context object is not a State object with IsAdd property set to true,
    ///     the method returns a successful ValidationResult.
    ///     Otherwise, it makes a request to the API to check if the state code exists and returns a ValidationResult
    ///     accordingly.
    ///     If the state code exists, it returns a ValidationResult with an error message.
    ///     Otherwise, it returns a ValidationResult indicating success.
    /// </remarks>
    public static ValidationResult CheckStateCodeExists(string stateCode, ValidationContext context)
    {
        string[] _memberNames = {context.MemberName};
        if (context.ObjectInstance is not State {IsAdd: true})
        {
            return ValidationResult.Success;
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckStateCode")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", stateCode);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered State Code already exists. Please enter another State Code.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a state with the given name exists.
    /// </summary>
    /// <param name="state">The name of the state to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the state exists or not.</returns>
    /// <remarks>
    ///     This method checks if a state with the given name exists.
    ///     If the context object is not a State object, the method returns a ValidationResult with an error message.
    ///     If the API host name is null or white space, the method returns a ValidationResult with an error message.
    ///     Otherwise, it makes a request to the API to check if the state exists and returns a ValidationResult accordingly.
    /// </remarks>
    public static ValidationResult CheckStateExists(string state, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not State _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckState")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", _currentContext.Code);
        _request.AddQueryParameter("text", state);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered State Name already exists. Please enter another State Name.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a status code already exists in the context.
    /// </summary>
    /// <param name="statusCode">The status code to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the status code exists or not.</returns>
    /// <remarks>
    ///     This method checks if a status code already exists in the context.
    ///     If the context object is not a StatusCode object with IsAdd property set to true,
    ///     or if the status code is null or white space,
    ///     the method returns a ValidationResult indicating success.
    ///     Otherwise, it makes a request to the API to check if the status code exists and returns a ValidationResult
    ///     accordingly.
    /// </remarks>
    public static ValidationResult CheckStatusCodeExists(string statusCode, ValidationContext context)
    {
        string[] _memberNames = {context.MemberName};
        if (context.ObjectInstance is not StatusCode {IsAdd: true})
        {
            return ValidationResult.Success;
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckStatusCode")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", statusCode);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered Status Code already exists. Please enter another Status Code.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a status already exists in the context.
    /// </summary>
    /// <param name="status">The status to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the status exists or not.</returns>
    /// <remarks>
    ///     This method checks if a status already exists in the context.
    ///     If the context object is not a StatusCode object, it returns a ValidationResult with an error message.
    ///     Otherwise, it makes a request to the API to check if the status exists and returns a ValidationResult accordingly.
    /// </remarks>
    public static ValidationResult CheckStatusExists(string status, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not StatusCode _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckStatus")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", _currentContext.Code);
        _request.AddQueryParameter("text", status);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered Status already exists. Please enter another Status.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a template with the given name already exists.
    /// </summary>
    /// <param name="templateName">The name of the template to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the template name exists or not.</returns>
    /// <remarks>
    ///     This method checks if a template with the given name already exists.
    ///     If the context object is not a Template object, the method returns a ValidationResult with an error message.
    ///     If the API host name is null or white space, the method returns a ValidationResult with an error message.
    ///     Otherwise, it makes a request to the API to check if the template name exists and returns a ValidationResult
    ///     accordingly.
    /// </remarks>
    public static ValidationResult CheckTemplateNameExists(string templateName, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not AppTemplate _currentContext)
        {
            return new("Could not verify. Try again.", _memberNames);
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckTemplateName")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", _currentContext.ID);
        _request.AddQueryParameter("templateName", templateName);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new("Entered Template Name already exists. Please enter another Template Name.", _memberNames) : ValidationResult.Success;
    }

    /// <summary>
    ///     Checks if a user with the given username already exists.
    /// </summary>
    /// <param name="userName">The username to check.</param>
    /// <param name="context">The validation context.</param>
    /// <returns>A ValidationResult object indicating whether the user exists or not.</returns>
    /// <remarks>
    ///     This method checks if a user with the given username exists.
    ///     If the context object is not a User object with IsAdd property set to true,
    ///     or if the username is null or white space,
    ///     the method returns a successful ValidationResult.
    ///     Otherwise, it makes a request to the API to check if the username exists and returns a ValidationResult
    ///     accordingly.
    /// </remarks>
    public static ValidationResult CheckUserNameExists(string userName, ValidationContext context)
    {
        string[] _memberNames =
        {
            context.MemberName
        };

        if (context.ObjectInstance is not User {IsAdd: true})
        {
            return ValidationResult.Success;
        }

        string _name = Configuration.GetSection("APIHost").Value;

        if (_name.NullOrWhiteSpace())
        {
            return new("Error validating result. Please try again later.");
        }

        const string entity = "User Name";

        RestClient _restClient = new(_name ?? string.Empty);
        RestRequest _request = new("Admin/CheckText")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", 0);
        _request.AddQueryParameter("text", userName);
        _request.AddQueryParameter("entity", entity);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return _response ? new($"Entered {entity} already exists. Please enter another {entity}.", _memberNames) : ValidationResult.Success;
    }
}