#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           CandidateDetails.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     10-26-2023 21:07
// *****************************************/

#endregion

#region Using

using ProfSvc_Classes.Validation;

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Represents the details of a candidate.
/// </summary>
/// <remarks>
///     This class is used to manage and manipulate candidate data, including personal information, contact details, and
///     preferences.
/// </remarks>
public class CandidateDetails
{
	/// <summary>
	///     Initializes a new instance of the <see cref="CandidateDetails" /> class.
	/// </summary>
	/// <remarks>
	///     This constructor is used to create a new candidate with default data.
	/// </remarks>
	public CandidateDetails()
	{
		Clear();
	}

	/// <summary>
	///     Initializes a new instance of the CandidateDetails class.
	/// </summary>
	/// <param name="firstName">The first name of the candidate.</param>
	/// <param name="middleName">The middle name of the candidate.</param>
	/// <param name="lastName">The last name of the candidate.</param>
	/// <param name="address1">The primary address of the candidate.</param>
	/// <param name="address2">The secondary address of the candidate.</param>
	/// <param name="city">The city of the candidate's address.</param>
	/// <param name="stateID">The state ID of the candidate's address.</param>
	/// <param name="zipCode">The zip code of the candidate's address.</param>
	/// <param name="email">The email address of the candidate.</param>
	/// <param name="phone1">The primary phone number of the candidate.</param>
	/// <param name="phone2">The secondary phone number of the candidate.</param>
	/// <param name="phone3">The tertiary phone number of the candidate.</param>
	/// <param name="phoneExt">The phone extension of the candidate.</param>
	/// <param name="linkedIn">The LinkedIn profile of the candidate.</param>
	/// <param name="facebook">The Facebook profile of the candidate.</param>
	/// <param name="twitter">The Twitter profile of the candidate.</param>
	/// <param name="title">The title of the candidate.</param>
	/// <param name="eligibilityID">The eligibility ID of the candidate.</param>
	/// <param name="relocate">Indicates whether the candidate is willing to relocate.</param>
	/// <param name="background">Indicates whether the candidate has a background check.</param>
	/// <param name="jobOptions">The job options of the candidate.</param>
	/// <param name="taxTerm">The tax term of the candidate.</param>
	/// <param name="originalResume">The original resume of the candidate.</param>
	/// <param name="formattedResume">The formatted resume of the candidate.</param>
	/// <param name="textResume">The text resume of the candidate.</param>
	/// <param name="keywords">The keywords associated with the candidate.</param>
	/// <param name="communication">The communication preferences of the candidate.</param>
	/// <param name="rateCandidate">The rating of the candidate.</param>
	/// <param name="rateNotes">The notes for the candidate's rating.</param>
	/// <param name="mpc">Indicates whether the candidate is a most placeable candidate (MPC).</param>
	/// <param name="mpcNotes">The notes for the candidate's MPC status.</param>
	/// <param name="experienceID">The experience ID of the candidate.</param>
	/// <param name="hourlyRate">The hourly rate of the candidate.</param>
	/// <param name="hourlyRateHigh">The high range of the candidate's hourly rate.</param>
	/// <param name="salaryHigh">The high range of the candidate's salary.</param>
	/// <param name="salaryLow">The low range of the candidate's salary.</param>
	/// <param name="relocationNotes">The notes for the candidate's relocation status.</param>
	/// <param name="securityNotes">The security notes for the candidate.</param>
	/// <param name="refer">Indicates whether the candidate was referred.</param>
	/// <param name="referAccountManager">The account manager who referred the candidate.</param>
	/// <param name="eeo">Indicates whether the candidate has an Equal Employment Opportunity (EEO) file.</param>
	/// <param name="eeoFile">The EEO file of the candidate.</param>
	/// <param name="summary">The summary of the candidate's profile.</param>
	/// <param name="googlePlus">The Google Plus profile of the candidate.</param>
	/// <param name="created">The date the candidate's profile was created.</param>
	/// <param name="updated">The date the candidate's profile was last updated.</param>
	/// <param name="candidateID">The ID of the candidate.</param>
	/// <param name="status">The status of the candidate.</param>
	public CandidateDetails(string firstName, string middleName, string lastName, string address1, string address2, string city, int stateID, string zipCode,
							string email, string phone1, string phone2, string phone3, string phoneExt, string linkedIn, string facebook, string twitter,
							string title, int eligibilityID, bool relocate, bool background, string jobOptions, string taxTerm, string originalResume, string formattedResume,
							string textResume, string keywords, string communication, byte rateCandidate, string rateNotes, bool mpc, string mpcNotes, int experienceID,
							decimal hourlyRate, decimal hourlyRateHigh, decimal salaryHigh, decimal salaryLow, string relocationNotes, string securityNotes, bool refer, string referAccountManager,
							bool eeo, string eeoFile, string summary, string googlePlus, string created, string updated, int candidateID, string status)
	{
		FirstName = firstName;
		MiddleName = middleName;
		LastName = lastName;
		Address1 = address1;
		Address2 = address2;
		City = city;
		StateID = stateID;
		ZipCode = zipCode;
		Email = email;
		Phone1 = phone1;
		Phone2 = phone2;
		Phone3 = phone3;
		PhoneExt = phoneExt;
		LinkedIn = linkedIn;
		Facebook = facebook;
		Twitter = twitter;
		Title = title;
		EligibilityID = eligibilityID;
		Relocate = relocate;
		Background = background;
		JobOptions = jobOptions;
		TaxTerm = taxTerm;
		OriginalResume = originalResume;
		FormattedResume = formattedResume;
		TextResume = textResume;
		Keywords = keywords;
		Communication = communication;
		RateCandidate = rateCandidate;
		RateNotes = rateNotes;
		MPC = mpc;
		MPCNotes = mpcNotes;
		ExperienceID = experienceID;
		HourlyRate = hourlyRate;
		HourlyRateHigh = hourlyRateHigh;
		SalaryHigh = salaryHigh;
		SalaryLow = salaryLow;
		RelocationNotes = relocationNotes;
		SecurityNotes = securityNotes;
		Refer = refer;
		ReferAccountManager = referAccountManager;
		EEO = eeo;
		EEOFile = eeoFile;
		Summary = summary;
		GooglePlus = googlePlus;
		Created = created;
		Updated = updated;
		CandidateID = candidateID;
		Status = status;
	}

