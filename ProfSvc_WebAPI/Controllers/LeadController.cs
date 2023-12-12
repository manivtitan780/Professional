#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           LeadController.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-21-2023 15:57
// Last Updated On:     08-21-2023 21:15
// *****************************************/

#endregion

namespace ProfSvc_WebAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class LeadController : ControllerBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LeadController" /> class.
    /// </summary>
    /// <param name="configuration">
    ///     An instance of <see cref="IConfiguration" />, used to access application configuration
    ///     properties.
    /// </param>
    /// <param name="env">
    ///     An instance of <see cref="IWebHostEnvironment" />, used to provide information about the web hosting
    ///     environment an application is running in.
    /// </param>
    public LeadController(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _hostingEnvironment = env;
    }

    private readonly IConfiguration _configuration;

    private readonly IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    ///     Converts a lead to a company.
    /// </summary>
    /// <param name="leadID">The ID of the lead to be converted.</param>
    /// <param name="user">The user performing the conversion.</param>
    /// <param name="path">The path where the lead's files are stored.</param>
    /// <returns>The ID of the company that the lead was converted to.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Opens a connection to the database.
    ///     - Executes the "ConvertLead" stored procedure, passing in the lead ID and user name. The stored procedure returns
    ///     the company ID.
    ///     - Copies all files associated with the lead to the company's folder.
    ///     - Closes the database connection.
    ///     - Returns the company ID.
    /// </remarks>
    [HttpPost]
    public async Task<int> ConvertLead(int leadID, string user, string path)
    {
        await Task.Yield();
        if (leadID == 0)
        {
            return 0;
        }

        await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
        await _connection.OpenAsync();
        int _companyID = 0;
        try
        {
            /*Execute the Convert Lead stored procedure passing the Lead ID and User Name, return the Company ID*/
            await using SqlCommand _command = new("ConvertLead", _connection);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Int("LeadId", leadID);
            _command.Varchar("User", 10, user);
            _companyID = _command.ExecuteScalar().ToInt32();

            /*Copy all files for the Leads, if any, to the Companies folder*/
            string _destinationPath = Path.Combine(path, "Uploads", "Company", _companyID.ToString());
            Directory.CreateDirectory(_destinationPath);
            string _sourcePath = Path.Combine(path, "Uploads", "Leads", leadID.ToString());
            DirectoryInfo _directory = new(_sourcePath);
            if (_directory.Exists)
            {
                foreach (FileInfo _fileInfo in _directory.GetFiles())
                {
                    string _destPath = Path.Combine(_destinationPath, _fileInfo.Name);
                    _fileInfo.CopyTo(_destPath, true);
                }
            }
        }
        catch
        {
            //
        }

        await _connection.CloseAsync();

        return _companyID;
    }

    /// <summary>
    ///     Deletes the notes associated with a lead.
    /// </summary>
    /// <param name="id">The ID of the note to be deleted.</param>
    /// <param name="leadID">The ID of the lead associated with the note.</param>
    /// <param name="user">The user performing the deletion.</param>
    /// <returns>A dictionary containing the remaining notes after deletion.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Opens a connection to the database.
    ///     - Executes the "DeleteLeadNotes" stored procedure, passing in the note ID, lead ID, and user name.
    ///     - Reads the remaining notes from the database and adds them to a list.
    ///     - Closes the database connection.
    ///     - Returns a dictionary containing the list of remaining notes.
    /// </remarks>
    [HttpPost]
    public async Task<Dictionary<string, object>> DeleteNotes(int id, int leadID, string user)
    {
        await Task.Yield();
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
            await using SqlCommand _command = new("DeleteLeadNotes", _connection);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Int("Id", id);
            _command.Int("leadId", leadID);
            _command.Varchar("User", 10, user);
            await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
            if (_reader.HasRows)
            {
                while (_reader.Read())
                {
                    _notes.Add(new(_reader.GetInt32(3), _reader.GetDateTime(0), _reader.GetString(1), _reader.GetString(2)));
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
    ///     Downloads the file associated with a specific document ID.
    /// </summary>
    /// <param name="documentID">The ID of the document to be downloaded.</param>
    /// <returns>An instance of <see cref="DocumentDetails" /> containing the details of the downloaded document.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Opens a connection to the database.
    ///     - Executes the "GetLeadDocumentDetails" stored procedure, passing in the document ID.
    ///     - Reads the document details from the database and creates a new instance of <see cref="DocumentDetails" />.
    ///     - Closes the database connection.
    ///     - Returns the instance of <see cref="DocumentDetails" />.
    /// </remarks>
    [HttpGet]
    public async Task<DocumentDetails> DownloadFile(int documentID)
    {
        await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
        await using SqlCommand _command = new("GetLeadDocumentDetails", _connection);
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
    ///     This method is an HTTP GET endpoint that retrieves leads data for a grid view.
    /// </summary>
    /// <param name="searchModel">
    ///     An instance of LeadSearch class which includes search parameters such as ItemCount, Page,
    ///     SortField, SortDirection, and Name.
    /// </param>
    /// <returns>
    ///     A dictionary containing a list of leads and the total count of leads. The "Leads" key contains a list of
    ///     LeadClass objects representing the leads. The "Count" key contains the total number of leads.
    /// </returns>
    /// <remarks>
    ///     This method connects to the database using a connection string from the configuration. It executes a stored
    ///     procedure named "GetGridLeads" with parameters derived from the searchModel. The method reads the results from the
    ///     stored procedure into a list of LeadClass objects and a count of total leads. These are then returned in a
    ///     dictionary.
    /// </remarks>
    [HttpGet]
    public async Task<Dictionary<string, object>> GetGridLeads([FromBody] LeadSearch searchModel)
    {
        await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
        List<LeadClass> _leads = new();
        await using SqlCommand _command = new("GetGridLeads", _connection);
        _command.CommandType = CommandType.StoredProcedure;
        _command.Int("Count", searchModel.ItemCount);
        _command.Int("Page", searchModel.Page);
        _command.Int("SortRow", searchModel.SortField);
        _command.TinyInt("SortOrder", searchModel.SortDirection);
        _command.Varchar("Company", 100, searchModel.Name);

        await _connection.OpenAsync();
        await using SqlDataReader _reader = await _command.ExecuteReaderAsync();

        _reader.Read();
        int _count = _reader.GetInt32(0);

        _reader.NextResult();

        while (_reader.Read())
        {
            string _location = _reader.GetString(4);
            if (_location.StartsWith(","))
            {
                _location = _location[1..].Trim();
            }

            _leads.Add(new(_reader.GetInt32(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3).StripPhoneNumber().FormatPhoneNumber(),
                           _location, _reader.GetString(5), _reader.GetString(6), $"{_reader.GetDateTime(7).CultureDate()} [{_reader.GetString(8)}]", _reader.GetString(8)));
        }

        await _reader.CloseAsync();

        await _connection.CloseAsync();

        return new()
               {
                   {
                       "Leads", _leads
                   },
                   {
                       "Count", _count
                   }
               };
    }

    /// <summary>
    ///     Retrieves the details of a specific lead.
    /// </summary>
    /// <param name="leadID">The ID of the lead.</param>
    /// <returns>
    ///     A dictionary containing the details of the lead, notes related to the lead, and documents associated with the
    ///     lead.
    /// </returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Opens a connection to the database.
    ///     - Executes the "GetLeadDetails" stored procedure, passing in the lead ID. The stored procedure returns the lead
    ///     details, notes, and documents.
    ///     - Reads the lead details and stores them in a LeadDetails object.
    ///     - Reads the notes related to the lead and stores them in a list of CandidateNotes objects.
    ///     - Reads the documents associated with the lead and stores them in a list of RequisitionDocuments objects.
    ///     - Closes the database connection.
    ///     - Returns a dictionary containing the lead details, notes, and documents.
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<Dictionary<string, object>>> GetLeadDetails(int leadID)
    {
        await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
        LeadDetails _lead = null;

        await using SqlCommand _command = new("GetLeadDetails", _connection);
        _command.CommandType = CommandType.StoredProcedure;
        _command.Int("@LeadID", leadID);

        await _connection.OpenAsync();
        await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
        if (_reader.HasRows) //Lead Details
        {
            _reader.Read();
            _lead = new(leadID, _reader.GetString(1), _reader.GetString(2), _reader.GetString(3), _reader.GetString(4), _reader.GetString(5), _reader.GetString(6),
                        _reader.GetString(8), _reader.GetInt32(7), _reader.GetString(9), _reader.GetInt32(10), _reader.GetDecimal(11), _reader.GetByte(12),
                        _reader.GetByte(13), _reader.GetString(14), _reader.GetString(24), _reader.GetByte(15), _reader.GetString(16), _reader.GetDateTime(17),
                        _reader.GetString(18), _reader.GetDateTime(19), _reader.GetString(20), _reader.GetString(21), _reader.GetString(22),
                        _reader.GetString(23));
        }

        _reader.NextResult(); //Notes
        List<CandidateNotes> _notes = new();
        while (_reader.Read())
        {
            _notes.Add(new(_reader.GetInt32(3), _reader.GetDateTime(0), _reader.GetString(1), _reader.GetString(2)));
        }

        _reader.NextResult(); //Documents
        List<RequisitionDocuments> _documents = new();
        while (_reader.Read())
        {
            _documents.Add(new(_reader.GetInt32(0), _reader.GetInt32(1), _reader.NString(2), _reader.NString(3), _reader.NString(6),
                               $"{_reader.NDateTime(5)} [{_reader.NString(4)}]", _reader.NString(7), ""));
        }

        await _reader.CloseAsync();

        await _connection.CloseAsync();

        return new Dictionary<string, object>
               {
                   {
                       "Lead", _lead
                   },
                   {
                       "Notes", _notes
                   },
                   {
                       "Documents", _documents
                   }
               };
    }

    /// <summary>
    ///     Saves the details of a lead.
    /// </summary>
    /// <param name="leadDetails">An instance of <see cref="LeadDetails" />, containing the details of the lead to be saved.</param>
    /// <param name="user">The user performing the save operation.</param>
    /// <returns>The ID of the saved lead.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Opens a connection to the database.
    ///     - Executes the "SaveLeads" stored procedure, passing in the lead details and user name. The stored procedure
    ///     returns
    ///     the lead ID.
    ///     - Closes the database connection.
    ///     - Returns the lead ID.
    /// </remarks>
    [HttpPost]
    public async Task<int> SaveLeads(LeadDetails leadDetails, string user)
    {
        await Task.Yield();
        if (leadDetails == null)
        {
            return -1;
        }

        await using SqlConnection _connection = new(_configuration.GetConnectionString("DBConnect"));
        await _connection.OpenAsync();
        int _returnID = 0;
        try
        {
            await using SqlCommand _command = new("SaveLeads", _connection);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Int("Id", leadDetails.ID);
            _command.Varchar("Company", 100, leadDetails.Company);
            _command.Varchar("FirstName", 50, leadDetails.FirstName);
            _command.Varchar("LastName", 50, leadDetails.LastName);
            _command.Varchar("Phone", 20, leadDetails.Phone.StripPhoneNumber());
            _command.Varchar("Email", 255, leadDetails.Email);
            _command.Varchar("Street", 1000, leadDetails.Street);
            _command.Varchar("City", 100, leadDetails.City);
            _command.Int("State", leadDetails.State);
            _command.Varchar("ZipCode", 20, leadDetails.ZipCode);
            _command.TinyInt("Status", leadDetails.CurrentStatus);
            _command.TinyInt("LeadSource", leadDetails.Source);
            _command.TinyInt("Industry", leadDetails.Industry);
            _command.Int("NoEmployees", leadDetails.NoOfEmployees);
            _command.Decimal("Revenue", 18, 2, leadDetails.Revenue);
            _command.Varchar("Description", 4000, leadDetails.Description);
            _command.Varchar("Website", 255, leadDetails.Website);
            _command.Varchar("User", 10, user);
            _returnID = _command.ExecuteScalarAsync().Result.ToInt32();
        }
        catch
        {
            //
        }

        await _connection.CloseAsync();

        return _returnID;
    }

    /// <summary>
    ///     Saves the notes associated with a lead.
    /// </summary>
    /// <param name="leadNote">An instance of <see cref="CandidateNotes" /> containing the note to be saved.</param>
    /// <param name="leadID">The ID of the lead associated with the note.</param>
    /// <param name="user">The user performing the save operation.</param>
    /// <returns>A dictionary containing the updated notes after the save operation.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Checks if the provided note is null. If it is, returns an empty dictionary.
    ///     - Opens a connection to the database.
    ///     - Executes the "SaveLeadNote" stored procedure, passing in the note details, lead ID, and user name.
    ///     - Reads the updated notes from the database and adds them to a list.
    ///     - Closes the database connection.
    ///     - Returns a dictionary containing the list of updated notes.
    /// </remarks>
    [HttpPost]
    public async Task<Dictionary<string, object>> SaveNotes(CandidateNotes leadNote, int leadID, string user)
    {
        await Task.Yield();
        List<CandidateNotes> _notes = new();
        if (leadNote == null)
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
            await using SqlCommand _command = new("SaveLeadNote", _connection);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Int("Id", leadNote.ID);
            _command.Int("LeadID", leadID);
            _command.Varchar("Note", -1, leadNote.Notes);
            _command.Bit("IsPrimary", false);
            _command.Varchar("EntityType", 5, "LED");
            _command.Varchar("User", 10, user);
            await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
            if (_reader.HasRows)
            {
                while (_reader.Read())
                {
                    _notes.Add(new(_reader.GetInt32(3), _reader.GetDateTime(0), _reader.GetString(1), _reader.GetString(2)));
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
    ///     Uploads a document associated with a lead.
    /// </summary>
    /// <param name="file">The file to be uploaded.</param>
    /// <returns>A dictionary containing the uploaded document details.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Creates a directory for the lead's files if it doesn't exist.
    ///     - Opens a FileStream to the destination file.
    ///     - Copies the uploaded file to the FileStream.
    ///     - Opens a connection to the database.
    ///     - Executes the "SaveLeadDocuments" stored procedure, passing in the lead ID, document name, document location,
    ///     document notes, internal file name, and user name.
    ///     - Reads the document details from the database and adds them to a list.
    ///     - Closes the database connection.
    ///     - Returns a dictionary containing the list of document details.
    /// </remarks>
    [HttpPost, RequestSizeLimit(62914560)]
    public async Task<Dictionary<string, object>> UploadDocument(IFormFile file)
    {
        await Task.Yield();
        string _fileName = file.FileName;
        string _requisitionID = Request.Form["leadID"].ToString();
        //string _mime = Request.Form.Files[0].ContentDisposition;
        string _internalFileName = Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(Path.Combine(Request.Form["path"].ToString(), "Uploads", "Leads", _requisitionID));
        string _destinationFileName = Path.Combine(Request.Form["path"].ToString(), "Uploads", "Leads", _requisitionID, _internalFileName);

        //await using MemoryStream _stream = new();
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
            await using SqlCommand _command = new("SaveLeadDocuments", _connection);
            _command.CommandType = CommandType.StoredProcedure;
            _command.Int("LeadId", _requisitionID);
            _command.Varchar("DocumentName", 255, Request.Form["name"].ToString());
            _command.Varchar("DocumentLocation", 255, _fileName);
            _command.Varchar("DocumentNotes", 2000, Request.Form["notes"].ToString());
            _command.Varchar("InternalFileName", 50, _internalFileName);
            _command.Varchar("DocsUser", 10, Request.Form["user"].ToString());
            await using SqlDataReader _reader = await _command.ExecuteReaderAsync();
            if (_reader.HasRows)
            {
                while (_reader.Read())
                {
                    _documents.Add(new(_reader.GetInt32(0), _reader.GetInt32(1), _reader.NString(2), _reader.NString(3), _reader.NString(6),
                                       $"{_reader.NDateTime(5)} [{_reader.NString(4)}]", _reader.NString(7), ""));
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