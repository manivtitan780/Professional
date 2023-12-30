#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           AdminController.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 18:50
// Last Updated On:     12-30-2023 15:6
// *****************************************/

#endregion

//using Azure.Identity;

namespace ProfSvc_WebAPI.Controllers;

/// <summary>
///     The AdminController is a controller class in the ProfSvc_WebAPI project. It provides endpoints for administrative
///     tasks.
/// </summary>
/// <remarks>
///     This controller includes methods for checking various codes and states, retrieving administrative lists, and saving
///     data.
///     It interacts with the database using stored procedures and returns data in various formats such as boolean,
///     Dictionary, and List.
/// </remarks>
[ApiController, Route("api/[controller]/[action]")]
public class AdminController : ControllerBase
{
	/// <summary>
	///     The AdminController is a controller class in the ProfSvc_WebAPI project. It provides endpoints for administrative
	///     tasks.
	/// </summary>
	/// <remarks>
	///     This controller includes methods for checking various codes and states, retrieving administrative lists, and saving
	///     data.
	///     It interacts with the database using stored procedures and returns data in various formats such as boolean,
	///     Dictionary, and List.
	/// </remarks>
	public AdminController(IConfiguration configuration, RedisService redisService)
	{
		_redisService = redisService;
		_configuration = configuration;
	}

	private readonly IConfiguration _configuration;
	private readonly RedisService _redisService;

