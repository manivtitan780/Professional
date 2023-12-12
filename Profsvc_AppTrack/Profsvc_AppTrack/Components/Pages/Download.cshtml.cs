#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Download.cshtml.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     10-03-2023 21:00
// *****************************************/

#endregion

#region Using

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Profsvc_AppTrack.Components.Pages;

/// <summary>
///     Represents a model for handling file downloads in the application.
/// </summary>
/// <remarks>
///     This class is responsible for processing download requests. It takes a base64 encoded string as input,
///     which contains information about the file to be downloaded and the entity it is associated with.
///     The class decodes this string, validates the input, and then attempts to locate the file in the system.
///     If the file is found, it is read into a byte array and returned as a FileResult for download.
/// </remarks>
public class DownloadModel : PageModel
{
    //public DownloadModel(IWebHostEnvironment environment) => _environment = environment;

    //private IWebHostEnvironment _environment;

    /// <summary>
    ///     Handles the GET request for file download.
    /// </summary>
    /// <param name="file">
    ///     A Base64 encoded string containing the internal file name, the ID of the associated entity (Candidate, Requisition,
    ///     Company, or Lead),
    ///     the final download file name, and the type of entity, all separated by a carat (^).
    /// </param>
    /// <returns>
    ///     A FileResult for the requested file if it exists, otherwise an OkResult if the file parameter is null or white
    ///     space,
    ///     or if the decoded string array does not have exactly 4 elements.
    /// </returns>
    /// <remarks>
    ///     The method decodes the Base64 string, validates the input, and attempts to locate the file in the system.
    ///     If the file is found, it is read into a byte array and returned as a FileResult for download.
    /// </remarks>
    public IActionResult OnGet(string file)
    {
        if (file.NullOrWhiteSpace())
        {
            return new OkResult();
        }

        string[] _decodedStringArray = file.FromBase64String().Split('^');
        if (_decodedStringArray.Length != 4)
        {
            return new OkResult();
        }

        string _type = _decodedStringArray[3] switch
                       {
                           "1" => "Requisition",
                           "2" => "Company",
                           "3" => "Lead",
                           _ => "Candidate"
                       };

        string _filePath = Path.Combine(Start.UploadsPath, "Uploads", _type, _decodedStringArray[1], _decodedStringArray[0]);
        if (!System.IO.File.Exists(_filePath))
        {
            return null;
        }

        byte[] _fileBytes = System.IO.File.ReadAllBytes(_filePath);
        return File(_fileBytes, "application/force-download", _decodedStringArray[2]);
    }
}