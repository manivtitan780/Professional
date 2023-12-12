#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           CandidatesController.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     10-31-2023 15:52
// *****************************************/

#endregion

#region Using

using System.Diagnostics.CodeAnalysis;

using GMailService;

using ProfSvc_AppTrack.Code;

#pragma warning disable CS0162

#endregion

namespace ProfSvc_WebAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class CandidatesController : ControllerBase
{
	/// <summary>
	///     Initializes a new instance of the <see cref="CandidatesController" /> class.
	/// </summary>
	/// <param name="configuration">An instance of <see cref="IConfiguration" /> to access application configuration settings.</param>
	/// <param name="env">
	///     An instance of <see cref="IWebHostEnvironment" /> to provide information about the web hosting
	///     environment an application is running in.
	/// </param>
	public CandidatesController(IConfiguration configuration, IWebHostEnvironment env)
	{
		_configuration = configuration;
		_hostingEnvironment = env;
	}

	private readonly IConfiguration _configuration;

	private readonly IWebHostEnvironment _hostingEnvironment;

	/// <summary>
	///     Cancels the parsing of a resume by deleting the uploaded file from the server.
	/// </summary>
	/// <param name="uploadFiles">
	///     A list of files uploaded by the client. The first file in the list is considered as the
	///     resume file.
	/// </param>
	/// <remarks>
	///     If the file does not exist on the server, no action is taken. If an error occurs during the process, the response
	///     status code is set to 200 and the reason phrase is set to the error message.
	/// </remarks>
	[HttpPost]
	public void CancelParseResume(IList<IFormFile> uploadFiles)
	{
		try
		{
			string filename = _hostingEnvironment.ContentRootPath + $@"\{uploadFiles[0].FileName}";
			if (System.IO.File.Exists(filename))
			{
				System.IO.File.Delete(filename);
			}
		}
		catch (Exception e)
		{
			Response.Clear();
			Response.StatusCode = 200;
			Response.HttpContext.Features.Get<IHttpResponseFeature>()!.ReasonPhrase = "File removed successfully";
			Response.HttpContext.Features.Get<IHttpResponseFeature>()!.ReasonPhrase = e.Message;
		}
	}

	/// <summary>
	///     Deletes a candidate's document from the database.
	/// </summary>
	/// <param name="documentID">The ID of the document to be deleted.</param>
	/// <param name="user">The user who is performing the delete operation.</param>
	/// <returns>A dictionary containing the status of the operation and any relevant data.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to delete the document,
	///     and returns a dictionary containing the result of the operation.
	///     If the operation is successful, the dictionary will contain a list of remaining documents for the candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> DeleteCandidateDocument(int documentID, string user)
	{
		await Task.Delay(1);
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		List<CandidateDocument> _documents = new();
		try
		{
			await using SqlCommand _command = new("DeleteCandidateDocument", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("CandidateDocumentId", documentID);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_documents.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), $"{_reader.NDateTime(4)} [{_reader.NString(5)}]",
									   _reader.GetString(6),
									   _reader.GetString(7), _reader.GetInt32(8)));
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

