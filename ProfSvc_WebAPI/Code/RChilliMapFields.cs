#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           RChilliMapFields.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          04-24-2023 20:59
// Last Updated On:     04-24-2023 21:27
// *****************************************/

#endregion

// ReSharper disable UnusedMember.Global
namespace ProfSvc_AppTrack.Code;

public class Error
{
	public int ErrorCode
	{
		get;
		set;
	}

	public string ErrorMsg
	{
		get;
		set;
	}
}

public class ErrorClass
{
	public Error Error
	{
		get;
		set;
	}
}

public class ResumeLanguage
{
	public string Language
	{
		get;
		set;
	}

	public string LanguageCode
	{
		get;
		set;
	}
}

public class CountryCode
{
	public string IsoAlpha2
	{
		get;
		set;
	}

	public string IsoAlpha3
	{
		get;
		set;
	}

	public string UNCode
	{
		get;
		set;
	}
}

public class ResumeCountry
{
	public string Country
	{
		get;
		set;
	}

	public CountryCode CountryCode
	{
		get;
		set;
	}

	public string Evidence
	{
		get;
		set;
	}
}

public class Name
{
	public int ConfidenceScore
	{
		get;
		set;
	}

	public string FirstName
	{
		get;
		set;
	}

	public string FormattedName
	{
		get;
		set;
	}

	public string FullName
	{
		get;
		set;
	}

	public string LastName
	{
		get;
		set;
	}

	public string MiddleName
	{
		get;
		set;
	}

	public string TitleName
	{
		get;
		set;
	}
}

public class LanguageKnown
{
	public string Language
	{
		get;
		set;
	}

	public string LanguageCode
	{
		get;
		set;
	}
}

public class PassportDetail
{
	public string DateOfExpiry
	{
		get;
		set;
	}

	public string DateOfIssue
	{
		get;
		set;
	}

	public string PassportNumber
	{
		get;
		set;
	}

	public string PlaceOfIssue
	{
		get;
		set;
	}
}

public class Email
{
	public int ConfidenceScore
	{
		get;
		set;
	}

	public string EmailAddress
	{
		get;
		set;
	}
}

public class PhoneNumber
{
	public int ConfidenceScore
	{
		get;
		set;
	}

	public string FormattedNumber
	{
		get;
		set;
	}

	public string IDDCode
	{
		get;
		set;
	}

	public string Number
	{
		get;
		set;
	}

	public string OriginalNumber
	{
		get;
		set;
	}

	public string Type
	{
		get;
		set;
	}
}

public class WebSite
{
	public string Type
	{
		get;
		set;
	}

	public string Url
	{
		get;
		set;
	}
}

public class Address
{
	public string City
	{
		get;
		set;
	}

	public int ConfidenceScore
	{
		get;
		set;
	}

	public string Country
	{
		get;
		set;
	}

	public CountryCode CountryCode
	{
		get;
		set;
	}

	public string FormattedAddress
	{
		get;
		set;
	}

	public string State
	{
		get;
		set;
	}

	public string StateIsoCode
	{
		get;
		set;
	}

	public string Street
	{
		get;
		set;
	}

	public string Type
	{
		get;
		set;
	}

	public string ZipCode
	{
		get;
		set;
	}
}

public class CurrentSalary
{
	public string Amount
	{
		get;
		set;
	}

	public string Currency
	{
		get;
		set;
	}

	public string Symbol
	{
		get;
		set;
	}

	public string Text
	{
		get;
		set;
	}

	public string Unit
	{
		get;
		set;
	}
}

public class ExpectedSalary
{
	public string Amount
	{
		get;
		set;
	}

	public string Currency
	{
		get;
		set;
	}

	public string Symbol
	{
		get;
		set;
	}

	public string Text
	{
		get;
		set;
	}

	public string Unit
	{
		get;
		set;
	}
}

public class Location
{
	public string City
	{
		get;
		set;
	}

	public string Country
	{
		get;
		set;
	}

	public CountryCode CountryCode
	{
		get;
		set;
	}

	public string State
	{
		get;
		set;
	}

	public string StateIsoCode
	{
		get;
		set;
	}
}

public class Institution
{
	public int ConfidenceScore
	{
		get;
		set;
	}

	public Location Location
	{
		get;
		set;
	}

	public string Name
	{
		get;
		set;
	}

	public string Type
	{
		get;
		set;
	}
}

public class SubInstitution
{
	public int ConfidenceScore
	{
		get;
		set;
	}

	public Location Location
	{
		get;
		set;
	}

	public string Name
	{
		get;
		set;
	}

	public string Type
	{
		get;
		set;
	}
}

public class Degree
{
	public int ConfidenceScore
	{
		get;
		set;
	}

	public string DegreeName
	{
		get;
		set;
	}

	public string NormalizeDegree
	{
		get;
		set;
	}

	public List<object> Specialization
	{
		get;
		set;
	}
}

public class Aggregate
{
	public string MeasureType
	{
		get;
		set;
	}

	public string Value
	{
		get;
		set;
	}
}

public class SegregatedQualification
{
	public Aggregate Aggregate
	{
		get;
		set;
	}

