#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           CompanyController.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     08-30-2023 20:26
// *****************************************/

#endregion

namespace ProfSvc_WebAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class CompanyController : ControllerBase
{
	/// <summary>
	///     Initializes a new instance of the <see cref="CompanyController" /> class.
	/// </summary>
	/// <param name="configuration">The application configuration, injected by the ASP.NET Core DI container.</param>
	public CompanyController(IConfiguration configuration) => _configuration = configuration;

	private readonly IConfiguration _configuration;

	/// <summary>
	///     Deletes a company document based on the provided document ID and user.
	/// </summary>
	/// <param name="documentID">The ID of the document to be deleted.</param>
	/// <param name="user">The user requesting the deletion.</param>
	/// <returns>A dictionary containing the status of the deleted document.</returns>
	[HttpPost]
	public async Task<Dictionary<string, object>> DeleteCompanyDocument(int documentID, string user)
	{
		await Task.Yield();
		List<RequisitionDocuments> _documents = new();
		if (documentID == 0)
		{
			return new()
				   {
					   {
						   "Document", _documents
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("DeleteClientDocuments", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("ClientDocumentID", documentID);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_documents.Add(new(_reader.GetInt32(0), _reader.GetInt32(1), _reader.NString(2), _reader.NString(3),
									   _reader.NString(6), $"{_reader.NDateTime(5)} [{_reader.NString(4)}]", _reader.NString(7), _reader.GetString(8)));
				}
			}

			await _reader.CloseAsync();
		}
		catch
		{
			//
		}

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Document", _documents
				   }
			   };
	}

	/// <summary>
	///     Deletes a contact from a company based on the provided contact ID, company ID, company name, and user.
	/// </summary>
	/// <param name="id">The ID of the contact to be deleted.</param>
	/// <param name="companyID">The ID of the company from which the contact will be deleted.</param>
	/// <param name="companyName">The name of the company from which the contact will be deleted.</param>
	/// <param name="user">The user requesting the deletion.</param>
	/// <returns>A dictionary containing the list of remaining contacts after deletion.</returns>
	[HttpPost]
	public async Task<Dictionary<string, object>> DeleteContact(int id, int companyID, string companyName, string user)
	{
		await Task.Yield();
		List<CompanyContact> _contacts = new();
		if (id == 0)
		{
			return new()
				   {
					   {
						   "Contacts", _contacts
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("ToggleCompanyContact", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("ContactId", id);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_contacts.Add(new(_reader.GetInt32(0), companyID, $"{_reader.GetString(1)} {_reader.GetString(2)}", _reader.NString(17),
									  _reader.GetInt32(18), _reader.GetString(19), companyName, _reader.GetString(1), _reader.GetString(2),
									  _reader.GetString(3), _reader.GetString(4), _reader.GetString(5).StripPhoneNumber().FormatPhoneNumber(),
									  _reader.GetString(6), _reader.GetString(7).StripPhoneNumber().FormatPhoneNumber(),
									  _reader.GetString(8).StripPhoneNumber().FormatPhoneNumber(), _reader.GetInt32(9), _reader.NString(25),
									  _reader.GetString(10), _reader.GetString(16), _reader.GetString(20), _reader.GetString(21),
									  _reader.GetBoolean(22), _reader.GetString(23), _reader.GetString(24), _reader.NString(12),
									  _reader.GetDateTime(13), _reader.NString(14), _reader.GetDateTime(15)));
				}
			}

			await _reader.CloseAsync();
		}
		catch
		{
			//
		}

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Contacts", _contacts
				   }
			   };
	}

	/// <summary>
	///     Downloads a file based on the provided document ID.
	/// </summary>
	/// <param name="documentID">The ID of the document to be downloaded.</param>
	/// <returns>A <see cref="DocumentDetails" /> object containing the details of the downloaded document.</returns>
	[HttpGet]
	public async Task<DocumentDetails> DownloadFile(int documentID)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("GetCompaniesDocumentDetails", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Int("DocumentID", documentID);

		await _connection.OpenAsync();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

		DocumentDetails _documentDetails = null;
		while (_reader.Read())
		{
			_documentDetails = new(_reader.GetInt32(0), _reader.NString(1), _reader.NString(2), _reader.NString(3));
		}

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return _documentDetails;
	}

	/// <summary>
	///     Retrieves the details of a company based on the provided company ID and user.
	/// </summary>
	/// <param name="companyID">The ID of the company whose details are to be retrieved.</param>
	/// <param name="user">The user requesting the company details.</param>
	/// <returns>A dictionary containing the details of the company, its contacts, requisitions, and documents.</returns>
	[HttpGet]
	public async Task<ActionResult<Dictionary<string, object>>> GetCompanyDetails(int companyID, string user)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		CompanyDetails _company = null;
		string _companyName = "";
		//string _candRating = "", _candMPC = "";

		await using SqlCommand _command = new("GetCompanyDetails", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Int("@CompanyID", companyID);
		_command.Varchar("@User", 10, user);

		await _connection.OpenAsync();
		List<CompanyContact> _contacts = new();
		List<RequisitionDocuments> _documents = new();
		List<Requisitions> _requisitions = new();
		try
		{
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows) //Candidate Details
			{
				_reader.Read();
				_company = new(companyID, _reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetInt32(3),
							   _reader.GetString(4), _reader.GetString(5), _reader.GetString(6), _reader.GetString(7), _reader.GetString(8),
							   _reader.GetString(9), _reader.GetBoolean(10), _reader.GetString(11), _reader.GetString(12), _reader.GetString(13),
							   _reader.GetString(14), _reader.GetString(15), _reader.GetString(16), _reader.GetDateTime(17), _reader.GetString(18),
							   _reader.GetDateTime(19));
				_companyName = _reader.GetString(0);
				//_candRating = _reader.NString(28);
				//_candMPC = _reader.NString(30);
			}

			_reader.NextResult(); //Company Contacts
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_contacts.Add(new(_reader.GetInt32(0), companyID, $"{_reader.GetString(1)} {_reader.GetString(2)}", _reader.NString(17),
									  _reader.GetInt32(18), _reader.GetString(19), _companyName, _reader.GetString(1), _reader.GetString(2),
									  _reader.GetString(3), _reader.GetString(4), _reader.GetString(5).StripPhoneNumber().FormatPhoneNumber(),
									  _reader.GetString(6), _reader.GetString(7).StripPhoneNumber().FormatPhoneNumber(),
									  _reader.GetString(8).StripPhoneNumber().FormatPhoneNumber(), _reader.GetInt32(9), _reader.NString(25),
									  _reader.GetString(10), _reader.GetString(16), _reader.GetString(20), _reader.GetString(21),
									  _reader.GetBoolean(22), _reader.GetString(23), _reader.GetString(24), _reader.NString(12),
									  _reader.GetDateTime(13), _reader.NString(14), _reader.GetDateTime(15)));
				}
			}

			_reader.NextResult(); //Documents
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_documents.Add(new(_reader.GetInt32(0), _reader.GetInt32(1), _reader.NString(2), _reader.NString(3),
									   _reader.NString(6), $"{_reader.NDateTime(5)} [{_reader.NString(4)}]", _reader.NString(7), _reader.GetString(8)));
				}
			}

			_reader.NextResult(); //Activity
			while (_reader.Read())
			{
				_requisitions.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4),
									  _reader.GetString(5), $"{_reader.GetDateTime(6).CultureDate()} [{_reader.GetString(7)}]", _reader.GetDateTime(8).CultureDate(),
									  _reader.GetString(9), GetPriority(_reader.GetByte(10)), _reader.GetBoolean(11), _reader.GetBoolean(12),
									  _reader.GetBoolean(13), _reader.GetString(14), _reader.GetString(15), _reader.GetString(7)));
			}

			await _reader.CloseAsync();

			await _connection.CloseAsync();
		}
		catch (Exception)
		{
			//
		}

		return new Dictionary<string, object>
			   {
				   {
					   "Company", _company
				   },
				   {
					   "Contacts", _contacts
				   },
				   {
					   "Requisitions", _requisitions
				   },
				   {
					   "Document", _documents
				   }
			   };
	}

	/// <summary>
	///     Asynchronously retrieves a dictionary containing information about companies based on the provided search model.
	/// </summary>
	/// <param name="searchModel">The search model containing the search parameters for the companies.</param>
	/// <param name="getCompanyInformation">
	///     A boolean value indicating whether to retrieve additional company information.
	///     Default value is false.
	/// </param>
	/// <returns>
	///     A task that represents the asynchronous operation. The task result contains a dictionary with the following keys:
	///     "Companies" - A list of companies that match the search parameters.
	///     "Count" - The total number of companies that match the search parameters.
	///     "CompaniesList" - A list of companies with additional information if getCompanyInformation is set to true.
	///     "Contacts" - A list of company contacts if getCompanyInformation is set to true.
	///     "Skills" - A list of skills if getCompanyInformation is set to true.
	/// </returns>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetGridCompanies([FromBody] CompanySearch searchModel, bool getCompanyInformation = false)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		List<Company> _companies = new();
		await using SqlCommand _command = new("GetGridCompanies", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Int("Count", searchModel.ItemCount);
		_command.Int("Page", searchModel.Page);
		_command.Int("SortRow", searchModel.SortField);
		_command.TinyInt("SortOrder", searchModel.SortDirection);
		_command.Varchar("Name", 255, searchModel.CompanyName);
		_command.Varchar("Phone", 20, searchModel.Phone);
		_command.Varchar("Email", 255, searchModel.EmailAddress);
		_command.Varchar("State", 255, searchModel.State);
		_command.Bit("MyCompanies", searchModel.MyCompanies);
		_command.Varchar("Status", 50, searchModel.Status);
		_command.Varchar("User", 10, searchModel.User);
		_command.Bit("GetCompanyInformation", getCompanyInformation);

		await _connection.OpenAsync();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

		_reader.Read();
		int _count = _reader.GetInt32(0);

		_reader.NextResult();

		while (_reader.Read())
		{
			_companies.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.NString(9), _reader.NString(5), _reader.GetInt32(10),
							   _reader.NString(11), _reader.NString(2), _reader.NString(6), _reader.NString(3).StripPhoneNumber().FormatPhoneNumber(), 
							   _reader.GetString(4)));
		}

		List<Company> _companiesList = new();
		List<CompanyContact> _companyContacts = new();
		List<IntValues> _skills = new();

		if (getCompanyInformation)
		{
			_reader.NextResult();
			while (_reader.Read())
			{
				_companiesList.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}

			_reader.NextResult();
			while (_reader.Read())
			{
				_companyContacts.Add(new(_reader.GetInt32(0), _reader.GetInt32(2), _reader.GetString(1)));
			}

			_reader.NextResult();
			while (_reader.Read())
			{
				_skills.Add(new(_reader.GetInt32(0), _reader.GetString(1)));
			}
		}

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Companies", _companies
				   },
				   {
					   "Count", _count
				   },
				   {
					   "CompaniesList", _companiesList
				   },
				   {
					   "Contacts", _companyContacts
				   },
				   {
					   "Skills", _skills
				   }
			   };
	}

	/// <summary>
	///     Converts the provided priority value to a string representation.
	/// </summary>
	/// <param name="priority">
	///     The priority value to be converted. The value should be a byte where 0 represents "Low", 2
	///     represents "High", and any other value represents "Medium".
	/// </param>
	/// <returns>A string representation of the priority. Possible return values are "Low", "Medium", and "High".</returns>
	private static string GetPriority(byte priority)
	{
		return priority switch
			   {
				   0 => "Low",
				   2 => "High",
				   _ => "Medium"
			   };
	}

	/// <summary>
	///     Asynchronously saves the details of a company.
	/// </summary>
	/// <param name="companyDetails">The details of the company to be saved.</param>
	/// <param name="user">The user performing the save operation. Default value is "ADMIN".</param>
	/// <returns>
	///     A task that represents the asynchronous operation. The task result contains an integer that represents the
	///     return code of the operation. A return code of -1 indicates that the companyDetails parameter was null. Any other
	///     return code is the result of the SaveCompany stored procedure.
	/// </returns>
	[HttpPost]
	public async Task<int> SaveCompany(CompanyDetails companyDetails, string user = "ADMIN")
	{
		if (companyDetails == null)
		{
			return -1;
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		int _returnCode = 0;
		try
		{
			await using SqlCommand _command = new("SaveCompany", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("ID", companyDetails.ID, true);
			_command.Varchar("CompanyName", 200, companyDetails.CompanyName);
			_command.Varchar("Address", 500, companyDetails.Address);
			_command.Varchar("City", 50, companyDetails.City);
			_command.Int("StateID", companyDetails.StateID);
			_command.Varchar("Zip", 20, companyDetails.ZipCode);
			_command.Varchar("Email", 255, companyDetails.EmailAddress);
			_command.Varchar("Website", 200, companyDetails.Website);
			_command.Varchar("Phone", 20, companyDetails.Phone);
			_command.Varchar("Extension", 10, companyDetails.Extension);
			_command.Varchar("Fax", 20, companyDetails.Fax);
			_command.Bit("IsHot", companyDetails.IsHot);
			_command.Varchar("Notes", 2000, companyDetails.Notes);
			_command.Varchar("User", 10, user);

			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

			_reader.Read();
			if (_reader.HasRows)
			{
				_returnCode = _reader.GetInt32(0);
			}

			await _reader.CloseAsync();
		}
		catch
		{
			// ignored
		}

		await _connection.CloseAsync();

		return _returnCode;
	}

	/// <summary>
	///     Saves the provided contact details to the database.
	/// </summary>
	/// <param name="contactDetails">The details of the contact to be saved.</param>
	/// <param name="user">The user performing the save operation. Defaults to "ADMIN" if not provided.</param>
	/// <returns>A dictionary containing the list of contacts after the save operation.</returns>
	/// <remarks>
	///     This method uses a stored procedure named "SaveContact" to perform the save operation.
	///     If the provided contact details are null, the method will return a dictionary with a null value for the "Contacts"
	///     key.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveContact(CompanyContact contactDetails, string user = "ADMIN")
	{
		if (contactDetails == null)
		{
			return new()
				   {
					   {
						   "Contacts", null
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		List<CompanyContact> _contacts = new();
		try
		{
			await using SqlCommand _command = new("SaveContact", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("ID", contactDetails.ID, true);
			_command.Varchar("First", 50, contactDetails.FirstName);
			_command.Varchar("Middle", 50, contactDetails.MiddleName);
			_command.Varchar("Last", 50, contactDetails.LastName);
			_command.Varchar("Address", 500, contactDetails.Address);
			_command.Varchar("City", 50, contactDetails.City);
			_command.Int("StateID", contactDetails.StateID);
			_command.Varchar("Zip", 20, contactDetails.ZipCode);
			_command.Varchar("Email", 255, contactDetails.EmailAddress);
			_command.Varchar("Phone", 20, contactDetails.Phone);
			_command.Varchar("Extension", 10, contactDetails.Extension);
			_command.Varchar("Fax", 20, contactDetails.Fax);
			_command.Int("Designation", contactDetails.TitleID);
			_command.Varchar("Department", 100, contactDetails.Department);
			_command.Char("Status", 3, contactDetails.StatusCode);
			_command.Bit("IsPrimary", contactDetails.IsPrimary);
			_command.Varchar("User", 10, user);
			_command.Int("CompanyId", contactDetails.ClientID);

			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

			//_reader.Read();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_contacts.Add(new(_reader.GetInt32(0), contactDetails.ClientID, $"{_reader.GetString(1)} {_reader.GetString(2)}", _reader.NString(17), _reader.GetInt32(18),
									  _reader.GetString(19), "", _reader.GetString(1), _reader.GetString(2), _reader.GetString(3),
									  _reader.GetString(4), _reader.GetString(5).StripPhoneNumber().FormatPhoneNumber(), _reader.GetString(6),
									  _reader.GetString(7).StripPhoneNumber().FormatPhoneNumber(), _reader.GetString(8).StripPhoneNumber().FormatPhoneNumber(),
									  _reader.GetInt32(9), _reader.NString(25), _reader.GetString(10), _reader.GetString(16), _reader.GetString(20),
									  _reader.GetString(21), _reader.GetBoolean(22), _reader.GetString(23), _reader.GetString(24),
									  _reader.NString(12), _reader.GetDateTime(13), _reader.NString(14), _reader.GetDateTime(15)));
				}
			}

			await _reader.CloseAsync();
		}
		catch
		{
			// ignored
		}

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Contacts", _contacts
				   }
			   };
	}

	/// <summary>
	///     Uploads a document to the server and saves its details in the database.
	/// </summary>
	/// <param name="file">The file to be uploaded.</param>
	/// <returns>A dictionary containing the details of the uploaded document.</returns>
	[HttpPost, RequestSizeLimit(60 * 1024 * 1024)]
	public async Task<Dictionary<string, object>> UploadDocument(IFormFile file)
	{
		await Task.Yield();
		string _fileName = file.FileName;
		string _companyID = Request.Form["companyID"].ToString();
		//string _mime = Request.Form.Files[0].ContentDisposition;
		string _internalFileName = Guid.NewGuid().ToString("N");
		Directory.CreateDirectory(Path.Combine(Request.Form["path"].ToString(), "Uploads", "Company", _companyID));
		string _destinationFileName = Path.Combine(Request.Form["path"].ToString(), "Uploads", "Company", _companyID, _internalFileName);

		await using MemoryStream _stream = new();
		await using FileStream _fs = System.IO.File.Open(_destinationFileName, FileMode.OpenOrCreate, FileAccess.Write);
		try
		{
			await file.CopyToAsync(_fs);
			_fs.Flush();
			_fs.Close();
		}
		catch
		{
			_fs.Close();
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		List<RequisitionDocuments> _documents = new();
		try
		{
			await using SqlCommand _command = new("SaveClientDocuments", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("ClientId", _companyID);
			_command.Varchar("DocumentName", 255, Request.Form["name"].ToString());
			_command.Varchar("DocumentLocation", 255, _internalFileName); // GUID File Name
			_command.Varchar("DocumentNotes", 2000, Request.Form["notes"].ToString());
			_command.Varchar("OriginalFileName", 50, _fileName); // Original File Name
			_command.Varchar("DocsUser", 10, Request.Form["user"].ToString());
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_documents.Add(new(_reader.GetInt32(0), _reader.GetInt32(1), _reader.NString(2), _reader.NString(3),
									   _reader.NString(6), $"{_reader.NDateTime(5)} [{_reader.NString(4)}]", _reader.NString(7), _reader.GetString(8)));
				}
			}

			await _reader.CloseAsync();
		}
		catch
		{
			//
		}

		return new()
			   {
				   {
					   "Document", _documents
				   }
			   };
	}
}