	/// <summary>
	///     Deletes a candidate's education record from the database.
	/// </summary>
	/// <param name="id">The ID of the education record to be deleted.</param>
	/// <param name="candidateID">The ID of the candidate whose education record is to be deleted.</param>
	/// <param name="user">The user who is performing the delete operation.</param>
	/// <returns>A dictionary containing the updated list of education records for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to delete the education record,
	///     and returns a dictionary containing the updated list of education records.
	///     If the operation is successful, the dictionary will contain a list of remaining education records for the
	///     candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> DeleteEducation(int id, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateEducation> _education = new();
		if (id == 0)
		{
			return new()
				   {
					   {
						   "Education", _education
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("DeleteCandidateEducation", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", id);
			_command.Int("candidateId", candidateID);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_education.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4), _reader.GetString(5),
									   _reader.GetString(6)));
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
					   "Education", _education
				   }
			   };
	}

	/// <summary>
	///     Deletes a candidate's experience record from the database.
	/// </summary>
	/// <param name="id">The ID of the experience record to be deleted.</param>
	/// <param name="candidateID">The ID of the candidate whose experience record is to be deleted.</param>
	/// <param name="user">The user who is performing the delete operation.</param>
	/// <returns>A dictionary containing the updated list of experience records for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to delete the experience record,
	///     and returns a dictionary containing the updated list of experience records.
	///     If the operation is successful, the dictionary will contain a list of remaining experience records for the
	///     candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> DeleteExperience(int id, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateExperience> _experiences = new();
		if (id == 0)
		{
			return new()
				   {
					   {
						   "Experience", _experiences
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("DeleteCandidateExperience", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", id);
			_command.Int("candidateId", candidateID);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_experiences.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4), _reader.GetString(5),
										 _reader.GetString(6),
										 _reader.GetString(7)));
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
					   "Experience", _experiences
				   }
			   };
	}

	/// <summary>
	///     Deletes a candidate's note from the database.
	/// </summary>
	/// <param name="id">The ID of the note to be deleted.</param>
	/// <param name="candidateID">The ID of the candidate whose note is to be deleted.</param>
	/// <param name="user">The user who is performing the delete operation.</param>
	/// <returns>A dictionary containing the updated list of notes for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to delete the note,
	///     and returns a dictionary containing the updated list of notes.
	///     If the operation is successful, the dictionary will contain a list of remaining notes for the candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> DeleteNotes(int id, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateNotes> _notes = new();
		if (id == 0)
		{
			return new()
				   {
					   {
						   "Notes", _notes
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("DeleteCandidateNotes", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", id);
			_command.Int("candidateId", candidateID);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_notes.Add(new(_reader.GetInt32(0), _reader.GetDateTime(1), _reader.GetString(2), _reader.GetString(3)));
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
					   "Notes", _notes
				   }
			   };
	}

	/// <summary>
	///     Deletes a candidate's skill record from the database.
	/// </summary>
	/// <param name="id">The ID of the skill record to be deleted.</param>
	/// <param name="candidateID">The ID of the candidate whose skill record is to be deleted.</param>
	/// <param name="user">The user who is performing the delete operation.</param>
	/// <returns>A dictionary containing the updated list of skill records for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to delete the skill record,
	///     and returns a dictionary containing the updated list of skill records.
	///     If the operation is successful, the dictionary will contain a list of remaining skill records for the candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> DeleteSkill(int id, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateSkills> _skills = new();
		if (id == 0)
		{
			return new()
				   {
					   {
						   "Skills", _skills
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("DeleteCandidateSkill", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", id);
			_command.Int("candidateId", candidateID);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_skills.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetInt16(2), _reader.GetInt16(3), _reader.GetString(4)));
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
					   "Skills", _skills
				   }
			   };
	}

	/// <summary>
	///     Downloads a file associated with a specific document ID.
	/// </summary>
	/// <param name="documentID">The ID of the document to be downloaded.</param>
	/// <returns>A <see cref="DocumentDetails" /> object containing details of the downloaded document.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to fetch the document details,
	///     and returns a <see cref="DocumentDetails" /> object containing the details of the document.
	///     If the document does not exist, null is returned.
	/// </remarks>
	[HttpGet]
	public async Task<DocumentDetails> DownloadFile(int documentID)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("GetCandidateDocumentDetails", _connection);
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
	///     Downloads the resume of a candidate.
	/// </summary>
	/// <param name="candidateID">The ID of the candidate whose resume is to be downloaded.</param>
	/// <param name="resumeType">The type of the resume to be downloaded, Original or Formatted.</param>
	/// <returns>A <see cref="DocumentDetails" /> object containing the details of the downloaded resume.</returns>
	/// <remarks>
	///     This method connects to the database using a stored procedure to download the candidate's resume.
	///     The resume details are then encapsulated in a <see cref="DocumentDetails" /> object and returned.
	/// </remarks>
	[HttpGet]
	public async Task<DocumentDetails> DownloadResume(int candidateID, string resumeType)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await using SqlCommand _command = new("DownloadCandidateResume", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Int("CandidateID", candidateID);
		_command.Varchar("ResumeType", 20, resumeType);

		await _connection.OpenAsync();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

		DocumentDetails _documentDetails = null;
		while (_reader.Read())
		{
			_documentDetails = new(candidateID, $"{resumeType} Resume", _reader.NString(0), _reader.NString(1));
		}

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return _documentDetails;
	}

	/// <summary>
	///     Retrieves the detailed information of a candidate by their ID.
	/// </summary>
	/// <param name="candidateID">The ID of the candidate.</param>
	/// <param name="roleID">
	///     The role ID associated with the user making the request. This is used for access control and
	///     permissions management.
	/// </param>
	/// <returns>
	///     A dictionary containing the candidate's details, notes, skills, education, experience, activity, rating, MPC,
	///     RatingMPC, and documents.
	/// </returns>
	/// <remarks>
	///     This method performs a database operation using a stored procedure named "GetDetailCandidate".
	///     It reads multiple result sets from the database to populate various aspects of the candidate's information.
	/// </remarks>
	[HttpGet]
	public async Task<ActionResult<Dictionary<string, object>>> GetCandidateDetails(int candidateID, string roleID)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		CandidateDetails _candidate = null;
		string _candRating = "", _candMPC = "";

		await using SqlCommand _command = new("GetDetailCandidate", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Int("@CandidateID", candidateID);
		_command.Char("@RoleID", 2, roleID);

		await _connection.OpenAsync();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
		if (_reader.HasRows) //Candidate Details
		{
			_reader.Read();
			_candidate = new(_reader.NString(0), _reader.NString(1), _reader.NString(2), _reader.NString(3), _reader.NString(4), 
							 _reader.NString(5), _reader.GetInt32(6), _reader.NString(7), _reader.NString(8), _reader.NString(9), _reader.NString(10), 
							 _reader.NString(11), _reader.NInt16(12).ToString(), _reader.NString(13), _reader.NString(14), _reader.NString(15), 
							 _reader.NString(16), _reader.GetInt32(17), _reader.GetBoolean(18), _reader.GetBoolean(19), _reader.NString(20), 
							 _reader.NString(21), _reader.NString(22), _reader.NString(23), _reader.NString(24), _reader.NString(25), 
							 _reader.NString(26), _reader.GetByte(27), _reader.NString(28), _reader.GetBoolean(29), _reader.NString(30), 
							 _reader.GetInt32(31), _reader.GetDecimal(32), _reader.GetDecimal(33), _reader.GetDecimal(34), _reader.GetDecimal(35), 
							 _reader.NString(36), _reader.NString(37), _reader.GetBoolean(38), _reader.NString(39), _reader.GetBoolean(40), 
							 _reader.NString(41), _reader.NString(42), _reader.NString(43), _reader.NString(44), _reader.NString(45), candidateID, 
							 _reader.NString(46));
			_candRating = _reader.NString(28);
			_candMPC = _reader.NString(30);
		}

		_reader.NextResult(); //Notes
		List<CandidateNotes> _notes = new();
		while (_reader.Read())
		{
			_notes.Add(new(_reader.GetInt32(0), _reader.GetDateTime(1), _reader.GetString(2), _reader.GetString(3)));
		}

		_reader.NextResult(); //Skills
		List<CandidateSkills> _skills = new();
		while (_reader.Read())
		{
			_skills.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetInt16(2), _reader.GetInt16(3), _reader.GetString(4)));
		}

		_reader.NextResult(); //Education
		List<CandidateEducation> _education = new();
		while (_reader.Read())
		{
			_education.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4), _reader.GetString(5), 
							   _reader.GetString(6)));
		}

		_reader.NextResult(); //Experience
		List<CandidateExperience> _experience = new();
		while (_reader.Read())
		{
			_experience.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4), _reader.GetString(5), 
								_reader.GetString(6),
								_reader.GetString(7)));
		}

		_reader.NextResult(); //Activity
		List<CandidateActivity> _activity = new();
		while (_reader.Read())
		{
			_activity.Add(new(_reader.GetString(0), _reader.GetDateTime(1), _reader.GetString(2), _reader.GetInt32(3), _reader.GetInt32(4), 
							  _reader.GetString(5), _reader.GetString(6), _reader.GetInt32(7), _reader.GetBoolean(8), _reader.GetString(9), _reader.GetString(10), 
							  _reader.GetString(11), _reader.GetBoolean(12), _reader.GetString(13), _reader.GetInt32(14), _reader.GetString(15), 
							  _reader.GetInt32(16), _reader.GetString(17), _reader.GetBoolean(18), _reader.NDateTime(19), _reader.GetString(20),
							  _reader.NString(21), _reader.NString(22), _reader.GetBoolean(23)));
		}

		_reader.NextResult(); //Managers

		_reader.NextResult(); //Documents
		List<CandidateDocument> _documents = new();
		while (_reader.Read())
		{
			_documents.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), $"{_reader.NDateTime(4)} [{_reader.NString(5)}]", 
							   _reader.GetString(6), _reader.GetString(7), _reader.GetInt32(8)));
		}

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		//Candidate Rating
		List<CandidateRating> _rating = new();
		if (!_candRating.NullOrWhiteSpace())
		{
			string[] _ratingArray = _candRating.Split('?');
			_rating.AddRange(_ratingArray
							.Select(str => new
										   {
											   _str = str,
											   _innerArray = str.Split('^')
										   })
							.Where(t => t._innerArray.Length == 4)
							.Select(t => new CandidateRating(t._innerArray[0].Replace("  ", " ").ToDateTime("M/d/yy h:mm:ss tt"), t._innerArray[1], 
															 t._innerArray[2].ToByte(), t._innerArray[3])));

			_rating = _rating.OrderByDescending(x => x.Date).ToList();
		}

		//Candidate MPC
		List<CandidateMPC> _mpc = new();
		if (_candMPC.NullOrWhiteSpace())
		{
			return new Dictionary<string, object>
				   {
					   {
						   "Candidate", _candidate
					   },
					   {
						   "Notes", _notes
					   },
					   {
						   "Skills", _skills
					   },
					   {
						   "Education", _education
					   },
					   {
						   "Experience", _experience
					   },
					   {
						   "Activity", _activity
					   },
					   {
						   "Rating", _rating
					   },
					   {
						   "MPC", _mpc
					   },
					   {
						   "RatingMPC", null
					   },
					   {
						   "Document", _documents
					   }
				   };
		}

		string[] _mpcArray = _candMPC.Split('?');
		_mpc.AddRange(_mpcArray
					 .Select(str => new
									{
										_str = str,
										_innerArray = str.Split('^')
									})
					 .Where(t => t._innerArray.Length == 4)
					 .Select(t => new CandidateMPC(t._innerArray[0].Replace("  ", " ").ToDateTime("M/d/yy h:mm:ss tt"), t._innerArray[1], t._innerArray[2].ToBoolean(),
												   t._innerArray[3])));

		_mpc = _mpc.OrderByDescending(x => x.Date).ToList();

		int _ratingFirst = 0;
		bool _mpcFirst = false;
		string _ratingComments = "", _mpcComments = "";
		if (!_candRating.NullOrWhiteSpace())
		{
			CandidateRating _ratingFirstCandidate = _rating.FirstOrDefault();
			if (_ratingFirstCandidate != null)
			{
				_ratingFirst = _ratingFirstCandidate.Rating;
				_ratingComments = _ratingFirstCandidate.Comments;
			}
		}

		if (!_candMPC.NullOrWhiteSpace())
		{
			CandidateMPC _mpcFirstCandidate = _mpc.FirstOrDefault();
			if (_mpcFirstCandidate != null)
			{
				_mpcFirst = _mpcFirstCandidate.MPC;
				_mpcComments = _mpcFirstCandidate.Comments;
			}
		}

		CandidateRatingMPC _ratingMPC = new(candidateID, _ratingFirst, _ratingComments, _mpcFirst, _mpcComments);

		return new Dictionary<string, object>
			   {
				   {
					   "Candidate", _candidate
				   },
				   {
					   "Notes", _notes
				   },
				   {
					   "Skills", _skills
				   },
				   {
					   "Education", _education
				   },
				   {
					   "Experience", _experience
				   },
				   {
					   "Activity", _activity
				   },
				   {
					   "Rating", _rating
				   },
				   {
					   "MPC", _mpc
				   },
				   {
					   "RatingMPC", _ratingMPC
				   },
				   {
					   "Document", _documents
				   }
			   };
	}

	private static string GetCandidateLocation(CandidateDetails candidateDetails, string stateName)
	{
		string _location = "";

		if (!candidateDetails.City.NullOrWhiteSpace())
		{
			_location = candidateDetails.City;
		}

		if (!stateName.NullOrWhiteSpace())
		{
			_location += ", " + stateName;
		}
		else
		{
			_location = stateName;
		}

		if (!candidateDetails.ZipCode.NullOrWhiteSpace())
		{
			_location += ", " + candidateDetails.ZipCode;
		}
		else
		{
			_location = candidateDetails.ZipCode;
		}

		return _location;
	}

	/// <summary>
	///     Asynchronously retrieves a dictionary containing a list of candidates, the total count of candidates, and the
	///     current page number.
	///     This method uses a stored procedure to fetch the data from the database.
	/// </summary>
	/// <param name="searchModel">An instance of the CandidateSearch class containing the search parameters.</param>
	/// <param name="candidateID">An optional parameter specifying the ID of a candidate. Default is 0.</param>
	/// <param name="thenProceed">
	///     An optional parameter specifying whether to proceed if the candidate ID is greater than 0.
	///     Default is false.
	/// </param>
	/// <returns>
	///     A Task resulting in a dictionary containing a list of candidates, the total count of candidates, and the
	///     current page number.
	/// </returns>
	[HttpGet]
	public async Task<Dictionary<string, object>> GetGridCandidates(CandidateSearch searchModel, int candidateID = 0, bool thenProceed = false)
	{
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		List<Candidates> _candidates = new();
		await using SqlCommand _command = new("GetGridCandidates", _connection);
		_command.CommandType = CommandType.StoredProcedure;
		_command.Int("Count", searchModel.ItemCount);
		_command.Int("Page", searchModel.Page);
		_command.Int("SortRow", searchModel.SortField);
		_command.TinyInt("SortOrder", searchModel.SortDirection);
		_command.Varchar("Name", 255, searchModel.Name);
		_command.Bit("MyCandidates", !searchModel.AllCandidates);
		_command.Bit("IncludeAdmin", searchModel.IncludeAdmin);
		_command.Varchar("Keywords", 2000, searchModel.Keywords);
		_command.Varchar("Skill", 2000, searchModel.Skills);
		_command.Bit("SearchState", !searchModel.CityZip);
		_command.Varchar("City", 30, searchModel.CityName);
		_command.Varchar("State", 1000, searchModel.StateID);
		_command.Int("Proximity", searchModel.Proximity);
		_command.TinyInt("ProximityUnit", searchModel.ProximityUnit);
		_command.Varchar("Eligibility", 10, searchModel.Eligibility);
		_command.Varchar("Reloc", 10, searchModel.Relocate);
		_command.Varchar("JobOptions", 10, searchModel.JobOptions);
		//_command.Varchar("Communications",10, searchModel.Communication);
		_command.Varchar("Security", 10, searchModel.SecurityClearance);
		_command.Varchar("User", 10, searchModel.User);
		_command.Bit("ActiveRequisitionsOnly", searchModel.ActiveRequisitionsOnly);
		_command.Int("OptionalCandidateID", candidateID);
		_command.Bit("ThenProceed", thenProceed);

		await _connection.OpenAsync();
		await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

		int _count = 0, _page = 0;

		//await _reader.NextResultAsync();

		if (candidateID > 0 && !thenProceed)
		{
			try
			{
				await _reader.ReadAsync();
				_page = _reader.GetInt32(0);
				await _reader.CloseAsync();

				await _connection.CloseAsync();
			}
			catch (Exception)
			{
				//
			}

			return new()
				   {
					   {
						   "Page", _page
					   }
				   };
		}

		try
		{
			await _reader.ReadAsync();
			_count = _reader.GetInt32(0);
		}
		catch (Exception)
		{
			//
		}

		await _reader.NextResultAsync();

		while (await _reader.ReadAsync())
		{
			string _location = _reader.GetString(4);
			if (_location.StartsWith(","))
			{
				_location = _location[1..].Trim();
			}

			_candidates.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _location, _reader.GetString(5),
								_reader.GetString(6), _reader.GetBoolean(7), _reader.GetByte(8), _reader.GetBoolean(9), _reader.GetBoolean(10)));
		}

		await _reader.NextResultAsync();
		_page = searchModel.Page;
		while (await _reader.ReadAsync())
		{
			_page = _reader.GetInt32(0);
		}

		await _reader.CloseAsync();

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Candidates", _candidates
				   },
				   {
					   "Count", _count
				   },
				   {
					   "Page", _page
				   }
			   };
	}

	/// <summary>
	///     Converts a string date to a short year.
	/// </summary>
	/// <param name="date">The date string in "MM/dd/yyyy" format.</param>
	/// <returns>
	///     The year as a short. If the date string is null, empty, consists only of white-space characters,
	///     or does not match the "MM/dd/yyyy" format, it returns 0.
	/// </returns>
	private static short GetYear(string date)
	{
		if (date.Trim().NullOrWhiteSpace())
		{
			return 0;
		}

		return DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _formatDate) ? _formatDate.Year.ToInt16() : (short)0;
	}

	/// <summary>
	///     Asynchronously parses the resume from the provided file.
	/// </summary>
	/// <param name="file">The form file containing the resume to parse.</param>
	/// <param name="fileName">The name of the file (optional).</param>
	/// <param name="user">The user performing the operation (optional).</param>
	/// <param name="path">The path where the file is located (optional).</param>
	/// <param name="candidateID">The ID of the candidate the resume belongs to (optional).</param>
	/// <param name="pageCount">The number of pages to parse from the resume (default is 50).</param>
	/// <returns>A dictionary containing the parsed data from the resume.</returns>
	[HttpPost, SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
	public async Task<Dictionary<string, object>> ParseResume(IFormFile file, string fileName = "", string user = "", string path = "", int candidateID = 0, int pageCount = 50)
	{
		/*
		 // DAXTRA
		 string _name = Request.Form["filename"].ToString();
		 string _filename = _hostingEnvironment.ContentRootPath + $@"Upload\{_name}";

		 using MemoryStream _stream = new();
		 using FileStream _fs = System.IO.File.Open(_filename, FileMode.Create, FileAccess.Write);
		 try
		 {
			 Request.Form.Files[0].CopyTo(_fs);
			 Request.Form.Files[0].CopyTo(_stream);
			 _fs.Flush();
			 _fs.Close();
		 }
		 catch
		 {
			 _fs.Close();
		 }

		 RestClient _client = new("https://cvxdemo.daxtra.com/cvx/rest/api/v1/profile/full/json")
							  {
								  Timeout = -1
							  };
		 RestRequest request = new(Method.POST)
							   {
								   AlwaysMultipartFormData = true
							   };
		 request.AddParameter("account", "TitanTech");
		 request.AddParameter("file", Convert.ToBase64String(_stream.ToArray()));
		 IRestResponse response = _client.Execute(request);

		 using FileStream _fs1 = System.IO.File.Open(@"C:\Projects\ProfSvc_WebAPI\Upload\ParsedJSON3.txt", FileMode.Create, FileAccess.Write);
		 using StreamWriter _streamWriter = new(_fs1);
		 _streamWriter.WriteLine(response.Content);
		 _streamWriter.Flush();
		 _streamWriter.Close();
		 _fs1.Close();

		 return;
		*/

		/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------*/

		// SOVREN
		/*
		string _fileData = Request.Form["fileData"].ToString();
		string _name = _fileData.Split('^')[0];
		string _filename = _hostingEnvironment.ContentRootPath + $@"Upload\{_name}";

		await using MemoryStream _stream = new();
		await using FileStream _fs = System.IO.File.Open(_filename, FileMode.Create, FileAccess.Write);
		try
		{
			await Request.Form.Files[0].CopyToAsync(_fs);
			await Request.Form.Files[0].CopyToAsync(_stream);
			_fs.Flush();
			_fs.Close();
		}
		catch
		{
			_fs.Close();
		}

		SovrenClient _clientSovren = new("40423445", "L5VLzl7TeAukjSXJLIGNRVA1f8bVFYFbr5GWlUip", DataCenter.US);
		Document _doc = new(_stream.ToArray(), DateTime.Today);

		ParseRequest _parseRequest = new(_doc, new());

		ParseResumeResponse _parseResume = null;
		try
		{
			Task<ParseResumeResponse> _parseResumeTask = _clientSovren.ParseResume(_parseRequest);
			_parseResume = _parseResumeTask.Result;
			_parseResume.EasyAccess().SaveResumeJsonToFile(_filename + ".json", true, false);
			/////// *using FileStream _fs1 = System.IO.File.Open(_filename + ".json", FileMode.Create, FileAccess.Write);
			using StreamWriter _streamWriter = new(_fs1);
			_streamWriter.WriteLine(_sovrenJson);
			_streamWriter.Flush();
			_streamWriter.Close();
			_fs1.Close();* ////////
		}
		catch //(SovrenException)
		{
			//
		}

		//return;

		if (_parseResume != null)
		{
			////// *using FileStream _fs1 = System.IO.File.Open(_filename + ".json", FileMode.OpenOrCreate, FileAccess.Read);
			using StreamReader _streamReader = new(_fs1);
			string _jsonFile = _streamReader.ReadToEnd();
			_streamReader.Close();
			_fs.Close();

			JObject _parsedData = JObject.Parse(_jsonFile);
			JToken _personToken = _parsedData.SelectToken("Resume.StructuredResume.PersonName");* ///////
			ParseResumeResponseExtensions _parseData = _parseResume.EasyAccess();
			string _firstName = "", _lastName = "", _middleName = "", _emailAddress = "", _phoneMain = "", _altPhone = "", _address1 = "", _address2 = "", _city = "", _state = "", _zipCode = "";
			string _keywords = "", _experienceSummary = "", _backgroundNotes = "", _textResume = "", _objective = "", _user = _fileData.Split('^')[3];
			if (_parseData.GetCandidateName() != null)
			{
				PersonName _candidateName = _parseData.GetCandidateName();
				_firstName = _candidateName.GivenName ?? "";
				_middleName = _candidateName.MiddleName ?? "";
				_lastName = _candidateName.FamilyName ?? "";
			}

			bool _background = _parseData.HasSecurityClearance();

			//_parseResume
			///// *JToken _contactToken = _parsedData.SelectToken("Resume.StructuredResume.ContactMethod");
			Regex _pattern = new("[^0-9]");* //////

			if (_parseData.GetContactInfo() != null)
			{
				ContactInformation _contactInfo = _parseData.GetContactInfo();
				if (_contactInfo.EmailAddresses?.Count > 0)
				{
					_emailAddress = _contactInfo.EmailAddresses[0];
				}

				if (_contactInfo.Telephones?.Count > 0)
				{
					_phoneMain = $"{_contactInfo.Telephones[0].AreaCityCode}{_contactInfo.Telephones[0].SubscriberNumber}".Replace("-", "").Replace(" ", "");
				}

				if (_contactInfo.Telephones?.Count > 1)
				{
					_altPhone = $"{_contactInfo.Telephones[1].AreaCityCode}{_contactInfo.Telephones[1].SubscriberNumber}".Replace("-", "").Replace(" ", "");
				}

				if (_contactInfo.Location != null)
				{
					Location _location = _contactInfo.Location;
					if (_location.StreetAddressLines?.Count > 0)
					{
						_address1 = _location.StreetAddressLines[0];
					}

					if (_location.StreetAddressLines?.Count > 1)
					{
						_address2 = _location.StreetAddressLines[0];
					}

					_city = _location.Municipality ?? "";
					if (_location.Regions.Count > 0)
					{
						_state = _location.Regions[0];
					}

					_zipCode = _location.PostalCode ?? "";
				}
			}

			_experienceSummary = _parseResume.Value.ResumeData.ProfessionalSummary;

			DataTable _tableEducation = new();
			_tableEducation.Columns.Add("Degree", typeof(string));
			_tableEducation.Columns.Add("College", typeof(string));
			_tableEducation.Columns.Add("State", typeof(string));
			_tableEducation.Columns.Add("Country", typeof(string));
			_tableEducation.Columns.Add("Year", typeof(string));

			//_parseResume.
			List<EducationDetails> _educationDetails = _parseResume.Value.ResumeData?.Education?.EducationDetails;
			if (_educationDetails != null)
			{
				foreach (EducationDetails _education in _educationDetails)
				{
					DataRow _dr = _tableEducation.NewRow();
					if (_education == null)
					{
						return;
					}

					_dr["Degree"] = _education.Degree?.Name?.Normalized ?? string.Empty;
					_dr["College"] = _education.SchoolName?.Normalized ?? string.Empty;
					if (_education.Location?.Regions?.Count > 0)
					{
						_dr["State"] = _education.Location.Regions[0];
					}

					_dr["Country"] = _education.Location?.CountryCode ?? string.Empty;
					_dr["Year"] = _education.LastEducationDate?.Date.Year.ToString() ?? string.Empty;
					_tableEducation.Rows.Add(_dr);
				}
			}

			DataTable _tableEmployer = new();
			_tableEmployer.Columns.Add("Employer", typeof(string));
			_tableEmployer.Columns.Add("Start", typeof(string));
			_tableEmployer.Columns.Add("End", typeof(string));
			_tableEmployer.Columns.Add("Location", typeof(string));
			_tableEmployer.Columns.Add("Title", typeof(string));
			_tableEmployer.Columns.Add("Description", typeof(string));

			_parseResume.Value.ResumeData?.EmploymentHistory?.Positions?.ForEach(position =>
																				 {
																					 DataRow _dr = _tableEmployer.NewRow();
																					 if (position != null)
																					 {
																						 _dr["Employer"] = position.Employer?.Name?.Normalized ?? string.Empty;

																						 _dr["Start"] = position.StartDate?.Date.CultureDate() ?? string.Empty;
																						 _dr["End"] = position.EndDate?.Date.CultureDate() ?? string.Empty;

																						 string _location = "";
																						 if (position.Employer?.Location != null)
																						 {
																							 Location _positionLocation = position.Employer?.Location;
																							 if (_positionLocation != null)
																							 {
																								 _location += ", " + _positionLocation.Municipality;
																								 if (_positionLocation.Regions.Any())
																								 {
																									 _location += ", " + _positionLocation.Regions.FirstOrDefault();
																								 }

																								 _location += ", " + _positionLocation.CountryCode;
																							 }

																							 if (_location != "")
																							 {
																								 _location = _location[2..];
																							 }
																						 }

																						 _dr["Location"] = _location;
																						 _dr["Title"] = position.JobTitle?.Normalized ?? string.Empty;
																						 _dr["Description"] = position.Description;
																					 }

																					 _tableEmployer.Rows.Add(_dr);
																				 });

			DataTable _tableSkills = new();
			_tableSkills.Columns.Add("Skill", typeof(string));
			_tableSkills.Columns.Add("LastUsed", typeof(int));
			_tableSkills.Columns.Add("Month", typeof(int));

			List<ResumeTaxonomyRoot> _skillsData = _parseResume.Value.ResumeData?.SkillsData;

			if (_skillsData is {Count: > 0})
			{
				try
				{
					foreach (ResumeTaxonomyRoot _skillResume in _skillsData)
					{
						List<ResumeTaxonomy> _taxonomies = _skillResume?.Taxonomies;
						if (_taxonomies is not {Count: > 0})
						{
							return;
						}

						foreach (ResumeTaxonomy _taxonomy in _taxonomies)
						{
							if (_taxonomy == null)
							{
								continue;
							}

							_keywords += ", " + _taxonomy.Name;
							List<ResumeSubTaxonomy> _subTaxonomies = _taxonomy.SubTaxonomies;
							if (_subTaxonomies is not {Count: > 0})
							{
								return;
							}

							foreach (ResumeSubTaxonomy _subTaxonomy in _subTaxonomies)
							{
								List<ResumeSkill> _skills = _subTaxonomy?.Skills;
								if (_skills is not {Count: > 0})
								{
									return;
								}

								foreach (ResumeSkill _skillDetails in _skills)
								{
									DataRow _dr = _tableSkills.NewRow();
									_dr["Skill"] = _skillDetails.Name ?? "";
									_dr["LastUsed"] = _skillDetails.LastUsed?.Value.Year.ToString() ?? "";
									_dr["Month"] = _skillDetails.MonthsExperience?.Value.ToString() ?? "0";
									_tableSkills.Rows.Add(_dr);
								}
							}
						}
					}
				}
				catch
				{
					await Task.Delay(1);
				}
			}

			if (!_keywords.NullOrWhiteSpace())
			{
				_keywords = _keywords[2..(_keywords.Length > 502 ? 502 : _keywords.Length)].Trim();
			}

			if (_parseResume.Value.ResumeData?.SecurityCredentials?.Any() ?? true)
			{
				_parseResume.Value.ResumeData.SecurityCredentials?.ForEach(security => { _backgroundNotes += ", " + security.Name; });
			}

			if (!_backgroundNotes.NullOrWhiteSpace())
			{
				_backgroundNotes = _backgroundNotes[2..].Trim();
			}

			_objective = _parseResume.Value.ResumeData?.Objective ?? "";
			_textResume = _parseResume.Value.ResumeData?.ResumeMetadata.PlainText ?? "";

			await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
			await _connection.OpenAsync();
			//int _returnCode = 0;
			try
			{
				await using SqlCommand _command = new("SaveParsedCandidate", _connection)
												  {
													  CommandType = CommandType.StoredProcedure
												  };
				_command.Int("@ID", DBNull.Value, true);
				_command.Varchar("@FirstName", 50, _firstName);
				_command.Varchar("@MiddleName", 50, _middleName);
				_command.Varchar("@LastName", 50, _lastName);
				_command.Varchar("@Address", 255, _address1);
				_command.Varchar("@Address2", 255, _address2);
				_command.Varchar("@City", 50, _city);
				_command.Varchar("@State", 50, _state);
				_command.Varchar("@Zip", 20, _zipCode);
				_command.Varchar("@Email", 255, _emailAddress);
				_command.Bit("@Background", _background);
				_command.Varchar("@SecurityNotes", -1, _backgroundNotes);
				_command.Varchar("@Phone1", 15, _phoneMain);
				_command.Varchar("@Phone2", 15, _altPhone);
				_command.Varchar("@ExperienceSummary", -1, _experienceSummary);
				_command.Int("@Experience", _parseResume.Value.ResumeData.EmploymentHistory?.ExperienceSummary?.MonthsOfWorkExperience ?? 0);
				_command.Varchar("@Summary", -1, _parseResume.Value.ResumeData.EmploymentHistory?.ExperienceSummary?.Description ?? "");
				_command.Varchar("@Objective", -1, _objective);
				_command.Varchar("@Keywords", 500, _keywords);
				_command.Varchar("@JobOptions", 5, "F");
				_command.Varchar("@TaxTerm", 10, "E");
				_command.Varchar("@TextResume", -1, _textResume);
				_command.Varchar("@OriginalResume", 255, _name);
				_command.Varchar("@User", 10, _user);
				_command.Parameters.AddWithValue("@Education", _tableEducation);
				_command.Parameters.AddWithValue("@Employer", _tableEmployer);
				_command.Parameters.AddWithValue("@Skills", _tableSkills);

				await _command.ExecuteNonQueryAsync();

				///// *_command.Varchar("@Title", 50, candidateDetails.Title);
				_command.Int("@Eligibility", candidateDetails.EligibilityID);
				_command.Decimal("@HourlyRate", 6, 2, candidateDetails.HourlyRate);
				_command.Decimal("@HourlyRateHigh", 6, 2, candidateDetails.HourlyRateHigh);
				_command.Decimal("@SalaryLow", 9, 2, candidateDetails.SalaryLow);
				_command.Decimal("@SalaryHigh", 9, 2, candidateDetails.SalaryHigh);
				_command.Varchar("@JobOptions", 50, candidateDetails.JobOptions);
				_command.Char("@Communication", 1, candidateDetails.Communication);
				_command.Varchar("@TextResume", -1, candidateDetails.TextResume);
				_command.Varchar("@OriginalResume", 255, candidateDetails.OriginalResume);
				_command.Varchar("@FormattedResume", 255, candidateDetails.FormattedResume);
				_command.Varchar("@Keywords", 500, candidateDetails.Keywords);
				_command.Varchar("@Status", 3, "AVL");
				_command.UniqueIdentifier("@OriginalFileID", DBNull.Value);
				_command.UniqueIdentifier("@FormattedFileID", DBNull.Value);
				_command.Varchar("@Phone3", 15, candidateDetails.Phone3);
				_command.SmallInt("@Phone3Ext", candidateDetails.PhoneExt.ToInt16());
				_command.VarcharD("@OriginalFileType", 10);
				_command.VarcharD("@OriginalContentType", 255);
				_command.VarcharD("@FormattedFileType", 10);
				_command.VarcharD("@FormattedContentType", 255);
				_command.Varchar("@LinkedIn", 255, candidateDetails.LinkedIn);
				_command.Varchar("@Facebook", 255, candidateDetails.Facebook);
				_command.Varchar("@Twitter", 255, candidateDetails.Twitter);
				_command.Varchar("@Google", 255, candidateDetails.GooglePlus);
				_command.Bit("@Refer", candidateDetails.Refer);
				_command.Varchar("@ReferAccountMgr", 10, candidateDetails.ReferAccountManager);
				_command.Varchar("@TaxTerm", 10, candidateDetails.TaxTerm);
				_command.Bit("@Background", candidateDetails.Background);
				_command.Varchar("@Summary", -1, candidateDetails.Summary);
				_command.Varchar("@Objective", -1, "");
				_command.Bit("@EEO", candidateDetails.Eeo);
				_command.Varchar("@EEOFile", 255, candidateDetails.EeoFile);
				_command.VarcharD("@EEOFileType", 10);
				_command.VarcharD("@EEOContentType", 255);
				_command.Varchar("@RelocNotes", 200, candidateDetails.RelocationNotes);
				_command.Varchar("@SecurityClearanceNotes", 200, candidateDetails.SecurityNotes);* //////
				////// *_command.Varchar("@User", 10, "ADMIN");

				await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

				_reader.Read();
				if (_reader.HasRows)
				{
					_returnCode = _reader.GetInt32(0);
				}

				await _reader.CloseAsync();* //////
			}
			catch //(Exception exception)
			{
				await Task.Delay(1);
				// ignored
			}

			await _connection.CloseAsync();

		}
		*/
		/*-----------------------------------------------------------------------------------------------------------------------------------------------------------------*/

		// RChilli

		RChilliParseResume _rChilli = new()
									  {
										  ServiceUrl = "https://rest.rchilli.com/RChilliParser/Rchilli/parseResumeBinary"
									  };

		Directory.CreateDirectory(Path.Combine(path, "Uploads", "Candidate", "0"));
		string _destinationFileName = Path.Combine(path, "Uploads", "Candidate", "0", $"Original_{fileName.Replace(" ", "")}");
		await using FileStream _fs = System.IO.File.Create(_destinationFileName);
		try
		{
			await file.CopyToAsync(_fs);
			await _fs.FlushAsync();
			_fs.Close();
		}
		catch
		{
			_fs.Close();
		}

		RChilliMapFields _mapFields = await _rChilli.ParseResume(_destinationFileName, "SO7WG13O", "8.0.0", "Info");
		string _json = _rChilli.OutputJson;
		string _jsonFileName = Path.Combine(path, "Uploads", "Candidate", "0", $"Original_{fileName.Replace(" ", "")}.json");
		try
		{
			await System.IO.File.WriteAllTextAsync(_jsonFileName, _json);
		}
		catch
		{
			//
		}

		return await SaveParsedData(_mapFields, _jsonFileName, fileName, user, candidateID, path, true, pageCount);

		/*ResumeParserData _objRChilli = _mapFields.ResumeParserData;
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		//int _returnCode = 0;
		List<ExistingCandidate> _existingCandidates = new();
		int _returnCode = 0;
		try
		{
			await using SqlCommand _command = new("CheckCandidateExists", _connection)
											  {
												  CommandType = CommandType.StoredProcedure
											  };
			_command.Varchar("@FirstName", 50, _objRChilli.Name.FirstName);
			_command.Varchar("@LastName", 50, _objRChilli.Name.LastName);
			_command.Varchar("@Email", 50, _objRChilli.Email.Count > 0 ? _objRChilli.Email[0].EmailAddress : "");
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (await _reader.ReadAsync())
				{
					_existingCandidates.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3).FormatPhoneNumber()));
				}

				await _reader.CloseAsync();
				await _connection.CloseAsync();

				return new()
					   {
						   {
							   "Json", jsonFileName
						   },
						   {
							   "Candidates", _existingCandidates
						   },
						   {
							   "FileName", fileName
						   },
						   {
							   "Email", _objRChilli.Email.Count > 0 ? _objRChilli.Email[0].EmailAddress : ""
						   },
						   {
							   "Phone", _objRChilli.PhoneNumber.Count > 0 ? _objRChilli.PhoneNumber[0].OriginalNumber.StripPhoneNumber().FormatPhoneNumber() : ""
						   }
					   };
			}

			await _reader.CloseAsync();
			string _firstName = "", _lastName = "", _middleName = "", _emailAddress = "", _phoneMain = "", _altPhone = "", _address1 = "", _city = "", _state = "", _zipCode = "";
			string _linkedIn = "", _internalFileName = Guid.NewGuid().ToString("N");

			if (_objRChilli.Name != null)
			{
				Name _name = _objRChilli.Name;
				_firstName = _name.FirstName;
				_middleName = _name.MiddleName;
				_lastName = _name.LastName;
			}

			if (_objRChilli.Email is {Count: > 0})
			{
				_emailAddress = _objRChilli.Email[0].EmailAddress;
			}

			if (_objRChilli.Address is {Count: > 0})
			{
				Address _address = _objRChilli.Address[0];
				_address1 = _address.Street;
				_city = _address.City;
				_state = _address.State;
				_zipCode = _address.ZipCode;
			}

			if (_objRChilli.PhoneNumber is {Count: > 0})
			{
				List<PhoneNumber> _phone = _objRChilli.PhoneNumber;
				_phoneMain = _phone[0].OriginalNumber.StripPhoneNumber();
				if (_phone.Count > 1)
				{
					_altPhone = _phone[1].OriginalNumber.StripPhoneNumber();
				}
			}

			DataTable _tableEducation = new();
			_tableEducation.Columns.Add("Degree", typeof(string));
			_tableEducation.Columns.Add("College", typeof(string));
			_tableEducation.Columns.Add("State", typeof(string));
			_tableEducation.Columns.Add("Country", typeof(string));
			_tableEducation.Columns.Add("Year", typeof(string));

			if (_objRChilli.SegregatedQualification is {Count: > 0})
			{
				_objRChilli.SegregatedQualification.ForEach(qualification =>
															{
																DataRow _dr = _tableEducation.NewRow();
																_dr["Degree"] = qualification.Degree.DegreeName;
																_dr["College"] = qualification.Institution.Name;
																_dr["State"] = qualification.Institution.Location.State;
																_dr["Country"] = qualification.Institution.Location.Country;
																_dr["Year"] = qualification.FormattedDegreePeriod;
																_tableEducation.Rows.Add(_dr);
															});
			}

			DataTable _tableEmployer = new();
			_tableEmployer.Columns.Add("Employer", typeof(string));
			_tableEmployer.Columns.Add("Start", typeof(string));
			_tableEmployer.Columns.Add("End", typeof(string));
			_tableEmployer.Columns.Add("Location", typeof(string));
			_tableEmployer.Columns.Add("Title", typeof(string));
			_tableEmployer.Columns.Add("Description", typeof(string));

			if (_objRChilli.SegregatedExperience is {Count: > 0})
			{
				_objRChilli.SegregatedExperience.ForEach(experience =>
														 {
															 DataRow _dr = _tableEmployer.NewRow();
															 _dr["Employer"] = experience.Employer.EmployerName;
															 _dr["Start"] = experience.StartDate;
															 _dr["End"] = experience.IsCurrentEmployer == "true" ? "" : experience.EndDate;
															 _dr["Location"] = experience.Location.City + ", " + experience.Location.State + ", " + experience.Location.Country;
															 _dr["Title"] = experience.JobProfile.Title;
															 _dr["Description"] = experience.JobDescription;
															 _tableEmployer.Rows.Add(_dr);
														 });
			}

			DataTable _tableSkills = new();
			_tableSkills.Columns.Add("Skill", typeof(string));
			_tableSkills.Columns.Add("LastUsed", typeof(short));
			_tableSkills.Columns.Add("Month", typeof(short));

			if (_objRChilli.SegregatedSkill is {Count: > 0})
			{
				_objRChilli.SegregatedSkill.ForEach(skill =>
													{
														if (skill.Type != "OperationSkill")
														{
															return;
														}

														DataRow _dr = _tableSkills.NewRow();
														_dr["Skill"] = skill.Skill;
														_dr["LastUsed"] = GetYear(skill.LastUsed);
														_dr["Month"] = skill.ExperienceInMonths;
														_tableSkills.Rows.Add(_dr);
													});
			}

			if (_objRChilli.WebSite is {Count: > 0})
			{
				foreach (WebSite _webSite in _objRChilli.WebSite.Where(webSite => webSite.Type.ToLowerInvariant() == "linkedin"))
				{
					_linkedIn = _webSite.Url;
				}
			}

			string _keywords = _objRChilli.SkillKeywords[..500];
			string _experienceSummary = $"{_objRChilli.ExecutiveSummary}{Environment.NewLine}{_objRChilli.ManagementSummary}";
			string _objective = _objRChilli.Summary;
			string _textResume = _objRChilli.DetailResume;
			try
			{
				await using SqlCommand _command1 = new("SaveParsedCandidate", _connection)
												   {
													   CommandType = CommandType.StoredProcedure
												   };
				_command1.Int("@ID", DBNull.Value, true);
				_command1.Varchar("@FirstName", 50, _firstName);
				_command1.Varchar("@MiddleName", 50, _middleName);
				_command1.Varchar("@LastName", 50, _lastName);
				_command1.Varchar("@Address", 255, _address1);
				_command1.Varchar("@Address2", 255, "");
				_command1.Varchar("@City", 50, _city);
				_command1.Varchar("@State", 50, _state);
				_command1.Varchar("@Zip", 20, _zipCode);
				_command1.Varchar("@Email", 255, _emailAddress);
				_command1.Bit("@Background", false);
				_command1.Varchar("@SecurityNotes", -1, "");
				_command1.Varchar("@Phone1", 15, _phoneMain);
				_command1.Varchar("@Phone2", 15, _altPhone);
				_command1.Varchar("@ExperienceSummary", -1, _experienceSummary);
				_command1.Int("@Experience", _objRChilli.WorkedPeriod.TotalExperienceInMonths);
				_command1.Varchar("@Summary", -1, _objective);
				_command1.Varchar("@Objective", -1, _objRChilli.Objectives);
				_command1.Varchar("@Keywords", 500, _keywords);
				_command1.Varchar("@JobOptions", 5, "F");
				_command1.Varchar("@TaxTerm", 10, "E");
				_command1.Varchar("@TextResume", -1, _textResume);
				_command1.Varchar("@OriginalResume", 255, $"Original_{fileName.Replace(" ", "")}");
				_command1.Varchar("@User", 10, user);
				_command1.Varchar("@InternalFileName", 255, _internalFileName);
				_command1.Varchar("@LinkedIn", 10, _linkedIn);
				_command1.Varchar("@JsonFileName", 255, jsonFileName);
				_command1.Parameters.AddWithValue("@Education", _tableEducation).SqlDbType = SqlDbType.Structured;
				_command1.Parameters.AddWithValue("@Employer", _tableEmployer).SqlDbType = SqlDbType.Structured;
				_command1.Parameters.AddWithValue("@Skills", _tableSkills).SqlDbType = SqlDbType.Structured;

				await using SqlDataReader _reader1 = await _command1.ExecuteReaderAsync();

				_reader1.Read();
				if (_reader1.HasRows)
				{
					_returnCode = _reader1.GetInt32(0);
				}

				await _reader.CloseAsync();
			}
			catch //(Exception exception)
			{
				await Task.Yield();
				// ignored
			}

			await _connection.CloseAsync();
		}
		catch
		{
			//
		}

		return new()
			   {
				   {
					   "CandidateID", _returnCode
				   }
			   };*/
	}

	/// <summary>
	///     Asynchronously saves the details of a candidate to the database.
	/// </summary>
	/// <param name="candidateDetails">The details of the candidate to be saved.</param>
	/// <param name="jsonPath">The path to the JSON file containing the email template.</param>
	/// <param name="userName">The username of the user performing the operation. Default is an empty string.</param>
	/// <param name="emailAddress">
	///     The email address to which the operation result should be sent. Default is
	///     "maniv@titan-techs.com".
	/// </param>
	/// <returns>
	///     Returns an integer indicating the result of the operation. A return value of -1 indicates that the candidate
	///     details were null.
	/// </returns>
	/// <remarks>
	///     This method performs the following operations:
	///     - Opens a connection to the database.
	///     - Creates a new SQL command with the stored procedure "SaveCandidate".
	///     - Adds the details of the candidate as parameters to the SQL command.
	///     - Executes the SQL command and reads the result.
	///     - If there are any email templates, it sends an email with the operation result.
	///     - Closes the connection to the database.
	///     - Returns the result of the operation.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, int>> SaveCandidate(CandidateDetails candidateDetails, string jsonPath, string userName = "", string emailAddress = "maniv@titan-techs.com")
	{
		if (candidateDetails == null)
		{
			return new()
				   {
					   {
						   "returnCode", -1
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		int _returnCode = 0;
		try
		{
			await using SqlCommand _command = new("SaveCandidate", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("@ID", candidateDetails.CandidateID, true);
			_command.Varchar("@FirstName", 50, candidateDetails.FirstName);
			_command.Varchar("@MiddleName", 50, candidateDetails.MiddleName);
			_command.Varchar("@LastName", 50, candidateDetails.LastName);
			_command.Varchar("@Title", 50, candidateDetails.Title);
			_command.Int("@Eligibility", candidateDetails.EligibilityID);
			_command.Decimal("@HourlyRate", 6, 2, candidateDetails.HourlyRate);
			_command.Decimal("@HourlyRateHigh", 6, 2, candidateDetails.HourlyRateHigh);
			_command.Decimal("@SalaryLow", 9, 2, candidateDetails.SalaryLow);
			_command.Decimal("@SalaryHigh", 9, 2, candidateDetails.SalaryHigh);
			_command.Int("@Experience", candidateDetails.ExperienceID);
			_command.Bit("@Relocate", candidateDetails.Relocate);
			_command.Varchar("@JobOptions", 50, candidateDetails.JobOptions);
			_command.Char("@Communication", 1, candidateDetails.Communication);
			_command.Varchar("@Keywords", 500, candidateDetails.Keywords);
			_command.Varchar("@Status", 3, "AVL");
			_command.Varchar("@TextResume", -1, candidateDetails.TextResume);
			_command.Varchar("@OriginalResume", 255, candidateDetails.OriginalResume);
			_command.Varchar("@FormattedResume", 255, candidateDetails.FormattedResume);
			_command.UniqueIdentifier("@OriginalFileID", DBNull.Value);
			_command.UniqueIdentifier("@FormattedFileID", DBNull.Value);
			_command.Varchar("@Address1", 255, candidateDetails.Address1);
			_command.Varchar("@Address2", 255, candidateDetails.Address2);
			_command.Varchar("@City", 50, candidateDetails.City);
			_command.Int("@StateID", candidateDetails.StateID);
			_command.Varchar("@ZipCode", 20, candidateDetails.ZipCode);
			_command.Varchar("@Email", 255, candidateDetails.Email);
			_command.Varchar("@Phone1", 15, candidateDetails.Phone1);
			_command.Varchar("@Phone2", 15, candidateDetails.Phone2);
			_command.Varchar("@Phone3", 15, candidateDetails.Phone3);
			_command.SmallInt("@Phone3Ext", candidateDetails.PhoneExt.ToInt16());
			_command.Varchar("@LinkedIn", 255, candidateDetails.LinkedIn);
			_command.Varchar("@Facebook", 255, candidateDetails.Facebook);
			_command.Varchar("@Twitter", 255, candidateDetails.Twitter);
			_command.Varchar("@Google", 255, candidateDetails.GooglePlus);
			_command.Bit("@Refer", candidateDetails.Refer);
			_command.Varchar("@ReferAccountMgr", 10, candidateDetails.ReferAccountManager);
			_command.Varchar("@TaxTerm", 10, candidateDetails.TaxTerm);
			_command.Bit("@Background", candidateDetails.Background);
			_command.Varchar("@Summary", -1, candidateDetails.Summary);
			_command.Varchar("@Objective", -1, "");
			_command.Bit("@EEO", candidateDetails.EEO);
			_command.Varchar("@RelocNotes", 200, candidateDetails.RelocationNotes);
			_command.Varchar("@SecurityClearanceNotes", 200, candidateDetails.SecurityNotes);
			_command.Varchar("@User", 10, userName);

			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

			List<EmailTemplates> _templates = new();
			Dictionary<string, string> _emailAddresses = new();
			Dictionary<string, string> _emailCC = new();

			while (await _reader.ReadAsync())
			{
				_templates.Add(new(_reader.NString(0), _reader.NString(1), _reader.NString(2)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync())
			{
				_emailAddresses.Add(_reader.NString(0), _reader.NString(1));
			}

			string _stateName = "";
			await _reader.NextResultAsync();
			while (await _reader.ReadAsync())
			{
				_stateName = _reader.GetString(0);
			}

			await _reader.CloseAsync();

			if (_templates.Count > 0)
			{
				EmailTemplates _templateSingle = _templates[0];
				if (!_templateSingle.CC.NullOrWhiteSpace())
				{
					string[] _ccArray = _templateSingle.CC.Split(",");
					foreach (string _cc in _ccArray)
					{
						_emailCC.Add(_cc, _cc);
					}
				}

				_templateSingle.Subject = _templateSingle.Subject.Replace("$TODAY$", DateTime.Today.CultureDate())
														 .Replace("$FULL_NAME$", $"{candidateDetails.FirstName} {candidateDetails.LastName}")
														 .Replace("$FIRST_NAME$", candidateDetails.FirstName)
														 .Replace("$LAST_NAME$", candidateDetails.LastName)
														 .Replace("$CAND_LOCATION$", GetCandidateLocation(candidateDetails, _stateName))
														 .Replace("$CAND_PHONE_PRIMARY$", candidateDetails.Phone1.StripPhoneNumber().FormatPhoneNumber())
														 .Replace("$CAND_SUMMARY$", candidateDetails.Summary)
														 .Replace("$LOGGED_USER$", userName);
				_templateSingle.Template = _templateSingle.Template.Replace("$TODAY$", DateTime.Today.CultureDate())
														  .Replace("$FULL_NAME$", $"{candidateDetails.FirstName} {candidateDetails.LastName}")
														  .Replace("$FIRST_NAME$", candidateDetails.FirstName)
														  .Replace("$LAST_NAME$", candidateDetails.LastName)
														  .Replace("$CAND_LOCATION$", GetCandidateLocation(candidateDetails, _stateName))
														  .Replace("$CAND_PHONE_PRIMARY$", candidateDetails.Phone1.StripPhoneNumber().FormatPhoneNumber())
														  .Replace("$CAND_SUMMARY$", candidateDetails.Summary)
														  .Replace("$LOGGED_USER$", userName);

				GMailSend.SendEmail(jsonPath, emailAddress, _emailCC, _emailAddresses, _templateSingle.Subject, _templateSingle.Template, null);
			}
		}
		catch
		{
			// ignored
		}

		await _connection.CloseAsync();

		return new() {{"returnCode", _returnCode}};
	}

	/// <summary>
	///     Saves a candidate's activity to the database.
	/// </summary>
	/// <param name="activity">The activity of the candidate to be saved.</param>
	/// <param name="candidateID">The ID of the candidate.</param>
	/// <param name="user">The user who is performing the save operation.</param>
	/// <param name="roleID">The role ID of the user, default is "RS".</param>
	/// <param name="isCandidateScreen">
	///     A flag indicating whether the operation is performed from the candidate screen, default
	///     is true.
	/// </param>
	/// <param name="emailAddress">The email address to which notifications will be sent, default is "maniv@titan-techs.com".</param>
	/// <param name="uploadPath">The path where files will be uploaded, default is an empty string.</param>
	/// <param name="jsonPath">The path where JSON files will be stored, default is an empty string.</param>
	/// <returns>A dictionary containing the status of the operation and any relevant data.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to save the activity,
	///     and returns a dictionary containing the result of the operation.
	///     If the operation is successful, the dictionary will contain a list of activities for the candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveCandidateActivity(CandidateActivity activity, int candidateID, string user, string roleID = "RS",
																		bool isCandidateScreen = true, string emailAddress = "maniv@titan-techs.com", string uploadPath = "", string jsonPath = "")
	{
		await Task.Delay(1);
		List<CandidateActivity> _activities = new();
		if (activity == null)
		{
			return new()
				   {
					   {
						   "Activity", _activities
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("SaveCandidateActivity", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("SubmissionId", activity.ID);
			_command.Int("CandidateID", candidateID);
			_command.Int("RequisitionID", activity.RequisitionID);
			_command.Varchar("Notes", 1000, activity.Notes);
			_command.Char("Status", 3, activity.NewStatusCode.NullOrWhiteSpace() ? activity.StatusCode : activity.NewStatusCode);
			_command.Varchar("User", 10, user);
			_command.Bit("ShowCalendar", activity.ShowCalendar);
			_command.Date("DateTime", activity.DateTimeInterview == DateTime.MinValue ? DBNull.Value : activity.DateTimeInterview);
			_command.Char("Type", 1, activity.TypeOfInterview);
			_command.Varchar("PhoneNumber", 20, activity.PhoneNumber);
			_command.Varchar("InterviewDetails", 2000, activity.InterviewDetails);
			_command.Bit("UpdateSchedule", false);
			_command.Bit("CandScreen", isCandidateScreen);
			_command.Char("RoleID", 2, roleID);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			while (await _reader.ReadAsync())
			{
				_activities.Add(new(_reader.GetString(0), _reader.GetDateTime(1), _reader.GetString(2), _reader.GetInt32(3), _reader.GetInt32(4),
									_reader.GetString(5), _reader.GetString(6), _reader.GetInt32(7), _reader.GetBoolean(8), _reader.GetString(9), _reader.GetString(10),
									_reader.GetString(11), _reader.GetBoolean(12), _reader.GetString(13), _reader.GetInt32(14), _reader.GetString(15),
									_reader.GetInt32(16), _reader.GetString(17), _reader.GetBoolean(18), _reader.NDateTime(19), _reader.GetString(20),
									_reader.NString(21), _reader.NString(22), _reader.GetBoolean(23)));
			}

			await _reader.NextResultAsync();
			string _firstName = "", _lastName = "", _reqCode = "", _reqTitle = "", _company = ""; //, _original = "", _originalInternal = "", _formatted = "", _formattedInternal = "";
			//bool _firstTime = false;
			await _reader.ReadAsync();
			_firstName = _reader.NString(0);
			_lastName = _reader.NString(1);
			_reqCode = _reader.NString(2);
			_reqTitle = _reader.NString(3);
			//_original = _reader.NString(4);
			//_originalInternal = _reader.NString(5);
			//_formatted = _reader.NString(6);
			//_formattedInternal = _reader.NString(7);
			//_firstTime = _reader.GetBoolean(8);
			_company = _reader.GetString(8);

			List<EmailTemplates> _templates = new();
			Dictionary<string, string> _emailAddresses = new();
			Dictionary<string, string> _emailCC = new();

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync())
			{
				_templates.Add(new(_reader.NString(0), _reader.NString(1), _reader.NString(2)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync())
			{
				_emailAddresses.Add(_reader.NString(0), _reader.NString(1));
			}

			await _reader.CloseAsync();

			if (_templates.Count > 0)
			{
				EmailTemplates _templateSingle = _templates[0];
				if (!_templateSingle.CC.NullOrWhiteSpace())
				{
					string[] _ccArray = _templateSingle.CC.Split(",");
					foreach (string _cc in _ccArray)
					{
						_emailCC.Add(_cc, _cc);
					}
				}

				_templateSingle.Subject = _templateSingle.Subject.Replace("$TODAY$", DateTime.Today.CultureDate())
														 .Replace("$FULL_NAME$", $"{_firstName} {_lastName}")
														 .Replace("$FIRST_NAME$", _firstName)
														 .Replace("$LAST_NAME$", _lastName)
														 .Replace("$REQ_ID$", _reqCode)
														 .Replace("$REQ_TITLE$", _reqTitle)
														 .Replace("$COMPANY$", _company)
														 .Replace("$SUBMISSION_NOTES$", activity.Notes)
														 .Replace("$SUBMISSION_STATUS$", activity.Status)
														 .Replace("$LOGGED_USER$", user);

				_templateSingle.Template = _templateSingle.Template.Replace("$TODAY$", DateTime.Today.CultureDate())
														  .Replace("$FULL_NAME$", $"{_firstName} {_lastName}")
														  .Replace("$FIRST_NAME$", _firstName)
														  .Replace("$LAST_NAME$", _lastName)
														  .Replace("$REQ_ID$", _reqCode)
														  .Replace("$REQ_TITLE$", _reqTitle)
														  .Replace("$COMPANY$", _company)
														  .Replace("$SUBMISSION_NOTES$", activity.Notes)
														  .Replace("$SUBMISSION_STATUS$", activity.Status)
														  .Replace("$LOGGED_USER$", user);

				List<string> _attachments = new();
				//string _pathDest = "";
				//if (_firstTime)
				//{
				//    string _path = "";
				//    if (!_formatted.NullOrWhiteSpace())
				//    {
				//        _path = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _formattedInternal);
				//        _pathDest = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _formatted);
				//    }
				//    else
				//    {
				//        _path = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _originalInternal);
				//        _pathDest = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _original);
				//    }

				//    if (!_path.NullOrWhiteSpace() && !_pathDest.NullOrWhiteSpace() && System.IO.File.Exists(_path))
				//    {
				//        System.IO.File.Copy(_path, _pathDest, true);
				//        _attachments.Add(_pathDest);
				//    }
				//}

				GMailSend.SendEmail(jsonPath, emailAddress, _emailCC, _emailAddresses, _templateSingle.Subject, _templateSingle.Template, _attachments);
				//if (!_pathDest.NullOrWhiteSpace() && System.IO.File.Exists(_pathDest))
				//{
				//    System.IO.File.Delete(_pathDest);
				//}
			}
		}
		catch
		{
			//
		}

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Activity", _activities
				   }
			   };
	}

	/// <summary>
	///     Saves a candidate's education record to the database.
	/// </summary>
	/// <param name="education">The education record of the candidate to be saved.</param>
	/// <param name="candidateID">The ID of the candidate whose education record is to be saved.</param>
	/// <param name="user">The user who is performing the save operation.</param>
	/// <returns>A dictionary containing the updated list of education records for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to save the education record,
	///     and returns a dictionary containing the updated list of education records.
	///     If the operation is successful, the dictionary will contain a list of the candidate's education records.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveEducation(CandidateEducation education, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateEducation> _education = new();
		if (education == null)
		{
			return new()
				   {
					   {
						   "Education", _education
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("SaveEducation", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", education.ID);
			_command.Int("CandidateID", candidateID);
			_command.Varchar("Degree", 100, education.Degree);
			_command.Varchar("College", 255, education.College);
			_command.Varchar("State", 100, education.State);
			_command.Varchar("Country", 100, education.Country);
			_command.Varchar("Year", 10, education.Year);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_education.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4), _reader.GetString(5),
									   _reader.GetString(6)));
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
					   "Education", _education
				   }
			   };
	}

	/// <summary>
	///     Saves a candidate's experience record to the database.
	/// </summary>
	/// <param name="experience">The experience record of the candidate to be saved.</param>
	/// <param name="candidateID">The ID of the candidate whose experience record is to be saved.</param>
	/// <param name="user">The user who is performing the save operation.</param>
	/// <returns>A dictionary containing the updated list of experience records for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to save the experience record,
	///     and returns a dictionary containing the updated list of experience records.
	///     If the operation is successful, the dictionary will contain a list of updated experience records for the
	///     candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveExperience(CandidateExperience experience, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateExperience> _experiences = new();
		if (experience == null)
		{
			return new()
				   {
					   {
						   "Experience", _experiences
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("SaveExperience", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", experience.ID);
			_command.Int("CandidateID", candidateID);
			_command.Varchar("Employer", 100, experience.Employer);
			_command.Varchar("Start", 10, experience.Start);
			_command.Varchar("End", 10, experience.End);
			_command.Varchar("Location", 100, experience.Location);
			_command.Varchar("Description", 1000, experience.Description);
			_command.Varchar("Title", 1000, experience.Title);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_experiences.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4), _reader.GetString(5),
										 _reader.GetString(6),
										 _reader.GetString(7)));
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
					   "Experience", _experiences
				   }
			   };
	}

	/// <summary>
	///     Saves the MPC (Most Placeable Candidate) rating for a candidate.
	/// </summary>
	/// <param name="mpc">
	///     An instance of <see cref="CandidateRatingMPC" /> representing the MPC rating and comments for a candidate.
	/// </param>
	/// <param name="user">
	///     A string representing the user who is saving the MPC rating.
	/// </param>
	/// <returns>
	///     A dictionary containing a list of all MPC ratings for the candidate and the first MPC rating.
	/// </returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to save the MPC rating, and retrieves all MPC
	///     ratings for the candidate.
	///     If the provided MPC rating is null, it returns a dictionary with an empty list and null as the first MPC.
	///     The method handles any exceptions that occur during the database operations and continues execution.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveMPC(CandidateRatingMPC mpc, string user)
	{
		string _mpcNotes = "";
		List<CandidateMPC> _mpc = new();
		if (mpc == null)
		{
			return new()
				   {
					   {
						   "MPCList", _mpc
					   },
					   {
						   "FirstMPC", null
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("ChangeMPC", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("@CandidateId", mpc.ID);
			_command.Bit("@MPC", mpc.MPC);
			_command.Varchar("@Notes", -1, mpc.MPCComments);
			_command.Varchar("@From", 10, user);
			_mpcNotes = _command.ExecuteScalar().ToString();
		}
		catch
		{
			//
		}

		await _connection.CloseAsync();

		string[] _mpcArray = _mpcNotes?.Split('?');
		if (_mpcArray != null)
		{
			foreach (string str in _mpcArray)
			{
				string[] _innerArray = str.Split('^');
				if (_innerArray.Length == 4)
				{
					CandidateMPC _candidateMPC = new(_innerArray[0].Replace(" ", " ").ToDateTime("m/d/yy h:mm:ss tt"), _innerArray[1], _innerArray[2].ToBoolean(), _innerArray[3]);
					_mpc.Add(_candidateMPC);
				}
			}
		}

		//_mpc.AddRange((_mpcArray ?? Array.Empty<string>())
		//			 .Select(str => new
		//							{
		//								_str = str,
		//								_innerArray = str.Split('^')
		//							})
		//			 .Where(t => t._innerArray.Length == 4)
		//			 .Select(t => new CandidateMPC(t._innerArray[0].ToDateTime(), t._innerArray[1], t._innerArray[2].ToBoolean(), t._innerArray[3])));

		_mpc = _mpc.OrderByDescending(x => x.Date).ToList();
		bool _mpcFirst = false;
		string _mpcComments = "";

		if (!_mpcNotes.NullOrWhiteSpace())
		{
			CandidateMPC _mpcFirstCandidate = _mpc.FirstOrDefault();
			if (_mpcFirstCandidate != null)
			{
				_mpcFirst = _mpcFirstCandidate.MPC;
				_mpcComments = _mpcFirstCandidate.Comments;
			}
		}

		mpc.MPC = _mpcFirst;
		mpc.MPCComments = _mpcComments;

		return new()
			   {
				   {
					   "MPCList", _mpc
				   },
				   {
					   "FirstMPC", mpc
				   }
			   };
	}

	/// <summary>
	///     Saves the notes of a candidate in the database.
	/// </summary>
	/// <param name="candidateNote">An instance of <see cref="CandidateNotes" /> containing the note details.</param>
	/// <param name="candidateID">The ID of the candidate for whom the note is to be saved.</param>
	/// <param name="user">The user who is performing the save operation.</param>
	/// <returns>A dictionary containing the updated list of notes for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to save the note,
	///     and returns a dictionary containing the updated list of notes.
	///     If the operation is successful, the dictionary will contain a list of notes for the candidate.
	///     If the candidateNote parameter is null, an empty list of notes is returned.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveNotes(CandidateNotes candidateNote, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateNotes> _notes = new();
		if (candidateNote == null)
		{
			return new()
				   {
					   {
						   "Notes", _notes
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("SaveNote", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", candidateNote.ID);
			_command.Int("CandidateID", candidateID);
			_command.Varchar("Note", -1, candidateNote.Notes);
			_command.Bit("IsPrimary", false);
			_command.Varchar("EntityType", 5, "CND");
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_notes.Add(new(_reader.GetInt32(0), _reader.GetDateTime(1), _reader.GetString(2), _reader.GetString(3)));
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
					   "Notes", _notes
				   }
			   };
	}

	/// <summary>
	///     Saves the parsed data from a candidate's resume into the database.
	/// </summary>
	/// <param name="jsonFileName">The name of the JSON file containing the parsed data.</param>
	/// <param name="fileName">The name of the original resume file.</param>
	/// <param name="user">The user who is performing the save operation.</param>
	/// <param name="candidateID">The ID of the candidate whose data is to be saved.</param>
	/// <param name="path">The path where the resume file is located. Default is an empty string.</param>
	/// <param name="checkDuplicate">A flag indicating whether to check for duplicate candidates. Default is false.</param>
	/// <param name="pageCount">The number of candidates to return in the result. Default is 50.</param>
	/// <returns>A dictionary containing the status of the operation and any relevant data.</returns>
	/// <remarks>
	///     This method parses the resume data from the provided JSON file using the RChilliParseResume class,
	///     then saves the parsed data into the database. If the checkDuplicate flag is set to true,
	///     it checks for existing candidates with the same data. If a duplicate is found,
	///     it returns a list of existing candidates instead of saving the new data.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveParsedData(string jsonFileName, string fileName, string user, int candidateID, string path = "", bool checkDuplicate = false,
																 int pageCount = 50)
	{
		RChilliParseResume _rChilli = new()
			/*{
				ServiceUrl = "https://rest.rchilli.com/RChilliParser/Rchilli/parseResumeBinary"
			}*/;

		return await SaveParsedData(_rChilli.ParseResume(jsonFileName), jsonFileName, fileName, user, candidateID, path, checkDuplicate, pageCount);
	}

	/// <summary>
	///     Saves the parsed data from a candidate's resume into the database.
	/// </summary>
	/// <param name="mapFields">The mapping fields for the resume data.</param>
	/// <param name="jsonFileName">The name of the JSON file containing the parsed data.</param>
	/// <param name="fileName">The name of the original resume file.</param>
	/// <param name="user">The user who is performing the save operation.</param>
	/// <param name="candidateID">The ID of the candidate whose data is to be saved.</param>
	/// <param name="path">The path where the resume file is located. Default is an empty string.</param>
	/// <param name="checkDuplicate">A flag indicating whether to check for duplicate candidates. Default is false.</param>
	/// <param name="pageCount">The number of candidates to return in the result. Default is 50.</param>
	/// <returns>A dictionary containing the status of the operation and any relevant data.</returns>
	/// <remarks>
	///     This method parses the resume data from the provided JSON file using the RChilliMapFields class,
	///     then saves the parsed data into the database. If the checkDuplicate flag is set to true,
	///     it checks for existing candidates with the same data. If a duplicate is found,
	///     it returns a list of existing candidates instead of saving the new data.
	/// </remarks>
	private async Task<Dictionary<string, object>> SaveParsedData(RChilliMapFields mapFields, string jsonFileName, string fileName, string user, int candidateID, string path = "",
																  bool checkDuplicate = false, int pageCount = 50)
	{
		ResumeParserData _objRChilli = mapFields.ResumeParserData;
		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		//int _returnCode = 0;
		List<ExistingCandidate> _existingCandidates = new();
		int _returnCode = 0;
		Dictionary<string, object> _returnObject = new();
		try
		{
			if (checkDuplicate)
			{
				await using SqlCommand _command = new("CheckCandidateExists", _connection);
				_command.CommandType = CommandType.StoredProcedure;
				_command.Varchar("@FirstName", 50, _objRChilli.Name.FirstName);
				_command.Varchar("@LastName", 50, _objRChilli.Name.LastName);
				_command.Varchar("@Email", 50, _objRChilli.Email.Count > 0 ? _objRChilli.Email[0].EmailAddress : "");
				await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
				if (_reader.HasRows)
				{
					while (await _reader.ReadAsync())
					{
						_existingCandidates.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3).FormatPhoneNumber()));
					}

					await _reader.CloseAsync();
					await _connection.CloseAsync();

					return new()
						   {
							   {
								   "Json", jsonFileName
							   },
							   {
								   "Candidates", _existingCandidates
							   },
							   {
								   "FileName", fileName
							   },
							   {
								   "Email", _objRChilli.Email.Count > 0 ? _objRChilli.Email[0].EmailAddress : ""
							   },
							   {
								   "Phone", _objRChilli.PhoneNumber.Count > 0 ? _objRChilli.PhoneNumber[0].OriginalNumber.StripPhoneNumber().FormatPhoneNumber() : ""
							   }
						   };
				}

				await _reader.CloseAsync();
			}

			string _firstName = "", _lastName = "", _middleName = "", _emailAddress = "", _phoneMain = "", _altPhone = "", _address1 = "", _city = "", _state = "", _zipCode = "";
			string _linkedIn = "", _internalFileName = Guid.NewGuid().ToString("N");

			if (_objRChilli.Name != null)
			{
				Name _name = _objRChilli.Name;
				_firstName = _name.FirstName;
				_middleName = _name.MiddleName;
				_lastName = _name.LastName;
			}

			if (_objRChilli.Email is {Count: > 0})
			{
				_emailAddress = _objRChilli.Email[0].EmailAddress;
			}

			if (_objRChilli.Address is {Count: > 0})
			{
				Address _address = _objRChilli.Address[0];
				_address1 = _address.Street;
				_city = _address.City;
				_state = _address.State;
				_zipCode = _address.ZipCode;
			}

			if (_objRChilli.PhoneNumber is {Count: > 0})
			{
				List<PhoneNumber> _phone = _objRChilli.PhoneNumber;
				_phoneMain = _phone[0].OriginalNumber.StripPhoneNumber();
				if (_phone.Count > 1)
				{
					_altPhone = _phone[1].OriginalNumber.StripPhoneNumber();
				}
			}

			DataTable _tableEducation = new();
			_tableEducation.Columns.Add("Degree", typeof(string));
			_tableEducation.Columns.Add("College", typeof(string));
			_tableEducation.Columns.Add("State", typeof(string));
			_tableEducation.Columns.Add("Country", typeof(string));
			_tableEducation.Columns.Add("Year", typeof(string));

			if (_objRChilli.SegregatedQualification is {Count: > 0})
			{
				_objRChilli.SegregatedQualification.ForEach(qualification =>
															{
																DataRow _dr = _tableEducation.NewRow();
																_dr["Degree"] = qualification.Degree.DegreeName;
																_dr["College"] = qualification.Institution.Name;
																_dr["State"] = qualification.Institution.Location.State;
																_dr["Country"] = qualification.Institution.Location.Country;
																_dr["Year"] = qualification.FormattedDegreePeriod;
																_tableEducation.Rows.Add(_dr);
															});
			}

			DataTable _tableEmployer = new();
			_tableEmployer.Columns.Add("Employer", typeof(string));
			_tableEmployer.Columns.Add("Start", typeof(string));
			_tableEmployer.Columns.Add("End", typeof(string));
			_tableEmployer.Columns.Add("Location", typeof(string));
			_tableEmployer.Columns.Add("Title", typeof(string));
			_tableEmployer.Columns.Add("Description", typeof(string));

			if (_objRChilli.SegregatedExperience is {Count: > 0})
			{
				_objRChilli.SegregatedExperience.ForEach(experience =>
														 {
															 DataRow _dr = _tableEmployer.NewRow();
															 _dr["Employer"] = experience.Employer.EmployerName;
															 _dr["Start"] = experience.StartDate;
															 _dr["End"] = experience.IsCurrentEmployer == "true" ? "" : experience.EndDate;
															 _dr["Location"] = experience.Location.City + ", " + experience.Location.State + ", " + experience.Location.Country;
															 _dr["Title"] = experience.JobProfile.Title;
															 _dr["Description"] = experience.JobDescription;
															 _tableEmployer.Rows.Add(_dr);
														 });
			}

			DataTable _tableSkills = new();
			_tableSkills.Columns.Add("Skill", typeof(string));
			_tableSkills.Columns.Add("LastUsed", typeof(short));
			_tableSkills.Columns.Add("Month", typeof(short));

			DataRow _newRow = _tableSkills.NewRow();
			_newRow["Skill"] = "[TECHNICAL]";
			_newRow["LastUsed"] = 0;
			_newRow["Month"] = 0;
			_tableSkills.Rows.Add(_newRow);
			if (_objRChilli.SegregatedSkill is {Count: > 0})
			{
				_objRChilli.SegregatedSkill.ForEach(skill =>
													{
														if (skill.Type != "OperationalSkill")
														{
															return;
														}

														DataRow _dr = _tableSkills.NewRow();
														_dr["Skill"] = skill.Skill;
														_dr["LastUsed"] = GetYear(skill.LastUsed);
														_dr["Month"] = skill.ExperienceInMonths;
														_tableSkills.Rows.Add(_dr);
													});
			}

			if (_objRChilli.WebSite is {Count: > 0})
			{
				foreach (WebSite _webSite in _objRChilli.WebSite.Where(webSite => webSite.Type.ToLowerInvariant() == "linkedin"))
				{
					_linkedIn = _webSite.Url;
				}
			}

			string _keywords = _objRChilli.SkillKeywords.Length > 500 ? _objRChilli.SkillKeywords[..500] : _objRChilli.SkillKeywords;
			string _experienceSummary = $"{_objRChilli.ExecutiveSummary}{Environment.NewLine}{_objRChilli.ManagementSummary}";
			string _objective = _objRChilli.Summary;
			string _textResume = _objRChilli.HtmlResume;
			try
			{
				await using SqlCommand _command1 = new("SaveParsedCandidate", _connection);
				_command1.CommandType = CommandType.StoredProcedure;
				_command1.Int("@ID", candidateID == 0 ? DBNull.Value : candidateID, true);
				_command1.Varchar("@FirstName", 50, _firstName);
				_command1.Varchar("@MiddleName", 50, _middleName);
				_command1.Varchar("@LastName", 50, _lastName);
				_command1.Varchar("@Address", 255, _address1);
				_command1.Varchar("@Address2", 255, "");
				_command1.Varchar("@City", 50, _city);
				_command1.Varchar("@State", 50, _state);
				_command1.Varchar("@Zip", 20, _zipCode);
				_command1.Varchar("@Email", 255, _emailAddress);
				_command1.Bit("@Background", false);
				_command1.Varchar("@SecurityNotes", -1, "");
				_command1.Varchar("@Phone1", 15, _phoneMain);
				_command1.Varchar("@Phone2", 15, _altPhone);
				_command1.Varchar("@ExperienceSummary", -1, _experienceSummary);
				_command1.Int("@Experience", _objRChilli.WorkedPeriod.TotalExperienceInMonths);
				_command1.Varchar("@Summary", -1, _objective);
				_command1.Varchar("@Objective", -1, _objRChilli.Objectives);
				_command1.Varchar("@Keywords", 500, _keywords);
				_command1.Varchar("@JobOptions", 5, "F");
				_command1.Varchar("@TaxTerm", 10, "E");
				_command1.Varchar("@TextResume", -1, _textResume);
				_command1.Varchar("@OriginalResume", 255, $"Original_{fileName.Replace(" ", "")}");
				_command1.Varchar("@User", 10, user);
				_command1.Varchar("@InternalFileName", 255, _internalFileName);
				_command1.Varchar("@LinkedIn", 10, _linkedIn);
				_command1.Varchar("@JsonFileName", 255, jsonFileName);
				_command1.Parameters.AddWithValue("@Education", _tableEducation).SqlDbType = SqlDbType.Structured;
				_command1.Parameters.AddWithValue("@Employer", _tableEmployer).SqlDbType = SqlDbType.Structured;
				_command1.Parameters.AddWithValue("@Skills", _tableSkills).SqlDbType = SqlDbType.Structured;

				await using SqlDataReader _reader1 = await _command1.ExecuteReaderAsync();

				_reader1.Read();
				if (_reader1.HasRows)
				{
					_returnCode = _reader1.GetInt32(0);
				}

				await _reader1.CloseAsync();

				string _destinationFileName = Path.Combine(path, "Uploads", "Candidate", "0", $"Original_{fileName.Replace(" ", "")}");
				string _newDestination = Path.Combine(path, "Uploads", "Candidate", _returnCode.ToString(), _internalFileName);

				Directory.CreateDirectory(Path.Combine(path, "Uploads", "Candidate", _returnCode.ToString()));
				if (System.IO.File.Exists(_destinationFileName))
				{
					System.IO.File.Move(_destinationFileName, _newDestination, true);
				}

				string _destinationJsonFileName = Path.Combine(path, "Uploads", "Candidate", "0", jsonFileName);
				string _newJsonDestination = jsonFileName.Replace("\\0\\", "\\" + _returnCode + "\\");
				if (System.IO.File.Exists(_destinationJsonFileName))
				{
					System.IO.File.Move(_destinationJsonFileName, _newJsonDestination, true);
				}

				CandidateSearch _search = new()
										  {
											  ItemCount = pageCount,
											  User = user,
											  AllCandidates = true,
											  MyCandidates = false,
											  ActiveRequisitionsOnly = false
										  };

				_returnObject = await GetGridCandidates(_search, candidateID);
				_returnObject.Add("CandidateID", _returnCode);
			}
			catch //(Exception exception)
			{
				await Task.Yield();
				// ignored
			}

			await _connection.CloseAsync();
		}
		catch
		{
			//
		}

		return _returnObject;
	}

	/// <summary>
	///     Asynchronously saves the rating of a candidate.
	/// </summary>
	/// <param name="rating">An instance of <see cref="CandidateRatingMPC" /> representing the rating to be saved.</param>
	/// <param name="user">A string representing the user who is saving the rating.</param>
	/// <returns>
	///     A task that represents the asynchronous operation. The task result contains a dictionary with two keys:
	///     "RatingList" - a list of <see cref="CandidateRating" /> objects representing the rating history of the candidate,
	///     and "FirstRating" - an instance of <see cref="CandidateRatingMPC" /> representing the first rating in the rating
	///     history.
	/// </returns>
	/// <remarks>
	///     This method performs a database operation using a stored procedure named "ChangeRating".
	///     If the rating parameter is null, the method returns a dictionary with "RatingList" key set to an empty list and
	///     "FirstRating" key set to null.
	///     If the rating parameter is not null, the method executes the stored procedure with the provided rating and user
	///     parameters,
	///     then parses the result to create a list of <see cref="CandidateRating" /> objects and an instance of
	///     <see cref="CandidateRatingMPC" />.
	///     These are then returned in a dictionary.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveRating(CandidateRatingMPC rating, string user)
	{
		string _ratingNotes = "";
		List<CandidateRating> _rating = new();
		if (rating == null)
		{
			return new()
				   {
					   {
						   "RatingList", _rating
					   },
					   {
						   "FirstRating", null
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("ChangeRating", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("@CandidateId", rating.ID);
			_command.TinyInt("@Rating", rating.Rating);
			_command.Varchar("@Notes", -1, rating.RatingComments);
			_command.Varchar("@From", 10, user);
			_ratingNotes = _command.ExecuteScalar().ToString();
		}
		catch
		{
			//
		}

		await _connection.CloseAsync();

		string[] _ratingArray = _ratingNotes?.Split('?');
		_rating.AddRange((_ratingArray ?? Array.Empty<string>())
						.Select(str => new
									   {
										   _str = str,
										   _innerArray = str.Split('^')
									   })
						.Where(t => t._innerArray.Length == 4)
						.Select(t => new CandidateRating(t._innerArray[0].ToDateTime(), t._innerArray[1], t._innerArray[2].ToByte(), t._innerArray[3])));

		_rating = _rating.OrderByDescending(x => x.Date).ToList();
		int _ratingFirst = 0;
		string _ratingComments = "";

		if (!_ratingNotes.NullOrWhiteSpace())
		{
			CandidateRating _ratingFirstCandidate = _rating.FirstOrDefault();
			if (_ratingFirstCandidate != null)
			{
				_ratingFirst = _ratingFirstCandidate.Rating;
				_ratingComments = _ratingFirstCandidate.Comments;
			}
		}

		rating.Rating = _ratingFirst;
		rating.RatingComments = _ratingComments;

		return new()
			   {
				   {
					   "RatingList", _rating
				   },
				   {
					   "FirstRating", rating
				   }
			   };
	}

	/// <summary>
	///     Saves a candidate's skill to the database.
	/// </summary>
	/// <param name="skill">The skill of the candidate to be saved.</param>
	/// <param name="candidateID">The ID of the candidate whose skill is to be saved.</param>
	/// <param name="user">The user who is performing the save operation.</param>
	/// <returns>A dictionary containing the updated list of skills for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to save the skill,
	///     and returns a dictionary containing the updated list of skills.
	///     If the operation is successful, the dictionary will contain a list of remaining skills for the
	///     candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SaveSkill(CandidateSkills skill, int candidateID, string user)
	{
		await Task.Delay(1);
		List<CandidateSkills> _skills = new();
		if (skill == null)
		{
			return new()
				   {
					   {
						   "Skills", _skills
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("SaveSkill", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("EntitySkillId", skill.ID);
			_command.Varchar("Skill", 100, skill.Skill);
			_command.Int("CandidateID", candidateID);
			_command.SmallInt("LastUsed", skill.LastUsed);
			_command.SmallInt("ExpMonth", skill.ExpMonth);
			_command.Varchar("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_skills.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetInt16(2), _reader.GetInt16(3), _reader.GetString(4)));
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
					   "Skills", _skills
				   }
			   };
	}

	/// <summary>
	///     Submits a candidate for a specific requisition.
	/// </summary>
	/// <param name="requisitionID">The ID of the requisition.</param>
	/// <param name="candidateID">The ID of the candidate.</param>
	/// <param name="notes">Additional notes about the submission (optional).</param>
	/// <param name="user">The user who is performing the submission (optional).</param>
	/// <param name="roleID">The role ID of the user (optional, default is "RS").</param>
	/// <param name="emailAddress">
	///     The email address to which notifications will be sent (optional, default is
	///     "maniv@titan-techs.com").
	/// </param>
	/// <param name="uploadPath">The path where files will be uploaded (optional).</param>
	/// <param name="jsonPath">The path of the JSON file (optional).</param>
	/// <returns>A dictionary containing the status of the operation and any relevant data.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to submit the candidate for the requisition,
	///     and returns a dictionary containing the result of the operation.
	///     If the operation is successful, the dictionary will contain a list of activities for the candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> SubmitCandidateRequisition(int requisitionID, int candidateID, string notes = "", string user = "", string roleID = "RS",
																			 string emailAddress = "maniv@titan-techs.com", string uploadPath = "", string jsonPath = "")
	{
		await Task.Delay(1);
		List<CandidateActivity> _activities = new();
		if (candidateID == 0 || requisitionID == 0)
		{
			return new()
				   {
					   {
						   "Activity", null
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("SubmitCandidateRequisition", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("RequisitionID", requisitionID);
			_command.Int("CandidateID", candidateID);
			_command.Varchar("Notes", 1000, notes);
			_command.Char("RoleID", 2, roleID);
			_command.Char("User", 10, user);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_activities.Add(new(_reader.GetString(0), _reader.GetDateTime(1), _reader.GetString(2), _reader.GetInt32(3), _reader.GetInt32(4),
										_reader.GetString(5), _reader.GetString(6), _reader.GetInt32(7), _reader.GetBoolean(8), _reader.GetString(9), _reader.GetString(10),
										_reader.GetString(11), _reader.GetBoolean(12), _reader.GetString(13), _reader.GetInt32(14), _reader.GetString(15),
										_reader.GetInt32(16), _reader.GetString(17), _reader.GetBoolean(18), _reader.NDateTime(19),
										_reader.GetString(20), _reader.NString(21), _reader.NString(22), _reader.GetBoolean(23)));
				}
			}

			await _reader.NextResultAsync();
			string _firstName = "", _lastName = "", _reqCode = "", _reqTitle = "", _original = "", _originalInternal = "", _formatted = "", _formattedInternal = "", _company = "";
			//bool _firstTime = false;
			await _reader.ReadAsync();
			_firstName = _reader.NString(0);
			_lastName = _reader.NString(1);
			_reqCode = _reader.NString(2);
			_reqTitle = _reader.NString(3);
			_original = _reader.NString(4);
			_originalInternal = _reader.NString(5);
			_formatted = _reader.NString(6);
			_formattedInternal = _reader.NString(7);
			//_firstTime = true;
			_company = _reader.GetString(8);

			List<EmailTemplates> _templates = new();
			Dictionary<string, string> _emailAddresses = new();
			Dictionary<string, string> _emailCC = new();

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync())
			{
				_templates.Add(new(_reader.NString(0), _reader.NString(1), _reader.NString(2)));
			}

			await _reader.NextResultAsync();
			while (await _reader.ReadAsync())
			{
				_emailAddresses.Add(_reader.NString(0), _reader.NString(1));
			}

			await _reader.CloseAsync();

			if (_templates.Count > 0)
			{
				EmailTemplates _templateSingle = _templates[0];
				if (!_templateSingle.CC.NullOrWhiteSpace())
				{
					string[] _ccArray = _templateSingle.CC.Split(",");
					foreach (string _cc in _ccArray)
					{
						_emailCC.Add(_cc, _cc);
					}
				}

				_templateSingle.Subject = _templateSingle.Subject.Replace("$TODAY$", DateTime.Today.CultureDate())
														 .Replace("$FULL_NAME$", $"{_firstName} {_lastName}")
														 .Replace("$FIRST_NAME$", _firstName)
														 .Replace("$LAST_NAME$", _lastName)
														 .Replace("$REQ_ID$", _reqCode)
														 .Replace("$REQ_TITLE$", _reqTitle)
														 .Replace("$COMPANY$", _company)
														 .Replace("$SUBMISSION_NOTES$", notes)
														 .Replace("$LOGGED_USER$", user);

				_templateSingle.Template = _templateSingle.Template.Replace("$TODAY$", DateTime.Today.CultureDate())
														  .Replace("$FULL_NAME$", $"{_firstName} {_lastName}")
														  .Replace("$FIRST_NAME$", _firstName)
														  .Replace("$LAST_NAME$", _lastName)
														  .Replace("$REQ_ID$", _reqCode)
														  .Replace("$REQ_TITLE$", _reqTitle)
														  .Replace("$COMPANY$", _company)
														  .Replace("$SUBMISSION_NOTES$", notes)
														  .Replace("$LOGGED_USER$", user);

				List<string> _attachments = new();
				string _pathDest = "";
				//if (_firstTime)
				//{
				string _path = "";
				if (!_formatted.NullOrWhiteSpace())
				{
					_path = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _formattedInternal);
					_pathDest = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _formatted);
				}
				else
				{
					_path = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _originalInternal);
					_pathDest = Path.Combine(uploadPath, "Uploads", "Candidate", candidateID.ToString(), _original);
				}

				if (!_path.NullOrWhiteSpace() && !_pathDest.NullOrWhiteSpace() && System.IO.File.Exists(_path))
				{
					System.IO.File.Copy(_path, _pathDest, true);
					_attachments.Add(_pathDest);
				}
				//}

				GMailSend.SendEmail(jsonPath, emailAddress, _emailCC, _emailAddresses, _templateSingle.Subject, _templateSingle.Template, _attachments);
				await Task.Yield();
				if (!_pathDest.NullOrWhiteSpace() && System.IO.File.Exists(_pathDest))
				{
					bool _waiting = true;
					byte _attempts = 0;
					while (_waiting && _attempts < 10)
					{
						try
						{
							await Task.Yield();
							System.IO.File.Delete(_pathDest);
							_waiting = false;
						}
						catch
						{
							_attempts++;
							_waiting = _attempts < 10;
						}
					}
				}
			}
		}
		catch
		{
			//
		}

		await _connection.CloseAsync();

		return new()
			   {
				   {
					   "Activity", _activities
				   }
			   };
	}

	/// <summary>
	///     Reverts the last activity of a candidate.
	/// </summary>
	/// <param name="submissionID">The ID of the submission related to the candidate's activity.</param>
	/// <param name="user">The user who is performing the undo operation.</param>
	/// <param name="roleID">The role ID of the user, default is "RS".</param>
	/// <param name="isCandidateScreen">
	///     A boolean value indicating if the operation is performed from the candidate screen,
	///     default is true.
	/// </param>
	/// <returns>A dictionary containing the list of remaining activities for the candidate.</returns>
	/// <remarks>
	///     This method connects to the database, executes a stored procedure to undo the candidate's last activity,
	///     and returns a dictionary containing the updated list of activities.
	///     If the operation is successful, the dictionary will contain a list of remaining activities for the candidate.
	/// </remarks>
	[HttpPost]
	public async Task<Dictionary<string, object>> UndoCandidateActivity(int submissionID, string user, string roleID = "RS", bool isCandidateScreen = true)
	{
		await Task.Delay(1);
		List<CandidateActivity> _activities = new();
		if (submissionID == 0)
		{
			return new()
				   {
					   {
						   "Activity", null
					   }
				   };
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("UndoCandidateActivity", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("Id", submissionID);
			_command.Varchar("User", 10, user);
			_command.Bit("CandScreen", isCandidateScreen);
			_command.Char("RoleID", 2, roleID);
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_activities.Add(new(_reader.GetString(0), _reader.GetDateTime(1), _reader.GetString(2), _reader.GetInt32(3), _reader.GetInt32(4), _reader.GetString(5),
										_reader.GetString(6),
										_reader.GetInt32(7), _reader.GetBoolean(8), _reader.GetString(9), _reader.GetString(10), _reader.GetString(11), _reader.GetBoolean(12),
										_reader.GetString(13),
										_reader.GetInt32(14), _reader.GetString(15), _reader.GetInt32(16), _reader.GetString(17), _reader.GetBoolean(18), _reader.NDateTime(19),
										_reader.GetString(20),
										_reader.NString(21), _reader.NString(22), _reader.GetBoolean(23)));
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
					   "Activity", _activities
				   }
			   };
	}

	/// <summary>
	///     Uploads a document for a candidate.
	/// </summary>
	/// <param name="file">The file to be uploaded.</param>
	/// <returns>A dictionary containing the status of the operation and any relevant data.</returns>
	/// <remarks>
	///     This method creates a directory for the candidate's documents if it doesn't exist,
	///     saves the uploaded file to the server, and then saves the document details to the database
	///     using a stored procedure. If the operation is successful, the dictionary will contain a list
	///     of documents for the candidate.
	/// </remarks>
	[HttpPost, RequestSizeLimit(62914560)]
	public async Task<Dictionary<string, object>> UploadDocument(IFormFile file)
	{
		await Task.Yield();
		string _fileName = file.FileName;
		string _candidateID = Request.Form["candidateID"].ToString();
		//string _mime = Request.Form["mime"];
		string _internalFileName = Guid.NewGuid().ToString("N");
		Directory.CreateDirectory(Path.Combine(Request.Form["path"].ToString(), "Uploads", "Candidate", _candidateID));
		string _destinationFileName = Path.Combine(Request.Form["path"].ToString(), "Uploads", "Candidate", _candidateID, _internalFileName);

		//using MemoryStream _stream = new();
		await using (FileStream _fs = System.IO.File.Open(_destinationFileName, FileMode.OpenOrCreate, FileAccess.Write))
		{
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
		}

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		List<CandidateDocument> _documents = new();
		try
		{
			await using SqlCommand _command = new("SaveCandidateDocuments", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("CandidateId", _candidateID.ToInt32());
			_command.Varchar("DocumentName", 255, Request.Form["name"].ToString());
			_command.Varchar("DocumentLocation", 255, _fileName);
			_command.Varchar("DocumentNotes", 2000, Request.Form["notes"].ToString());
			_command.Varchar("InternalFileName", 50, _internalFileName);
			_command.Int("DocumentType", Request.Form["type"].ToInt32());
			_command.Varchar("DocsUser", 10, Request.Form["user"].ToString());
			await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
			if (_reader.HasRows)
			{
				while (_reader.Read())
				{
					_documents.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), $"{_reader.NDateTime(4)} [{_reader.NString(5)}]",
									   _reader.GetString(6),
									   _reader.GetString(7), _reader.GetInt32(8)));
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

	/// <summary>
	///     Uploads a candidate's resume to the server and saves its details in the database.
	/// </summary>
	/// <param name="file">The resume file to be uploaded.</param>
	/// <remarks>
	///     This method receives a resume file from the client, generates a unique internal file name, and saves the file to a
	///     specific directory on the server.
	///     It also saves the resume details in the database by executing a stored procedure.
	///     The method handles any exceptions that may occur during the file upload and database operations.
	///     If the directory for storing the file does not exist, it is created.
	///     The method is asynchronous and returns a Task.
	/// </remarks>
	[HttpPost, RequestSizeLimit(10 * 1024 * 1024)]
	public async Task UploadResume(IFormFile file)
	{
		await Task.Yield();
		string _fileName = file.FileName; //Request.Form["filename"].ToString();
		string _candidateID = Request.Form["candidateID"].ToString();
		string _internalFileName = Guid.NewGuid().ToString("N");

		await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
		await _connection.OpenAsync();
		try
		{
			await using SqlCommand _command = new("SaveCandidateResume", _connection);
			_command.CommandType = CommandType.StoredProcedure;
			_command.Int("CandidateId", _candidateID.ToInt32());
			_command.Varchar("DocumentLocation", 255, _fileName);
			_command.Varchar("InternalFileName", 50, _internalFileName);
			_command.Varchar("DocumentType", 20, Request.Form["type"].ToString());
			_command.Varchar("DocsUser", 10, Request.Form["user"].ToString());
			object _returnValue = await _command.ExecuteScalarAsync();
			if (_returnValue != null)
			{
				_internalFileName = _returnValue.ToString();
			}
		}
		catch
		{
			//
		}

		try
		{
			Directory.CreateDirectory(Path.Combine(Request.Form["path"].ToString(), "Uploads", "Candidate", _candidateID));
		}
		catch
		{
			return;
		}

		if (_internalFileName != null)
		{
			string _destinationFileName = Path.Combine(Request.Form["path"].ToString(), "Uploads", "Candidate", _candidateID, _internalFileName);
			await using FileStream _fs = System.IO.File.Create(_destinationFileName);
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
		}
	}
}