	public Degree Degree
	{
		get;
		set;
	}

	public string EndDate
	{
		get;
		set;
	}

	public string FormattedDegreePeriod
	{
		get;
		set;
	}

	public Institution Institution
	{
		get;
		set;
	}

	public string StartDate
	{
		get;
		set;
	}

	public SubInstitution SubInstitution
	{
		get;
		set;
	}
}

public class SegregatedCertification
{
	public string Authority
	{
		get;
		set;
	}

	public string CertificationCode { get; set; }

	public string CertificationTitle
	{
		get;
		set;
	}

	public string CertificationUrl
	{
		get;
		set;
	}

	public string EndDate
	{
		get;
		set;
	}

	public string IsExpiry
	{
		get;
		set;
	}

	public string StartDate
	{
		get;
		set;
	}
}

public class SegregatedSkill
{
	public string Alias
	{
		get;
		set;
	}

	public string Evidence
	{
		get;
		set;
	}

	public int ExperienceInMonths
	{
		get;
		set;
	}

	public string FormattedName
	{
		get;
		set;
	}

	public string LastUsed
	{
		get;
		set;
	}

	public string Ontology
	{
		get;
		set;
	}

	public string Skill
	{
		get;
		set;
	}

	public string Type
	{
		get;
		set;
	}
}

public class Employer
{
	public int ConfidenceScore
	{
		get;
		set;
	}

	public string EmployerName
	{
		get;
		set;
	}

	public string FormattedName
	{
		get;
		set;
	}
}

public class RelatedSkills
{
	/// <summary>
	/// </summary>
	public string ProficiencyLevel
	{
		get;
		set;
	}

	/// <summary>
	/// </summary>
	public string Skill
	{
		get;
		set;
	}
}

public class JobProfile
{
	public string Alias
	{
		get;
		set;
	}

	public int ConfidenceScore
	{
		get;
		set;
	}

	public string FormattedName
	{
		get;
		set;
	}

	public List<RelatedSkills> RelatedSkills
	{
		get;
		set;
	}

	public string Title
	{
		get;
		set;
	}
}

public class Project
{
	public string ProjectName
	{
		get;
		set;
	}

	public string TeamSize
	{
		get;
		set;
	}

	public string UsedSkills
	{
		get;
		set;
	}
}

public class SegregatedExperience
{
	public Employer Employer
	{
		get;
		set;
	}

	public string EndDate
	{
		get;
		set;
	}

	public string FormattedJobPeriod
	{
		get;
		set;
	}

	public string IsCurrentEmployer
	{
		get;
		set;
	}

	public string JobDescription
	{
		get;
		set;
	}

	public string JobPeriod
	{
		get;
		set;
	}

	public JobProfile JobProfile
	{
		get;
		set;
	}

	public Location Location
	{
		get;
		set;
	}

	public List<Project> Projects
	{
		get;
		set;
	}

	public string StartDate
	{
		get;
		set;
	}
}

public class WorkedPeriod
{
	public string TotalExperienceInMonths
	{
		get;
		set;
	}

	public string TotalExperienceInYear
	{
		get;
		set;
	}

	public string TotalExperienceRange
	{
		get;
		set;
	}
}

public class SegregatedPublication
{
	public string Authors
	{
		get;
		set;
	}

	public string Description
	{
		get;
		set;
	}

	public string PublicationNumber
	{
		get;
		set;
	}

	public string PublicationTitle
	{
		get;
		set;
	}

	public string PublicationUrl
	{
		get;

		set;
	}

	public string Publisher
	{
		get;
		set;
	}
}

public class CurrentLocation
{
	public string City
	{
		get;
		set;
	}

	public string Country
	{
		get;
		set;
	}

	public CountryCode CountryCode
	{
		get;
		set;
	}

	public string State
	{
		get;
		set;
	}

	public string StateIsoCode
	{
		get;
		set;
	}
}

public class PreferredLocation
{
	public string City
	{
		get;
		set;
	}

	public string Country
	{
		get;
		set;
	}

	public CountryCode CountryCode
	{
		get;
		set;
	}

	public string State
	{
		get;
		set;
	}

	public string StateIsoCode
	{
		get;
		set;
	}
}

public class SegregatedAchievement
{
	public string AssociatedWith
	{
		get;
		set;
	}

	public string AwardTitle
	{
		get;
		set;
	}

	public string Description
	{
		get;
		set;
	}

	public string Issuer
	{
		get;
		set;
	}

	public string IssuingDate
	{
		get;
		set;
	}
}

public class EmailInfo
{
	public string EmailBody
	{
		get;
		set;
	}

	public string EmailCC
	{
		get;
		set;
	}

	public string EmailFrom
	{
		get;
		set;
	}

	public string EmailReplyTo
	{
		get;
		set;
	}

	public string EmailSignature
	{
		get;
		set;
	}

	public string EmailSubject
	{
		get;
		set;
	}

	public string EmailTo
	{
		get;
		set;
	}
}

public class Recommendation
{
	public string CompanyName
	{
		get;
		set;
	}

