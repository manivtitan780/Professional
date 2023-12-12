#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           LoginController.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     08-21-2023 20:13
// *****************************************/

#endregion

namespace ProfSvc_WebAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class LoginController : ControllerBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LoginController" /> class.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="IConfiguration" /> to access application configuration settings.</param>
    public LoginController(IConfiguration configuration) => _configuration = configuration;

    private readonly IConfiguration _configuration;

    /// <summary>
    ///     Retrieves the dashboard data for a user with a specific role.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="roleID">The role ID of the user.</param>
    /// <returns>
    ///     A dictionary containing various types of data related to the user's dashboard, such as appointments,
    ///     requisitions, candidates, submission statuses, and user statuses.
    /// </returns>
    [HttpGet("Dashboard")]
    public async Task<Dictionary<string, object>> Dashboard(string userName, string roleID)
    {
        await Task.Yield();
        await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
        string _method = roleID switch
                         {
                             "RC" => "FetchRecruiterDashboard",
                             "SM" => "FetchSalesManagerDashboard",
                             "AD" or "RS" => "FetchFullDeskDashboard",
                             _ => ""
                         };
        await using SqlCommand _command = new(_method, _connection);
        _command.CommandType = CommandType.StoredProcedure;
        _command.Varchar("@UserID", 10, userName);
        _connection.Open();
        await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
        if (!_reader.HasRows)
        {
            return null;
        }

        List<AppointmentData> _appointments = new();
        while (await _reader.ReadAsync()) //Interviews from Current Week-1 to Year+1
        {
            _appointments.Add(new(_reader.GetInt32(0), _reader.GetDateTime(3), _reader.GetDateTime(3).AddHours(1),
                                  $"Title: {_reader.GetString(1)}{Environment.NewLine}Candidate: {_reader.GetString(2)}",
                                  "Interview for " + _reader.GetString(2), false,
                                  $"Location: {_reader.GetString(4)}{(_reader.GetString(5).NullOrWhiteSpace() ? "" : $"{Environment.NewLine}Phone Number:{_reader.GetString(5)}")}",
                                  "", null, "", _reader.GetString(7)));
        }

        List<Requisitions> _requisitions = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync()) //My Requisitions, assigned or Creator or Updater
        {
            _requisitions.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.NString(3), _reader.NString(4), _reader.GetString(6),
                                  _reader.GetString(5), "", "", "", false, false, false, "", "", ""));
        }

        List<Candidates> _candidates = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync()) //Candidates last actioned on to in last 2 months
        {
            _candidates.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.NString(2).FormatPhoneNumber(), _reader.GetString(3), _reader.GetString(4), "", "",
                                false, 0, false, false));
        }

        List<Requisitions> _requisitionsActivity = new(); //All my requisitions to which activity have been recorded in last 2 months
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync()) //My Requisitions, assigned or Creator or Updater
        {
            _requisitionsActivity.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.NString(3), _reader.NString(4), _reader.GetString(6),
                                          _reader.GetString(5), "", "", "", false, false, false, "", "", ""));
        }

        List<SubmissionStatus> _submissionStatus7 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _submissionStatus7.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetInt32(2), _reader.NString(3)));
        }

        List<SubmissionStatus> _submissionStatus30 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _submissionStatus30.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetInt32(2), _reader.NString(3)));
        }

        List<SubmissionStatus> _submissionStatus90 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _submissionStatus90.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetInt32(2), _reader.NString(3)));
        }

        List<SubmissionStatus> _submissionStatus365 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _submissionStatus365.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetInt32(2), _reader.NString(3)));
        }

        //List<ChartStatusPoints> _statusPoints = new();
        await _reader.NextResultAsync();
        string _penColor = "", _hirColor = "", _oexColor = "", _wdrColor = "", _penText = "", _hirText = "", _oexText = "", _wdrText = "";
        while (await _reader.ReadAsync())
        {
            switch (_reader.GetString(0))
            {
                case "PEN":
                    _penColor = _reader.GetString(2);
                    _penText = _reader.GetString(1);
                    break;
                case "HIR":
                    _hirColor = _reader.GetString(2);
                    _hirText = _reader.GetString(1);
                    break;
                case "OEX":
                    _oexColor = _reader.GetString(2);
                    _oexText = _reader.GetString(1);
                    break;
                case "WDR":
                    _wdrColor = _reader.GetString(2);
                    _wdrText = _reader.GetString(1);
                    break;
            }
            //_statusPoints.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetString(2)));
        }

        ChartStatusPoints _csp = new("PEN", _penText, _penColor, "HIR", _hirText, _hirColor, "OEX", _oexText, _oexColor,
                                     "WDR", _wdrText, _wdrColor);

        List<ChartUsersStatus> _userStatus7 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _userStatus7.Add(new(_reader.GetString(0), _reader.GetString(5), _reader.GetInt32(1), _reader.GetInt32(2), _reader.GetInt32(3), _reader.GetInt32(4)));
        }

        List<ChartUsersStatus> _userStatus30 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _userStatus30.Add(new(_reader.GetString(0), _reader.GetString(5), _reader.GetInt32(1), _reader.GetInt32(2), _reader.GetInt32(3), _reader.GetInt32(4)));
        }

        List<ChartUsersStatus> _userStatus90 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _userStatus90.Add(new(_reader.GetString(0), _reader.GetString(5), _reader.GetInt32(1), _reader.GetInt32(2), _reader.GetInt32(3), _reader.GetInt32(4)));
        }

        List<ChartUsersStatus> _userStatus365 = new();
        await _reader.NextResultAsync();
        while (await _reader.ReadAsync())
        {
            _userStatus365.Add(new(_reader.GetString(0), _reader.GetString(5), _reader.GetInt32(1), _reader.GetInt32(2), _reader.GetInt32(3), _reader.GetInt32(4)));
        }

        return new()
               {
                   {
                       "Appointments", _appointments
                   },
                   {
                       "Requisitions", _requisitions
                   },
                   {
                       "Candidates", _candidates
                   },
                   {
                       "RequisitionsSubmitted", _requisitionsActivity
                   },
                   {
                       "SubmissionStatus7", _submissionStatus7
                   },
                   {
                       "SubmissionStatus30", _submissionStatus30
                   },
                   {
                       "SubmissionStatus90", _submissionStatus90
                   },
                   {
                       "SubmissionStatus365", _submissionStatus365
                   },
                   {
                       "UserStatus7", _userStatus7
                   },
                   {
                       "UserStatus30", _userStatus30
                   },
                   {
                       "UserStatus90", _userStatus90
                   },
                   {
                       "UserStatus365", _userStatus365
                   },
                   {
                       "ChartStatus", _csp
                   }
               };
    }

    /// <summary>
    ///     Performs user login operation.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="password">The password of the user in base64 format.</param>
    /// <param name="ipAddress">The IP address of the user.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the LoginCooky object with user
    ///     details if login is successful, null otherwise.
    /// </returns>
    [HttpPost("Login")]
    public async Task<LoginCooky> Login(string userName, string password, string ipAddress)
    {
        await Task.Yield();
        byte[] _password = Convert.FromBase64String(password);
        await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
        await using SqlCommand _command = new("ValidateCandidate", _connection);
        _command.CommandType = CommandType.StoredProcedure;
        _command.Varchar("@User", 10, userName);
        _command.Binary("@Password", 16, _password);
        _command.Varchar("@IP", 15, ipAddress);
        _connection.Open();
        await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
        if (!_reader.HasRows)
        {
            return null;
        }

        _reader.Read();
        return new(userName, _reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(5), _reader.GetString(3),
                   _reader.IsDBNull(4) ? DateTime.MinValue : _reader.GetDateTime(4), _reader.NString(6));
    }
}