	/// <summary>
	///     Checks the validity of a given code using a specified method.
	/// </summary>
	/// <param name="methodName">The name of the stored procedure to be executed.</param>
	/// <param name="code">The code to be checked. It can be an integer or a string.</param>
	/// <param name="isString">
	///     A flag indicating whether the provided code is a string. If false, the code is treated as an
	///     integer.
	/// </param>
	/// <returns>A boolean value indicating whether the code is valid.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the provided method name and sets the command type to stored procedure.
	///     Depending on the 'isString' flag, it either adds an integer parameter 'ID' or a character parameter 'Code' to the
	///     command.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	[HttpGet]
	public async Task<bool> CheckCode(string methodName = "", string code = "", bool isString = false)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new(methodName, _connection);
		_command.CommandType = CommandType.StoredProcedure;
		if (!isString)
		{
			_command.Int("ID", code.ToInt32());
		}
		else
		{
			_command.Char("Code", 1, code);
		}

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean();

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the validity of a given job code.
	/// </summary>
	/// <param name="id">The job code to be checked.</param>
	/// <returns>A boolean value indicating whether the job code is valid.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_CheckJobCode" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a character parameter 'Code' to the command with the provided job code.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	public async Task<bool> CheckJobCode(string id)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckJobCode", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("Code", 1, id);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the job option based on the provided code and text.
	/// </summary>
	/// <param name="code">The code to be checked. It is a string.</param>
	/// <param name="text">The text to be checked. It is a string.</param>
	/// <returns>A boolean value indicating whether the text exists for the provided code.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_CheckJobOption".
	///     It adds a character parameter 'Code' and a varchar parameter 'Text' to the command.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	///     A return value of true means the text exists for the provided code, and false means it doesn't exist.
	/// </remarks>
	public async Task<bool> CheckJobOption(string code, string text)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckJobOption", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("Code", 1, code);
		_command.Varchar("Text", 50, text);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks if a role exists in the database.
	/// </summary>
	/// <param name="id">The ID of the role to be checked.</param>
	/// <param name="text">The text representation of the role to be checked.</param>
	/// <returns>A boolean value indicating whether the role exists. True means the role exists, false means it doesn't.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_CheckRole" and sets the command type to stored
	///     procedure.
	///     It adds a character parameter 'ID' and a varchar parameter 'Text' to the command.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	public async Task<bool> CheckRole(string id, string text)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckRole", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("ID", 2, id);
		_command.Varchar("Text", 50, text);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the validity of a given role ID.
	/// </summary>
	/// <param name="id">The role ID to be checked. It is a string.</param>
	/// <returns>A boolean value indicating whether the role ID is valid.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_CheckRoleID" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a character parameter 'ID' to the command with the provided role ID.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	///     A return value of true means the role ID exists, and false means it doesn't exist.
	/// </remarks>
	public async Task<bool> CheckRoleID(string id)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckRoleID", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("ID", 2, id);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks if a given text exists for a specified code.
	/// </summary>
	/// <param name="code">The code associated with the text to be checked.</param>
	/// <param name="text">The text to be checked for existence.</param>
	/// <returns>
	///     A boolean value indicating whether the text exists for the given code. True means the text exists, and false
	///     means the text doesn't exist.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_CheckState" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a character parameter 'Code' and a varchar parameter 'Text' to the command.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	public async Task<bool> CheckState(string code, string text)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckState", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("Code", 2, code);
		_command.Varchar("Text", 50, text);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the validity of a given state code.
	/// </summary>
	/// <param name="code">The state code to be checked. It is a string of 2 characters.</param>
	/// <returns>A boolean value indicating whether the state code is valid.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_CheckStateCode" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a character parameter 'Code' to the command with the provided state code.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	///     A return value of true means the state code exists, and false means it doesn't exist.
	/// </remarks>
	public async Task<bool> CheckStateCode(string code)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckStateCode", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("Code", 2, code);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the status of a given code, text, and appliesTo using the stored procedure 'Admin_CheckStatus'.
	/// </summary>
	/// <param name="code">The code to be checked. It is a string of length 3.</param>
	/// <param name="text">The text to be checked. It is a string of maximum length 50.</param>
	/// <param name="appliesTo">The appliesTo parameter to be checked. It is a string of length 3.</param>
	/// <returns>A boolean value indicating whether the text exists. True means text exists and false means text doesn't exist.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the stored procedure 'Admin_CheckStatus' and sets the command type to stored
	///     procedure.
	///     It adds character parameters 'Code', 'Text', and 'AppliesTo' to the command.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	public async Task<bool> CheckStatus(string code, string text, string appliesTo)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckStatus", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("Code", 3, code);
		_command.Varchar("Text", 50, text);
		_command.Char("AppliesTo", 3, appliesTo);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the validity of a given status code.
	/// </summary>
	/// <param name="code">The status code to be checked. It is a string.</param>
	/// <param name="appliesTo">The context to which the status code applies. It is a string.</param>
	/// <returns>A boolean value indicating whether the status code is valid within the given context.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_CheckStatusCode" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a character parameter 'Code' and a character parameter 'AppliesTo' to the command with the provided status
	///     code and context respectively.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	///     A return value of true means the status code exists for the given context, and false means it doesn't exist.
	/// </remarks>
	public async Task<bool> CheckStatusCode(string code, string appliesTo)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckStatusCode", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("Code", 3, code);
		_command.Char("AppliesTo", 3, appliesTo);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the validity of a given tax term code.
	/// </summary>
	/// <param name="code">The tax term code to be checked.</param>
	/// <returns>A boolean value indicating whether the tax term code is valid.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_CheckTaxTermCode" stored procedure and sets the command type to
	///     stored
	///     procedure.
	///     It adds a character parameter 'Code' to the command with the provided tax term code.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	public async Task<bool> CheckTaxTermCode(string code)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckTaxTermCode", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Char("Code", 1, code);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks if a template name already exists in the database.
	/// </summary>
	/// <param name="id">The ID of the template.</param>
	/// <param name="templateName">The name of the template to be checked.</param>
	/// <returns>
	///     A boolean value indicating whether the template name exists. True means the template name exists, and false
	///     means it does not exist.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the stored procedure 'Admin_CheckTemplateName' and sets the command type to
	///     stored procedure.
	///     It adds an integer parameter 'ID' and a character parameter 'TemplateName' to the command.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	public async Task<bool> CheckTemplateName(int id, string templateName)
	{
		await Task.Yield();

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("Admin_CheckTemplateName", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Int("ID", id);
		_command.Varchar("TemplateName", 50, templateName);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Checks the existence of a given text in the database using a specified entity.
	/// </summary>
	/// <param name="id">The identifier of the entity.</param>
	/// <param name="text">The text to be checked.</param>
	/// <param name="entity">
	///     The entity type. It can be "Title", "Document Type", "Username", "Education", "Eligibility",
	///     "Experience", "Lead Industry", "Lead Source", "Lead Status", "Skill", "Tax Term".
	/// </param>
	/// <param name="code">The code of the entity. If not provided, the 'id' parameter will be used.</param>
	/// <returns>
	///     A boolean value indicating whether the text exists in the database. True means the text exists and false means
	///     the text doesn't exist.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the method name derived from the 'entity' parameter and sets the command type
	///     to stored procedure.
	///     Depending on the 'code' parameter, it either adds an integer parameter 'ID' or a character parameter 'Code' to the
	///     command.
	///     After executing the command, it converts the result to a boolean using the 'ToBoolean' extension method and returns
	///     this value.
	/// </remarks>
	public async Task<bool> CheckText(int id, string text, string entity, string code = "")
	{
		await Task.Yield();
		string _methodName = entity switch
							 {
								 "Title" => "Admin_CheckTitle",
								 "Document Type" => "Admin_CheckDocumentType",
								 "User Name" => "Admin_CheckUserName",
								 "Education" => "Admin_CheckEducation",
								 "Eligibility" => "Admin_CheckEligibility",
								 "Experience" => "Admin_CheckExperience",
								 "Lead Industry" => "Admin_CheckLeadIndustry",
								 "Lead Source" => "Admin_CheckLeadSource",
								 "Lead Status" => "Admin_CheckLeadStatus",
								 "Skill" => "Admin_CheckSkill",
								 "Tax Term" => "Admin_CheckTaxTerm",
								 _ => ""
							 };

		if (_methodName == "")
		{
			return false;
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new(_methodName, _connection);
		_command.CommandType = CommandType.StoredProcedure;
		if (code.NullOrWhiteSpace())
		{
			_command.Int("ID", id.ToInt32());
		}
		else
		{
			_command.Varchar("Code", 10, code);
		}

		_command.Varchar("Text", 100, text);

		await _connection.OpenAsync();

		bool _returnValue = _command.ExecuteScalarAsync().Result.ToBoolean(); //true means Text exists and false means text doesn't exists.

		await _connection.CloseAsync();

		return _returnValue;
	}

	/// <summary>
	///     Retrieves a list of administrative items based on the specified method and filter.
	/// </summary>
	/// <param name="methodName">The name of the stored procedure to be executed.</param>
	/// <param name="filter">An optional filter to apply to the list. If not provided, all items are returned.</param>
	/// <param name="isString">A flag indicating whether the filter is a string. If false, the filter is treated as an integer.</param>
	/// <returns>
	///     A dictionary containing a list of administrative items and the total count of items.
	///     The "GeneralItems" key contains a list of items, each represented as an instance of the AdminList class.
	///     The "Count" key contains the total count of items.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the provided method name and sets the command type to stored procedure.
	///     If a filter is provided, it adds a character parameter 'Filter' to the command.
	///     After executing the command, it reads the results into a list of AdminList instances and counts the total number of
	///     items.
	///     The method then returns a dictionary containing the list of items and the total count.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetAdminList(string methodName, string filter = "", bool isString = true)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		List<AdminList> _generalItems = [];
		await using SqlCommand _command = new(methodName, _connection);
		_command.CommandType = CommandType.StoredProcedure;

		if (!filter.NullOrWhiteSpace())
		{
			_command.Varchar("Filter", 100, filter);
		}

		await _connection.OpenAsync();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync())
		{
			if (isString)
			{
				_generalItems.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4),
									  _reader.GetBoolean(5)));
			}
			else
			{
				_generalItems.Add(new(_reader.GetDataTypeName(0) == "tinyint" ? _reader.GetByte(0) : _reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2),
									  _reader.GetString(3), _reader.GetString(4), _reader.GetBoolean(5)));
			}
		}

		await _reader.NextResultAsync();
		await _reader.ReadAsync();
		int _count = _reader.GetInt32(0);

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "GeneralItems", _generalItems
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Handles the HTTP GET request to retrieve the cache objects. These objects are saved in the memory singleton object
	///     at the front-end views.
	/// </summary>
	/// <returns>
	///     A task that represents the asynchronous operation. The task result contains a dictionary where the key is the cache
	///     name and the value is the cache data.
	/// </returns>
	[HttpGet]
	public async Task GetCache() //Task<Dictionary<string, object>>
	{
		bool _keyExists = await _redisService.CheckKeyExists("States");
		if (!_keyExists)
		{
			List<IntValues> _states = [];
			List<IntValues> _eligibility = [];
			List<KeyValues> _jobOptions = [];
			List<KeyValues> _taxTerms = [];
			List<IntValues> _skills = [];
			List<IntValues> _experience = [];
			List<Template> _templates = [];
			List<User> _users = [];
			List<StatusCode> _statusCodes = [];
			List<Zip> _zips = [];
			List<IntValues> _education = [];
			List<Company> _companies = [];
			List<CompanyContact> _companyContacts = [];
			List<Role> _roles = [];
			List<IntValues> _titles = [];
			List<ByteValues> _leadSources = [];
			List<ByteValues> _leadIndustries = [];
			List<ByteValues> _leadStatus = [];
			List<CommissionConfigurator> _commissionConfigurators = [];
			List<VariableCommission> _variableCommissions = [];
			List<AppWorkflow> _workflows = [];
			List<IntValues> _documentTypes = [];
			List<KeyValues> _communications = [];
			Preferences _preferences = null;

			await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
			await using SqlCommand _command = new("SetCacheTables", _connection);
			_command.CommandType = CommandType.StoredProcedure;

			await _connection.OpenAsync();
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			while (await _reader.ReadAsync()) //States
			{
				_states.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Eligibility
			{
				_eligibility.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Job Options
			{
				_jobOptions.Add(new(_reader.GetString(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Tax Terms
			{
				_taxTerms.Add(new(_reader.GetString(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Skills
			{
				_skills.Add(new(_reader.GetInt32(1), _reader.GetString(0)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Experience
			{
				_experience.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Templates
			{
				_templates.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Users
			{
				_users.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Status Codes
			{
				_statusCodes.Add(new(_reader.GetInt32(6), _reader.GetString(0), _reader.GetString(1), _reader.NString(2), _reader.GetString(3),
									 _reader.GetBoolean(4), _reader.GetBoolean(5)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Zips
			{
				_zips.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetInt32(3)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Education
			{
				_education.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Companies
			{
				_companies.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetInt32(4), _reader.GetString(5),
								   "", _reader.NString(6), "", _reader.GetString(7)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Company Contacts
			{
				_companyContacts.Add(new(_reader.GetInt32(0), _reader.GetInt32(1), _reader.GetString(2), _reader.GetString(3), _reader.GetInt32(4),
										 _reader.GetString(5), _reader.GetString(6)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Roles
			{
				_roles.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetBoolean(2), _reader.GetBoolean(3), _reader.GetBoolean(4),
							   _reader.GetBoolean(5), _reader.GetBoolean(6), _reader.GetBoolean(7), _reader.GetBoolean(8), _reader.GetBoolean(9),
							   _reader.GetBoolean(10), _reader.GetBoolean(11), _reader.GetBoolean(12), _reader.GetBoolean(13), _reader.GetString(14)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Titles
			{
				_titles.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Lead Sources
			{
				_leadSources.Add(new(_reader.GetByte(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Lead Industries
			{
				_leadIndustries.Add(new(_reader.GetByte(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Lead Status
			{
				_leadStatus.Add(new(_reader.GetByte(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Commission Configurators
			{
				_commissionConfigurators.Add(new(_reader.GetInt32(0), _reader.GetInt16(1), _reader.GetInt16(2), _reader.GetByte(3), _reader.GetByte(4)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Variable Commission
			{
				_variableCommissions.Add(new(_reader.GetInt32(0), _reader.GetInt16(1), _reader.GetByte(2), _reader.GetByte(3), _reader.GetByte(4),
											 _reader.GetByte(5)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Workflow
			{
				_workflows.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.NString(2), _reader.GetBoolean(3), _reader.GetString(4),
								   _reader.GetBoolean(5), _reader.GetBoolean(6), "", ""));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync()) //Document Types
			{
				_documentTypes.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}

			await _reader.NextResultAsync();
			await _reader.ReadAsync();
			_preferences = new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3),
							   _reader.GetBoolean(4), _reader.GetBoolean(5), _reader.GetBoolean(6), _reader.GetByte(7),
							   _reader.GetByte(8), _reader.GetByte(9), _reader.GetByte(10), _reader.GetBoolean(11));

			_communications.AddRange(new[]
									 {
										 new KeyValues("A", "Average"), new KeyValues("X", "Excellent"), new KeyValues("F", "Fair"),
										 new KeyValues("G", "Good")
									 });

			await _reader.CloseAsync();

			await _connection.CloseAsync();

			List<string> _keys =
			[
				CacheObjects.States.ToString(), CacheObjects.Eligibility.ToString(), CacheObjects.JobOptions.ToString(), CacheObjects.TaxTerms.ToString(), CacheObjects.Skills.ToString(),
				CacheObjects.Experience.ToString(), CacheObjects.Templates.ToString(), CacheObjects.Users.ToString(), CacheObjects.StatusCodes.ToString(), CacheObjects.Zips.ToString(),
				CacheObjects.Education.ToString(), CacheObjects.Companies.ToString(), CacheObjects.CompanyContacts.ToString(), CacheObjects.Roles.ToString(), CacheObjects.Titles.ToString(),
				CacheObjects.LeadSources.ToString(), CacheObjects.LeadIndustries.ToString(), CacheObjects.LeadStatus.ToString(), CacheObjects.CommissionConfigurators.ToString(),
				CacheObjects.VariableCommissions.ToString(), CacheObjects.Workflow.ToString(), CacheObjects.DocumentTypes.ToString(), CacheObjects.Preferences.ToString(),
				CacheObjects.Communication.ToString()
				//"States", "Eligibility", "JobOptions", "TaxTerms", "Skills", "Experience", "Templates", "Users", "StatusCodes", "Zips", "Education", "Companies", "CompanyContacts", "Roles", "Titles",
				//"LeadSources", "LeadIndustries", "LeadStatus", "CommissionConfigurators", "VariableCommissions", "Workflow", "DocumentTypes", "Preferences", "Communication"
			];
			List<object> _values =
			[
				_states, _eligibility, _jobOptions, _taxTerms, _skills, _experience, _templates, _users, _statusCodes, _zips, _education, _companies, _companyContacts, _roles, _titles, _leadSources,
				_leadIndustries, _leadStatus, _commissionConfigurators, _variableCommissions, _workflows, _documentTypes, _preferences, _communications
			];
			await _redisService.CreateBatchSet(_keys, _values);
		}
	}

	/// <summary>
	///     Retrieves a list of document types from the database.
	/// </summary>
	/// <param name="filter">
	///     An optional filter to apply to the document types. If provided, only document types that match the
	///     filter will be returned.
	/// </param>
	/// <returns>
	///     A dictionary containing two key-value pairs:
	///     - "DocTypes": A list of DocumentType objects retrieved from the database.
	///     - "Count": The total number of DocumentType objects retrieved.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_GetDocumentTypes" stored procedure and sets the command type to
	///     stored procedure.
	///     If a filter is provided, it adds a varchar parameter 'Filter' to the command.
	///     After executing the command, it reads the results into a list of DocumentType objects and a count of the total
	///     number of document types.
	///     These are then returned in a dictionary.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetDocTypes(string filter = "")
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		List<DocumentType> _generalItems = [];
		await using SqlCommand _command = new("Admin_GetDocumentTypes", _connection);
		_command.CommandType = CommandType.StoredProcedure;

		if (!filter.NullOrWhiteSpace())
		{
			_command.Varchar("Filter", 100, filter.UrlDecode());
		}

		await _connection.OpenAsync();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync())
		{
			_generalItems.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
		}

		await _reader.NextResultAsync();
		await _reader.ReadAsync();
		int _count = _reader.GetInt32(0);

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "DocTypes", _generalItems
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Retrieves job options based on a given filter.
	/// </summary>
	/// <param name="filter">The filter to be applied to the job options. It can be a string.</param>
	/// <param name="setTaxTerm">
	///     A flag indicating whether to set the TaxTerm. If false, the TaxTerm is not set.
	/// </param>
	/// <returns>A dictionary containing job options, count, and tax terms.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the "Admin_GetJobOptions" stored procedure and sets the command type to stored
	///     procedure.
	///     If the filter is not null or white space, it adds a character parameter 'Filter' to the command with the provided
	///     filter.
	///     After executing the command, it reads the results into a list of JobOption objects and a count.
	///     If the 'setTaxTerm' flag is true and there are rows in the reader, it reads the results into a list of KeyValues
	///     objects.
	///     It then returns a dictionary with the job options, count, and tax terms.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetJobOptions(string filter = "", bool setTaxTerm = true)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		List<JobOption> _jobOptions = [];
		await using SqlCommand _command = new("Admin_GetJobOptions", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		if (!filter.NullOrWhiteSpace())
		{
			_command.Varchar("Filter", 100, filter);
		}

		_connection.Open();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync())
		{
			_jobOptions.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3),
								_reader.GetBoolean(4), _reader.GetBoolean(5), _reader.GetBoolean(6), _reader.GetString(7),
								_reader.GetBoolean(8), _reader.GetBoolean(9), _reader.GetBoolean(10), _reader.GetBoolean(11),
								_reader.GetString(12), _reader.GetString(13), _reader.GetDecimal(14), _reader.GetBoolean(15)));
		}

		await _reader.NextResultAsync();
		_reader.Read();
		int _count = _reader.GetInt32(0);

		await _reader.NextResultAsync();
		if (!_reader.HasRows || !setTaxTerm)
		{
			return new()
				   {
					   {
						   "JobOptions", _jobOptions
					   },
					   {
						   "Count", _count
					   },
					   {
						   "TaxTerms", null
					   }
				   };
		}

		List<KeyValues> _taxTermKeyValues = [];
		while (await _reader.ReadAsync())
		{
			_taxTermKeyValues.Add(new(_reader.GetString(0), _reader.GetString(1)));
		}

		return new()
			   {
				   {
					   "JobOptions", _jobOptions
				   },
				   {
					   "Count", _count
				   },
				   {
					   "TaxTerms", _taxTermKeyValues
				   }
			   };
	}

	/// <summary>
	///     Retrieves a list of roles based on a provided filter.
	/// </summary>
	/// <param name="filter">A string used to filter the roles to be retrieved. An empty string retrieves all roles.</param>
	/// <returns>
	///     A dictionary containing a list of roles and the total count of roles.
	///     The "Roles" key contains a list of roles, each represented as an instance of the Role class.
	///     The "Count" key contains the total count of roles retrieved.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_GetRoles" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a varchar parameter 'Filter' to the command with the provided filter.
	///     The method then reads the data from the database, creating a new Role instance for each row and adding it to a
	///     list.
	///     After reading all the roles, it reads the total count of roles from the next result set.
	///     Finally, it returns a dictionary containing the list of roles and the total count.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetRoles(string filter = "")
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		_connection.Open();
		await using SqlCommand _command = new("Admin_GetRoles", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Varchar("Filter", 100, filter);
		List<Role> _roles = [];
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync())
		{
			_roles.Add(new(_reader.GetString(0), _reader.GetString(1), _reader.GetBoolean(2), _reader.GetBoolean(3), _reader.GetBoolean(4),
						   _reader.GetBoolean(5), _reader.GetBoolean(6), _reader.GetBoolean(7), _reader.GetBoolean(8), _reader.GetBoolean(9),
						   _reader.GetBoolean(10), _reader.GetBoolean(11), _reader.GetBoolean(12), _reader.GetBoolean(13), _reader.GetString(14)));
		}

		await _reader.NextResultAsync();
		_reader.Read();
		int _count = _reader.GetInt32(0);

		return new()
			   {
				   {
					   "Roles", _roles
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Retrieves a list of search options from the database using a specified stored procedure.
	/// </summary>
	/// <param name="methodName">The name of the stored procedure to be executed.</param>
	/// <param name="paramName">The name of the parameter to be passed to the stored procedure.</param>
	/// <param name="filter">The filter to be applied on the search options.</param>
	/// <returns>A list of strings representing the search options.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the provided method name and sets the command type to stored procedure.
	///     It adds a varchar parameter to the command with the provided parameter name and filter.
	///     After executing the command, it reads the result into a list of strings and returns this list.
	/// </remarks>
	[HttpGet]
	public async Task<List<string>> GetSearchDropDown(string methodName = "", string paramName = "", string filter = "")
	{
		SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new(methodName, _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Varchar(paramName, 100, filter);

		_connection.Open();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		List<string> _listOptions = [];
		while (await _reader.ReadAsync())
		{
			_listOptions.Add(_reader.GetString(0));
		}

		return _listOptions;
	}

	/// <summary>
	///     Retrieves a list of job options based on the provided filter.
	/// </summary>
	/// <param name="filter">The filter to be applied on the job options. It is a string.</param>
	/// <returns>A list of job options that match the provided filter.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_SearchJobOption".
	///     It adds a varchar parameter 'JobOption' to the command with the provided filter.
	///     After executing the command, it reads the result into a list of strings and returns this list.
	///     Each string in the list represents a job option that matches the provided filter.
	/// </remarks>
	[HttpGet]
	public async Task<List<string>> GetSearchJobOptions(string filter = "")
	{
		SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("[Admin_SearchJobOption]", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Varchar("JobOption", 100, filter);

		_connection.Open();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		List<string> _listOptions = [];
		while (await _reader.ReadAsync())
		{
			_listOptions.Add(_reader.GetString(0));
		}

		return _listOptions;
	}

	/// <summary>
	///     Retrieves a list of states based on the provided filter.
	/// </summary>
	/// <param name="filter">
	///     The filter to be applied on the states. It is an optional parameter with a default value of an
	///     empty string.
	/// </param>
	/// <returns>A dictionary containing a list of states and the count of states that match the provided filter.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_GetStates" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a varchar parameter 'Filter' to the command with the provided filter.
	///     It then reads the result into a list of State objects and the count of states that match the filter.
	///     The result is returned as a dictionary with two entries: "States" containing the list of State objects and "Count"
	///     containing the count of states.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetStates(string filter = "")
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		_connection.Open();
		await using SqlCommand _command = new("Admin_GetStates", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Varchar("Filter", 100, filter);
		List<State> _state = [];
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync())
		{
			_state.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2)));
		}

		await _reader.NextResultAsync();
		_reader.Read();
		int _count = _reader.GetInt32(0);

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "States", _state
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Retrieves a list of status codes from the database based on the provided filter.
	/// </summary>
	/// <param name="filter">
	///     The filter to be applied when retrieving status codes. It can be an empty string, in which case
	///     all status codes are retrieved.
	/// </param>
	/// <returns>A dictionary containing a list of status codes and the total count of retrieved status codes.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_GetStatusCodes" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a varchar parameter 'Filter' to the command with the provided filter.
	///     After executing the command, it reads the results into a list of StatusCode objects and the total count of
	///     retrieved status codes.
	///     It then returns a dictionary containing the list of status codes and the count.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetStatusCodes(string filter = "")
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		_connection.Open();
		await using SqlCommand _command = new("Admin_GetStatusCodes", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Varchar("Filter", 100, filter);
		List<StatusCode> _statusCodes = [];
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync())
		{
			_statusCodes.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4),
								 _reader.GetString(5), _reader.GetString(6), _reader.GetBoolean(7), _reader.GetBoolean(8), _reader.GetString(9),
								 _reader.GetString(10), _reader.GetString(11)));
		}

		await _reader.NextResultAsync();
		_reader.Read();
		int _count = _reader.GetInt32(0);

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "StatusCodes", _statusCodes
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Retrieves a list of templates based on the provided filter.
	/// </summary>
	/// <param name="filter">The filter to be applied. It is a string.</param>
	/// <returns>
	///     A dictionary containing a list of templates and the count of templates.
	///     The dictionary has two keys: "Templates" and "Count".
	///     "Templates" is a list of Template objects.
	///     "Count" is an integer representing the total number of templates.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_GetTemplate".
	///     If the filter is not null, empty, or consists only of white-space characters, it adds a varchar parameter 'Filter'
	///     to the command.
	///     After executing the command, it reads the result and creates a list of Template objects.
	///     It then reads the next result set to get the count of templates.
	///     Finally, it returns a dictionary containing the list of templates and the count.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetTemplateList(string filter = "")
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		List<Template> _templates = [];
		await using SqlCommand _command = new("Admin_GetTemplate", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		if (!filter.NullOrWhiteSpace())
		{
			_command.Varchar("Filter", 100, filter);
		}

		_connection.Open();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync())
		{
			_templates.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(3),
							   _reader.GetString(2), _reader.GetString(4), _reader.GetString(5), _reader.GetDateTime(6), _reader.GetString(7),
							   _reader.GetDateTime(8), _reader.GetString(9), _reader.GetString(10), _reader.GetBoolean(11), _reader.NString(13),
							   _reader.GetByte(12)));
		}

		await _reader.NextResultAsync();
		_reader.Read();
		int _count = _reader.GetInt32(0);

		return new()
			   {
				   {
					   "Templates", _templates
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Retrieves a list of users based on a provided filter.
	/// </summary>
	/// <param name="filter">A string used to filter the users. The default value is an empty string.</param>
	/// <returns>
	///     A dictionary containing a list of users, a list of roles, and a count of users.
	///     The "Users" key contains a list of User objects.
	///     The "Roles" key contains a list of KeyValues objects representing roles.
	///     The "Count" key contains an integer representing the total number of users.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_GetUsers" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a varchar parameter 'Filter' to the command with the provided filter.
	///     It then executes the command and reads the results, adding User objects to the users list and KeyValues objects to
	///     the roles list.
	///     It also reads the total count of users.
	///     After all data is read, it closes the reader and the connection, and returns a dictionary with the retrieved data.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetUserList(string filter = "")
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		_connection.Open();
		await using SqlCommand _command = new("Admin_GetUsers", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Varchar("Filter", 100, filter);
		List<User> _users = [];
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		while (await _reader.ReadAsync()) // Get Users
		{
			_users.Add(new(_reader.GetString(0), _reader.NString(1) + " " + _reader.NString(2), _reader.NString(4), _reader.NString(5), _reader.NString(3),
						   _reader.NString(1), _reader.NString(2), _reader.NString(9), _reader.NString(8), ""));
		}

		await _reader.NextResultAsync(); // Get Count
		_reader.Read();
		int _count = _reader.GetInt32(0);

		await _reader.NextResultAsync();
		List<KeyValues> _roles = [];
		while (await _reader.ReadAsync()) // Get Roles
		{
			_roles.Add(new(_reader.GetString(0), _reader.GetString(1)));
		}

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Users", _users
				   },
				   {
					   "Roles", _roles
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Retrieves the variable commission data from the database.
	/// </summary>
	/// <returns>
	///     An instance of the VariableCommission class containing the variable commission data.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_GetVariableCommission" stored procedure and sets the command type to
	///     stored procedure.
	///     After executing the command, it reads the result into a new instance of the VariableCommission class.
	///     If no rows are returned from the database, it returns a new instance of the VariableCommission class with default
	///     values.
	/// </remarks>
	[HttpGet]
	public async Task<VariableCommission> GetVariableCommission()
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		_connection.Open();
		await using SqlCommand _command = new("Admin_GetVariableCommission", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		VariableCommission _variableCommission = null;
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

		if (_reader.HasRows)
		{
			while (await _reader.ReadAsync()) // Get Users
			{
				_variableCommission = new(_reader.GetInt32(0), _reader.GetInt16(1), _reader.GetByte(2), _reader.GetByte(3), _reader.GetByte(4), _reader.GetByte(5));
			}
		}
		else
		{
			_variableCommission = new();
		}

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return _variableCommission;
	}

	/// <summary>
	///     Retrieves workflows based on the provided filter.
	/// </summary>
	/// <param name="filter">A string used to filter the workflows. If no filter is provided, all workflows are returned.</param>
	/// <returns>
	///     A dictionary containing the workflows, roles, status, and count. The "Workflows" key contains a list of AppWorkflow
	///     objects,
	///     the "Roles" key contains a list of KeyValues objects representing roles, the "Status" key contains a list of
	///     KeyValues objects
	///     representing status, and the "Count" key contains the total count of workflows.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the "Admin_GetWorkflow" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds a varchar parameter "Filter" to the command.
	///     The method then reads the data from the database and populates the workflows, roles, and status lists.
	///     After all data is read, it closes the reader and the connection.
	///     Finally, it returns a dictionary containing the workflows, roles, status, and count.
	/// </remarks>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetWorkflows(string filter = "")
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		_connection.Open();
		await using SqlCommand _command = new("Admin_GetWorkflow", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Varchar("Filter", 100, filter);

		List<AppWorkflow> _workflows = [];
		List<KeyValues> _roles = [];
		List<KeyValues> _status = [];

		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

		while (await _reader.ReadAsync()) //Roles
		{
			_roles.Add(new(_reader.GetString(0), _reader.GetString(1)));
		}

		await _reader.NextResultAsync(); //Status
		while (await _reader.ReadAsync())
		{
			_status.Add(new(_reader.GetString(0), _reader.GetString(1)));
		}

		await _reader.NextResultAsync(); //Workflows
		while (await _reader.ReadAsync())
		{
			string _stepName = _reader.NString(1);
			string _nextSteps = _reader.NString(2);
			string _rolesIDs = _reader.NString(4);
			string _stepsString = "", _rolesString = "", _stageString = "";

			if (!_stepName.NullOrWhiteSpace())
			{
				foreach (KeyValues _statusKey in _status.Where(statusKey => statusKey.Key == _stepName))
				{
					_stageString = _statusKey.Value;
					break;
				}
			}

			if (!_nextSteps.NullOrWhiteSpace())
			{
				string[] _nextStringArray = _nextSteps.Split(',');
				foreach (string _nextString in _nextStringArray)
				{
					foreach (KeyValues _statusKey in _status.Where(statusKey => statusKey.Key == _nextString))
					{
						_stepsString += "<br/>" + _statusKey.Value;
						break;
					}
				}

				if (_stepsString.StartsWith("<br/>"))
				{
					_stepsString = _stepsString[5..];
				}
			}

			if (!_rolesIDs.NullOrWhiteSpace())
			{
				string[] _roleStringArray = _rolesIDs.Split(',');
				foreach (string _roleString in _roleStringArray)
				{
					foreach (KeyValues _role in _roles.Where(role => role.Key == _roleString))
					{
						_rolesString += "<br/>" + _role.Value;
						break;
					}
				}

				if (_rolesString.StartsWith("<br/>"))
				{
					_rolesString = _rolesString[5..];
				}
			}

			_workflows.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.NString(2), _reader.GetBoolean(3),
							   _reader.GetString(4), _reader.GetBoolean(5), _reader.GetBoolean(6), _stepsString,
							   _rolesString, _stageString));
		}

		await _reader.NextResultAsync();
		_reader.Read();
		int _count = _reader.GetInt32(0);

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Workflows", _workflows
				   },
				   {
					   "Roles", _roles
				   },
				   {
					   "Status", _status
				   },
				   {
					   "Count", _count
				   }
			   };
	}

	/// <summary>
	///     Saves the administrative list to the database.
	/// </summary>
	/// <param name="methodName">The name of the stored procedure to be executed.</param>
	/// <param name="parameterName">The name of the parameter to be passed to the stored procedure.</param>
	/// <param name="containDescription">A flag indicating whether the list contains a description.</param>
	/// <param name="isString">
	///     A flag indicating whether the provided code is a string. If false, the code is treated as an
	///     integer.
	/// </param>
	/// <param name="adminList">The administrative list to be saved.</param>
	/// <returns>A string value indicating the return code of the operation.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the provided method name and sets the command type to stored procedure.
	///     Depending on the 'isString' flag, it either adds an integer parameter 'ID' or a character parameter 'Code' to the
	///     command.
	///     It also adds a varchar parameter with the provided parameter name and the text from the admin list.
	///     If 'containDescription' is true, it adds another varchar parameter 'Desc' with the text from the admin list.
	///     It also adds a varchar parameter 'User' with the value 'ADMIN' and a bit parameter 'Enabled' with the enabled
	///     status from the admin list.
	///     After executing the command, it reads the return code from the first column of the first row in the result set and
	///     returns this value.
	///     If an exception occurs during the execution of the command, it is caught and ignored, and an empty string is
	///     returned.
	/// </remarks>
	[HttpPost]
	public async Task<string> SaveAdminList(string methodName, string parameterName, bool containDescription, bool isString, [FromBody] AdminList adminList)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();
		string _returnCode = "";
		try
		{
			await using SqlCommand _command = new(methodName, _con);
			_command.CommandType = CommandType.StoredProcedure;
			if (isString)
			{
				_command.Char("Code", 1, adminList.Code.DBNull());
			}
			else
			{
				_command.Int("ID", adminList.ID.DBNull());
			}

			_command.Varchar("" + parameterName, 50, adminList.Text);
			if (containDescription)
			{
				_command.Varchar("Desc", 500, adminList.Text);
			}

			_command.Varchar("User", 10, "ADMIN");
			_command.Bit("Enabled", adminList.IsEnabled);

			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (!_reader.HasRows)
			{
				_returnCode = "";
			}
			else
			{
				_reader.Read();
				_returnCode = _reader.GetValue(0).ToString();
			}
		}
		catch
		{
			// ignored
		}

		return _returnCode;
	}

	/// <summary>
	///     Saves a document type to the database.
	/// </summary>
	/// <param name="docType">The document type to be saved. It is an instance of the DocumentType class.</param>
	/// <returns>
	///     An integer value indicating the result of the save operation. A non-zero value indicates success, while zero
	///     indicates failure.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_SaveDocumentType" and sets the command type to
	///     stored procedure.
	///     It adds a varchar parameter 'ID' and a varchar parameter 'DocumentType' to the command with the properties of the
	///     provided DocumentType instance.
	///     After executing the command, it converts the result to an integer using the 'ToInt32' extension method and returns
	///     this value.
	///     If an exception occurs during the execution of the command, it is caught and ignored, and the method returns zero.
	/// </remarks>
	[HttpPost]
	public async Task<int> SaveDocType(DocumentType docType)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();
		int _returnValue = 0;

		try
		{
			await using SqlCommand _command = new("Admin_SaveDocumentType", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Varchar("Id", 10, docType.ID);
			_command.Varchar("DocumentType", 50, docType.DocType);

			_returnValue = (await _command.ExecuteScalarAsync()).ToInt32();
		}
		catch
		{
			// ignored
		}

		return _returnValue;
	}

	/// <summary>
	///     Saves the provided job options to the database.
	/// </summary>
	/// <param name="jobOption">The job options to be saved. It is an instance of the JobOption class.</param>
	/// <returns>A string value representing the code of the saved job options.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_SaveJobOptions".
	///     It adds various parameters to the command based on the properties of the provided JobOption instance.
	///     After executing the command, it returns the code of the saved job options.
	///     In case of any exception during the execution, the exception is caught and ignored, and the method proceeds to
	///     return the job option code.
	/// </remarks>
	[HttpPost]
	public async Task<string> SaveJobOptions(JobOption jobOption)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();

		try
		{
			await using SqlCommand _command = new("Admin_SaveJobOptions", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Char("Code", 1, jobOption.Code);
			_command.Varchar("JobOptions", 50, jobOption.Option);
			_command.Varchar("Desc", 500, jobOption.Description);
			_command.Bit("Duration", jobOption.Duration);
			_command.Bit("Rate", jobOption.Rate);
			_command.Bit("Sal", jobOption.Sal);
			_command.Varchar("TaxTerms", 20, jobOption.Tax);
			_command.Bit("Expenses", jobOption.Exp);
			_command.Bit("PlaceFee", jobOption.PlaceFee);
			_command.Bit("Benefits", jobOption.Benefits);
			_command.Bit("ShowHours", jobOption.ShowHours);
			_command.Varchar("RateText", 255, jobOption.RateText);
			_command.Varchar("PercentText", 255, jobOption.PercentText);
			_command.Decimal("CostPercent", 5, 2, jobOption.CostPercent);
			_command.Bit("ShowPercent", jobOption.ShowPercent);
			_command.Varchar("User", 10, "ADMIN");

			_command.ExecuteNonQuery();
		}
		catch
		{
			// ignored
		}

		return jobOption.Code;
	}

	/// <summary>
	///     Saves the preferences data to the database.
	/// </summary>
	/// <param name="preferences">
	///     The preferences data to be saved. It is an instance of the Preferences class.
	/// </param>
	/// <returns>
	///     An integer value indicating the result of the save operation. A return value of 1 means the operation was
	///     successful.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_SavePreferences" stored procedure and sets the command type to
	///     stored procedure.
	///     It adds parameters to the command with the properties of the provided Preferences instance.
	///     After executing the command, it returns 1 to indicate the operation was successful.
	///     If an exception occurs during the operation, it is caught and ignored, and the method returns 1.
	/// </remarks>
	[HttpPost]
	public async Task<int> SavePreferences(Preferences preferences)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();

		try
		{
			await using SqlCommand _command = new("Admin_SavePreferences", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Varchar("ReqPriorityHigh", 7, preferences.HighPriorityColor);
			_command.Varchar("ReqPriorityNormal", 7, preferences.NormalPriorityColor);
			_command.Varchar("ReqPriorityLow", 7, preferences.LowPriorityColor);
			_command.TinyInt("PageSize", preferences.PageSize);

			await _command.ExecuteNonQueryAsync();
		}
		catch
		{
			// ignored
		}

		return 1;
	}

	/// <summary>
	///     Saves a role to the database.
	/// </summary>
	/// <param name="role">The role to be saved. It is an instance of the Role class.</param>
	/// <returns>A string value representing the ID of the saved role.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_SaveRole" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds parameters to the command with the properties of the provided role.
	///     After executing the command, it returns the ID of the saved role.
	///     If an exception occurs during the execution of the command, it is caught and ignored, and the method proceeds to
	///     return the role ID.
	/// </remarks>
	[HttpPost]
	public async Task<string> SaveRole(Role role)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();

		try
		{
			await using SqlCommand _command = new("Admin_SaveRole", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Char("Code", 2, role.ID);
			_command.Varchar("Role", 50, role.RoleName);
			_command.Varchar("Desc", 200, role.Description);
			_command.Bit("ViewCandidate", role.ViewCandidate);
			_command.Bit("ViewRequisition", role.ViewRequisition);
			_command.Bit("EditCandidate", role.EditCandidate);
			_command.Bit("EditRequisition", role.EditRequisition);
			_command.Bit("ChangeCandidateStatus", role.ChangeCandidateStatus);
			_command.Bit("ChangeRequisitionStatus", role.ChangeRequisitionStatus);
			_command.Bit("SendEmail", role.SendEmailCandidate);
			_command.Bit("ForwardResume", role.ForwardResume);
			_command.Bit("DownloadResume", role.DownloadResume);
			_command.Bit("SubmitCandidate", role.SubmitCandidate);
			_command.Bit("ViewClients", role.ViewClients);
			_command.Bit("EditClients", role.EditClients);
			_command.Varchar("User", 10, "ADMIN");

			_command.ExecuteNonQuery();
		}
		catch
		{
			// ignored
		}

		return role.ID;
	}

	/// <summary>
	///     Saves the state information to the database.
	/// </summary>
	/// <param name="state">The state object containing the state information to be saved.</param>
	/// <returns>The ID of the saved state as an integer.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_SaveState" and sets the command type to stored
	///     procedure.
	///     It adds parameters to the command with the state information, including ID, Code, StateName, Country, and User.
	///     After executing the command, it converts the result to an integer using the 'ToInt32' extension method and returns
	///     this value.
	///     If an error occurs during the execution of the command, the method returns 0.
	/// </remarks>
	[HttpPost]
	public async Task<int> SaveState(State state)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();
		int _id = 0;
		try
		{
			await using SqlCommand _command = new("Admin_SaveState", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("ID", state.ID.DBNull());
			_command.Char("Code", 2, state.Code);
			_command.Varchar("State", 50, state.StateName);
			_command.Varchar("Country", 50, "USA");
			_command.Varchar("User", 10, "ADMIN");

			_id = _command.ExecuteScalar().ToInt32();
		}
		catch
		{
			// ignored
		}

		return _id;
	}

	/// <summary>
	///     Saves a StatusCode object to the database.
	/// </summary>
	/// <param name="statusCode">The StatusCode object to be saved.</param>
	/// <returns>An integer value representing the ID of the saved StatusCode object.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_SaveStatusCode" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds various parameters to the command, including the ID, Code, Status, Description, AppliesToCode, Icon, Color,
	///     SubmitCandidate, ShowCommission, and User of the StatusCode object.
	///     After executing the command, it converts the result to an integer using the 'ToInt32' extension method and returns
	///     this value.
	///     If an exception occurs during the execution of the command, it is caught and ignored, and the method returns 0.
	/// </remarks>
	[HttpPost]
	public async Task<int> SaveStatusCode(StatusCode statusCode)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();
		int _id = 0;

		try
		{
			await using SqlCommand _command = new("Admin_SaveStatusCode", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("ID", statusCode.ID.DBNull());
			_command.Char("Code", 3, statusCode.Code);
			_command.Varchar("Status", 50, statusCode.Status);
			_command.Varchar("Desc", 100, statusCode.Description);
			_command.Char("AppliesTo", 3, statusCode.AppliesToCode);
			_command.Varchar("Icon", 255, statusCode.Icon);
			_command.Varchar("Color", 10, statusCode.Color);
			_command.Bit("SubmitCandidate", statusCode.SubmitCandidate);
			_command.Bit("ShowCommission", statusCode.ShowCommission);
			_command.Varchar("User", 10, "ADMIN");

			_id = _command.ExecuteScalar().ToInt32();
		}
		catch
		{
			// ignored
		}

		return _id;
	}

	/// <summary>
	///     Saves a template to the database.
	/// </summary>
	/// <param name="template">The template to be saved. It is an instance of the Template class.</param>
	/// <returns>
	///     An integer value indicating the result of the save operation. A non-zero value indicates success, while zero
	///     indicates failure.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_SaveTemplate" stored procedure and sets the command type to stored
	///     procedure.
	///     It adds various parameters to the command, including the template's ID, name, CC, subject, content, notes, send to,
	///     action, user, and enabled status.
	///     After executing the command, it converts the result to an integer using the 'ToInt32' extension method and returns
	///     this value.
	///     In case of any exception during the execution, it is caught and ignored, and the method returns zero.
	/// </remarks>
	[HttpPost]
	public async Task<int> SaveTemplate(Template template)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();
		int _returnValue = 0;

		try
		{
			await using SqlCommand _command = new("Admin_SaveTemplate", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", template.ID);
			_command.Varchar("TemplateName", 50, template.TemplateName);
			_command.Varchar("Cc", 2000, template.CC);
			_command.Varchar("Subject", 255, template.Subject);
			_command.Varchar("Template", -1, template.TemplateContent);
			_command.Varchar("Notes", 500, template.Notes);
			_command.Varchar("SendTo", 200, template.SendTo);
			_command.TinyInt("Action", template.Action);
			_command.Varchar("User", 10, "ADMIN");
			_command.Bit("Enabled", template.IsEnabled);

			_returnValue = _command.ExecuteScalar().ToInt32();
		}
		catch
		{
			// ignored
		}

		return _returnValue;
	}

	/// <summary>
	///     Saves a user to the database.
	/// </summary>
	/// <param name="user">The user to be saved. It is an instance of the User class.</param>
	/// <returns>A string value representing the username of the saved user.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_SaveUser".
	///     It adds various parameters to the command, such as 'ID', 'FirstName', 'LastName', 'Email', 'Role', 'Status',
	///     'User', 'Password', and 'Passwd', using the properties of the provided User instance.
	///     After executing the command, it returns the username of the saved user.
	///     If an exception occurs during the execution of the command, it is caught and ignored, and the method proceeds to
	///     return the username.
	/// </remarks>
	[HttpPost]
	public async Task<string> SaveUser(User user)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();

		try
		{
			await using SqlCommand _command = new("Admin_SaveUser", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Varchar("Id", 10, user.UserName);
			_command.Varchar("FirstName", 50, user.FirstName);
			_command.Varchar("LastName", 200, user.LastName);
			_command.Varchar("Email", 200, user.EmailAddress);
			_command.Varchar("Role", 2, user.RoleID);
			_command.Varchar("Status", 3, user.StatusEnabled ? "ACT" : "INA");
			_command.Varchar("User", 10, "ADMIN");
			_command.Binary("Password", 16, user.Password.NullOrWhiteSpace() ? DBNull.Value : General.SHA512PasswordHash(user.Password));
			_command.Varchar("Passwd", 30, user.Password.NullOrWhiteSpace() ? DBNull.Value : user.Password);

			_command.ExecuteNonQuery();
		}
		catch
		{
			// ignored
		}

		return user.UserName;
	}

	/// <summary>
	///     Saves the variable commission data to the database.
	/// </summary>
	/// <param name="variableCommission">
	///     The variable commission data to be saved. It is an instance of the VariableCommission
	///     class.
	/// </param>
	/// <returns>
	///     An integer value indicating the result of the save operation. A return value of 1 means the operation was
	///     successful.
	/// </returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the "Admin_SaveVariableCommission" stored procedure and sets the command type to
	///     stored procedure.
	///     It adds parameters to the command with the properties of the provided VariableCommission instance.
	///     After executing the command, it returns 1 to indicate the operation was successful.
	///     If an exception occurs during the operation, it is caught and ignored, and the method returns 1.
	/// </remarks>
	[HttpPost]
	public async Task<int> SaveVariableCommission(VariableCommission variableCommission)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();

		try
		{
			await using SqlCommand _command = new("Admin_SaveVariableCommission", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.SmallInt("@NoofHours", variableCommission.NoOfHours);
			_command.TinyInt("@OverHeadCost", variableCommission.OverheadCost);
			_command.TinyInt("@W2TaxLoadingRate", variableCommission.W2TaxLoadingRate);
			_command.TinyInt("@1099CostRate", variableCommission.CostRate1099);
			_command.TinyInt("@FTERateOffered", variableCommission.FTERateOffered);

			await _command.ExecuteNonQueryAsync();
		}
		catch
		{
			// ignored
		}

		return 1;
	}

	/// <summary>
	///     Saves the provided workflow into the database.
	/// </summary>
	/// <param name="workflow">The workflow to be saved. It is an instance of the AppWorkflow class.</param>
	/// <returns>An integer value indicating the success of the operation. Returns 1 if the operation is successful.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command with the stored procedure "Admin_SaveWorkflow".
	///     It adds various parameters to the command, such as 'ID', 'Step', 'Next', 'IsLast', 'Role', 'Schedule', 'AnyStage',
	///     and 'User'.
	///     After executing the command, it returns 1 indicating the operation was successful.
	///     If any exception occurs during the execution, it is caught and ignored, and the method still returns 1.
	/// </remarks>
	[HttpPost]
	public async Task<int> SaveWorkflow(AppWorkflow workflow)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		await _con.OpenAsync();

		try
		{
			await using SqlCommand _command = new("Admin_SaveWorkflow", _con);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", workflow.ID);
			_command.Varchar("Step", 3, workflow.Step);
			_command.Varchar("Next", 100, workflow.Next);
			_command.Bit("IsLast", workflow.IsLast);
			_command.Varchar("Role", 50, workflow.RoleIDs);
			_command.Bit("Schedule", workflow.Schedule);
			_command.Bit("AnyStage", workflow.AnyStage);
			_command.Varchar("User", 10, "ADMIN");

			await _command.ExecuteNonQueryAsync();

			return workflow.ID;
		}
		catch
		{
			return -1;
			// ignored
		}
	}

	/// <summary>
	///     Toggles the administrative list based on the provided method name, ID, and username.
	/// </summary>
	/// <param name="methodName">The name of the stored procedure to be executed.</param>
	/// <param name="id">The ID or code to be processed. It can be an integer or a string.</param>
	/// <param name="userName">The username to be processed.</param>
	/// <param name="idIsString">
	///     A flag indicating whether the provided ID is a string. If false, the ID is treated as an
	///     integer.
	/// </param>
	/// <param name="isUser">
	///     A flag indicating whether the provided ID is a user. If true, the ID is treated as a user code.
	/// </param>
	/// <returns>The ID or code that was processed.</returns>
	/// <remarks>
	///     This method establishes a connection to the database using a connection string from the configuration.
	///     It then creates a SQL command using the provided method name and sets the command type to stored procedure.
	///     Depending on the 'idIsString' and 'isUser' flags, it either adds an integer parameter 'ID', a character parameter
	///     'Code', or a varchar parameter 'Code' to the
	///     command.
	///     It also adds a varchar parameter 'User' to the command with the provided username.
	///     After executing the command, it returns the ID or code that was processed.
	/// </remarks>
	[HttpPost]
	public async Task<string> ToggleAdminList(string methodName, string id, string userName, bool idIsString, bool isUser = false)
	{
		await using SqlConnection _con = new(_configuration.GetConnectionString("DBConnect"));
		_con.Open();
		try
		{
			await using SqlCommand _command = new(methodName, _con);
			_command.CommandType = CommandType.StoredProcedure;
			if (!idIsString)
			{
				_command.Int("ID", id.ToInt32());
			}
			else if (!isUser)
			{
				_command.Char("Code", 1, id);
			}
			else
			{
				_command.Varchar("Code", 10, id);
			}

			_command.Varchar("User", 10, userName);
			_command.ExecuteNonQuery();
		}
		catch
		{
			// ignored
		}

		return id;
	}
}