	public string Description
	{
		get;
		set;
	}

	public string PersonName
	{
		get;
		set;
	}

	public string PositionTitle
	{
		get;
		set;
	}

	public string Relation
	{
		get;
		set;
	}
}

public class CandidateImage
{
	public string CandidateImageData
	{
		get;
		set;
	}

	public string CandidateImageFormat
	{
		get;
		set;
	}
}

public class TemplateOutput
{
	public string TemplateOutputData
	{
		get;
		set;
	}

	public string TemplateOutputFileName
	{
		get;
		set;
	}
}

public class ApiInfo
{
	public string AccountExpiryDate
	{
		get;
		set;
	}

	public string BuildVersion
	{
		get;
		set;
	}

	public string CreditLeft
	{
		get;
		set;
	}

	public string Metered
	{
		get;
		set;
	}
}

public class ResumeParserData
{
	public string Achievements
	{
		get;
		set;
	}

	public List<Address> Address
	{
		get;
		set;
	}

	public ApiInfo ApiInfo
	{
		get;
		set;
	}

	public string Availability
	{
		get;
		set;
	}

	public string AverageStay
	{
		get;
		set;
	}

	public CandidateImage CandidateImage
	{
		get;
		set;
	}

	public string Category
	{
		get;
		set;
	}

	public string Certification
	{
		get;
		set;
	}

	public string CoverLetter
	{
		get;
		set;
	}

	public string CurrentEmployer
	{
		get;
		set;
	}

	public List<CurrentLocation> CurrentLocation
	{
		get;
		set;
	}

	public CurrentSalary CurrentSalary
	{
		get;
		set;
	}

	public string CustomFields
	{
		get;
		set;
	}

	public string DateOfBirth
	{
		get;
		set;
	}

	public string DetailResume
	{
		get;
		set;
	}

	public List<Email> Email
	{
		get;
		set;
	}

	public EmailInfo EmailInfo
	{
		get;
		set;
	}

	public string ExecutiveSummary
	{
		get;
		set;
	}

	public ExpectedSalary ExpectedSalary
	{
		get;
		set;
	}

	public string Experience
	{
		get;
		set;
	}

	public string FatherName
	{
		get;
		set;
	}

	public string GapPeriod
	{
		get;
		set;
	}

	public string Gender
	{
		get;
		set;
	}

	public string Hobbies
	{
		get;
		set;
	}

	public string HtmlResume
	{
		get;
		set;
	}

	public string JobProfile
	{
		get;
		set;
	}

	public List<LanguageKnown> LanguageKnown
	{
		get;
		set;
	}

	public string LicenseNo
	{
		get;
		set;
	}

	public string LongestStay
	{
		get;
		set;
	}

	public string ManagementSummary
	{
		get;
		set;
	}

	public string MaritalStatus
	{
		get;
		set;
	}

	public string MotherName
	{
		get;
		set;
	}

	public Name Name
	{
		get;
		set;
	}

	public string Nationality
	{
		get;
		set;
	}

	public string Objectives
	{
		get;
		set;
	}

	public string PanNo
	{
		get;
		set;
	}

	public string ParsingDate
	{
		get;
		set;
	}

	public PassportDetail PassportDetail
	{
		get;
		set;
	}

	public List<PhoneNumber> PhoneNumber
	{
		get;
		set;
	}

	public List<PreferredLocation> PreferredLocation
	{
		get;
		set;
	}

	public string Publication
	{
		get;
		set;
	}

	public string Qualification
	{
		get;
		set;
	}

	public List<Recommendation> Recommendations
	{
		get;
		set;
	}

	public string References
	{
		get;
		set;
	}

	public ResumeCountry ResumeCountry
	{
		get;
		set;
	}

	public string ResumeFileName
	{
		get;
		set;
	}

	public ResumeLanguage ResumeLanguage
	{
		get;
		set;
	}

	public List<SegregatedAchievement> SegregatedAchievement
	{
		get;
		set;
	}

	public List<SegregatedCertification> SegregatedCertification
	{
		get;
		set;
	}

	public List<SegregatedExperience> SegregatedExperience
	{
		get;
		set;
	}

	public List<SegregatedPublication> SegregatedPublication
	{
		get;
		set;
	}

	public List<SegregatedQualification> SegregatedQualification
	{
		get;
		set;
	}

	public List<SegregatedSkill> SegregatedSkill
	{
		get;
		set;
	}

	public string SkillBlock
	{
		get;
		set;
	}

	public string SkillKeywords
	{
		get;
		set;
	}

	public string SubCategory
	{
		get;
		set;
	}

	public string Summary
	{
		get;
		set;
	}

	public TemplateOutput TemplateOutput
	{
		get;
		set;
	}

	public string UniqueID
	{
		get;
		set;
	}

	public string VisaStatus
	{
		get;
		set;
	}

	public List<WebSite> WebSite
	{
		get;
		set;
	}

	public WorkedPeriod WorkedPeriod
	{
		get;
		set;
	}
}

public class RChilliMapFields
{
	public ResumeParserData ResumeParserData
	{
		get;
		set;
	}
}