	/// <summary>
	///     Gets or sets the first line of the candidate's address.
	/// </summary>
	/// <value>
	///     The first line of the candidate's address.
	/// </value>
	/// <remarks>
	///     This property is required and its length should be between 1 and 255 characters.
	/// </remarks>
	[Required(ErrorMessage = "Address is required."),
	 StringLength(255, MinimumLength = 1, ErrorMessage = "Address should be between 1 and 255 characters.")]
	public string Address1
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the secondary address line for the candidate.
	/// </summary>
	/// <value>
	///     A string representing the secondary address line of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used when the candidate's address requires an additional line for more detailed information.
	///     The maximum length of this property is 255 characters.
	/// </remarks>
	[MaxLength(255, ErrorMessage = "Address 2 field cannot be greater than 255 characters.")]
	public string Address2
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the background check has been completed for the candidate.
	/// </summary>
	/// <value>
	///     true if the background check is completed; otherwise, false.
	/// </value>
	/// <remarks>
	///     Use this property to track the status of the candidate's background check process.
	/// </remarks>
	public bool Background
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the unique identifier for the candidate.
	/// </summary>
	/// <value>
	///     The unique identifier for the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to uniquely identify a candidate in the system.
	/// </remarks>
	public int CandidateID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the city of the candidate.
	/// </summary>
	/// <value>
	///     The city of the candidate.
	/// </value>
	/// <remarks>
	///     This property is required and should be between 1 and 50 characters.
	/// </remarks>
	[Required(ErrorMessage = "City is required."),
	 StringLength(50, MinimumLength = 1, ErrorMessage = "City should be between 1 and 50 characters.")]
	public string City
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the communication rating of the candidate.
	/// </summary>
	/// <value>
	///     The communication rating, represented as a string value of the CommunicationEnum.
	/// </value>
	/// <remarks>
	///     This property is used to store the candidate's communication rating. The rating is represented as a string value of
	///     the CommunicationEnum:
	///     - "G" => "Good"
	///     - "A" => "Average"
	///     - "X" => "Excellent"
	///     - Any other value => "Fair"
	/// </remarks>
	[EnumDataType(typeof(CommunicationEnum))]
	public string Communication
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the creation date of the candidate details.
	/// </summary>
	/// <value>
	///     The creation date of the candidate details.
	/// </value>
	/// <remarks>
	///     This property is used to track when the candidate details were first created.
	/// </remarks>
	public string Created
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the candidate has Equal Employment Opportunity (EEO) status.
	/// </summary>
	/// <value>
	///     <c>true</c> if the candidate has EEO status; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     This property is used to track the EEO status of the candidate. EEO status is a legal framework that prohibits
	///     employment discrimination.
	/// </remarks>
	public bool EEO
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Equal Employment Opportunity (EEO) file associated with the candidate.
	/// </summary>
	/// <value>
	///     The EEO file as a string.
	/// </value>
	/// <remarks>
	///     This property is used to store and retrieve the EEO file of the candidate.
	/// </remarks>
	public string EEOFile
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the eligibility ID of the candidate.
	/// </summary>
	/// <value>
	///     The eligibility ID is an integer value that represents the eligibility status of the candidate.
	///     This ID is used to link the candidate with their corresponding eligibility status in the system.
	/// </value>
	/// <remarks>
	///     The eligibility ID is used in various parts of the application, such as the Candidate page and the
	///     EditCandidateDialog component,
	///     to determine and display the eligibility status of the candidate.
	/// </remarks>
	public int EligibilityID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the email address of the candidate.
	/// </summary>
	/// <value>
	///     The email address of the candidate.
	/// </value>
	/// <remarks>
	///     This property is required and should be in proper email format.
	///     The length of the email address should be between 5 and 255 characters.
	///     The email address is also validated to check if the candidate already exists.
	/// </remarks>
	[Required(ErrorMessage = "Email Address is required."),
	 StringLength(255, MinimumLength = 5, ErrorMessage = "Email Address should be between 5 and 255 characters."),
	 EmailAddress(ErrorMessage = "Email Address should be in proper format."),
	 CustomValidation(typeof(Validations), "CheckCandidateExists")]
	public string Email
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the experience ID of the candidate.
	/// </summary>
	/// <value>
	///     The experience ID is an integer that represents the candidate's experience level.
	///     It is used to match the candidate's experience with the requirements of the job position.
	/// </value>
	public int ExperienceID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Facebook profile ID of the candidate.
	/// </summary>
	/// <value>
	///     The Facebook profile ID of the candidate. This value should be less than 255 characters.
	/// </value>
	/// <remarks>
	///     This property is used when saving candidate details in the `CandidatesController.SaveCandidate()` method.
	/// </remarks>
	[StringLength(255, MinimumLength = 0, ErrorMessage = "Facebook Profile ID should be less than 255 characters.")]
	public string Facebook
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the first name of the candidate.
	/// </summary>
	/// <value>
	///     The first name of the candidate. This value is required and should be between 1 and 50 characters.
	/// </value>
	/// <remarks>
	///     This property is validated by the CheckCandidateExists method in the Validations class.
	/// </remarks>
	[Required(ErrorMessage = "First Name is required."),
	 StringLength(50, MinimumLength = 1, ErrorMessage = "First Name should be between 1 and 50 characters."),
	 CustomValidation(typeof(Validations), "CheckCandidateExists")]
	public string FirstName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the formatted resume of the candidate.
	/// </summary>
	/// <value>
	///     The formatted resume of the candidate as a string.
	/// </value>
	/// <remarks>
	///     This property is used when saving candidate details to the database via the 'SaveCandidate' stored procedure.
	/// </remarks>
	public string FormattedResume
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Google+ profile ID of the candidate.
	/// </summary>
	/// <value>
	///     The Google+ profile ID of the candidate. This value can be up to 255 characters long.
	/// </value>
	/// <remarks>
	///     This property is used to store the Google+ profile ID of the candidate. It can be used to access the candidate's
	///     Google+ profile.
	/// </remarks>
	[StringLength(255, MinimumLength = 0, ErrorMessage = "Google+ Profile ID should be less than 255 characters.")]
	public string GooglePlus
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the hourly rate for the candidate.
	/// </summary>
	/// <value>
	///     The hourly rate is a decimal value ranging from $0.0 to $2000.0.
	/// </value>
	/// <remarks>
	///     This property is used to represent the candidate's expected hourly compensation.
	///     An error message is displayed if the value is not within the specified range.
	/// </remarks>
	[Range(0.0, 2000.0, ErrorMessage = "Hourly Rate should be between $0 and $2,000")]
	public decimal HourlyRate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the upper limit of the hourly rate for the candidate.
	/// </summary>
	/// <value>
	///     A decimal representing the high end of the candidate's hourly rate.
	/// </value>
	/// <remarks>
	///     This property is validated to be between $0 and $2,000.
	/// </remarks>
	[Range(0.0, 2000.0, ErrorMessage = "Hourly Rate should be between $0 and $2,000"),
	 CustomValidation(typeof(Validations), "CheckHourlyRate")]
	public decimal HourlyRateHigh
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether a new candidate is being added.
	/// </summary>
	/// <value>
	///     <c>true</c> if a new candidate is being added; otherwise, <c>false</c> if an existing candidate is being edited.
	/// </value>
	/// <remarks>
	///     This property is used in the Candidate page in the ProfSvc_AppTrack application to distinguish between adding a new
	///     candidate and editing an existing one.
	/// </remarks>
	public bool IsAdd
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the job options for the candidate.
	/// </summary>
	/// <value>
	///     A string representing the job options preferred by the candidate.
	/// </value>
	/// <remarks>
	///     This property can be used to store and retrieve the job options that a candidate is interested in.
	/// </remarks>
	public string JobOptions
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the keywords associated with the candidate.
	/// </summary>
	/// <value>
	///     A string containing the keywords. This value is required and its length should be between 3 and 500 characters.
	/// </value>
	/// <remarks>
	///     The keywords are used to categorize the candidate and improve searchability.
	/// </remarks>
	[Required(ErrorMessage = "Keywords is required."),
	 StringLength(500, MinimumLength = 3, ErrorMessage = "Keywords should be between 3 and 500 characters.")]
	public string Keywords
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the last name of the candidate.
	/// </summary>
	/// <remarks>
	///     This property is required and must be between 1 and 50 characters.
	///     It also uses custom validation to check if the candidate exists.
	/// </remarks>
	[Required(ErrorMessage = "Last Name is required."),
	 StringLength(50, MinimumLength = 1, ErrorMessage = "Last Name should be between 1 and 50 characters."),
	 CustomValidation(typeof(Validations), "CheckCandidateExists")]
	public string LastName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the LinkedIn profile ID of the candidate.
	/// </summary>
	/// <value>
	///     The LinkedIn profile ID of the candidate.
	/// </value>
	/// <remarks>
	///     The LinkedIn profile ID should be less than 255 characters.
	/// </remarks>
	[StringLength(255, MinimumLength = 0, ErrorMessage = "LinkedIn Profile ID should be less than 255 characters.")]
	public string LinkedIn
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the middle name of the candidate.
	/// </summary>
	/// <value>
	///     The middle name of the candidate.
	/// </value>
	/// <remarks>
	///     This property is validated to ensure that the middle name is no more than 50 characters in length.
	/// </remarks>
	[StringLength(50, MinimumLength = 0, ErrorMessage = "Middle Name should be less than 50 characters.")]
	public string MiddleName
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the MPC condition is met.
	/// </summary>
	/// <value>
	///     <c>true</c> if MPC is met; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     Use this property to check or define the MPC status of the candidate.
	/// </remarks>
	public bool MPC
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the notes associated with the candidate in the MPC context.
	/// </summary>
	/// <value>
	///     The notes associated with the candidate in the MPC context.
	/// </value>
	/// <remarks>
	///     This property is used in the Candidate page of the ProfSvc_AppTrack application for retrieving the most recent note
	///     from the CandidateMPC object list and for retrieving the most recent date from the CandidateMPC list.
	/// </remarks>
	public string MPCNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the original resume of the candidate.
	/// </summary>
	/// <value>
	///     The original resume of the candidate as a string.
	/// </value>
	/// <remarks>
	///     This property is used to store the original format of the candidate's resume.
	///     It is used in the `SaveCandidate` method of the `CandidatesController` class.
	/// </remarks>
	public string OriginalResume
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the primary phone number of the candidate.
	/// </summary>
	/// <value>
	///     The primary phone number of the candidate.
	/// </value>
	/// <remarks>
	///     This property is required and should be in proper phone number format. The phone number should be 10 digits long,
	///     including the area code.
	/// </remarks>
	[Required(ErrorMessage = "Phone is required."), Phone(ErrorMessage = "Phone Number should be in proper format."),
	 StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number should be 10 digits including area code.")]
	public string Phone1
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the secondary phone number of the candidate.
	/// </summary>
	/// <value>
	///     The secondary phone number of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used when the candidate has more than one phone number.
	/// </remarks>
	public string Phone2
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the third phone number of the candidate.
	/// </summary>
	/// <value>
	///     The third phone number of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used when the candidate has more than two phone numbers.
	/// </remarks>
	public string Phone3
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the phone extension for the candidate.
	/// </summary>
	/// <value>
	///     The phone extension as a string.
	/// </value>
	/// <remarks>
	///     This property is used when more than one phone line is associated with the same phone number.
	/// </remarks>
	public string PhoneExt
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the rating of the candidate.
	/// </summary>
	/// <value>
	///     The rating of the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to store the rating of the candidate. The value is an integer where a higher value indicates
	///     a better rating.
	/// </remarks>
	public int RateCandidate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the rate notes for the candidate.
	/// </summary>
	/// <value>
	///     The rate notes for the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to store any notes related to the candidate's rating.
	///     It is used in the Candidate page of the ProfSvc_AppTrack application to retrieve and display the rating note for a
	///     candidate.
	/// </remarks>
	public string RateNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the candidate is referred or not.
	/// </summary>
	/// <value>
	///     <c>true</c> if the candidate is referred; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     This property is used to track the referral status of a candidate. If the candidate is referred by someone, this
	///     property will be set to <c>true</c>; otherwise, it will be set to <c>false</c>.
	/// </remarks>
	public bool Refer
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the account manager reference for the candidate.
	/// </summary>
	/// <value>
	///     The account manager reference.
	/// </value>
	/// <remarks>
	///     This property is used when the candidate is referred by an account manager.
	///     The value corresponds to the account manager's identifier.
	/// </remarks>
	public string ReferAccountManager
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the candidate is willing to relocate.
	/// </summary>
	/// <value>
	///     <c>true</c> if the candidate is willing to relocate; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     This property is used to understand the candidate's flexibility in terms of job location.
	/// </remarks>
	public bool Relocate
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the relocation notes for the candidate.
	/// </summary>
	/// <value>
	///     The relocation notes, which should be less than 2000 characters.
	/// </value>
	/// <remarks>
	///     This property is used to store any additional notes or details about the candidate's relocation preferences or
	///     requirements.
	/// </remarks>
	[StringLength(2000, MinimumLength = 0, ErrorMessage = "Relocation Notes should be less than 255 characters.")]
	public string RelocationNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the maximum expected salary for the candidate.
	/// </summary>
	/// <value>
	///     The maximum salary that the candidate expects, expressed as a decimal.
	/// </value>
	/// <remarks>
	///     This property is validated to be within the range of $0 to $1,000,000.
	/// </remarks>
	[Range(0.0, 1000000.0, ErrorMessage = "Hourly Rate should be between $0 and $1,000,000")]
	public decimal SalaryHigh
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the lower limit of the salary range for the candidate.
	/// </summary>
	/// <value>
	///     A decimal representing the lower limit of the salary range in dollars.
	/// </value>
	/// <remarks>
	///     The value should be between $0 and $1,000,000. An error message will be displayed if the value is outside this
	///     range.
	/// </remarks>
	[Range(0.0, 1000000.0, ErrorMessage = "Hourly Rate should be between $0 and $1,000,000")]
	public decimal SalaryLow
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the security notes for the candidate.
	/// </summary>
	/// <value>
	///     The security notes for the candidate.
	/// </value>
	/// <remarks>
	///     This property is used to store any security-related notes or comments about the candidate.
	///     The length of the notes should be less than 2000 characters.
	/// </remarks>
	[StringLength(2000, MinimumLength = 0, ErrorMessage = "Security Notes should be less than 2000 characters.")]
	public string SecurityNotes
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the identifier for the state of the candidate.
	/// </summary>
	/// <value>
	///     The identifier for the state.
	/// </value>
	/// <remarks>
	///     This property is used to link the candidate to a specific state.
	///     The value corresponds to the key of the state in the states dictionary.
	/// </remarks>
	public int StateID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the status of the candidate.
	/// </summary>
	/// <value>
	///     A string representing the current status of the candidate.
	/// </value>
	/// <remarks>
	///     The status of the candidate can be used to track the candidate's progress in the recruitment process.
	/// </remarks>
	public string Status
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the summary of the candidate's profile.
	/// </summary>
	/// <value>
	///     A string representing the summary of the candidate's profile.
	/// </value>
	/// <remarks>
	///     This property is bound to a text box control in the user interface, allowing the user to view and edit the summary
	///     of the candidate's profile.
	///     The summary should be less than 32767 characters.
	/// </remarks>
	[StringLength(32767, MinimumLength = 0, ErrorMessage = "Summary should be less than 32767 characters.")]
	public string Summary
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the tax term for the candidate.
	/// </summary>
	/// <value>
	///     A string representing the tax term. The tax term is a comma-separated list of tax-related terms applicable to the
	///     candidate.
	/// </value>
	/// <remarks>
	///     This property is used in the `Candidate.SetTaxTerm()` method in the `Candidate` class and the
	///     `EditCandidateDialog.BuildRenderTree()` method in the `EditCandidateDialog` class.
	/// </remarks>
	public string TaxTerm
	{
		get;
		set;
	} = "";

	/// <summary>
	///     Gets or sets the text resume of the candidate.
	/// </summary>
	/// <value>
	///     The text resume of the candidate.
	/// </value>
	/// <remarks>
	///     This property is validated to ensure that the text resume is less than 262,136 characters.
	/// </remarks>
	[StringLength(262136, MinimumLength = 0, ErrorMessage = "Text Resume should be less than 262,136 characters.")]
	public string TextResume
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the title of the candidate.
	/// </summary>
	/// <value>
	///     The title of the candidate.
	/// </value>
	/// <remarks>
	///     This property is required and its length should be between 1 and 200 characters.
	/// </remarks>
	[Required(ErrorMessage = "Title is required."),
	 StringLength(200, MinimumLength = 1, ErrorMessage = "Title should be between 1 and 255 characters.")]
	public string Title
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Twitter profile ID of the candidate.
	/// </summary>
	/// <value>
	///     The Twitter profile ID of the candidate.
	/// </value>
	/// <remarks>
	///     This property is validated to ensure that the Twitter profile ID is less than 255 characters.
	/// </remarks>
	[StringLength(255, MinimumLength = 0, ErrorMessage = "Twitter Profile ID should be less than 255 characters.")]
	public string Twitter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the timestamp of the last update made to the candidate details.
	/// </summary>
	/// <value>
	///     A string representing the last update timestamp.
	/// </value>
	/// <remarks>
	///     The value is typically set automatically when changes are made to the candidate details.
	/// </remarks>
	public string Updated
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Zip Code of the candidate.
	/// </summary>
	/// <value>
	///     The Zip Code of the candidate.
	/// </value>
	/// <remarks>
	///     This property is required and must be a minimum of 5 digits and a maximum of 10 digits.
	/// </remarks>
	[Required(ErrorMessage = "Zip Code is required."),
	 StringLength(10, MinimumLength = 5, ErrorMessage = "Phone number should be minimum 5 digits.")]
	public string ZipCode
	{
		get;
		set;
	}

	/// <summary>
	///     Clears all the properties of the CandidateDetails instance.
	/// </summary>
	/// <remarks>
	///     This method is used to reset all the properties of the CandidateDetails instance to their default values.
	///     It is typically used when we need to clear the existing data of a candidate.
	/// </remarks>
	public void Clear()
	{
		FirstName = "";
		MiddleName = "";
		LastName = "";
		Address1 = "";
		Address2 = "";
		City = "";
		StateID = 0;
		ZipCode = "";
		Email = "";
		Phone1 = "";
		Phone2 = "";
		Phone3 = "";
		PhoneExt = "";
		LinkedIn = "";
		Facebook = "";
		Twitter = "";
		Title = "";
		EligibilityID = 0;
		Relocate = false;
		Background = false;
		JobOptions = "";
		TaxTerm = "";
		OriginalResume = "";
		FormattedResume = "";
		TextResume = "";
		Keywords = "";
		Communication = "A";
		RateCandidate = 3;
		RateNotes = "";
		MPC = false;
		MPCNotes = "";
		ExperienceID = 0;
		HourlyRate = decimal.Zero;
		HourlyRateHigh = decimal.Zero;
		SalaryHigh = decimal.Zero;
		SalaryLow = decimal.Zero;
		RelocationNotes = "";
		SecurityNotes = "";
		Refer = false;
		ReferAccountManager = "";
		EEO = false;
		EEOFile = "";
		Summary = "";
		GooglePlus = "";
		Created = "";
		Updated = "";
		CandidateID = 0;
		Status = "AVL";
	}

	/// <summary>
	///     Creates a copy of the current instance of the <see cref="CandidateDetails" /> class.
	/// </summary>
	/// <returns>
	///     A new instance of the <see cref="CandidateDetails" /> class with the same values as the current instance.
	/// </returns>
	/// <remarks>
	///     This method uses the MemberwiseClone method to create a shallow copy of the current object.
	/// </remarks>
	public CandidateDetails Copy() => MemberwiseClone() as CandidateDetails;

	/// <summary>
	///     Enumerates the possible communication ratings for a candidate.
	/// </summary>
	/// <remarks>
	///     This enum is used to represent the candidate's communication rating in the CandidateDetails class.
	///     The values are:
	///     - A (1) => Average
	///     - X (2) => Excellent
	///     - G (3) => Good
	///     - F (4) => Fair
	/// </remarks>
	private enum CommunicationEnum
	{
		A = 1, X = 2, G = 3, F = 4
	}
}